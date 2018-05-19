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
    public partial class EditResourceFeature : ContentPage
    {
        public EditResourceFeature(ResourceFeatureEditModel model)
        {
            BindingContext = Model = model;
            InitializeComponent();
        }

        public ResourceFeatureEditModel Model { get; private set; }

        protected override void OnDisappearing()
        {
            if (Model.ExclusionID != null && Model.ExclusionID.Trim() != "") ResourceFeatureEditModel.ExclusionIDs.Add(Model.ExclusionID);
            if (Model.ResourceID != null && Model.ResourceID.Trim() != "") ResourceFeatureEditModel.ResourceIDs.Add(Model.ResourceID);
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