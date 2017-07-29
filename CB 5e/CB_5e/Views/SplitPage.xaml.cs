using CB_5e.ViewModels;
using Character_Builder;
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
    public partial class SplitPage : ContentPage
    {
        public SplitPage(ItemViewModel model)
        {
            InitializeComponent();
            BindingContext = Model = model;
        }

        public ItemViewModel Model { get; private set; }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (Model.IsNew) Model.Context.Context.Player.AddPossession(Model.Value);
            Model.Context.MakeHistory("");
            Possession newp = new Possession(Model.Value);
            int stacksize = 1;
            if (Model.Value.Item != null) stacksize = Math.Max(1, Model.Value.Item.StackSize);
            newp.Count = Model.Value.Count - Model.Split;
            if (Model.Split % stacksize != 0 && Model.Value.Item != null) Model.Context.Context.Player.Items.Add(Model.Value.BaseItem);
            Model.Count = Model.Split;
            if (newp.Count > 0) Model.Context.Context.Player.AddPossession(newp);
            Model.Context.Save();
            Model.Context.RefreshItems.Execute(null);
            await Navigation.PopAsync();
        }
    }
}