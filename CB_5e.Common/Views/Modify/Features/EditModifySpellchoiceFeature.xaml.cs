using CB_5e.ViewModels.Modify.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify.Features
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditModifySpellchoiceFeature : ContentPage
    {
        public EditModifySpellchoiceFeature(ModifySpellchoiceEditModel model)
        {
            InitializeComponent();
            BindingContext = Model = model;
        }

        public ModifySpellchoiceEditModel Model { get; set; }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Model.SaveAsync(true);
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            Task.Run(async () =>
            {
                await Model.SaveAsync(true);
                await Navigation.PopModalAsync();
            });
            return true;
        }
        protected override void OnDisappearing()
        {
            SpellchoiceFeatureEditModel.UniqueIDs.Add(Model.UniqueID);
        }
    }
}