using CB_5e.Helpers;
using CB_5e.Services;
using CB_5e.ViewModels.Modify;
using CB_5e.Views.Modify.Collections;
using CB_5e.Views.Modify.Descriptions;
using CB_5e.Views.Modify.Features;
using OGL;
using OGL.Common;
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
    public partial class CompendiumClassesPage : ContentPage
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        private OGLContext Context = new OGLContext();
        public CompendiumClassesPage()
        {
            InitializeComponent();
            Refresh = new Command(async () =>
            {
                Entries.ReplaceRange(new List<ClassDefinition>());
                if (IsBusy) return;
                IsBusy = true;
                await PCLSourceManager.InitAsync();
                ConfigManager config = await Context.LoadConfigAsync(await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false)).ConfigureAwait(false);
                DependencyService.Get<IHTMLService>().Reset(config);
                await PCLImport.ImportClassesAsync(Context);
                UpdateEntries();
                IsBusy = false;
            });
            Title = "Classes";
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
            Entries.ReplaceRange(new List<ClassDefinition>());
            Entries.ReplaceRange(from r in Context.Classes.Values where search == null 
                                 || search == "" 
                                 || Culture.CompareInfo.IndexOf(r.Name ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 || Culture.CompareInfo.IndexOf(r.Source ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 || Culture.CompareInfo.IndexOf(r.Description ?? "", search, CompareOptions.IgnoreCase) >= 0
                                 || r.Descriptions.Exists(k => Culture.CompareInfo.IndexOf(k.Text ?? "", search, CompareOptions.IgnoreCase) >= 0)
                                 orderby r.Name, r.Source select r);
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
        }

        public ObservableRangeCollection<ClassDefinition> Entries { get; set; } = new ObservableRangeCollection<ClassDefinition>();

        protected override void OnAppearing()
        {
            if (Entries.Count == 0) Refresh.Execute(null);
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is ClassDefinition obj)
            {
                if (IsBusy) return;
                IsBusy = true;
                await Navigation.PushModalAsync(MakePage(new ClassEditModel(obj, Context)));
                Entries.Clear();
                IsBusy = false;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PushModalAsync(MakePage(new ClassEditModel(new ClassDefinition() { Source = Context.Config.DefaultSource, MulticlassingCondition = "false" }, Context)));
            Entries.Clear();
            IsBusy = false;
        }

        private Page MakePage(ClassEditModel m)
        {
            TabbedPage t = new TabbedPage();
            t.Children.Add(new NavigationPage(new EditCommonFlavor(m)) { Title = "Edit" });
            t.Children.Add(new NavigationPage(new EditHitDie(m)) { Title = "HD" });
            t.Children.Add(new NavigationPage(new DescriptionListPage(m, "Descriptions")) { Title = "Descriptions" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "Features")) { Title = "Features" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "FirstClassFeatures")) { Title = "1st Class Features" });
            t.Children.Add(new NavigationPage(new EditMulticlassing(m)) { Title = "Multiclassing" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "MulticlassingFeatures")) { Title = "2nd Class Features" });
            t.Children.Add(new NavigationPage(new StringListPage(m, "SpellsToAddClassKeywordTo", GetSpellsAsync(m.Context))) { Title = "Compatibility Spells" });
            t.Children.Add(new NavigationPage(new StringListPage(m, "FeaturesToAddClassKeywordTo", GetFeaturesAsync(m.Context))) { Title = "Compatibility Features" });
            return t;
        }

        public static async Task<IEnumerable<string>> GetSpellsAsync(OGLContext context)
        {
            if (context.SpellsSimple.Count == 0)
            {
                await context.ImportSpellsAsync();
            }
            return context.SpellsSimple.Keys.OrderBy(s => s);
        }

        public static async Task<IEnumerable<string>> GetFeaturesAsync(OGLContext context)
        {
            if (context.Features.Count == 0)
            {
                await context.ImportStandaloneFeaturesAsync();
            }
            return context.Features.Select(s=>s.Name).OrderBy(s => s).Distinct();
        }
    }
}