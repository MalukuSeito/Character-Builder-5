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


namespace CB_5e.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeatureListPage : ContentPage
	{
        private List<Feature> features;
        public ObservableRangeCollection<FeatureViewModel> Features { get; set; } = new ObservableRangeCollection<FeatureViewModel>();
        public IEditModel Model { get; private set; }
        public string Property { get; private set; }
        private int move = -1;

        public FeatureListPage (IEditModel parent, string property)
		{
            Model = parent;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            UpdateFeatures();
			InitializeComponent ();
            BindingContext = this;
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

        private void Add_Clicked(object sender, EventArgs e)
        {

        }

        private void Features_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
    }
}