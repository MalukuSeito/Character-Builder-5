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
using CB_5e.Services;
using CB_5e.ViewModels.Modify.Features;

namespace CB_5e.Views.Modify.Features
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditMulticlassing : ContentPage
	{
        private List<string> suggestions = new List<string>();
        public ObservableRangeCollection<IntViewModel> Entries { get; set; } = new ObservableRangeCollection<IntViewModel>();
        public ClassEditModel Model { get; private set; }
        private int move = -1;
        public string MulticlassingCondition
        {
            get => Model.MulticlassingCondition;
            set => Model.MulticlassingCondition = value;
        }

        public bool AvailableAtFirstLevel
        {
            get => Model.AvailableAtFirstLevel;
            set => Model.AvailableAtFirstLevel = value;
        }
        public string Prepend { get; private set; } = "Level ";
        public Keyboard Keyboard { get; private set; } = Keyboard.Numeric;
        public EditMulticlassing(ClassEditModel parent)
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
            ToolbarItem paste2 = new ToolbarItem() { Text = "Paste CSV" };
            paste2.Clicked += Paste2_Clicked;
            ToolbarItems.Add(paste2);
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == "MulticlassingSpellLevels") Fill();
            OnPropertyChanged(e.PropertyName);
        }

        private void Fill()
        {
            List<IntViewModel> res = new List<IntViewModel>(Model.MulticlassingSpellLevels.Count);
            for (int i = 0; i < Model.MulticlassingSpellLevels.Count; i++)
            {
                res.Add(new IntViewModel(i + 1, Model.MulticlassingSpellLevels[i], Prepend));
            }
            Entries.ReplaceRange(res);
        }
        private void Paste2_Clicked(object sender, EventArgs e)
        {
            try
            {
                Model.MakeHistory();
                string clip = DependencyService.Get<IClipboardService>().GetTextData();
                if (clip != null)
                {
                    foreach (string s in clip.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string r = s.Trim();
                        if (r != "" && int.TryParse(r, out int i)) Model.MulticlassingSpellLevels.Add(i);
                    }
                    Fill();
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private void Paste_Clicked(object sender, EventArgs e)
        {
            try
            {
                Model.MakeHistory();
                string clip = DependencyService.Get<IClipboardService>().GetTextData();
                if (clip != null && int.TryParse(clip, out int i))
                {
                    Model.MulticlassingSpellLevels.Add(i);
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
                if (par is string s && int.TryParse(s, out int i))
                {
                    Model.MakeHistory();
                    IntViewModel vm = new IntViewModel(Model.MulticlassingSpellLevels.Count + 1, i, Prepend);
                    Model.MulticlassingSpellLevels.Add(i);
                    Entries.Add(vm);
                }
            }), Keyboard));
        }

        private async void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is IntViewModel fvm)
            {
                if (move >= 0)
                {
                    Model.MakeHistory();
                    foreach (IntViewModel ff in Entries) ff.Moving = false;
                    int target = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (target >= 0 && move != target)
                    {
                        Model.MulticlassingSpellLevels.Insert(target, Model.MulticlassingSpellLevels[move]);
                        if (target < move) move++;
                        Model.MulticlassingSpellLevels.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                    }
                    move = -1;
                }
                else
                {
                    int i = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (i >= 0) await Navigation.PushAsync(new CustomTextEntryPage(Title, new Command((par) =>
                    {
                        if (par is string s && int.TryParse(s, out int ii))
                        {
                            Model.MakeHistory();
                            Model.MulticlassingSpellLevels[i] = ii;
                            fvm.Value = ii;
                            fvm.Refresh();
                        }
                    }), Keyboard, fvm.Text));
                    (sender as ListView).SelectedItem = null;
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is IntViewModel f)
            {
                Model.MakeHistory();
                int i = Entries.ToList().FindIndex(ff => f == ff);
                Model.MulticlassingSpellLevels.RemoveAt(i);
                Fill();
            }
        }

        private async void Info_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is IntViewModel f) await Navigation.PushAsync(InfoPage.Show(new Feature("Text", f.Text)));
        }

        private void Move_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is IntViewModel f)
            {
                foreach (IntViewModel ff in Entries) ff.Moving = false;
                f.Moving = true;
                move = Entries.ToList().FindIndex(ff => f == ff);
            }

        }

        private void Cut_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is IntViewModel f)
            {
                Model.MakeHistory();
                DependencyService.Get<IClipboardService>().PutTextData(f.Text, "String");
                int i = Entries.ToList().FindIndex(ff => f == ff);
                Model.MulticlassingSpellLevels.RemoveAt(i);
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