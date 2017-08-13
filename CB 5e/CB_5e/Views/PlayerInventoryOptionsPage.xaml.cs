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
    public partial class PlayerInventoryOptionsPage : ContentPage
    {
        public PlayerInventoryOptionsPage(PlayerInventoryChoicesViewModel model)
        {
            BindingContext = model;
            model.Navigation = Navigation;
            InitializeComponent();
        }
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is ChoiceOption obj) await Navigation.PushAsync(InfoPage.Show(obj.Value));
        }

        private async void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is ChoiceOption obj && obj.Feature is IXML) await Navigation.PushAsync(InfoPage.Show(obj.Feature));
        }
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}