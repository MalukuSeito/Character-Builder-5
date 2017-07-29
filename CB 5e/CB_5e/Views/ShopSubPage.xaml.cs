using CB_5e.ViewModels;
using OGL.Common;
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
    public partial class ShopSubPage : ContentPage
    {
        public ShopViewModel Model {get; private set;}

        public ShopSubPage(ShopViewModel model)
        {
            InitializeComponent();
            model.UpdateShop();
            BindingContext = Model = model;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(InfoPage.Show((IXML)e.SelectedItem, Model.Select, Model.CommandName));
        }
    }
}