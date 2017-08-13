using CB_5e.ViewModels;
using DLToolkit.Forms.Controls;
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
    public partial class FlowPage : ContentPage
    {
        public PlayerModel Model { get; private set; }
        public FlowPage(PlayerModel model)
        {
            model.Navigation = Navigation;
            FlowListView.Init();
            InitializeComponent();
            BindingContext = Model = model;
        }

        private async void FlowListView_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is SubModel s)
            {
                ContentPage p = PlayerModel.GetPage(s);
                ToolbarItem undo = new ToolbarItem()
                {
                    Text = "Undo"
                };
                undo.SetBinding(MenuItem.CommandProperty, new Binding("Undo"));
                p.ToolbarItems.Add(undo);
                ToolbarItem redo = new ToolbarItem()
                {
                    Text = "Redo"
                };
                redo.SetBinding(MenuItem.CommandProperty, new Binding("Redo"));
                p.ToolbarItems.Add(redo);
                await Navigation.PushAsync(p);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
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
                        if (await DisplayAlert("Unsaved Changes", "You have " + Model.Context.UnsavedChanges + " unsaved changes. Do you want to save them before leaving?", "Yes", "No"))
                        {
                            Model.DoSave();
                        }
                    }
                    if (Model.Modified) CharactersViewModel.Instance.LoadItemsCommand.Execute(null);
                }
                await Navigation.PopModalAsync();
            });
            return true;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (!Model.ChildModel)
            {
                if (Model is PlayerViewModel m && App.AutoSaveDuringPlay)
                {
                    Model.Save();
                    m.Saving.WaitForAll();
                }
                else if (Model.Context.UnsavedChanges > 0)
                {
                    if (await DisplayAlert("Unsaved Changes", "You have " + Model.Context.UnsavedChanges + " unsaved changes. Do you want to save them before leaving?", "Yes", "No"))
                    {
                        Model.DoSave();
                    }
                }
            }
            await Navigation.PopModalAsync();
            if (!Model.ChildModel && Model.Modified)
            {
                CharactersViewModel.Instance.LoadItemsCommand.Execute(null);
            }
            IsBusy = false;
        }
    }
}