using System;
using System.Diagnostics;
using System.Threading.Tasks;

using CB_5e.Helpers;
using CB_5e.Models;
using CB_5e.Views;

using Xamarin.Forms;
using CB_5e.Services;
using OGL;
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
        public ObservableRangeCollection<Models.Character> Items { get; set; }
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
            
            if (search == null || search == "") Items.ReplaceRange(items);
            else
            {
                Items.ReplaceRange(from c in items where culture.CompareInfo.IndexOf(c.Text ?? "", search, CompareOptions.IgnoreCase) >= 0 || culture.CompareInfo.IndexOf(c.Description ?? "", search, CompareOptions.IgnoreCase) >= 0 select c) ;
            }
        }
        private CharactersViewModel()
        {
            Title = "Characters";
            Items = new ObservableRangeCollection<Models.Character>();
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
                ConfigManager config = await TinyContext.LoadConfigAsync(new FileInfo(Path.Combine(PCLSourceManager.Data.FullName, "Config.xml"))).ConfigureAwait(false);
                await TinyContext.LoadLevelAsync(new FileInfo(Path.Combine(PCLSourceManager.Data.FullName, config.Levels))).ConfigureAwait(false);
                await TinyContext.ImportClassesAsync(false).ConfigureAwait(true);
                await TinyContext.ImportSubClassesAsync(false).ConfigureAwait(true);
                DirectoryInfo characters = new DirectoryInfo(Path.Combine(App.Storage.FullName, "Characters"));
                if (!characters.Exists) 
                {
                    characters.Create();
                    string example = Path.Combine(characters.FullName, "Ex Ample.cb5");
                    var assembly = typeof(PCLSourceManager).GetTypeInfo().Assembly;
                    using (Stream stream = assembly.GetManifestResourceStream("CB_5e.Common.Ex Ample.cb5"))
                    {
                        using (Stream ex = new FileStream(example, FileMode.OpenOrCreate))
                        {
                            await stream.CopyToAsync(ex);
                        }
                    }
                }
                items.Clear();
                foreach (FileInfo c in characters.GetFiles())
                {
                    if (!c.Name.EndsWith(".cb5", StringComparison.OrdinalIgnoreCase)) continue;
                    try
                    {
                        Player p = await TinyContext.LoadPlayerAsync(c);
                        items.Add(new Models.Character(p));
                        //{
                        //    Player = p
                        //});
                    } catch (Exception e)
                    {
                        ConfigManager.LogError(e);
                    }
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
    }
};