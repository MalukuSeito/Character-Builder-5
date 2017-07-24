using CB_5e.Helpers;
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
	public partial class PlayerOverview : CarouselPage
	{
        public static IntToStringConverter IntConverter = PlayerViewModel.IntConverter;

        public PlayerOverview()
		{
			InitializeComponent ();
            CurrentPageChanged += CarouselPage_CurrentPageChanged; 
            BindingContext = PlayerViewModel.Instance;
            PlayerViewModel.Instance.Navigation = Navigation;
            //Image.GestureRecognizers.Add(new TapGestureRecognizer(OnTap));
            Title = CurrentPage.Title;
        }

        //private async void OnTap(View arg1, object arg2)
        //{
        //    if (IsBusy) return;
        //    IsBusy = true;
        //    if (Image != null && Image.Source != null) await Navigation.PushAsync(new ImageViewer(Image.Source, Player.Current?.Portrait, Player.Current?.Name));
        //    IsBusy = false;
        //}

        private void HitDice_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            HitDice.SelectedItem = null;
        }

        private void ResetHitDie(object sender, EventArgs e)
        {
            PlayerViewModel.Instance.ResetHitDie.Execute(null);
        }

        private async void Money_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PushAsync(new EditMoneyPage());
            IsBusy = false;
        }

        private async void OnExit(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (App.AutoSaveDuringPlay)
            {
                PlayerViewModel.Saving.WaitForAll();
            }
            else if (Player.UnsavedChanges > 0)
            {
                if (DisplayAlert("Unsaved Changes", "You have " + Player.UnsavedChanges + " unsaved changes. Do you want to save them before leaving?", "Yes", "No").Result)
                {
                    PlayerViewModel.Instance.DoSave();
                }
            }
            await Navigation.PopModalAsync();
            Player.Current = null;
            CharactersViewModel.Instance.LoadItemsCommand.Execute(null);
            IsBusy = false;
        }

        private void OnNext(object sender, EventArgs e)
        {
            int i = Children.IndexOf(CurrentPage) + 1;
            if (i < Children.Count) CurrentPage = Children[i];
        }

        private void OnPrev(object sender, EventArgs e)
        {
            int i = Children.IndexOf(CurrentPage) - 1;
            if (i >= 0) CurrentPage = Children[i];
        }

        private void CarouselPage_CurrentPageChanged(object sender, EventArgs e)
        {
            Title = CurrentPage.Title;
        }
        private async void SkillInfo(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is SkillInfo obj) await Navigation.PushAsync(InfoPage.Show(obj.Skill));
        }
    }
}