using CB_5e.Helpers;
using CB_5e.ViewModels;
using Character_Builder;
using OGL.Common;
using OGL.Features;
using OGL.Spells;
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
        public PlayerViewModel Model { get; private set; }

        public PlayerOverview(PlayerViewModel model)
		{
            BindingContext = Model = model;
            Model.Navigation = Navigation;
            InitializeComponent ();
            CurrentPageChanged += CarouselPage_CurrentPageChanged; 
            
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
        

        

        

        
    }
}