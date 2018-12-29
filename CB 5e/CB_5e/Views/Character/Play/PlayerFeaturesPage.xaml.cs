using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CB_5e.ViewModels;
using OGL.Common;
using OGL.Features;
using CB_5e.ViewModels.Character;

namespace CB_5e.Views.Character.Play
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerFeaturesPage : ContentPage
    {
        public PlayerFeaturesViewModel Model { get; private set; }

        public PlayerFeaturesPage(PlayerFeaturesViewModel model)
        {
            BindingContext = Model = model;
            model.Navigation = Navigation;
            InitializeComponent();
        }
        private async void InfoSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
            ((ListView)sender).SelectedItem = null;
        }
        private void ShowOnSheet(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is Feature f)
            {
                Model.MakeHistory();
                Model.Context.Player.HiddenFeatures.RemoveAll(s => StringComparer.OrdinalIgnoreCase.Equals(s, f.Name) || StringComparer.OrdinalIgnoreCase.Equals(s, f.Name + "/" + f.Level));
                //Model.Context.Player.HiddenFeatures.Add(f.Name);
                Model.Save();
            };
        }
        private void HideOnSheet(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is Feature f)
            {
                Model.MakeHistory();
                Model.Context.Player.HiddenFeatures.Add(f.Name + "/" + f.Level);
                Model.Save();
            };
        }
    }
}