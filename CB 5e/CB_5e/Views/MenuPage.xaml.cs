using CB_5e.ViewModels;
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
    public partial class MenuPage : MasterDetailPage
    {
        public PlayerModel Model { get; private set; }
        private MenuPageMaster MasterPage;
        public MenuPage(PlayerModel model)
        {
            model.Navigation = Navigation;
            BindingContext = Model = model;
            InitializeComponent();
            Master = MasterPage = new MenuPageMaster(model);
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            Detail = new NavigationPage(PlayerModel.GetPage(model.FirstPage));
            IsPresented = false;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as SubModel;
            if (item == null)
                return;

            var page = PlayerModel.GetPage(item);

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }

        
        protected override bool OnBackButtonPressed()
        {
            if (!Model.ChildModel)
            {
                if (Model is PlayerViewModel m && App.AutoSaveDuringPlay)
                {
                    Model.Save();
                    m.Saving.WaitForAll();
                }
                else if (Model.Context.UnsavedChanges > 0)
                {
                    if (DisplayAlert("Unsaved Changes", "You have " + Model.Context.UnsavedChanges + " unsaved changes. Do you want to save them before leaving?", "Yes", "No").Result)
                    {
                        Model.DoSave();
                    }
                }
                if (Model.Modified) CharactersViewModel.Instance.LoadItemsCommand.Execute(null);
            }
            return base.OnBackButtonPressed();
        }
    }
}