using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CB_5e.ViewModels;
using OGL.Common;

namespace CB_5e.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerProficiencyPage : ContentPage
    {
        public PlayerProficiencyPage(PlayerProficiencyViewModel model)
        {
            BindingContext = model;
            model.Navigation = Navigation;
            InitializeComponent();
        }
        private async void InfoSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
            ((ListView)sender).SelectedItem = null;
        }
    }
}