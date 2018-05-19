using CB_5e.Helpers;
using CB_5e.Services;
using CB_5e.ViewModels.Modify;
using CB_5e.Views.Modify.Features;
using OGL;
using OGL.Common;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompendiumMagicPage : ContentPage
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        private OGLContext Context = new OGLContext();
        private string category = null;
        public CompendiumMagicPage(string cat)
        {
            InitializeComponent();
            category = cat;
            Refresh = new Command(async () =>
            {
                Entries.ReplaceRange(new List<MagicProperty>());
                if (IsBusy) return;
                IsBusy = true;
                await PCLSourceManager.InitAsync();
                ConfigManager config = await Context.LoadConfigAsync(new FileInfo(Path.Combine(PCLSourceManager.Data.FullName, "Config.xml"))).ConfigureAwait(false);
                DependencyService.Get<IHTMLService>().Reset(config);
                await PCLImport.ImportMagicAsync(Context);
                UpdateEntries();
                IsBusy = false;
            });
            Title = "Magic Properties";
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
            Entries.ReplaceRange(new List<MagicProperty>());
            Entries.ReplaceRange(from r in (category != null && Context.MagicCategories.ContainsKey(category) ? Context.MagicCategories[category].Contents : Context.Magic.Values as IEnumerable<MagicProperty>)
                                 where search == null 
                                 || search == "" 
                                 || Culture.CompareInfo.IndexOf(r.Name ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 || Culture.CompareInfo.IndexOf(r.Source ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 || Culture.CompareInfo.IndexOf(r.Description ?? "", search, CompareOptions.IgnoreCase) >= 0
                                 orderby r.Name, r.Source select r);
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
        }

        public ObservableRangeCollection<MagicProperty> Entries { get; set; } = new ObservableRangeCollection<MagicProperty>();

        protected override void OnAppearing()
        {
            if (Entries.Count == 0) Refresh.Execute(null);
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MagicProperty obj)
            {
                if (IsBusy) return;
                IsBusy = true;
                await Navigation.PushModalAsync(MakePage(new MagicEditModel(obj, Context)));
                Entries.Clear();
                IsBusy = false;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PushModalAsync(MakePage(new MagicEditModel(new MagicProperty() { Source = Context.Config.DefaultSource, Category = category }, Context)));
            Entries.Clear();
            IsBusy = false;
        }

        private Page MakePage(MagicEditModel m)
        {
            TabbedPage t = new TabbedPage();
            t.Children.Add(new NavigationPage(new EditCommon(m)) { Title = "Edit" });
            t.Children.Add(new NavigationPage(new EditMagic(m, GetItemsAsync(m.Context))) { Title = "Magic Property" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "CarryFeatures")) { Title = "Carry Features" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "EquipFeatures")) { Title = "Equipped Features" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "OnUseFeatures")) { Title = "On Use Features" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "AttunementFeatures")) { Title = "Attuned Features" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "AttunedEquipFeatures")) { Title = "Attuned Equip Features" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "AttunedOnUseFeatures")) { Title = "Attuned On Use Features" });
            return t;
        }

        private async Task GetItemsAsync(OGLContext context)
        {
            if (context.Items == null || context.Items.Count == 0)
            {
                await context.ImportItemsAsync();
            }
        }
    }
}