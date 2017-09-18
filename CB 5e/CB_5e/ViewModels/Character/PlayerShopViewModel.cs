using CB_5e.Helpers;
using CB_5e.Views;
using CB_5e.Views.Character;
using Character_Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class PlayerShopViewModel : SubModel
    {
        public PlayerShopViewModel(PlayerModel parent) : base(parent, "Shop")
        {
            OnOpenShop = new Command(async (par) =>
            {
                if (par is ShopViewModel svm) await Navigation.PushAsync(new ShopSubPage(svm));
            });
            ShowItemInfo = new Command(async (par) =>
            {
                if (par is InventoryViewModel ivm)
                {
                    if (ivm.Item is Possession p)
                    {
                        await Navigation.PushAsync(InfoPage.Show(new DisplayPossession(ivm.Item, Context.Player)));
                    }
                    else
                    {
                        await Navigation.PushAsync(InfoPage.Show(ivm.Boon));
                    }
                }
            });
            UpdateShops();
        }
        public ObservableRangeCollection<ShopGroupModel> Shops { get; set; } = new ObservableRangeCollection<ShopGroupModel>();
        public string ShopSearch
        {
            get => Context.Search;
            set
            {
                SetProperty(ref Context.Search, value);
                UpdateShops();
            }
        }
        public Command OnOpenShop { get; private set; }
        public Command ShowItemInfo { get; private set; }

        public void UpdateShops() => Shops.ReplaceRange(GetShops());

        private IEnumerable<ShopGroupModel> GetShops()
        {
            List<ShopGroupModel> res = new List<ShopGroupModel>();
            res.Add(new ShopGroupModel("Items", from i in Context.Section()
                                                where i.ToString() != ""
                                                select new ShopViewModel(this)
                                                {
                                                    Name = i.ToString(),
                                                    ItemCategory = i,
                                                    Type = "Items",
                                                    Open = OnOpenShop
                                                }));
            res.Add(new ShopGroupModel("Magic", from m in Context.MagicSection()
                                                select new ShopViewModel(this)
                                                {
                                                    Name = m.ToString(),
                                                    MagicCategory = m,
                                                    Type = "Magic",
                                                    Open = OnOpenShop
                                                }));
            if (Context.SpellSubsection().Count() > 0)
            {
                List<ShopViewModel> s = new List<ShopViewModel>() {
                    new ShopViewModel(this)
                    {
                        Name = "Spell Scrolls",
                        Type = "Spells",
                        Open = OnOpenShop

                    },
                    new ShopViewModel(this)
                    {
                        Name = "Spellbook Spells",
                        Type = "Spells",
                        Open = OnOpenShop

                    }
                };
                res.Add(new ShopGroupModel("Spells", s));
            }

            res.Add(new ShopGroupModel("Boon", from f in Context.FeatureSection()
                                               select new ShopViewModel(this)
                                               {
                                                   Name = f,
                                                   Type = "Boon",
                                                   Open = OnOpenShop
                                               }));
            return res;
        }
        public string Money
        {
            get
            {
                return Context.Player.GetMoney().ToString();
            }
        }
    }
}
