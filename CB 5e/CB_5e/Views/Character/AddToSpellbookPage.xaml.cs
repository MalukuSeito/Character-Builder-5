using CB_5e.ViewModels;
using CB_5e.ViewModels.Character;
using Character_Builder;
using OGL;
using OGL.Base;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Character
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToSpellbookPage : ContentPage
    {
        public AddToSpellbookPage(ShopViewModel model, Spell s)
        {
            InitializeComponent();
            Model = model;
            Spell = s;
            BindingContext = this;
            Title = Spell.Name;
        }

        public Spell Spell { get; private set; }
        public ShopViewModel Model { get; private set; }

        public IEnumerable<SpellcastingFeature> Spellbooks {
            get
            { 
                return from sc 
                       in Model.Model.Context.Player.GetFeatures()
                       where sc is SpellcastingFeature 
                       && ((SpellcastingFeature)sc).Preparation == PreparationMode.Spellbook
                       && Utils.Matches(Model.Model.Context, Spell, ((SpellcastingFeature)sc).PrepareableSpells, ((SpellcastingFeature)sc).SpellcastingID) && !Model.Model.Context.Player.GetSpellcasting(((SpellcastingFeature)sc).SpellcastingID).GetSpellbook(Model.Model.Context.Player, Model.Model.Context).Contains(Spell)
                       select ((SpellcastingFeature)sc);
            }
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is SpellcastingFeature sc)
            {
                Model.Model.Context.MakeHistory("");
                Model.Model.Context.Player.GetSpellcasting(sc.SpellcastingID).GetAdditionalList(Model.Model.Context.Player, Model.Model.Context).Add(Spell.Name + " " + ConfigManager.SourceSeperator + " " + Spell.Source);
                Model.Model.Save();
                Model.Model.UpdateSpellcasting();
                Model.Select.ChangeCanExecute();
                await Navigation.PopAsync();
            }
        }
    }
}