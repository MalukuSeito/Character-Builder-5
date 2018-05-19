using CB_5e.ViewModels;
using CB_5e.ViewModels.Character;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Character
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemPage : ContentPage
    {
        public ItemViewModel Model { get; set; }
        public ItemPage(ItemViewModel model)
        {
            InitializeComponent();
            BindingContext = Model = model;
        }
        private async void Unpack(object sender, EventArgs e)
        {
            if (Model.Value.Item is Pack p)
            {
                Model.Context.MakeHistory();
                for (int i = 0; i < Model.Value.Count; i++)
                    Model.Context.Context.Player.Items.AddRange(p.Contents);
                Model.Context.Context.Player.RemovePossessionAndItems(Model.Value);
                Model.Context.Save();
                Model.Context.RefreshItems.Execute(null);
                await Navigation.PopAsync();
            } else
            {
                await DisplayAlert("Error", "This item is not a Pack, so it can't be unpacked", "Cancel");
            }

        }
        private async void Split(object sender, EventArgs e)
        {
            if (Model.Count > 1)
            {
                await Navigation.PushAsync(new SplitPage(Model));
            }
            else
            {
                await DisplayAlert("Error", "You can't split a stack with only 1 item on it", "Cancel");
            }

        }

        private async void Delete(object sender, EventArgs e)
        {
            Model.Context.DeleteItem.Execute(new InventoryViewModel() { Item = Model.Value });
            await Navigation.PopAsync();
        }
        protected override void OnDisappearing()
        {
            if (Model.IsChanged)
            {
                Model.IsChanged = false;
                Model.Context.Save();
                Model.Context.RefreshItems.Execute(null);
            }
            base.OnDisappearing();
        }
    }
}