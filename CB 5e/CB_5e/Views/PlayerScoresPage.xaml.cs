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
        public PlayerScoresViewModel Model { get; private set; }

        public PlayerScoresPage(PlayerScoresViewModel model)
        {
            BindingContext = Model = model;
            model.Navigation = Navigation;
            InitializeComponent();
            //MakeBinding(Str, new Binding("Str"));
            //MakeBinding(Dex, new Binding("Dex"));
            //MakeBinding(Con, new Binding("Con"));
            //MakeBinding(Int, new Binding("Int"));
            //MakeBinding(Cha, new Binding("Cha"));
            //MakeBinding(Wis, new Binding("Wis"));
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

        //private static void MakeBinding(Entry entry, Binding b)
        //{
        //    entry.SetBinding(Entry.TextProperty, b);
        //    entry.Focused += (s, e) => {
        //        if (s is Entry ee)
        //        {
        //            ee.RemoveBinding(Entry.TextProperty);
        //        }
        //    };
        //    entry.Unfocused += (s, e) => {
        //        if (s is Entry ee)
        //        {
        //            string tmp = entry.Text;
        //            ee.RemoveBinding(Entry.TextProperty);
        //            ee.SetBinding(Entry.TextProperty, b);
        //            ee.SetValue(Entry.TextProperty, tmp);
        //        }
        //    };
        //}

        //private void Str_Unfocused(object sender, FocusEventArgs e)
        //{
        //    //if (int.TryParse(Str.Text, out int v)) Model.Str = v;
        //}

        //private void Dex_Unfocused(object sender, FocusEventArgs e)
        //{
        //    //if (int.TryParse(Dex.Text, out int v)) Model.Dex = v;
        //}

        //private void Con_Unfocused(object sender, FocusEventArgs e)
        //{
        //    //if (int.TryParse(Con.Text, out int v)) Model.Con = v;
        //}

        //private void Int_Unfocused(object sender, FocusEventArgs e)
        //{
        //    //if (int.TryParse(Int.Text, out int v)) Model.Int = v;
        //}

        //private void Wis_Unfocused(object sender, FocusEventArgs e)
        //{
        //    //if (int.TryParse(Wis.Text, out int v)) Model.Wis = v;
        //}

        //private void Cha_Unfocused(object sender, FocusEventArgs e)
        //{
        //    //if (int.TryParse(Cha.Text, out int v)) Model.Cha = v;
        //}
        //protected override void OnDisappearing()
        //{
        //    Model.FirePlayerChanged();
        //}
    }
}