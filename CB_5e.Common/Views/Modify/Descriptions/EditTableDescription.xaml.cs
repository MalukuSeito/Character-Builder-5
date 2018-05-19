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
using CB_5e.ViewModels.Modify;
using CB_5e.ViewModels.Modify.Collections;
using CB_5e.Views.Modify.Collections;
using CB_5e.Services;

namespace CB_5e.Views.Modify.Descriptions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTableDescription : ContentPage
	{
        public ObservableRangeCollection<TableEntryViewModel> Entries { get; set; } = new ObservableRangeCollection<TableEntryViewModel>();
        public IEditModel Model { get; private set; }
        private int move = -1;
        public bool Changed { get; private set; } = false;
        public TableDescription Desc { get; private set; }

        public int Amount
        {
            get => Desc.Amount;
            set
            {
                if (value == Desc.Amount) return;
                Desc.Amount = value;
                Changed = true;
                OnPropertyChanged("Amount");
            }
        }

        public string Name
        {
            get => Desc.TableName;
            set
            {
                if (value == Desc.TableName) return;
                Desc.TableName = value;
                Changed = true;
                OnPropertyChanged("Name");
            }
        }

        public string UniqueID
        {
            get => Desc.UniqueID;
            set
            {
                if (value == Desc.UniqueID) return;
                Desc.UniqueID = value;
                Changed = true;
                OnPropertyChanged("UniqueID");
            }
        }

        public bool Bond
        {
            get => Desc.BackgroundOption.HasFlag(BackgroundOption.Bond);
            set {
                if (value == Bond) return;
                if (Bond) Desc.BackgroundOption &= ~BackgroundOption.Bond;
                else Desc.BackgroundOption |= BackgroundOption.Bond;
                Changed = true;
            }
        }

        public bool Flaw
        {
            get => Desc.BackgroundOption.HasFlag(BackgroundOption.Flaw);
            set
            {
                if (value == Flaw) return;
                if (Flaw) Desc.BackgroundOption &= ~BackgroundOption.Flaw;
                else Desc.BackgroundOption |= BackgroundOption.Flaw;
                Changed = true;
            }
        }

        public bool Trait
        {
            get => Desc.BackgroundOption.HasFlag(BackgroundOption.Trait);
            set
            {
                if (value == Trait) return;
                if (Trait) Desc.BackgroundOption &= ~BackgroundOption.Trait;
                else Desc.BackgroundOption |= BackgroundOption.Trait;
                Changed = true;
            }
        }

        public bool Ideal
        {
            get => Desc.BackgroundOption.HasFlag(BackgroundOption.Ideal);
            set
            {
                if (value == Ideal) return;
                if (Ideal) Desc.BackgroundOption &= ~BackgroundOption.Ideal;
                else Desc.BackgroundOption |= BackgroundOption.Ideal;
                Changed = true;
            }
        }

        public EditTableDescription(IEditModel parent, TableDescription description)
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
        private void Fill() => Entries.ReplaceRange(Desc.Entries.Select(f => new TableEntryViewModel(f)));

        private void Paste_Clicked(object sender, EventArgs e)
        {
            try
            {
                Changed = true;
                foreach (Description d in DescriptionContainer.LoadString(DependencyService.Get<IClipboardService>().GetTextData()).Descriptions)
                {
                    if (d is TableDescription td)
                    {
                        foreach (TableEntry f in td.Entries) Desc.Entries.Add(f);
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
            TableEntry t = new TableEntry();
            TableEntryViewModel vm = new TableEntryViewModel(t);
            Desc.Entries.Add(t);
            Entries.Add(vm);
            await Navigation.PushAsync(new TableEntryEditPage(vm));
        }

        private async void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is TableEntryViewModel fvm) {
                if (move >= 0)
                {
                    Changed = true;
                    foreach (TableEntryViewModel ff in Entries) ff.Moving = false;
                    int target = Desc.Entries.FindIndex(ff => fvm.Entry == ff);
                    if (target >= 0 && move != target)
                    {
                        Desc.Entries.Insert(target, Desc.Entries[move]);
                        if (target < move) move++;
                        Desc.Entries.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                    }
                    move = -1;
                } else
                {
                    Changed = true;
                    await Navigation.PushAsync(new TableEntryEditPage(fvm));
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is TableEntryViewModel f)
            {
                Changed = true;
                int i = Desc.Entries.FindIndex(ff => f.Entry == ff);
                Desc.Entries.RemoveAt(i);
                Fill();
            }
        }

        private async void Info_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is TableEntryViewModel f) await Navigation.PushAsync(InfoPage.Show(new Feature(f.Text, f.Detail)));
        }

        private void Move_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is TableEntryViewModel f)
            {
                foreach (TableEntryViewModel ff in Entries) ff.Moving = false;
                f.Moving = true;
                move = Desc.Entries.FindIndex(ff => f.Entry == ff);
            }
            
        }

        private void Cut_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is TableEntryViewModel f)
            {
                Changed = true;
                DependencyService.Get<IClipboardService>().PutTextData(f.Entry.Save(), f.Detail);
                int i = Desc.Entries.FindIndex(ff => f.Entry == ff);
                Desc.Entries.RemoveAt(i);
                Fill();
            }
        }

        private void Copy_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is TableEntryViewModel f)
            {
                DependencyService.Get<IClipboardService>().PutTextData(f.Entry.Save(), f.Detail);
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