using CB_5e.ViewModels;
using CB_5e.ViewModels.Character.Build;
using CB_5e.ViewModels.Character.ChoiceOptions;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Character.Build
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClassesPage : ContentPage
	{
		public ClassesPage (ClassesViewModel model)
		{
            BindingContext = model;
            model.Navigation = Navigation;
            InitializeComponent ();
		}
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;
        }
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is ChoiceOption obj) await Navigation.PushAsync(InfoPage.Show(obj.Value));
        }

        private async void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is ChoiceOption obj && obj.Feature is IXML) await Navigation.PushAsync(InfoPage.Show(obj.Feature));
        }
    }
}