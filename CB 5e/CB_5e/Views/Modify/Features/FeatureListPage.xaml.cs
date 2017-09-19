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
using OGL.Features;
using CB_5e.ViewModels.Modify;
using CB_5e.ViewModels.Modify.Features;
using CB_5e.Services;

namespace CB_5e.Views.Modify.Features
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeatureListPage : ContentPage
	{
        private List<Feature> features;
        public ObservableRangeCollection<FeatureViewModel> Features { get; set; } = new ObservableRangeCollection<FeatureViewModel>();
        public IEditModel Model { get; private set; }
        public string Property { get; private set; }
        private int move = -1;
        private bool Modal = true;

        public Command Undo { get => Model.Undo; }
        public Command Redo { get => Model.Redo; }

        public FeatureListPage (IEditModel parent, string property, bool modal = true)
		{
            Model = parent;
            Modal = modal;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            UpdateFeatures();
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
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == Property) UpdateFeatures();
        }

        private void UpdateFeatures()
        {
            features = (List<Feature>)Model.GetType().GetRuntimeProperty(Property).GetValue(Model);
            Fill();
        }
        private void Fill() => Features.ReplaceRange(features.Select(f => new FeatureViewModel(f)));

        private void Paste_Clicked(object sender, EventArgs e)
        {
            try
            {
                Model.MakeHistory();
                foreach (Feature f in Feature.LoadString(DependencyService.Get<IClipboardService>().GetTextData())) features.Add(f);
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
                    new SelectOption("Basic Feature", "Text and description", new Feature()),
                    new SelectOption("Resource Feature", "Defines or modifies a trackable resource", new ResourceFeature()),
                    new SelectOption("Ability Score Feature", "Modifies ability scores or their maximum values", new AbilityScoreFeature()),
                    new SelectOption("Ability Score Increase / Feat Feature", "Option to increase 2 ability scores or gain a feat", new AbilityScoreFeatFeature(null, 4))
                }, new Command(async (par) => {
                    if (par is SelectOption o && o.Value is Feature d)
                    {
                        FeatureViewModel fvm = new FeatureViewModel(d);
                        Model.MakeHistory();
                        features.Add(d);
                        Features.Add(fvm);
                        await Edit(fvm);
                    }
                })));
        }

        private async void Features_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is FeatureViewModel fvm) {
                if (move >= 0)
                {
                    Model.MakeHistory();
                    foreach (FeatureViewModel ff in Features) ff.Moving = false;
                    int target = features.FindIndex(ff => fvm.Feature == ff);
                    if (target >= 0 && move != target)
                    {
                        features.Insert(target, features[move]);
                        if (target < move) move++;
                        features.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                    }
                    move = -1;
                } else
                {
                    await Edit(fvm);
                    (sender as ListView).SelectedItem = null;
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is FeatureViewModel f)
            {
                Model.MakeHistory();
                int i = features.FindIndex(ff => f.Feature == ff);
                features.RemoveAt(i);
                Fill();
            }
        }

        private async void Info_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is FeatureViewModel f) await Navigation.PushAsync(InfoPage.Show(f.Feature));
        }

        private void Move_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is FeatureViewModel f)
            {
                foreach (FeatureViewModel ff in Features) ff.Moving = false;
                f.Moving = true;
                move = features.FindIndex(ff => f.Feature == ff);
            }
            
        }

        private void Cut_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is FeatureViewModel f)
            {
                Model.MakeHistory();
                DependencyService.Get<IClipboardService>().PutTextData(f.Feature.Save(), f.Detail);
                int i = features.FindIndex(ff => f.Feature == ff);
                features.RemoveAt(i);
                Fill();
            }
        }

        private void Copy_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is FeatureViewModel f)
            {
                DependencyService.Get<IClipboardService>().PutTextData(f.Feature.Save(), f.Detail);
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

        private async Task Edit(FeatureViewModel fvm)
        {
            TabbedPage p;
            if (fvm.Feature is ResourceFeature rf)
            {
                ResourceFeatureEditModel model = new ResourceFeatureEditModel(rf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditResourceFeature(model))
                {
                    Title = "Resource"
                });
            }
            else if (fvm.Feature is AbilityScoreFeature asf)
            {
                AbilityFeatureEditModel model = new AbilityFeatureEditModel(asf, Model, fvm);
                p = Tab(model);
                p.Children.Add(new NavigationPage(new EditAbilityFeature(model))
                {
                    Title = "Abilities"
                });
            }
            else if (fvm.Feature is AbilityScoreFeatFeature aff)
            {
                AbilityFeatFeatureEditModel model = new AbilityFeatFeatureEditModel(aff, Model, fvm);
                p = new TabbedPage();
                p.Children.Add(new NavigationPage(new EditAbilityFeatFeature(model))
                {
                    Title = "Feature"
                });
                p.Children.Add(new NavigationPage(new FeatureKeywords(model))
                {
                    Title = "Standalone"
                });
            }
            else {
                IFeatureEditModel model = new FeatureEditModel<Feature>(fvm.Feature, Model, fvm);
                p = Tab(model);
            }
            await Navigation.PushModalAsync(p);
        }

        private TabbedPage Tab(IFeatureEditModel model)
        {
            TabbedPage p = new TabbedPage();

            p.Children.Add(new NavigationPage(new EditFeature(model))
            {
                Title = "Feature"
            });
            p.Children.Add(new NavigationPage(new FeatureKeywords(model))
            {
                Title = "Standalone"
            });
            return p;
        }
    }
}