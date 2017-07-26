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
        public static IntToStringConverter IntConverter = PlayerViewModel.IntConverter;
        private object old;

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

        private void HitDice_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            HitDice.SelectedItem = null;
        }

        private void ResetHitDie(object sender, EventArgs e)
        {
            Model.ResetHitDie.Execute(null);
        }

        private async void Money_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PushAsync(new EditMoneyPage(Model));
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
            else if (Model.Context.UnsavedChanges > 0)
            {
                if (DisplayAlert("Unsaved Changes", "You have " + Model.Context.UnsavedChanges + " unsaved changes. Do you want to save them before leaving?", "Yes", "No").Result)
                {
                    Model.DoSave();
                }
            }
            await Navigation.PopModalAsync();
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

        private async void Info(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
        }

        private async void ResourceInfo(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is Resource rs)
            {
                if (rs.Value is ModifiedSpell ms)
                {
                    ms.Info = Model.Context.Player.GetAttack(ms, ms.differentAbility);
                    ms.Modifikations.AddRange(from f in Model.Context.Player.GetFeatures() where f is SpellModifyFeature && Utils.Matches(Model.Context, ms, ((SpellModifyFeature)f).Spells, null) select f);
                    ms.Modifikations = ms.Modifikations.Distinct().ToList();
                    await Navigation.PushAsync(InfoPage.Show(ms));
                }
                if (rs.Value is ResourceInfo r)
                {
                    await Navigation.PushAsync(InfoPage.Show(Model.Context.Player.GetResourceFeatures(r.ResourceID)));
                }
            }
        }

        private async void InfoSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is IXML obj) await Navigation.PushAsync(InfoPage.Show(obj));
            ((ListView)sender).SelectedItem = null;
        }

        private void ShowOnSheet(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is Feature f)
            {
                Model.MakeHistory();
                Model.Context.Player.HiddenFeatures.RemoveAll(s => StringComparer.OrdinalIgnoreCase.Equals(s, f.Name));
                //Model.Context.Player.HiddenFeatures.Add(f.Name);
                Model.Save();
            };
        }
        private void HideOnSheet(object sender, EventArgs e)
        {
            if ((sender as Xamarin.Forms.MenuItem).BindingContext is Feature f)
            {
                Model.MakeHistory();
                Model.Context.Player.HiddenFeatures.Add(f.Name);
                Model.Save();
            };
        }

        private void AddCondition(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is OGL.Condition c)
            {
                Model.AddCondition.Execute(c);
            }
            (sender as ListView).SelectedItem = null;
        }
        private void RemoveCondition(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is OGL.Condition c)
            {
                Model.RemoveCondition.Execute(c);
            }
            (sender as ListView).SelectedItem = null;
        }
    }
}