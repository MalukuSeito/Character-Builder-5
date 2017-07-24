using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using PCLStorage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CB_5e.ViewModels;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace CB_5e.Views
{

    public interface IDocumentViewer
    {
        void ShowDocumentFile(string name, Stream content, string mimeType);
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileBrowser : ContentPage
    {
        private static object clipboard = null;
        private static bool isCopy = false;
        private FilesViewModel viewModel;

        public FileBrowser(IFolder folder)
        {
            InitializeComponent();

            BindingContext = viewModel = new FilesViewModel(folder);
            //MessagingCenter.Subscribe<NewFolderPage, String>(this, "AddFolder", async (obj, item) =>
            //{
            //    IFolder f = await folder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
            //    await Navigation.PushAsync(new FileBrowser(f));
            //});
        }

        async void Handle_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            if (e.SelectedItem is IFolder f)
            {
                await Navigation.PushAsync(new FileBrowser(f));
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewFolderPage(viewModel));
        }

        private async void Download_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DownloadFilePage(viewModel));
        }

        public async void OnUnpack(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            if (mi.CommandParameter is IFile file)
            {
                string s = file.Name;
                if (s.Contains("."))
                {
                    s = s.Substring(0, s.LastIndexOf('.'));
                }
                if (!await Unpack(file, viewModel.Folder, s))
                {
                    await DisplayAlert("Can't unpack", "Unknown Filetyp, please select a zip file", "OK");
                }
            }
            else await DisplayAlert("Can't unpack", "Nothing to Unpack", "OK");
            viewModel.LoadItemsCommand.Execute(null);
        }

        public static async Task<bool> Unpack(IFile file, IFolder folder, string name)
        {
            try
            {
                using (Stream s = await file.OpenAsync(FileAccess.Read))
                {
                    using (ZipFile zf = new ZipFile(s))
                    {
                        IFolder target = await folder.CreateFolderAsync(name, CreationCollisionOption.GenerateUniqueName);
                        zf.IsStreamOwner = true;
                        foreach (ZipEntry entry in zf)
                        {
                            if (!entry.IsFile) continue;

                            using (Stream ss = zf.GetInputStream(entry)) await Extract(ss, target, entry.Name);
                        }
                        return true;
                    }
                }
            } catch (Exception)
            {
            }

            try
            {
                using (Stream s = await file.OpenAsync(FileAccess.Read))
                {
                    using (TarInputStream zf = new TarInputStream(s))
                    {
                        IFolder target = await folder.CreateFolderAsync(name, CreationCollisionOption.GenerateUniqueName);
                        zf.IsStreamOwner = true;
                        while (true)
                        {
                            TarEntry entry = zf.GetNextEntry();
                            if (entry == null) break;
                            await Extract(zf, target, entry.Name);
                        }
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                using (Stream s = await file.OpenAsync(FileAccess.Read))
                {
                    using (BZip2InputStream zf = new BZip2InputStream(s))
                    {
                        zf.IsStreamOwner = true;
                        IFile target = await folder.CreateFileAsync(name, CreationCollisionOption.GenerateUniqueName);
                        using (var stream = await target.OpenAsync(FileAccess.ReadAndWrite)) zf.CopyTo(stream);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                using (Stream s = await file.OpenAsync(FileAccess.Read))
                {
                    using (GZipInputStream zf = new GZipInputStream(s))
                    {
                        zf.IsStreamOwner = true;
                        IFile target = await folder.CreateFileAsync(name, CreationCollisionOption.GenerateUniqueName);
                        using (var stream = await target.OpenAsync(FileAccess.ReadAndWrite)) zf.CopyTo(stream);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        private static async Task Extract(Stream stream, IFolder target, string path)
        {
            IFile file = await GetFile(target, path);
            if (file == null) return;
            using (Stream s = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                byte[] buffer = new byte[4096];
                stream.CopyTo(s);
            }
        }

        private static async Task<IFile> GetFile(IFolder target, string path)
        {
            if (path == null || path == "" || path.StartsWith(".") || path.EndsWith("/")) return null;
            int i = path.IndexOf('/');
            if (i >= 0)
            {
                string folder = path.Substring(0, i);
                if (folder != "" && !folder.StartsWith(".") && !folder.StartsWith("/"))
                {
                    return await GetFile(await target.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists), path.Substring(i + 1));
                }
                return null;
            } else
            {
                return await target.CreateFileAsync(path, CreationCollisionOption.GenerateUniqueName);
            }
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            if (mi.CommandParameter is IFolder f)
            {
                await f.DeleteAsync();
                viewModel.LoadItemsCommand.Execute(null);
            }
            else if (mi.CommandParameter is IFile file)
            {
                await file.DeleteAsync();
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        public static async Task Move(IFolder source, IFolder target)
        {
            foreach (IFolder f in await source.GetFoldersAsync())
            {
                await Move(f, await target.CreateFolderAsync(f.Name, CreationCollisionOption.GenerateUniqueName).ConfigureAwait(false)).ConfigureAwait(false);
            }
            foreach (IFile f in await source.GetFilesAsync())
            {
                await f.MoveAsync(PortablePath.Combine(target.Path, f.Name), NameCollisionOption.GenerateUniqueName).ConfigureAwait(false);
            }
            await source.DeleteAsync();
        }

        public static async Task Copy(IFolder source, IFolder target)
        {
            foreach (IFolder f in await source.GetFoldersAsync())
            {
                if (f.Name.StartsWith(".")) continue;
                await Copy(f, await target.CreateFolderAsync(f.Name, CreationCollisionOption.GenerateUniqueName).ConfigureAwait(false)).ConfigureAwait(false);
            }
            foreach (IFile f in await source.GetFilesAsync())
            {
                if (f.Name.StartsWith(".")) continue;
                await Copy(f, await target.CreateFileAsync(f.Name, CreationCollisionOption.GenerateUniqueName).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        public static async Task Copy(IFile source, IFile target)
        {
            if (source.Equals(target)) return;
            using (Stream fout = await target.OpenAsync(FileAccess.ReadAndWrite))
            {
                using (Stream fin = await source.OpenAsync(FileAccess.Read))
                {
                    await fin.CopyToAsync(fout);
                }
            }
        }

        public async void OnPaste(object sender, EventArgs e)
        {
            object clp = clipboard;
            if (!isCopy) clipboard = null;
            if (clp is IFolder f)
            {
                if (isCopy) await Copy(f, await viewModel.Folder.CreateFolderAsync(f.Name, CreationCollisionOption.GenerateUniqueName).ConfigureAwait(false)).ConfigureAwait(false);
                else await Move(f, await viewModel.Folder.CreateFolderAsync(f.Name, CreationCollisionOption.GenerateUniqueName).ConfigureAwait(false)).ConfigureAwait(false);
                viewModel.LoadItemsCommand.Execute(null);
            }
            else if (clp is IFile file)
            {
                if (isCopy) await Copy(file, await viewModel.Folder.CreateFileAsync(file.Name, CreationCollisionOption.GenerateUniqueName).ConfigureAwait(false)).ConfigureAwait(false);
                else await file.MoveAsync(PortablePath.Combine(viewModel.Folder.Path, file.Name), NameCollisionOption.GenerateUniqueName).ConfigureAwait(false);
                viewModel.LoadItemsCommand.Execute(null);
            }
            else await DisplayAlert("Can't paste", "Nothing cut or copied", "OK");
        }
        public void OnCopy(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            clipboard = mi.CommandParameter;
            isCopy = true;
        }
        public void OnCut(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            clipboard = mi.CommandParameter;
            isCopy = false;
        }
        private async void OnRename(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            await Navigation.PushAsync(new RenamePage(viewModel, mi.CommandParameter));
        }

        private async void OnShare(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            if (mi.CommandParameter is IFile file)
            {
                string mime = "application/octet-stream";
                if (file.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) mime = "text/xml";
                if (file.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) mime = "image/jpeg";
                if (file.Name.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) mime = "image/jpeg";
                if (file.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) mime = "image/png";
                if (file.Name.EndsWith(".cb5", StringComparison.OrdinalIgnoreCase)) mime = "text/xml";
                using (Stream s = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                {
                    DependencyService.Get<IDocumentViewer>().ShowDocumentFile(file.Name, s, mime);
                }
            }
            else if (mi.CommandParameter is IFolder f)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    using (ZipOutputStream z = new ZipOutputStream(mem))
                    {
                        z.IsStreamOwner = false;
                        await PackFolder(f, z).ConfigureAwait(false);
                    }
                    mem.Seek(0, SeekOrigin.Begin);
                    DependencyService.Get<IDocumentViewer>().ShowDocumentFile(f.Name + ".zip", mem, "application/zip");
                }
            }
        }

        private async Task PackFolder(IFolder source, ZipOutputStream z, string path = null)
        {
            foreach (IFolder f in await source.GetFoldersAsync().ConfigureAwait(false))
            {
                if (f.Name.StartsWith(".")) continue;
                await PackFolder(f, z, (path != null && path.Length > 0 ? "/" : "") + f.Name);
            }
            foreach (IFile f in await source.GetFilesAsync().ConfigureAwait(false))
            {
                if (f.Name.StartsWith(".")) continue;
                using (Stream s = await f.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                {
                    ZipEntry entry = new ZipEntry((path != null && path.Length > 0 ? "/" : "") + f.Name)
                    {
                        DateTime = DateTime.Now,
                        Size = s.Length
                    };
                    z.PutNextEntry(entry);
                    byte[] buffer = new byte[4096];
                    s.CopyTo(z);
                    z.CloseEntry();
                }
            }
        }
    }
}