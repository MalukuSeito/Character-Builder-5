using CB_5e.ViewModels;
using Character_Builder;
using PCLStorage;
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
    public partial class PlayPage : TabbedPage
    {
        private PlayerViewModel Model;
        public PlayPage(PlayerViewModel model)
        {
            BindingContext = Model = model;
            InitializeComponent();
            Children.Add(
                    new NavigationPage(new PlayerOverview(Model))
                    {
                        Title = "Stats",
                        Icon = Device.OnPlatform("tab_feed.png", null, null)
                    });
            Children.Add(
                    new NavigationPage(new SpellcastingPage(Model))
                    {
                        Title = "Spell",
                        Icon = Device.OnPlatform("tab_about.png", null, null)
                    });
            Children.Add(
                    new NavigationPage(new ShopPage(Model))
                    {
                        Title = "Item",
                        Icon = Device.OnPlatform("tab_about.png", null, null)
                    });
            Children.Add(
                    new NavigationPage(new JournalPage(Model))
                    {
                        Title = "Notes",
                        Icon = Device.OnPlatform("tab_about.png", null, null)
                    });
            Children.Add(
                    new NavigationPage(new AboutPage())
                    {
                        Title = "...",
                        Icon = Device.OnPlatform("tab_about.png", null, null)
                    });
        }
        protected override bool OnBackButtonPressed()
        {
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
                CharactersViewModel.Instance.LoadItemsCommand.Execute(null);
            }
            return base.OnBackButtonPressed();
        }
    }
}