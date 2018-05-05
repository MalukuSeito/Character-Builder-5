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
    public partial class EditLanguageChoiceFeature : ContentPage
    {
        public EditLanguageChoiceFeature(LanguageProficiencyChoiceFeatureEditModel model)
        {
            InitializeComponent();
            BindingContext = Model = model;
        }

        public LanguageProficiencyChoiceFeatureEditModel Model { get; set; }

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
        private double width;
        private double height;
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height) gridder.Padding = new Thickness(10, 0);
                else gridder.Padding = new Thickness(10);
            }
        }
    }
}