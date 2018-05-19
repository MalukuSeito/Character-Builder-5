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
using CB_5e.Services;
using CB_5e.ViewModels.Modify;
using OGL.Spells;

namespace CB_5e.Views.Modify
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditCantripDamage : ContentPage
	{
        public ObservableRangeCollection<CantripDamageViewModel> Entries { get; set; } = new ObservableRangeCollection<CantripDamageViewModel>();
        public SpellEditModel Model { get; private set; }
        private int move = -1;
        public List<string> CastingTimes { get => Model.CastingTimes; }
        public List<string> Ranges { get => Model.Ranges; }
        public List<string> Durations { get => Model.Durations; }
        public string CastingTime
        {
            get => Model.CastingTime;
            set
            {
                if (value == null) return;
                Model.CastingTime = value;
            }
        }
        public string Range
        {
            get => Model.Range;
            set
            {
                if (value == null) return;
                Model.Range = value;
            }
        }
        public string Duration
        {
            get => Model.Duration;
            set
            {
                if (value == null) return;
                Model.Duration = value;
            }
        }
        public int Level
        {
            get => Model.Level;
            set => Model.Level = value;
        }

        public string FormsCompanionsFilter { get => Model.FormsCompanionsFilter; set => Model.FormsCompanionsFilter = value; }
        public int FormsCompanionsCount { get => Model.FormsCompanionsCount; set => Model.FormsCompanionsCount = value; }
        public int FormsCompanionsCountValue { get => Model.FormsCompanionsCount + 1; set => Model.FormsCompanionsCount = value - 1; }

        public Keyboard Keyboard { get; private set; } = Keyboard.Numeric;
        public EditCantripDamage(SpellEditModel parent)
        {
            Model = parent;
            parent.PropertyChanged += Parent_PropertyChanged;
            Fill();
			InitializeComponent ();
            InitToolbar();
            BindingContext = this;
		}

        private void InitToolbar()
        {
            ToolbarItem undo = new ToolbarItem() { Text = "Undo" };
            undo.SetBinding(MenuItem.CommandProperty, new Binding("Undo"));
            ToolbarItems.Add(undo);
            ToolbarItem redo = new ToolbarItem() { Text = "Redo" };
            redo.SetBinding(MenuItem.CommandProperty, new Binding("Redo"));
            ToolbarItems.Add(redo);
            ToolbarItem add = new ToolbarItem() { Text = "Add" };
            add.Clicked += Add_Clicked;
            ToolbarItems.Add(add);
            ToolbarItem paste = new ToolbarItem() { Text = "Paste" };
            paste.Clicked += Paste_Clicked;
            ToolbarItems.Add(paste);
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == "CantripDamage") Fill();
            OnPropertyChanged(e.PropertyName);
        }

        private void Fill()
        {
            List<CantripDamageViewModel> res = new List<CantripDamageViewModel>(Model.CantripDamage.Count);
            for (int i = 0; i < Model.CantripDamage.Count; i++)
            {
                res.Add(new CantripDamageViewModel(Model.CantripDamage[i]));
            }
            Entries.ReplaceRange(res);
        }

        private void Paste_Clicked(object sender, EventArgs e)
        {
            try
            {
                Model.MakeHistory();
                string clip = DependencyService.Get<IClipboardService>().GetTextData();
                if (clip != null)
                {
                    string[] s = clip.Split(new char[] { ':' }, 2);
                    if (s.Length == 2 &&  int.TryParse(s[0], out int i)) {
                        Model.CantripDamage.Add(new CantripDamage(i, s[1].Trim()));
                        Fill();
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CustomDualTextEntryPage("Cantrip Damage", new Command((par) =>
            {
                if (par is string[] s && int.TryParse(s[0], out int i))
                {
                    Model.MakeHistory();
                    CantripDamage d = new CantripDamage(i, s[1]);
                    Model.CantripDamage.Add(d);
                    Entries.Add(new CantripDamageViewModel(d));
                }
            }), "Level: ", "Damage: ", Keyboard.Numeric, Keyboard.Default, "1", "1d6"));
        }

        private async void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is CantripDamageViewModel fvm)
            {
                if (move >= 0)
                {
                    Model.MakeHistory();
                    foreach (CantripDamageViewModel ff in Entries) ff.Moving = false;
                    int target = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (target >= 0 && move != target)
                    {
                        Model.CantripDamage.Insert(target, Model.CantripDamage[move]);
                        if (target < move) move++;
                        Model.CantripDamage.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                    }
                    move = -1;
                }
                else
                {
                    int i = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (i >= 0) await Navigation.PushAsync(new CustomDualTextEntryPage("Cantrip Damage", new Command((par) =>
                    {
                        if (par is string[] s && int.TryParse(s[0], out int ii))
                        {
                            Model.MakeHistory();
                            fvm.Entry.Damage = s[1];
                            fvm.Entry.Level = ii;
                            fvm.Refresh();
                        }
                    }), "Level: ", "Damage: ", Keyboard.Numeric, Keyboard.Default, fvm.Entry.Level.ToString(), fvm.Entry.Damage));
                    (sender as ListView).SelectedItem = null;
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is CantripDamageViewModel f)
            {
                Model.MakeHistory();
                int i = Entries.ToList().FindIndex(ff => f == ff);
                Model.CantripDamage.RemoveAt(i);
                Fill();
            }
        }

        private async void Info_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is CantripDamageViewModel f) await Navigation.PushAsync(InfoPage.Show(new Feature("Text", f.Text)));
        }

        private void Move_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is CantripDamageViewModel f)
            {
                foreach (CantripDamageViewModel ff in Entries) ff.Moving = false;
                f.Moving = true;
                move = Entries.ToList().FindIndex(ff => f == ff);
            }

        }

        private void Cut_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is CantripDamageViewModel f)
            {
                Model.MakeHistory();
                DependencyService.Get<IClipboardService>().PutTextData(f.Text, "String");
                int i = Entries.ToList().FindIndex(ff => f == ff);
                Model.CantripDamage.RemoveAt(i);
                Fill();
            }
        }

        private void Copy_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is IntViewModel f)
            {
                DependencyService.Get<IClipboardService>().PutTextData(f.Text, "String");
            }
        }

        protected override bool OnBackButtonPressed()
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
            return true;
        }
    }
}