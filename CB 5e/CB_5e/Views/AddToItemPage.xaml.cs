using CB_5e.Helpers;
using CB_5e.ViewModels;
using Character_Builder;
using OGL;
using OGL.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToItemPage : ContentPage
    {
        private static CultureInfo culture = CultureInfo.InvariantCulture;
        public AddToItemPage(ShopViewModel model, MagicProperty mp)
        {
            InitializeComponent();
            Model = model;
            Property = mp;
            BindingContext = this;
            Title = Property.Name;
            UpdateItems();
        }

        public MagicProperty Property { get; private set; }
        public ShopViewModel Model { get; private set; }

        public void UpdateInventory() => Inventory.ReplaceRange(from f in inventory
                                                                where inventorysearch == null || inventorysearch == ""
                                                                || culture.CompareInfo.IndexOf(f.Name, inventorysearch, CompareOptions.IgnoreCase) >= 0
                                                                || culture.CompareInfo.IndexOf(f.Detail, inventorysearch, CompareOptions.IgnoreCase) >= 0
                                                                || culture.CompareInfo.IndexOf(f.Description, inventorysearch, CompareOptions.IgnoreCase) >= 0
                                                                orderby f.Name
                                                                select f);

        private string inventorysearch;
        private List<InventoryViewModel> inventory = new List<InventoryViewModel>();

        public string InventorySearch
        {
            get => inventorysearch;
            set
            {
                inventorysearch = value;
                OnPropertyChanged("InventorySearch");
                UpdateInventory();
            }
        }
        public ObservableRangeCollection<InventoryViewModel> Inventory { get; set; } = new ObservableRangeCollection<InventoryViewModel>();
        public void UpdateItems()
        {
            inventory.Clear();
            Inventory.ReplaceRange(inventory);
            foreach (Possession p in Model.Model.Context.Player.GetItemsAndPossessions())
            {

                if (p.BaseItem != null && p.BaseItem != "" && Utils.Matches(Model.Model.Context,p.Item,Property.Base,Model.Model.Context.Player.GetLevel())) inventory.Add(new InventoryViewModel
                {
                    Item = p,
                    ShowInfo = Model.Model.ShowItemInfo
                });
            }
            UpdateInventory();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is InventoryViewModel ivm)
            {
                Model.Model.MakeHistory();
                Possession p = ivm.Item;
                int stack = 1;
                if (p.Item != null) stack = Math.Max(1, p.Item.StackSize);
                if (p.Count > stack)
                {
                    p.Count -= stack;
                    p = new Possession(p, Property);
                }
                else
                {
                    p.MagicProperties.Add(Property.Name + " " + ConfigManager.SourceSeperator + " " + Property.Source);
                }
                Model.Model.Context.Player.AddPossession(p);
                Model.Model.Save();
                Model.Model.FirePlayerChanged();
                Model.Select.ChangeCanExecute();
                await Navigation.PopAsync();
            }
        }
    }
}