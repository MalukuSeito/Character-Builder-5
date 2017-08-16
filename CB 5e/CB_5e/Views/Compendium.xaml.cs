using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Compendium : ContentPage
    {
        public List<string> Items { get; set; } = new List<string>()
        {
            "Races", "Subraces",
            "Classes", "Subclasses",
            "Backgrounds",
            "Standalone Features",
            "Spells",
            "Skills",
            "Languages",
            "Items",
            "Magic Items",
            "Conditions",
            "Level",
            "Ability Scores",
            "Settings"
        };
        public Compendium()
        {
            Title = "Compendium";
            InitializeComponent();
            BindingContext = this;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is string s)
            {
                if (s == "Races") await Navigation.PushAsync(new CompendiumRacesPage());
                else if (s == "Subraces") await Navigation.PushAsync(new CompendiumSubRacesPage());
                else if (s == "Classes") await Navigation.PushAsync(new CompendiumClassesPage());
                else if (s == "Subclasses") await Navigation.PushAsync(new CompendiumSubClassPage());
                else if (s == "Skills") await Navigation.PushAsync(new CompendiumSkillsPage());
                else if (s == "Backgrounds") await Navigation.PushAsync(new CompendiumBackgroundsPage());
                else if (s == "Conditions") await Navigation.PushAsync(new CompendiumConditionsPage());
                else if (s == "Spells") await Navigation.PushAsync(new CompendiumSpellsPage());
                else if (s == "Languages") await Navigation.PushAsync(new CompendiumLanguagesPage());
                else if (s == "Items") await Navigation.PushAsync(new CompendiumItemsOverviewPage());
                else if (s == "Standalone Features") await Navigation.PushAsync(new CompendiumFeatsOverviewPage());
                else if (s == "Magic Items") await Navigation.PushAsync(new CompendiumMagicOverviewPage());
            }
            (sender as ListView).SelectedItem = null;
        }
    }
}