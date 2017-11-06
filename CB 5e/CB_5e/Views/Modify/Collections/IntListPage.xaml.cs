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

namespace CB_5e.Views.Modify.Collections
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IntListPage : ContentPage
	{
        private List<int> entries;
        public ObservableRangeCollection<IntViewModel> Entries { get; set; } = new ObservableRangeCollection<IntViewModel>();
        public int Offset { get => off; set { off = value; Fill(); } }
        public string Format { get; private set; }
        public IEditModel Model { get; private set; }
        public string Property { get; private set; }
        public string Prepend { get; private set; }
        private int off = 1;
        private int move = -1;
        private bool TopLevelPage = true;
        public Keyboard Keyboard = Keyboard.Numeric;
        public Command Undo { get => Model.Undo; }
        public Command Save { get => Model.Save; }
        public Command Redo { get => Model.Redo; }

        public IntListPage(IEditModel parent, string property, string prepend = "Level ", string format = "0", Keyboard keyboard = null, bool toplevel = true, bool save = false, int offset = 1)
        {
            off = offset;
            Format = format;
            Model = parent;
            Prepend = prepend;
            TopLevelPage = toplevel;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            UpdateEntries();
			InitializeComponent ();
            InitToolbar(save);
            BindingContext = this;
            Keyboard = keyboard;
		}

        private void InitToolbar(bool save)
        {
            if (TopLevelPage)
            {
                if (save)
                {
                    ToolbarItem s = new ToolbarItem() { Text = "Save" };
                    s.SetBinding(MenuItem.CommandProperty, new Binding("Save"));
                    ToolbarItems.Add(s);
                }
                ToolbarItem undo = new ToolbarItem() { Text = "Undo" };
                undo.SetBinding(MenuItem.CommandProperty, new Binding("Undo"));
                ToolbarItems.Add(undo);
                ToolbarItem redo = new ToolbarItem() { Text = "Redo" };
                redo.SetBinding(MenuItem.CommandProperty, new Binding("Redo"));
                ToolbarItems.Add(redo);
            }
            else
            {
                ToolbarItem back = new ToolbarItem() { Text = "back" };
                back.Clicked += Back_Clicked;
                ToolbarItems.Add(back);
            }
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
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == Property) UpdateEntries();
        }

        public void UpdateEntries()
        {
            entries = (List<int>)Model.GetType().GetRuntimeProperty(Property).GetValue(Model);
            Fill();
        }
        private void Fill()
        {
            if (entries == null) return;
            List<IntViewModel> res = new List<IntViewModel>(entries.Count);
            for (int i = 0; i < entries.Count; i++)
            {
                res.Add(new IntViewModel(i + Offset, entries[i], Prepend, Format));
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
                        if (r != "" && int.TryParse(r, out int i)) entries.Add(i);
                    }
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
                Model.MakeHistory();
                string clip = DependencyService.Get<IClipboardService>().GetTextData();
                if (clip != null && int.TryParse(clip, out int i))
                {
                    entries.Add(i);
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
                    IntViewModel vm = new IntViewModel(entries.Count + Offset, i, Prepend);
                    entries.Add(i);
                    Entries.Add(vm);
                }
            }), Keyboard));
        }

        private async void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is IntViewModel fvm) {
                if (move >= 0)
                {
                    Model.MakeHistory();
                    foreach (IntViewModel ff in Entries) ff.Moving = false;
                    int target = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (target >= 0 && move != target)
                    {
                        entries.Insert(target, entries[move]);
                        if (target < move) move++;
                        entries.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                    }
                    move = -1;
                } else
                {
                    int i = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (i >= 0) await Navigation.PushAsync(new CustomTextEntryPage(Title, new Command((par) =>
                    {
                        if (par is string s && int.TryParse(s, out int ii))
                        {
                            Model.MakeHistory();
                            entries[i] = ii;
                            fvm.Value = ii;
                            fvm.Refresh();
                        }
                    }), Keyboard, fvm.Value.ToString()));
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
                entries.RemoveAt(i);
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
                entries.RemoveAt(i);
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
            if (TopLevelPage)
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
            else
            {
                Task.Run(async () =>
                {
                    await Model.SaveAsync(true);
                    await Navigation.PopModalAsync();
                });
            }
            return true;
        }
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Model.SaveAsync(true);
            await Navigation.PopModalAsync();
        }
    }
}