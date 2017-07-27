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

namespace CB_5e.ViewModels
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

        private List<Character> items = new List<Character>();
        public ObservableRangeCollection<Character> Items { get; set; }
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
                Items.ReplaceRange(from c in items where culture.CompareInfo.IndexOf(c.Text, search, CompareOptions.IgnoreCase) >= 0 || culture.CompareInfo.IndexOf(c.Description, search, CompareOptions.IgnoreCase) >= 0 select c) ;
            }
        }
        private CharactersViewModel()
        {
            Title = "Characters";
            Items = new ObservableRangeCollection<Character>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewFolderPage, Character>(this, "AddCharacter", (obj, item) =>
            {
                var _item = item as Character;
                Items.Add(_item);
            });
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
                IFolder characters = await App.Storage.CreateFolderAsync("Characters", CreationCollisionOption.OpenIfExists);
                items.Clear();
                foreach (IFile c in await characters.GetFilesAsync().ConfigureAwait(false))
                {
                    if (!c.Name.EndsWith(".cb5", StringComparison.OrdinalIgnoreCase)) continue;
                    try
                    {
                        Player p = await TinyContext.LoadPlayerAsync(c);
                        items.Add(new Character(p));
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