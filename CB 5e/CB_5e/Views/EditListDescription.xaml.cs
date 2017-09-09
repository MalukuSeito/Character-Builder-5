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
using OGL.Descriptions;
using OGL;

namespace CB_5e.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditListDescription : ContentPage
	{
        public ObservableRangeCollection<NamesViewModel> Entries { get; set; } = new ObservableRangeCollection<NamesViewModel>();
        public IEditModel Model { get; private set; }
        private int move = -1;
        public bool Changed { get; private set; } = false;
        public ListDescription Desc { get; private set; }

        public EditListDescription(IEditModel parent, ListDescription description)
        {
            Model = parent;
            Desc = description;
            UpdateEntries();
			InitializeComponent ();
            BindingContext = this;

        }

        private void UpdateEntries()
        {
            Fill();
        }
        private void Fill() => Entries.ReplaceRange(Desc.Names.Select(f => new NamesViewModel(f)));

        private void Paste_Clicked(object sender, EventArgs e)
        {
            try
            {
                Changed = true;
                foreach (Description d in DescriptionContainer.LoadString(DependencyService.Get<IClipboardService>().GetTextData()).Descriptions)
                {
                    if (d is ListDescription td)
                    {
                        foreach (Names f in td.Names) Desc.Names.Add(f);
                    }
                }
                Fill();
            } catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            Changed = true;
            Names t = new Names();
            NamesViewModel vm = new NamesViewModel(t);
            Desc.Names.Add(t);
            Entries.Add(vm);
            await Navigation.PushAsync(new NamesEditPage(vm));
        }

        private async void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is NamesViewModel fvm) {
                if (move >= 0)
                {
                    Changed = true;
                    foreach (NamesViewModel ff in Entries) ff.Moving = false;
                    int target = Desc.Names.FindIndex(ff => fvm.Entry == ff);
                    if (target >= 0 && move != target)
                    {
                        Desc.Names.Insert(target, Desc.Names[move]);
                        if (target < move) move++;
                        Desc.Names.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                    }
                    move = -1;
                } else
                {
                    Changed = true;
                    await Navigation.PushAsync(new NamesEditPage(fvm));
                    (sender as ListView).SelectedItem = null;
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is NamesViewModel f)
            {
                Changed = true;
                int i = Desc.Names.FindIndex(ff => f.Entry == ff);
                Desc.Names.RemoveAt(i);
                Fill();
            }
        }

        private async void Info_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is NamesViewModel f) await Navigation.PushAsync(InfoPage.Show(new Feature(f.Text, f.Detail)));
        }

        private void Move_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is NamesViewModel f)
            {
                foreach (NamesViewModel ff in Entries) ff.Moving = false;
                f.Moving = true;
                move = Desc.Names.FindIndex(ff => f.Entry == ff);
            }
            
        }

        private void Cut_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is NamesViewModel f)
            {
                Changed = true;
                DependencyService.Get<IClipboardService>().PutTextData(f.Save(), f.Detail);
                int i = Desc.Names.FindIndex(ff => f.Entry == ff);
                Desc.Names.RemoveAt(i);
                Fill();
            }
        }

        private void Copy_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is NamesViewModel f)
            {
                DependencyService.Get<IClipboardService>().PutTextData(f.Save(), f.Detail);
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (Changed) Model.MakeHistory("");
            Changed = false;
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            if (Changed) Model.MakeHistory("");
            Changed = false;
            return base.OnBackButtonPressed();
        }
    }
}