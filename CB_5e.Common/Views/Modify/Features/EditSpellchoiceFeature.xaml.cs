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
    public partial class EditSpellchoiceFeature : ContentPage
    {
        public EditSpellchoiceFeature(SpellchoiceFeatureEditModel model)
        {
            BindingContext = Model = model;
            InitializeComponent();
        }

        public SpellchoiceFeatureEditModel Model { get; private set; }

        protected override void OnDisappearing()
        {
            if (Model.UniqueID != null && Model.UniqueID.Trim() != "") SpellchoiceFeatureEditModel.UniqueIDs.Add(Model.UniqueID);
            if (Model.SpellcastingID != null && Model.SpellcastingID.Trim() != "") SpellcastingFeatureEditModel.SpellcastingIDs.Add(Model.SpellcastingID);
        }

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
    }
}