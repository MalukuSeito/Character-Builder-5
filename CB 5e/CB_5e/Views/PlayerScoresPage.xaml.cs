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
    public partial class PlayerScoresPage : ContentPage
    {
        public PlayerScoresPage(PlayerScoresViewModel model)
        {
            BindingContext = model;
            model.Navigation = Navigation;
            InitializeComponent();
            MakeBinding(Str, new Binding("Str"));
            MakeBinding(Dex, new Binding("Dex"));
            MakeBinding(Con, new Binding("Con"));
            MakeBinding(Int, new Binding("Int"));
            MakeBinding(Cha, new Binding("Cha"));
            MakeBinding(Wis, new Binding("Wis"));
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

        private static void MakeBinding(Entry entry, Binding b)
        {
            entry.SetBinding(Entry.TextProperty, b);
            entry.Focused+= (s, e) => { entry.RemoveBinding(Entry.TextProperty); };
            entry.Unfocused += (s, e) => {
                string tmp = entry.Text;
                entry.SetBinding(Entry.TextProperty, b);
                entry.SetValue(Entry.TextProperty, tmp);
            };
        }
    }
}