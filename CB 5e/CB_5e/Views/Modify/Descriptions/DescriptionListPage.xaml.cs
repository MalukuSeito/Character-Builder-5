using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CB_5e.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CB_5e.ViewModels;
using OGL.Descriptions;
using OGL;
using CB_5e.ViewModels.Modify;
using CB_5e.ViewModels.Modify.Descriptions;
using CB_5e.Services;

namespace CB_5e.Views.Modify.Descriptions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DescriptionListPage : ContentPage
	{
        private List<Description> descriptions;
        public ObservableRangeCollection<DescriptionViewModel> Descriptions { get; set; } = new ObservableRangeCollection<DescriptionViewModel>();
        public IEditModel Model { get; private set; }
        public string Property { get; private set; }
        private int move = -1;
        private bool Modal = true;
        public Command Undo { get => Model.Undo; }
        public Command Redo { get => Model.Redo; }

        public DescriptionListPage (IEditModel parent, string property, bool modal = true)
		{
            Model = parent;
            Modal = modal;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            UpdateDescriptions();
			InitializeComponent ();
            InitToolbar(Modal);
            BindingContext = this;
        }

        private void InitToolbar(bool modal)
        {
            if (Modal)
            {
                ToolbarItem undo = new ToolbarItem() { Text = "Undo" };
                undo.SetBinding(MenuItem.CommandProperty, new Binding("Undo"));
                ToolbarItems.Add(undo);
                ToolbarItem redo = new ToolbarItem() { Text = "Redo" };
                redo.SetBinding(MenuItem.CommandProperty, new Binding("Redo"));
                ToolbarItems.Add(redo);
            }
            ToolbarItem add = new ToolbarItem() { Text = "Add" };
            add.Clicked += Add_Clicked;
            ToolbarItems.Add(add);
            ToolbarItem paste = new ToolbarItem() { Text = "Paste" };
            paste.Clicked += Paste_Clicked;
            ToolbarItems.Add(paste);
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == Property) UpdateDescriptions();
        }

        private void UpdateDescriptions()
        {
            descriptions = (List<Description>)Model.GetType().GetRuntimeProperty(Property).GetValue(Model);
            Fill();
        }
        private void Fill() => Descriptions.ReplaceRange(descriptions.Select(f => new DescriptionViewModel(f)));

        private void Paste_Clicked(object sender, EventArgs e)
        {
            try
            {
                Model.MakeHistory();
                foreach (Description f in DescriptionContainer.LoadString(DependencyService.Get<IClipboardService>().GetTextData()).Descriptions) descriptions.Add(f);
                Fill();
            } catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectPage(new List<SelectOption>()
                {
                    new SelectOption("Description", "Simple Description", new Description()),
                    new SelectOption("Lists of Names", "Description with an attached lists of names", new ListDescription()),
                    new SelectOption("Table Description", "Description with an attached table of rolls", new TableDescription())
                }, new Command(async (par) => {
                    if (par is SelectOption o && o.Value is  Description d)
                    {
                        DescriptionViewModel fvm = new DescriptionViewModel(d);
                        Model.MakeHistory();
                        descriptions.Add(d);
                        Descriptions.Add(fvm);
                        if (fvm.Description is TableDescription td)
                        {
                            TabbedPage p = new TabbedPage();
                            p.Children.Add(new NavigationPage(new EditDescription(fvm, Model))
                            {
                                Title = "Description"
                            });
                            p.Children.Add(new NavigationPage(new EditTableDescription(Model, td))
                            {
                                Title = "Table"
                            });
                            await Navigation.PushModalAsync(p);

                        }
                        else if (fvm.Description is ListDescription ld)
                        {
                            TabbedPage p = new TabbedPage();
                            p.Children.Add(new NavigationPage(new EditDescription(fvm, Model))
                            {
                                Title = "Description"
                            });
                            p.Children.Add(new NavigationPage(new EditListDescription(Model, ld))
                            {
                                Title = "Names"
                            });
                            await Navigation.PushModalAsync(p);

                        }
                        else await Navigation.PushModalAsync(new NavigationPage(new EditDescription(fvm, Model))
                        {
                            Title = "Description"
                        });
                    }
                })));
        }

        private async void Descriptions_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is DescriptionViewModel fvm) {
                if (move >= 0)
                {
                    Model.MakeHistory();
                    foreach (DescriptionViewModel ff in Descriptions) ff.Moving = false;
                    int target = descriptions.FindIndex(ff => fvm.Description == ff);
                    if (target >= 0 && move != target)
                    {
                        descriptions.Insert(target, descriptions[move]);
                        if (target < move) move++;
                        descriptions.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                    }
                    move = -1;
                } else
                {
                    if (fvm.Description is TableDescription td)
                    {
                        TabbedPage p = new TabbedPage();
                        p.Children.Add(new NavigationPage(new EditDescription(fvm, Model))
                        {
                            Title = "Description"
                        });
                        p.Children.Add(new NavigationPage(new EditTableDescription(Model, td))
                        {
                            Title = "Table"
                        });
                        await Navigation.PushModalAsync(p);

                    }
                    else if (fvm.Description is ListDescription ld)
                    {
                        TabbedPage p = new TabbedPage();
                        p.Children.Add(new NavigationPage(new EditDescription(fvm, Model))
                        {
                            Title = "Description"
                        });
                        p.Children.Add(new NavigationPage(new EditListDescription(Model, ld))
                        {
                            Title = "Names"
                        });
                        await Navigation.PushModalAsync(p);

                    }
                    else await Navigation.PushModalAsync(new NavigationPage(new EditDescription(fvm, Model))
                    {
                        Title = "Description"
                    });
                    (sender as ListView).SelectedItem = null;
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is DescriptionViewModel f)
            {
                Model.MakeHistory();
                int i = descriptions.FindIndex(ff => f.Description == ff);
                descriptions.RemoveAt(i);
                Fill();
            }
        }

        private async void Info_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is DescriptionViewModel f) await Navigation.PushAsync(InfoPage.Show(new DescriptionContainer(f.Description)));
        }

        private void Move_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is DescriptionViewModel f)
            {
                foreach (DescriptionViewModel ff in Descriptions) ff.Moving = false;
                f.Moving = true;
                move = descriptions.FindIndex(ff => f.Description == ff);
            }
            
        }

        private void Cut_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is DescriptionViewModel f)
            {
                Model.MakeHistory();
                DependencyService.Get<IClipboardService>().PutTextData(f.Description.Save(), f.Detail);
                int i = descriptions.FindIndex(ff => f.Description == ff);
                descriptions.RemoveAt(i);
                Fill();
            }
        }

        private void Copy_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is DescriptionViewModel f)
            {
                DependencyService.Get<IClipboardService>().PutTextData(f.Description.Save(), f.Detail);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (Modal)
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
            }
            return Modal;
        }
    }
}