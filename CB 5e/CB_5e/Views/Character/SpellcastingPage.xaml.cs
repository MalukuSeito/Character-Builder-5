using CB_5e.ViewModels;
using CB_5e.ViewModels.Character;
using CB_5e.ViewModels.Character.Play;
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
	public partial class SpellcastingPage : CarouselPage
    {
		public SpellcastingPage (PlayerViewModel model)
		{
            BindingContext = Model = model;
            model.Navigation = Navigation;
            CurrentPageChanged += CarouselPage_CurrentPageChanged;
            InitializeComponent ();

		}

        public PlayerViewModel Model { get; private set; }

        private async void OnExit(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (!Model.ChildModel)
            {
                if (App.AutoSaveDuringPlay)
                {
                    Model.Save();
                    Model.Saving.WaitForAll();
                }
                else if (Model.Context.UnsavedChanges > 0)
                {
                    if (DisplayAlert("Unsaved Changes", "You have " + Model.Context.UnsavedChanges + " unsaved changes. Do you want to save them before leaving?", "Yes", "No").Result)
                    {
                        Model.DoSave();
                    }
                }
            }
            await Navigation.PopModalAsync();
            if (!Model.ChildModel)
            {
                CharactersViewModel.Instance.LoadItemsCommand.Execute(null);
            }
            IsBusy = false;
        }
        private void CarouselPage_CurrentPageChanged(object sender, EventArgs e)
        {
            Title = CurrentPage?.Title ?? "";
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}