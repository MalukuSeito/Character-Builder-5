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
using OGL.Keywords;
using OGL.Base;
using CB_5e.ViewModels.Modify;
using CB_5e.ViewModels.Modify.Collections;
using CB_5e.ViewModels.Modify.Features;
using CB_5e.Services;

namespace CB_5e.Views.Modify.Features
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeatureKeywords : ContentPage
    {
        private List<Keyword> entries;
        public ObservableRangeCollection<KeywordViewModel> Entries { get; set; } = new ObservableRangeCollection<KeywordViewModel>();
        public IFeatureEditModel Model { get; private set; }

        public string Prerequisite { get => Model.Prerequisite; set => Model.Prerequisite = value; }

        private static HashSet<String> userAdded = new HashSet<string>();
        private Task<IEnumerable<string>> LoadClasses;
        private List<Keyword> Classes = new List<Keyword>();

        public FeatureKeywords(IFeatureEditModel parent, Task<IEnumerable<string>> loadClasses)
        {
            Model = parent;
            parent.PropertyChanged += Parent_PropertyChanged;
            UpdateEntries();
            InitializeComponent();
            InitToolbar();
            BindingContext = this;
            LoadClasses = loadClasses;
        }

        private void InitToolbar()
        {
            ToolbarItem add = new ToolbarItem() { Text = "Custom" };
            add.Clicked += Add_Clicked;
            ToolbarItems.Add(add);
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == "Keywords") UpdateEntries();
        }
        public Color TextColor { get => Color.Accent; }
        private void UpdateEntries()
        {
            entries = Model.Keywords;
            Fill();
        }
        private void Fill() {
            List<KeywordViewModel> res = new List<KeywordViewModel>(entries.Select(f => new KeywordViewModel(f) { Selected = true }));
            res.AddRange(userAdded.Where(f => !entries.Exists(ff => StringComparer.OrdinalIgnoreCase.Equals(ff.Name, f))).Select(f => new KeywordViewModel(new Keyword(f))));
            res.AddRange(Classes.Where(f => !entries.Contains(f)).Select(f => new KeywordViewModel(f)));
            Entries.ReplaceRange(res);
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CustomTextEntryPage(Title, new Command((par) =>
            {
                if (par is string s)
                {
                    Model.MakeHistory();
                    userAdded.Add(s);
                    Keyword kw = new Keyword(s);
                    KeywordViewModel vm = new KeywordViewModel(kw);
                    entries.Add(kw);
                    Entries.Add(vm);
                }
            })));
        }

        private void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is KeywordViewModel fvm)
            {
                Model.MakeHistory();
                if (fvm.Selected)
                {
                    entries.Remove(fvm.Value);
                    fvm.Selected = !fvm.Selected;
                }
                else
                {
                    entries.Add(fvm.Value);
                    fvm.Selected = !fvm.Selected;
                }
                Fill();
                (sender as ListView).SelectedItem = null;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Model.UnsavedChanges > 0)
                {
                    await Model.SaveAsync(true);
                    await Navigation.PopModalAsync();
                }
                else await Navigation.PopModalAsync();
            });
            return true;
        }

        protected override async void OnAppearing()
        {
            Classes.AddRange((await LoadClasses).Select(f => new Keyword(f)));
            Fill();
        }
    }
}