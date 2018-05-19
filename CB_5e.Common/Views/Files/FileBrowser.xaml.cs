using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CB_5e.ViewModels;
using CB_5e.Services;
using CB_5e.ViewModels.Files;

namespace CB_5e.Views.Files
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileBrowser : ContentPage
    {
        private static object clipboard = null;
        private static bool isCopy = false;
        private FilesViewModel viewModel;

        public FileBrowser(DirectoryInfo folder)
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

            if (e.SelectedItem is DirectoryInfo f)
            {
                await Navigation.PushAsync(new FileBrowser(f));
            }
            else if (e.SelectedItem is FileInfo file)
            {
                await Navigation.PushAsync(new EditFilePage(file));
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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
            if (mi.CommandParameter is FileInfo file)
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

        public static async Task<bool> Unpack(FileInfo file, DirectoryInfo folder, string name)
        {
            try
            {
                string sub = Path.Combine(folder.FullName, name);
                int i = 0;
                while (Directory.Exists(sub) || File.Exists(sub))
                {
                    sub = Path.Combine(folder.FullName, name + "(" + ++i + ")");
                }

                await Task.Run(() => ZipFile.ExtractToDirectory(file.FullName, sub));
                return true;
            } catch (Exception e) {
                return false;
            }
        }


        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            if (mi.CommandParameter is FileInfo f)
            {
                f.Delete();
                viewModel.LoadItemsCommand.Execute(null);
            }
            else if (mi.CommandParameter is DirectoryInfo file)
            {
                file.Delete();
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        public static async Task Move(DirectoryInfo source, DirectoryInfo target)
        {
            string sub = Path.Combine(target.FullName, source.Name);
            int i = 0;
            while (Directory.Exists(sub) || File.Exists(sub))
            {
                sub = Path.Combine(target.FullName, source.Name + "(" + ++i + ")");
            }
            await Task.Run(() => Directory.Move(source.FullName, sub));
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static async Task Copy(DirectoryInfo source, DirectoryInfo target)
        {
            string sub = Path.Combine(target.FullName, source.Name);
            int i = 0;
            while (Directory.Exists(sub) || File.Exists(sub))
            {
                sub = Path.Combine(target.FullName, source.Name + "(" + ++i + ")");
            }
            await Task.Run(() => DirectoryCopy(source.FullName, sub, true));
        }

        public static async Task Copy(FileInfo source, string target)
        {            
            await Task.Run(() => source.CopyTo(target));
        }

        public async void OnPaste(object sender, EventArgs e)
        {
            object clp = clipboard;
            if (!isCopy) clipboard = null;
            if (clp is DirectoryInfo f)
            {
                if (isCopy) await Copy(f, viewModel.Folder).ConfigureAwait(false);
                else await Move(f, viewModel.Folder).ConfigureAwait(false);
                viewModel.LoadItemsCommand.Execute(null);
            }
            else if (clp is FileInfo file)
            {
                string sub = Path.Combine(viewModel.Folder.FullName, file.Name);
                int i = 0;
                while (File.Exists(sub) || Directory.Exists(sub))
                {
                    sub = Path.Combine(viewModel.Folder.FullName, file.Name.Contains(".") ? file.Name.Replace(".", " (" + ++i + ").") : file.Name + " (" + ++i + ")");
                }
                if (isCopy) await Task.Run(() => file.CopyTo(sub));
                else await Task.Run(() => file.MoveTo(sub));
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
            if (mi.CommandParameter is FileInfo file)
            {
                string mime = "application/octet-stream";
                if (file.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) mime = "text/xml";
                if (file.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) mime = "image/jpeg";
                if (file.Name.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) mime = "image/jpeg";
                if (file.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) mime = "image/png";
                if (file.Name.EndsWith(".cb5", StringComparison.OrdinalIgnoreCase)) mime = "text/xml";
                using (Stream s = new FileStream(file.FullName, FileMode.Open))
                {
                    DependencyService.Get<IDocumentViewer>().ShowDocumentFile(file.Name, s, mime);
                }
            }
            else if (mi.CommandParameter is DirectoryInfo f)
            {
                using (var mem = new MemoryStream())
                {
                    using (var archive = new ZipArchive(mem, ZipArchiveMode.Create, true))
                    {
                        await PackFolder(f, archive);
                    }
                    mem.Seek(0, SeekOrigin.Begin);
                    DependencyService.Get<IDocumentViewer>().ShowDocumentFile(f.Name + ".zip", mem, "application/zip");
                }
            }
        }

        private async Task PackFolder(DirectoryInfo source, ZipArchive z, string path = null)
        {
            foreach (DirectoryInfo f in source.GetDirectories())
            {
                if (f.Name.StartsWith(".")) continue;
                await PackFolder(f, z, (path != null && path.Length > 0 ? path + "/" : "") + f.Name);
            }
            foreach (FileInfo f in source.GetFiles())
            {
                if (f.Name.StartsWith(".")) continue;
                using (Stream s = new FileStream(f.FullName, FileMode.Open))
                {
                    ZipArchiveEntry entry = z.CreateEntry((path != null && path.Length > 0 ? path + "/" : "") + f.Name, CompressionLevel.Optimal);
                    entry.LastWriteTime = DateTime.Now;
                    using (var entryStream = entry.Open()) await s.CopyToAsync(entryStream);
                }
            }
        }

    }
}