using System;

using CB_5e.Models;
using CB_5e.ViewModels;

using Xamarin.Forms;

namespace CB_5e.Views
{
    public partial class CharactersBuildPage : ContentPage
    {
        CharactersViewModel viewModel;

        public CharactersBuildPage()
        {
            InitializeComponent();

            BindingContext = viewModel = CharactersViewModel.Instance;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Character;
            if (item == null)
                return;

            //await Navigation.PushAsync(new ItemDetailPage(new NewFolderViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NewFolderPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
