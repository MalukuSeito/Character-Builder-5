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
    public partial class FlexPage : ContentPage
    {
        public PlayerModel Model { get; private set; }
        public FlexPage(PlayerModel model)
        {
            model.Navigation = Navigation;
            InitializeComponent();
            if (!model.ChildModel)
            {
                ToolbarItem exit = new ToolbarItem()
                {
                    Text = "Exit"
                };
                exit.Clicked += ToolbarItem_Clicked;
                ToolbarItems.Add(exit);
                ToolbarItem undo = new ToolbarItem()
                {
                    Text = "Undo"
                };
                undo.SetBinding(MenuItem.CommandProperty, new Binding("Undo"));
                ToolbarItems.Add(undo);
                ToolbarItem redo = new ToolbarItem()
                {
                    Text = "Redo"
                };
                redo.SetBinding(MenuItem.CommandProperty, new Binding("Redo"));
                ToolbarItems.Add(redo);
            }
            BindingContext = Model = model;
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
                    await Navigation.PopModalAsync();
                }
                else
                {
                    Model.FirePlayerChanged();
                }
            });
            return !Model.ChildModel;
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

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (sender is BindableObject bindableObject)
            {
                if (bindableObject.BindingContext is SubModel s)
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
        }
    }
}