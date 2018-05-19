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

namespace CB_5e.Views.Character.Play
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerConditionPage : ContentPage
    {
        public PlayerConditionViewModel Model;
        public PlayerConditionPage(PlayerConditionViewModel model)
        {
            BindingContext = Model = model;
            model.Navigation = Navigation;
            InitializeComponent();
        }
        private async void Info(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
        }

        private void AddCondition(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is OGL.Condition c)
            {
                Model.AddCondition.Execute(c);
            }
            (sender as ListView).SelectedItem = null;
        }
        private void RemoveCondition(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is OGL.Condition c)
            {
                Model.RemoveCondition.Execute(c);
            }
            (sender as ListView).SelectedItem = null;
        }
    }
}