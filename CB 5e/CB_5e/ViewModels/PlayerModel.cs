using CB_5e.Helpers;
using CB_5e.Views;
using Character_Builder;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public abstract class PlayerModel: BaseViewModel
    {
        public event EventHandler PlayerChanged;
        public void FirePlayerChanged() => PlayerChanged?.Invoke(this, EventArgs.Empty);
        public BuilderContext Context { get; private set; }
        public bool ChildModel { get; set; } = false;
        public INavigation ShopNavigation { get; set; }
        public PlayerModel(BuilderContext context)
        {
            Context = context;
            EditItem = new Command(async (par) =>
            {
                if (par is InventoryViewModel ivm)
                {
                    if (ivm.Item != null) await ShopNavigation.PushAsync(new ItemPage(new ItemViewModel(this, ivm.Item)));
                    else await ShopNavigation.PushAsync(InfoPage.Show(ivm.Boon));
                }
            });
            ShowItemInfo = new Command(async (par) =>
            {
                if (par is InventoryViewModel ivm)
                {
                    if (ivm.Item is Possession p)
                    {
                        await ShopNavigation.PushAsync(InfoPage.Show(new DisplayPossession(ivm.Item, Context.Player)));
                    }
                    else
                    {
                        await ShopNavigation.PushAsync(InfoPage.Show(ivm.Boon));
                    }
                }
            });
            DeleteItem = new Command((par) =>
            {
                if (par is InventoryViewModel ivm)
                {
                    if (ivm.Item is Possession p)
                    {
                        Context.MakeHistory("");
                        Context.Player.RemovePossessionAndItems(p);
                        Save();
                    }
                    else if (ivm.Boon is Feature f)
                    {
                        Context.MakeHistory("");
                        Context.Player.RemoveBoon(f);
                        Save();
                    }
                    FirePlayerChanged();
                }
            });
            RefreshItems = new Command(() =>
            {
                ItemsBusy = true;
                UpdateItems();
                OnPropertyChanged("Carried");
                ItemsBusy = false;
            });
            UpdateItems();
            OnOpenShop = new Command(async (par) =>
            {
                if (par is ShopViewModel svm) await ShopNavigation.PushAsync(new ShopSubPage(svm));
            });
            UpdateShops();
            UpdateInventoryChoices();
        }

        public abstract void UpdateSpellcasting();

        public Command Undo { get; set; }
        public Command Redo { get; set; }

        public abstract void MakeHistory(string h = null);
        public abstract void Save();
        public abstract void DoSave();


        //SHOP
        private static CultureInfo culture = CultureInfo.InvariantCulture;
        private List<InventoryViewModel> inventory = new List<InventoryViewModel>();
        public ObservableRangeCollection<InventoryViewModel> Inventory { get; set; } = new ObservableRangeCollection<InventoryViewModel>();

        public void UpdateInventory() => Inventory.ReplaceRange(from f in inventory where inventorysearch == null || inventorysearch == ""
            || culture.CompareInfo.IndexOf(f.Name, inventorysearch, CompareOptions.IgnoreCase) >= 0
            || culture.CompareInfo.IndexOf(f.Detail, inventorysearch, CompareOptions.IgnoreCase) >= 0
            || culture.CompareInfo.IndexOf(f.Description, inventorysearch, CompareOptions.IgnoreCase) >= 0 orderby f.Name select f);

        private string inventorysearch;
        public string InventorySearch
        {
            get => inventorysearch;
            set
            {
                SetProperty(ref inventorysearch, value);
                UpdateInventory();
            }
        }
        private bool itemsBusy;
        public bool ItemsBusy { get => itemsBusy; set => SetProperty(ref itemsBusy, value); }
        public Command EditItem { get; private set; }
        public Command ShowItemInfo { get; private set; }
        public Command DeleteItem { get; private set; }
        public Command RefreshItems { get; private set; }

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
        public ObservableRangeCollection<ChoiceViewModel> InventoryChoices { get; set; } = new ObservableRangeCollection<ChoiceViewModel>();
        public void UpdateInventoryChoices()
        {
            List<ChoiceViewModel> choices = new List<ChoiceViewModel>();
            foreach (Feature f in Context.Player.GetPossessionFeatures())
            {
                ChoiceViewModel c = ChoiceViewModel.GetChoice(this, f);
                if (c != null) choices.Add(c);
            }
            InventoryChoices.ReplaceRange(choices);
        }
        public void UpdateItems()
        {
            inventory.Clear();
            Inventory.ReplaceRange(inventory);
            foreach (Possession p in Context.Player.GetItemsAndPossessions())
            {
                inventory.Add(new InventoryViewModel
                {
                    Item = p,
                    ShowInfo = ShowItemInfo,
                    Edit = EditItem,
                    Delete = DeleteItem
                });
            }
            foreach (Feature f in from b in Context.Player.Boons select Context.GetBoon(b, null))
            {
                inventory.Add(new InventoryViewModel
                {
                    Boon = f,
                    ShowInfo = ShowItemInfo,
                    Edit = EditItem,
                    Delete = DeleteItem
                });
            }
            UpdateInventory();
        }
    }
}
