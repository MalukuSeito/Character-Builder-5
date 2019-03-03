using CB_5e.Helpers;
using CB_5e.Views;
using CB_5e.Views.Character;
using Character_Builder;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class PlayerInventoryViewModel : SubModel
    {
        public PlayerInventoryViewModel(PlayerModel parent) : base(parent, "Inventory")
        {
            Image = ImageSource.FromResource("CB_5e.images.inventory.png");
            parent.PlayerChanged += Parent_PlayerChanged;
            EditItem = new Command(async (par) =>
            {
                if (par is InventoryViewModel ivm)
                {
                    if (ivm.Item != null) await Navigation.PushAsync(new ItemPage(new ItemViewModel(this, ivm.Item)));
                    else await Navigation.PushAsync(InfoPage.Show(ivm.Boon));
                }
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
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            UpdateItems();
        }

        private List<InventoryViewModel> inventory = new List<InventoryViewModel>();
        public ObservableRangeCollection<InventoryViewModel> Inventory { get; set; } = new ObservableRangeCollection<InventoryViewModel>();

        public void UpdateInventory() => Inventory.ReplaceRange(from f in inventory where inventorysearch == null || inventorysearch == ""
            || Culture.CompareInfo.IndexOf(f.Name ?? "", inventorysearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(f.Detail ?? "", inventorysearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(f.Description ?? "", inventorysearch, CompareOptions.IgnoreCase) >= 0 orderby f.Name select f);

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
        public void UpdateItems()
        {
            inventory.Clear();
            Inventory.ReplaceRange(inventory);
            foreach (Possession p in Context.Player.GetItemsAndPossessions())
            {
                if (p.Count > 0 || !App.HideLostItems) inventory.Add(new InventoryViewModel
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
