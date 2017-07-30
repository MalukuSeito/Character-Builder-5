using CB_5e.Helpers;
using CB_5e.Views;
using Character_Builder;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class ShopViewModel
    {
        public PlayerViewModel Model { get; private set; }

        public ShopViewModel(PlayerViewModel playerViewModel)
        {
            Model = playerViewModel;
            Select = new Command(async (par) =>
            {
                if (par is Item i)
                {
                    await Model.ShopNavigation.PushAsync(new BuyAddPage(this, i));
                }
                else if (par is MagicProperty mp)
                {
                    if (mp.Base == null || mp.Base == "")
                    {
                        Model.MakeHistory();
                        Model.Context.Player.Possessions.Add(new Possession(Model.Context, (Item)null, mp));
                        Model.Save();
                        Model.FirePlayerChanged();
                        await Model.ShopNavigation.PopAsync();
                    }
                    else
                    {
                        await Model.ShopNavigation.PushAsync(new AddToItemPage(this, mp));
                    }
                }
                else if (par is Feature f)
                {
                    if (!Model.Context.Player.Boons.Exists(b => ConfigManager.SourceInvariantComparer.Equals(b, f.Name + " " + ConfigManager.SourceSeperator + " " + f.Source)))
                    {
                        Model.MakeHistory();
                        Model.Context.Player.Boons.Add(f.Name + " " + ConfigManager.SourceSeperator + " " + f.Source);
                        Model.Save();
                        Model.FirePlayerChanged();
                        Select.ChangeCanExecute();
                        await Model.ShopNavigation.PopAsync();
                    }
                }
                else if (par is Spell s)
                {
                    if (Name == "Spell Scrolls")
                    {
                        Model.MakeHistory();
                        Model.Context.Player.Items.Add(s.Name + " " + ConfigManager.SourceSeperator + " " + s.Source);
                        Model.Save();
                        Model.RefreshItems.Execute(null);
                        await Model.ShopNavigation.PopAsync();
                    }
                    else
                    {
                        await Model.ShopNavigation.PushAsync(new AddToSpellbookPage(this, s));
                    }
                }
            }, (par) =>
            {
                if (par is Feature f)
                {
                    return !Model.Context.Player.Boons.Exists(b => ConfigManager.SourceInvariantComparer.Equals(b, f.Name + " " + ConfigManager.SourceSeperator + " " + f.Source));
                }
                else if (par is Spell s)
                {
                    if (Name != "Spell Scrolls")
                    {
                        return (from sc
                        in Model.Context.Player.GetFeatures()
                                where sc is SpellcastingFeature
                                && ((SpellcastingFeature)sc).Preparation == PreparationMode.Spellbook
                                && Utils.Matches(Model.Context, s, ((SpellcastingFeature)sc).PrepareableSpells, ((SpellcastingFeature)sc).SpellcastingID) && !Model.Context.Player.GetSpellcasting(((SpellcastingFeature)sc).SpellcastingID).GetSpellbook(Model.Context.Player, Model.Context).Contains(s)
                                select ((SpellcastingFeature)sc)).Count() > 0;
                    }
                }
                return true;
            });
        }

        public string Name { get; set; }
        public Category ItemCategory { get; set; }
        public MagicCategory MagicCategory { get; set; }
        private string type;
        public string Type { get => type; set => type = value; }
        public Command Open { get; set; }
        public string SType { get => Type.Substring(0, 1); }
        public Command Select { get; set; }

        public string CommandName { get => Type == "Items" ? "Add/Buy" : "Add"; }

        public string ShopSearch
        {
            get => Model.ShopSearch;
            set
            {
                Model.ShopSearch = value;
                UpdateShop();
            }
        }
        public ObservableRangeCollection<IXML> Shop { get; set; } = new ObservableRangeCollection<IXML>();
        public void UpdateShop()
        {
            if (Type == "Items") Shop.ReplaceRange(Model.Context.Subsection(ItemCategory));
            else if (Type == "Magic") Shop.ReplaceRange(MagicCategory.SubSection(Model.Context.Search));
            else if (Type == "Spells") Shop.ReplaceRange(Model.Context.SpellSubsection());
            else if (Model.Context.FeatureCategories.ContainsKey(Name)) Shop.ReplaceRange(Model.Context.FeatureSubsection(Name));
            else Shop.ReplaceRange(new List<IXML>());
        }

        public string Money { get => Model.Money; }
    }
}
