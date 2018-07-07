using CB_5e.Helpers;
using CB_5e.Services;
using CB_5e.ViewModels;
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
    public partial class CompendiumMonstersPage : ContentPage
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        private OGLContext Context = new OGLContext();
        public CompendiumMonstersPage()
        {
            InitializeComponent();
            Refresh = new Command(async () =>
            {
                Entries.ReplaceRange(new List<Monster>());
                if (IsBusy) return;
                IsBusy = true;
                await PCLSourceManager.InitAsync();
                ConfigManager config = await Context.LoadConfigAsync(await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false)).ConfigureAwait(false);
                DependencyService.Get<IHTMLService>().Reset(config);
                await PCLImport.ImportMonstersAsync(Context);
                UpdateEntries();
                IsBusy = false;
            });
            Title = "Monsters";
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
            Entries.ReplaceRange(new List<Monster>());
            Entries.ReplaceRange(from r in Context.Monsters.Values where search == null 
                                 || search == "" 
                                 || r.Matches(search, false)
                                 orderby r.Name, r.Source select r);
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
        }

        public ObservableRangeCollection<Monster> Entries { get; set; } = new ObservableRangeCollection<Monster>();

        protected override void OnAppearing()
        {
            if (Entries.Count == 0) Refresh.Execute(null);
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Monster obj)
            {
                if (IsBusy) return;
                IsBusy = true;
                await Navigation.PushModalAsync(MakePage(new MonsterEditModel(obj, Context)));
                Entries.Clear();
                IsBusy = false;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PushModalAsync(MakePage(new MonsterEditModel(new Monster() { Source = Context.Config.DefaultSource }, Context)));
            Entries.Clear();
            IsBusy = false;
        }

        private Page MakePage(MonsterEditModel m)
        {
            TabbedPage t = new TabbedPage();
            t.Children.Add(new NavigationPage(new EditCommonFlavor(m)) { Title = "Edit", Icon = Device.RuntimePlatform == Device.iOS ? "save.png" : null });
            t.Children.Add(new NavigationPage(new EditMonster(m)) { Title = "Monster", Icon = Device.RuntimePlatform == Device.iOS ? "happy.png" : null });
            t.Children.Add(new NavigationPage(new DescriptionListPage(m, "Descriptions")) { Title = "Descriptions", Icon = Device.RuntimePlatform == Device.iOS ? "list.png" : null });
            t.Children.Add(new NavigationPage(new KeywordListPage(m, "Keywords", "Type", KeywordListPage.KeywordGroup.MONSTER)) { Title = "Type", Icon = Device.RuntimePlatform == Device.iOS ? "face_ID.png" : null });
            t.Children.Add(new NavigationPage(new StringListPage(m, "Speed")) { Title = "Speed", Icon = Device.RuntimePlatform == Device.iOS ? "stopwatch.png" : null });
            t.Children.Add(new NavigationPage(new StringListPage(m, "Resistances")) { Title = "Resistances", Icon = Device.RuntimePlatform == Device.iOS ? "minus.png" : null });
            t.Children.Add(new NavigationPage(new StringListPage(m, "Vulnerabilities")) { Title = "Vulnerabilities", Icon = Device.RuntimePlatform == Device.iOS ? "plus.png" : null });
            t.Children.Add(new NavigationPage(new StringListPage(m, "Immunities")) { Title = "Immunities", Icon = Device.RuntimePlatform == Device.iOS ? "cancel.png" : null });
            t.Children.Add(new NavigationPage(new StringListPage(m, "ConditionImmunities")) { Title = "Condition Immunities", Icon = Device.RuntimePlatform == Device.iOS ? "attention.png" : null });
            t.Children.Add(new NavigationPage(new StringListPage(m, "Senses")) { Title = "Senses", Icon = Device.RuntimePlatform == Device.iOS ? "compass.png" : null });
            t.Children.Add(new NavigationPage(new StringListPage(m, "Languages", GetLangAsync(m.Context))) { Title = "Languages", Icon = Device.RuntimePlatform == Device.iOS ? "chat.png" : null });
            t.Children.Add(new NavigationPage(new MonsterSaveListPage(m, "SaveBonus")) { Title = "Save Prof.", Icon = Device.RuntimePlatform == Device.iOS ? "heart.png" : null });
            t.Children.Add(new NavigationPage(new MonsterSkillListPage(m, "SkillBonus")) { Title = "Skill Prof.", Icon = Device.RuntimePlatform == Device.iOS ? "graduation_cap.png" : null });
            t.Children.Add(new NavigationPage(new MonsterTraitListPage(m, "Traits", false)) { Title = "Traits", Icon = Device.RuntimePlatform == Device.iOS ? "medical_ID.png" : null });
            t.Children.Add(new NavigationPage(new MonsterTraitListPage(m, "Actions")) { Title = "Actions", Icon = Device.RuntimePlatform == Device.iOS ? "info.png" : null });
            t.Children.Add(new NavigationPage(new MonsterTraitListPage(m, "Reactions")) { Title = "Reactions", Icon = Device.RuntimePlatform == Device.iOS ? "help.png" : null });
            t.Children.Add(new NavigationPage(new MonsterTraitListPage(m, "LegendaryActions")) { Title = "Legendary Actions", Icon = Device.RuntimePlatform == Device.iOS ? "okay.png" : null });
            t.Children.Add(new NavigationPage(new StringListPage(m, "Spells", GetSpellsAsync(m.Context))) { Title = "Spells", Icon = Device.RuntimePlatform == Device.iOS ? "moon_symbol.png" : null });
            t.Children.Add(new NavigationPage(new IntListPage(m, "Slots")) { Title = "Spell Slots", Icon = Device.RuntimePlatform == Device.iOS ? "health_data.png" : null });
            return t;
        }

        public static async Task<IEnumerable<string>> GetLangAsync(OGLContext context)
        {
            if (context.LanguagesSimple.Count == 0)
            {
                await context.ImportLanguagesAsync();
            }
            return context.LanguagesSimple.Keys.OrderBy(s => s);
        }

        public static async Task<IEnumerable<string>> GetSpellsAsync(OGLContext context)
        {
            if (context.SpellsSimple.Count == 0)
            {
                await context.ImportSpellsAsync();
            }
            return context.SpellsSimple.Keys.OrderBy(s => s);
        }
    }
}