using CB_5e.ViewModels;
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
    public partial class PlayerInfoPage : ContentPage
    {
        public PlayerInfoPage(PlayerInfoViewModel model)
        {
            model.Navigation = Navigation;
            BindingContext = Model = model;
            InitializeComponent();
        }

        public PlayerInfoViewModel Model { get; private set; }
        private void HitDice_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            HitDice.SelectedItem = null;
        }

        private void ResetHitDie(object sender, EventArgs e)
        {
            Model.ResetHitDie.Execute(null);
        }
        private async void Money_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PushAsync(new EditMoneyPage(Model));
            IsBusy = false;
        }
    }
}