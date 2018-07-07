using CB_5e.Helpers;
using CB_5e.Services;
using CB_5e.ViewModels;
using CB_5e.ViewModels.Modify;
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
    public partial class CompendiumSubRacesPage : ContentPage
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        private OGLContext Context = new OGLContext();
        public CompendiumSubRacesPage()
        {
            InitializeComponent();
            Refresh = new Command(async () =>
            {
                Entries.ReplaceRange(new List<SubRace>());
                if (IsBusy) return;
                IsBusy = true;
                await PCLSourceManager.InitAsync();
                ConfigManager config = await Context.LoadConfigAsync(await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false)).ConfigureAwait(false);
                DependencyService.Get<IHTMLService>().Reset(config);
                await PCLImport.ImportSubRacesAsync(Context);
                UpdateEntries();
                await PCLImport.ImportRacesAsync(Context);
                IsBusy = false;
            });
            Title = "Subraces";
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
            Entries.ReplaceRange(new List<SubRace>());
            Entries.ReplaceRange(from r in Context.SubRaces.Values where search == null 
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

        public ObservableRangeCollection<SubRace> Entries { get; set; } = new ObservableRangeCollection<SubRace>();

        protected override void OnAppearing()
        {
            if (Entries.Count == 0)
            {
                Refresh.Execute(null);
            }

        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is SubRace obj)
            {
                if (IsBusy)
                {
                    (sender as ListView).SelectedItem = null;
                    return;
                }
                IsBusy = true;
                if (Context.RacesSimple.Count == 0)
                {
                    await Context.ImportRacesAsync().ConfigureAwait(true);
                }
                await Navigation.PushModalAsync(MakePage(new SubRaceEditModel(obj, Context)));
                Entries.Clear();
                IsBusy = false;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (Context.RacesSimple.Count == 0)
            {
                await Context.ImportRacesAsync().ConfigureAwait(true);
            }
            await Navigation.PushModalAsync(MakePage(new SubRaceEditModel(new SubRace() { Source = Context.Config.DefaultSource }, Context)));
            Entries.Clear();
            IsBusy = false;
        }

        private Page MakePage(SubRaceEditModel m)
        {
            TabbedPage t = new TabbedPage();
            t.Children.Add(new NavigationPage(new EditSubRace(m)) { Title = "Edit", Icon = Device.RuntimePlatform == Device.iOS ? "save.png" : null });
            t.Children.Add(new NavigationPage(new DescriptionListPage(m, "Descriptions")) { Title = "Descriptions", Icon = Device.RuntimePlatform == Device.iOS ? "list.png" : null });
            t.Children.Add(new NavigationPage(new FeatureListPage(m, "Features")) { Title = "Features", Icon = Device.RuntimePlatform == Device.iOS ? "wallet_app.png" : null });
            return t;
        }
    }
}