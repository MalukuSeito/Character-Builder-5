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
using CB_5e.ViewModels.Modify.Collections;
using CB_5e.ViewModels.Modify.Descriptions;
using CB_5e.Services;

namespace CB_5e.Views.Modify.Descriptions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NamesEditPage : ContentPage
	{
        private NamesViewModel entries;
        public ObservableRangeCollection<StringViewModel> Entries { get; set; } = new ObservableRangeCollection<StringViewModel>();
        public string Property { get; private set; }
        private int move = -1;

        public NamesEditPage(NamesViewModel names)
        {
            entries = names;
            UpdateEntries();
			InitializeComponent ();
            BindingContext = this;
		}


        public string Name
        {
            get => entries.Entry.Title;
            set
            {
                if (value == entries.Entry.Title) return;
                entries.Entry.Title = value;
                OnPropertyChanged("Name");
                entries.Refresh();
            }
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == Property) UpdateEntries();
        }

        private void UpdateEntries()
        {
            Fill();
        }
        private void Fill() => Entries.ReplaceRange(entries.Entry.ListOfNames.Select(f => new StringViewModel(f)));

        private void Paste2_Clicked(object sender, EventArgs e)
        {
            try
            {
                string clip = DependencyService.Get<IClipboardService>().GetTextData();
                if (clip != null)
                {
                    foreach (string s in clip.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string r = s.Trim();
                        if (r != "") entries.Entry.ListOfNames.Add(r);
                    }
                    entries.Refresh();
                    Fill();
                }
            } catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private void Paste_Clicked(object sender, EventArgs e)
        {
            try
            {
                string clip = DependencyService.Get<IClipboardService>().GetTextData();
                if (clip != null)
                {
                    entries.Entry.ListOfNames.Add(clip);
                    entries.Refresh();
                    Fill();
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CustomTextEntryPage(Title, new Command((par) =>
            {
                if (par is string s)
                {
                    StringViewModel vm = new StringViewModel(s);
                    entries.Entry.ListOfNames.Add(s);
                    Entries.Add(vm);
                    entries.Refresh();
                }
            })));
        }

        private async void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is StringViewModel fvm) {
                if (move >= 0)
                {
                    foreach (StringViewModel ff in Entries) ff.Moving = false;
                    int target = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (target >= 0 && move != target)
                    {
                        entries.Entry.ListOfNames.Insert(target, entries.Entry.ListOfNames[move]);
                        if (target < move) move++;
                        entries.Entry.ListOfNames.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                        entries.Refresh();
                    }
                    move = -1;
                } else
                {
                    int i = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (i >= 0) await Navigation.PushAsync(new CustomTextEntryPage(Title, new Command((par) =>
                    {
                        if (par is string s)
                        {
                            entries.Entry.ListOfNames[i] = s;
                            fvm.Text = s;
                            fvm.Refresh();
                            entries.Refresh();
                        }
                    }), null, fvm.Text));
                    (sender as ListView).SelectedItem = null;
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is StringViewModel f)
            {
                int i = Entries.ToList().FindIndex(ff => f == ff);
                entries.Entry.ListOfNames.RemoveAt(i);
                Fill();
                entries.Refresh();
            }
        }

        private async void Info_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is StringViewModel f) await Navigation.PushAsync(InfoPage.Show(new Feature("Text", f.Text)));
        }

        private void Move_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is StringViewModel f)
            {
                foreach (StringViewModel ff in Entries) ff.Moving = false;
                f.Moving = true;
                move = Entries.ToList().FindIndex(ff => f == ff);
            }
            
        }

        private void Cut_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is StringViewModel f)
            {
                DependencyService.Get<IClipboardService>().PutTextData(f.Text, "String");
                int i = Entries.ToList().FindIndex(ff => f == ff);
                entries.Entry.ListOfNames.RemoveAt(i);
                entries.Refresh();
                Fill();
            }
        }

        private void Copy_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is StringViewModel f)
            {
                DependencyService.Get<IClipboardService>().PutTextData(f.Text, "String");
            }
        }
    }
}