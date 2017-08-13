using CB_5e.ViewModels;
using Character_Builder;
using OGL;
using OGL.Base;
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
    public partial class BuyAddPage : ContentPage
    {
        public BuyAddPage(ShopViewModel model, Item item)
        {
            InitializeComponent();
            Model = model;
            Item = item;
            BindingContext = this;
            Title = Item.Name;
        }

        public Item Item { get; private set; }
        public ShopViewModel Model { get; private set; }

        private async void Buy(object sender, EventArgs e)
        {
            Model.Model.MakeHistory();
            for (int c = 0; c < Count; c++)
                Model.Model.Context.Player.Items.Add(Item.Name + " " + ConfigManager.SourceSeperator + " " + Item.Source);
            //Program.Context.Player.Pay(new Price(i.Price,count));
            if (Count > 1)
            {
                Model.Model.Context.Player.ComplexJournal.Add(new JournalEntry(Item.ToString() + " x " + Count, new Price(Item.Price, Count)));
            }
            else Model.Model.Context.Player.ComplexJournal.Add(new JournalEntry(Item.ToString(), new Price(Item.Price, Count)));
            Model.Model.Save();
            Model.Model.RefreshInventory.Execute(null);
            await Navigation.PopAsync();
        }

        private int count = 1;
        public int Count {
            get => count;
            set
            {
                if (value <= 1) value = 1;
                count = value;
                OnPropertyChanged("Count");
            }
        }

        private async void Add(object sender, EventArgs e)
        {
            Model.Model.MakeHistory();
            for (int c = 0; c < Count; c++)
                Model.Model.Context.Player.Items.Add(Item.Name + " " + ConfigManager.SourceSeperator + " " + Item.Source);
            Model.Model.Save();
            Model.Model.RefreshInventory.Execute(null);
            await Navigation.PopAsync();
        }

        public string Price { get => "Total: " + new Price(Item.Price, Count).ToString(); }
        public string Money { get => "Money: " + Model.Money; }
    }
}