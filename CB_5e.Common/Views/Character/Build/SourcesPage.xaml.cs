using CB_5e.ViewModels;
using CB_5e.ViewModels.Character.Build;
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
    public partial class SourcesPage : ContentPage
    {
        public SourcesPage(SourcesViewModel model)
        {
            model.Navigation = Navigation;
            BindingContext = model;
            InitializeComponent();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}