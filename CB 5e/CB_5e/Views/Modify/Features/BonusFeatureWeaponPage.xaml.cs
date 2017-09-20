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
using OGL.Items;

namespace CB_5e.Views.Modify.Features
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BonusFeatureWeaponPage : ContentPage
	{
        public ObservableRangeCollection<string> Suggestions { get; set; } = new ObservableRangeCollection<string>();
        public ObservableRangeCollection<StringViewModel> Entries { get; set; } = new ObservableRangeCollection<StringViewModel>();
        public BonusFeatureEditModel Model { get; private set; }
        private int move = -1;
        public string BaseItemChange
        {
            get => Model.BaseItemChange;
            set
            {
                if (value == null) return;
                Model.BaseItemChange = value;
            }
        }

        public BonusFeatureWeaponPage(BonusFeatureEditModel parent)
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
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == "ProficiencyOptions") Fill();
            OnPropertyChanged(e.PropertyName);
        }

        private void Fill() => Entries.ReplaceRange(Model.ProficiencyOptions.Select(f => new StringViewModel(f)));

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
                        if (r != "") Model.ProficiencyOptions.Add(r);
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
                if (clip != null)
                {
                    Model.ProficiencyOptions.Add(clip);
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
            await Navigation.PushAsync(new CustomTextEntrySuggestionsPage(Title, new Command((par) =>
            {
                if (par is string s)
                {
                    Model.MakeHistory();
                    StringViewModel vm = new StringViewModel(s);
                    Model.ProficiencyOptions.Add(s);
                    Entries.Add(vm);
                }
            }), Suggestions));
        }

        private async void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is StringViewModel fvm) {
                if (move >= 0)
                {
                    Model.MakeHistory();
                    foreach (StringViewModel ff in Entries) ff.Moving = false;
                    int target = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (target >= 0 && move != target)
                    {
                        Model.ProficiencyOptions.Insert(target, Model.ProficiencyOptions[move]);
                        if (target < move) move++;
                        Model.ProficiencyOptions.RemoveAt(move);
                        Fill();
                        (sender as ListView).SelectedItem = null;
                    }
                    move = -1;
                } else
                {
                    int i = Entries.ToList().FindIndex(ff => fvm == ff);
                    if (i >= 0) await Navigation.PushAsync(new CustomTextEntryPage(Title, new Command((par) =>
                    {
                        if (par is string s)
                        {
                            Model.MakeHistory();
                            Model.ProficiencyOptions[i] = s;
                            fvm.Text = s;
                            fvm.Refresh();
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
                Model.MakeHistory();
                int i = Entries.ToList().FindIndex(ff => f == ff);
                Model.ProficiencyOptions.RemoveAt(i);
                Fill();
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
                Model.MakeHistory();
                DependencyService.Get<IClipboardService>().PutTextData(f.Text, "String");
                int i = Entries.ToList().FindIndex(ff => f == ff);
                Model.ProficiencyOptions.RemoveAt(i);
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

        protected override bool OnBackButtonPressed()
        {
            Task.Run(async () =>
            {
                await Model.SaveAsync(true);
                await Navigation.PopModalAsync();
            });
            return true;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Model.Context.ItemsSimple.Count == 0) {
                await Model.Context.ImportItemsAsync();
            }
            Suggestions.ReplaceRange(Model.Context.ItemsSimple.Values.Where(s=>s is Weapon).Select(s=>s.Name).OrderBy(s => s));
        }
    }
}