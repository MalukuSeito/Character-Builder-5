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

namespace CB_5e.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DescriptionListPage : ContentPage
	{
        private List<Description> descriptions;
        public ObservableRangeCollection<DescriptionViewModel> Descriptions { get; set; } = new ObservableRangeCollection<DescriptionViewModel>();
        public IEditModel Model { get; private set; }
        public string Property { get; private set; }
        private int move = -1;

        public DescriptionListPage (IEditModel parent, string property)
		{
            Model = parent;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            UpdateDescriptions();
			InitializeComponent ();
            BindingContext = this;
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

        private void Add_Clicked(object sender, EventArgs e)
        {

        }

        private void Descriptions_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
    }
}