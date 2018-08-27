using System;
using System.Diagnostics;
using System.Threading.Tasks;

using CB_5e.Helpers;
using CB_5e.Models;
using CB_5e.Views;

using Xamarin.Forms;
using CB_5e.Services;
using OGL;
using PCLStorage;
using Character_Builder;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace CB_5e.ViewModels.Character
{
    public class CharactersViewModel : BaseViewModel
    {
        private static CultureInfo culture = CultureInfo.InvariantCulture;
        private static CharactersViewModel instance = null;
        public static CharactersViewModel Instance {
            get {
                if (instance == null) instance = new CharactersViewModel();
                return instance;
            }
        }

        private List<Models.Character> items = new List<Models.Character>();
        public ObservableRangeCollection<CharGroupModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        private string search;
        public string Search {
            get { return search; }
            set
            {
                search = value;
                UpdateItems();
            }
        }

        private void UpdateItems()
        {
            
            if (search == null || search == "") Items.ReplaceRange(from c in items orderby c.Text group c by c.Folder into g select new CharGroupModel(g.FirstOrDefault()?.Folder ?? "--", g) into cc orderby cc.Folder select cc);
            else
            {
                Items.ReplaceRange(from c in items where culture.CompareInfo.IndexOf(c.Text ?? "", search, CompareOptions.IgnoreCase) >= 0 || culture.CompareInfo.IndexOf(c.Description ?? "", search, CompareOptions.IgnoreCase) >= 0 orderby c.Text group c by c.Folder into g select new CharGroupModel(g.FirstOrDefault()?.Folder ?? "--", g) into cc orderby cc.Folder select cc);
            }
        }
        private CharactersViewModel()
        {
            Title = "Characters";
            Items = new ObservableRangeCollection<CharGroupModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                await PCLSourceManager.InitAsync().ConfigureAwait(false);
                BuilderContext TinyContext = new BuilderContext();
                ConfigManager config = await TinyContext.LoadConfigAsync(await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false)).ConfigureAwait(false);
                await TinyContext.LoadLevelAsync(await PCLSourceManager.Data.GetFileAsync(config.Levels).ConfigureAwait(false)).ConfigureAwait(false);
                await TinyContext.ImportClassesAsync(false).ConfigureAwait(true);
                await TinyContext.ImportSubClassesAsync(false).ConfigureAwait(true);
                ExistenceCheckResult res = await App.Storage.CheckExistsAsync("Characters");
                IFolder characters = await App.Storage.CreateFolderAsync("Characters", CreationCollisionOption.OpenIfExists);
                if (res == ExistenceCheckResult.NotFound)
                {
                    IFile example = await characters.CreateFileAsync("Ex Ample.cb5", CreationCollisionOption.ReplaceExisting);
                    var assembly = typeof(PCLSourceManager).GetTypeInfo().Assembly;
                    using (Stream stream = assembly.GetManifestResourceStream("CB_5e.Ex Ample.cb5"))
                    {
                        using (Stream ex = await example.OpenAsync(FileAccess.ReadAndWrite))
                        {
                            await stream.CopyToAsync(ex);
                        }
                    }
                }
                items.Clear();
                foreach (IFile c in await characters.GetFilesAsync().ConfigureAwait(false))
                {
                    if (!c.Name.EndsWith(".cb5", StringComparison.OrdinalIgnoreCase)) continue;
                    try
                    {
                        Player p = await TinyContext.LoadPlayerAsync(c);
                        items.Add(new Models.Character(p, "--"));
                        //{
                        //    Player = p
                        //});
                    } catch (Exception e)
                    {
                        ConfigManager.LogError(e);
                    }
                }
                foreach (IFolder c in await characters.GetFoldersAsync().ConfigureAwait(false))
                {
                    await Add(c, items, c.Name, TinyContext);
                }
                UpdateItems();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task Add(IFolder f, List<Models.Character> it, String Folder, BuilderContext TinyContext)
        {
            foreach (IFile c in await f.GetFilesAsync().ConfigureAwait(false))
            {
                if (!c.Name.EndsWith(".cb5", StringComparison.OrdinalIgnoreCase)) continue;
                try
                {
                    Player p = await TinyContext.LoadPlayerAsync(c);
                    items.Add(new Models.Character(p, Folder));
                    //{
                    //    Player = p
                    //});
                }
                catch (Exception e)
                {
                    ConfigManager.LogError(e);
                }
            }
            foreach(IFolder c in await f.GetFoldersAsync().ConfigureAwait(false))
            {
                await Add(c, it, Folder + "/" + c.Name, TinyContext);
            }
        }
    }

    
};