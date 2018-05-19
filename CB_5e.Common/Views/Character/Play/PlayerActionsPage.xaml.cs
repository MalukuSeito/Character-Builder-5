using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CB_5e.ViewModels;
using OGL.Common;
using CB_5e.ViewModels.Character.Play;
using Character_Builder;

namespace CB_5e.Views.Character.Play
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerActionsPage : ContentPage
    {
        public PlayerActionsPage(PlayerActionsViewModel model)
        {
            BindingContext = model;
            model.Navigation = Navigation;
            InitializeComponent();
        }
        private async void InfoSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is ActionInfo obj) await Navigation.PushAsync(InfoPage.Show(obj.Feature));
            ((ListView)sender).SelectedItem = null;
        }
    }
}