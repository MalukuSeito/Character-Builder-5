using CB_5e.Helpers;
using CB_5e.Services;
using CB_5e.ViewModels.Modify.Items;
using CB_5e.Views.Modify.Collections;
using CB_5e.Views.Modify.Items;
using OGL;
using OGL.Common;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompendiumItemsPage : ContentPage
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        private OGLContext Context = new OGLContext();
        private Category category = null;
        public CompendiumItemsPage(string cat)
        {
            InitializeComponent();
            Refresh = new Command(async () =>
            {
                Entries.ReplaceRange(new List<Item>());
                if (IsBusy) return;
                IsBusy = true;
                await PCLSourceManager.InitAsync();
                ConfigManager config = await Context.LoadConfigAsync(await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false)).ConfigureAwait(false);
                DependencyService.Get<IHTMLService>().Reset(config);
                category = PCLImport.Make(Context, cat);
                await PCLImport.ImportItemsAsync(Context);
                UpdateEntries();
                IsBusy = false;
            });
            Title = "Items";
            BindingContext = this;
        }

        public Command Refresh { get; private set; }

        private string search;

        public string Search {
            get => search;
            set
            {
                search = value;
                OnPropertyChanged("Search");
                if (!IsBusy) UpdateEntries();
            }
        }


        public void UpdateEntries()
        {
            Entries.ReplaceRange(new List<Item>());
            Entries.ReplaceRange(from r in Context.Items.Values
                                 where (category == null || category.Equals(r.Category)) && (search == null 
                                 || search == "" 
                                 || Culture.CompareInfo.IndexOf(r.Name ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 || Culture.CompareInfo.IndexOf(r.Source ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 || Culture.CompareInfo.IndexOf(r.Description ?? "", search, CompareOptions.IgnoreCase) >= 0)
                                 orderby r.Name, r.Source select r);
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
        }

        public ObservableRangeCollection<Item> Entries { get; set; } = new ObservableRangeCollection<Item>();

        protected override void OnAppearing()
        {
            if (Entries.Count == 0) Refresh.Execute(null);
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Item obj)
            {
                if (IsBusy) return;
                IsBusy = true;
                await Navigation.PushModalAsync(MakePage(new ItemEditModel<Item>(obj, Context)));
                Entries.Clear();
                IsBusy = false;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            //MakePage(new ItemEditModel<Item>(new Item() { Source = Context.Config.DefaultSource, Category = category }, Context))
            await Navigation.PushAsync(new SelectPage(new List<SelectOption>() {
                new SelectOption("Item", "A simple item", new Item() { Source = Context.Config.DefaultSource, Category = category }),
                new SelectOption("Tool", "An item that is also a tool", new Tool() { Source = Context.Config.DefaultSource, Category = category }),
                new SelectOption("Weapon", "A weapon (also counts as a tool)", new Weapon() { Source = Context.Config.DefaultSource, Category = category }),
                new SelectOption("Armor", "An armor (also counts as a tool)", new Armor() { Source = Context.Config.DefaultSource, Category = category }),
                new SelectOption("Shield", "A shield (also counts as a tool)", new Shield() { Source = Context.Config.DefaultSource, Category = category }),
                new SelectOption("Pack", "A pack of items (can be unpacked)", new Pack() { Source = Context.Config.DefaultSource, Category = category }),
            }, new Command(async (par) => {
                await ShowAsync(par);
            })));
            Entries.Clear();
            IsBusy = false;
        }
        private async Task ShowAsync(object par)
        {
            if (par is SelectOption s)
            {
                if (s.Value is Item i)
                {
                    await Navigation.PushModalAsync(MakePage(new ItemEditModel<Item>(i, Context)));
                }
            }
        }
        private Page MakePage(IItemEditModel m)
        {
            TabbedPage t = new TabbedPage();
            t.Children.Add(new NavigationPage(new EditCommon(m)) { Title = "Edit" });
            t.Children.Add(new NavigationPage(new EditItem(m)) { Title = "Item" });
            t.Children.Add(new NavigationPage(new KeywordListPage(m, "Keywords", "Item Keywords", KeywordListPage.KeywordGroup.ITEM, null)) { Title = "Keywords" });
            return t;
        }
    }
}