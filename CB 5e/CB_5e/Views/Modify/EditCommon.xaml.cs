using CB_5e.ViewModels;
using CB_5e.ViewModels.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCommon : ContentPage
    {
        public IEditModel Model { get; private set; }
        public EditCommon(IEditModel model)
        {
            BindingContext = Model = model;
            Model.Navigation = Navigation;
            InitializeComponent();
            Model.TrackChanges = true;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Model.UnsavedChanges > 0)
                {
                   if (await DisplayAlert("Unsaved Changes", "You have " + Model.UnsavedChanges + " unsaved changes. Do you want to save them before leaving?", "Yes", "No"))
                    {
                        bool written = await Model.SaveAsync(false);
                        if (!written)
                        {
                            if (await DisplayAlert("File Exists", "Overwrite File?", "Yes", "No"))
                            {
                                await Model.SaveAsync(true);
                                await Navigation.PopModalAsync();
                            }
                        }
                        else await Navigation.PopModalAsync();
                    }
                    else await Navigation.PopModalAsync();
                }
                else await Navigation.PopModalAsync();
            });
            return true;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (Model.UnsavedChanges > 0)
            {
                if (await DisplayAlert("Unsaved Changes", "You have " + Model.UnsavedChanges + " unsaved changes. Do you want to save them before leaving?", "Yes", "No"))
                {
                    bool written = await Model.SaveAsync(false);
                    if (!written)
                    {
                        if (await DisplayAlert("File Exists", "Overwrite File?", "Yes", "No"))
                        {
                            await Model.SaveAsync(true);
                            await Navigation.PopModalAsync();
                        }
                    }
                    else await Navigation.PopModalAsync();
                }
                else await Navigation.PopModalAsync();
            }
            else await Navigation.PopModalAsync();
        }
    }
}