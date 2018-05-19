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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompendiumSubClassPage : ContentPage
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        private OGLContext Context = new OGLContext();
        public CompendiumSubClassPage()
        {
            InitializeComponent();
            Refresh = new Command(async () =>
            {
                Entries.ReplaceRange(new List<SubClass>());
                if (IsBusy) return;
                IsBusy = true;
                await PCLSourceManager.InitAsync();
                ConfigManager config = await Context.LoadConfigAsync(new FileInfo(Path.Combine(PCLSourceManager.Data.FullName, "Config.xml"))).ConfigureAwait(false);
                DependencyService.Get<IHTMLService>().Reset(config);
                await PCLImport.ImportSubClassesAsync(Context);
                UpdateEntries();
                await PCLImport.ImportClassesAsync(Context);
                IsBusy = false;
            });
            Title = "Subclasses";
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
            Entries.ReplaceRange(new List<SubClass>());
            Entries.ReplaceRange(from r in Context.SubClasses.Values where search == null 
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

        public ObservableRangeCollection<SubClass> Entries { get; set; } = new ObservableRangeCollection<SubClass>();

        protected override void OnAppearing()
        {
            if (Entries.Count == 0) Refresh.Execute(null);
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is SubClass obj)
            {
                if (IsBusy) return;
                IsBusy = true;
                await Navigation.PushModalAsync(MakePage(new SubClassEditModel(obj, Context)));
                Entries.Clear();
                IsBusy = false;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PushModalAsync(MakePage(new SubClassEditModel(new SubClass() { Source = Context.Config.DefaultSource }, Context)));
            Entries.Clear();
            IsBusy = false;
        }

        private Page MakePage(SubClassEditModel m)
        {
            TabbedPage t = new TabbedPage();
            t.Children.Add(new NavigationPage(new EditSubClass(m)) { Title = "Edit" });
            t.Children.Add(new NavigationPage(new DescriptionListPage(m, "Descriptions")) { Title = "Descriptions" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "Features")) { Title = "Features" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "FirstClassFeatures")) { Title = "1st Class Features" });
            t.Children.Add(new NavigationPage(new IntListPage(m, "MulticlassingSpellLevels", "Level ", "0 'levels'", Keyboard.Numeric)) { Title = "Multiclassing" });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "MulticlassingFeatures")) { Title = "2nd Class Features" });
            return t;
        }
    }
}