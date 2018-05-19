using CB_5e.Services;
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
    public partial class EditBonusSpellFeature : ContentPage
    {
        public EditBonusSpellFeature(BonusSpellFeatureEditModel model)
        {
            BindingContext = Model = model;
            InitializeComponent();
        }

        public BonusSpellFeatureEditModel Model { get; private set; }


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

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (Model.Context.SpellsSimple.Count == 0)
            {
                await Model.Context.ImportSpellsAsync();
            }
            Model.Suggestions.ReplaceRange(Model.Context.SpellsSimple.Keys.OrderBy(s => s));
        }
    }
}