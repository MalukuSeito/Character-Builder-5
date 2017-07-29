using CB_5e.ViewModels;
using OGL.Items;
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
    public partial class NewItemPage : ContentPage
    {
        public ItemViewModel Model { get; set; }
        public NewItemPage(ItemViewModel model)
        {
            InitializeComponent();
            BindingContext = Model = model;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Model.Context.Context.Player.AddPossession(Model.Value);
            Model.Context.Save();
            Model.Context.RefreshItems.Execute(null);
            await Navigation.PopModalAsync();
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}