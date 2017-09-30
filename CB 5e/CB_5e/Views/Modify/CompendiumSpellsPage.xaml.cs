using CB_5e.Helpers;
using CB_5e.Services;
using CB_5e.ViewModels.Modify;
using CB_5e.Views.Modify.Collections;
using CB_5e.Views.Modify.Descriptions;
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
    public partial class CompendiumSpellsPage : ContentPage
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        private OGLContext Context = new OGLContext();
        public CompendiumSpellsPage()
        {
            InitializeComponent();
            Refresh = new Command(async () =>
            {
                Entries.ReplaceRange(new List<Spell>());
                if (IsBusy) return;
                IsBusy = true;
                await PCLSourceManager.InitAsync();
                ConfigManager config = await Context.LoadConfigAsync(await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false)).ConfigureAwait(false);
                DependencyService.Get<IHTMLService>().Reset(config);
                await PCLImport.ImportSpellsAsync(Context);
                UpdateEntries();
                IsBusy = false;
            });
            Title = "Spells";
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
            Entries.ReplaceRange(new List<Spell>());
            Entries.ReplaceRange(from r in Context.Spells.Values where search == null 
                                 || search == "" 
                                 || Culture.CompareInfo.IndexOf(r.Name ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 || Culture.CompareInfo.IndexOf(r.Source ?? "", search, CompareOptions.IgnoreCase) >= 0 
                                 || Culture.CompareInfo.IndexOf(r.Description ?? "", search, CompareOptions.IgnoreCase) >= 0
                                 || r.GetKeywords().Exists(k => Culture.CompareInfo.IndexOf(k.Name ?? "", search, CompareOptions.IgnoreCase) >= 0)
                                 || r.Descriptions.Exists(k => Culture.CompareInfo.IndexOf(k.Text ?? "", search, CompareOptions.IgnoreCase) >= 0)
                                 orderby r.Name, r.Source select r);
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
        }

        public ObservableRangeCollection<Spell> Entries { get; set; } = new ObservableRangeCollection<Spell>();

        protected override void OnAppearing()
        {
            if (Entries.Count == 0) Refresh.Execute(null);
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Spell obj)
            {
                if (IsBusy) return;
                IsBusy = true;
                await Navigation.PushModalAsync(MakePage(new SpellEditModel(obj, Context)));
                Entries.Clear();
                IsBusy = false;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PushModalAsync(MakePage(new SpellEditModel(new Spell() { Source = Context.Config.DefaultSource, Level = 1 }, Context)));
            Entries.Clear();
            IsBusy = false;
        }

        private Page MakePage(SpellEditModel m)
        {
            TabbedPage t = new TabbedPage();
            t.Children.Add(new NavigationPage(new EditSpell(m)) { Title = "Edit" });
            t.Children.Add(new NavigationPage(new DescriptionListPage(m, "Descriptions")) { Title = "Descriptions" });
            t.Children.Add(new NavigationPage(new EditCantripDamage(m)) { Title = "Spell" });
            t.Children.Add(new NavigationPage(new KeywordListPage(m, "Keywords", "Spell Keywords", KeywordListPage.KeywordGroup.SPELL, LoadClassesAsync(m.Context))) { Title = "Keywords" });
            return t;
        }

        private static async Task<IEnumerable<string>> LoadClassesAsync(OGLContext context)
        {
            if (context.ClassesSimple.Count == 0)
            {
                await context.ImportClassesAsync();
            }
            return context.ClassesSimple.Keys.OrderBy(s => s);
        }
    }
}