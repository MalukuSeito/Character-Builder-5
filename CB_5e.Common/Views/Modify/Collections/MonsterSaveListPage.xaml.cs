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
using OGL.Monsters;
using System.IO;
using System.Xml.Serialization;

namespace CB_5e.Views.Modify.Collections
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MonsterSaveListPage : ContentPage
	{
        public static XmlSerializer Serializer = new XmlSerializer(typeof(MonsterSaveBonus));
        private List<MonsterSaveBonus> entries;
        public ObservableRangeCollection<MonsterSaveViewModel> Entries { get; set; } = new ObservableRangeCollection<MonsterSaveViewModel>();
        public IEditModel Model { get; private set; }
        public string Property { get; private set; }
        private int move = -1;
        private bool Modal = true;
        public Command Undo { get => Model.Undo; }
        public Command Redo { get => Model.Redo; }

        public MonsterSaveListPage(IEditModel parent, string property, bool modal = true)
        {
            Model = parent;
            Modal = modal;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            UpdateEntries();
			InitializeComponent ();
            InitToolbar(Modal);
            BindingContext = this;
        }

        private void InitToolbar(bool modal)
        {
            if (Modal)
            {
                ToolbarItem undo = new ToolbarItem() { Text = "Undo" };
                undo.SetBinding(MenuItem.CommandProperty, new Binding("Undo"));
                ToolbarItems.Add(undo);
                ToolbarItem redo = new ToolbarItem() { Text = "Redo" };
                redo.SetBinding(MenuItem.CommandProperty, new Binding("Redo"));
                ToolbarItems.Add(redo);
            }
            ToolbarItem add = new ToolbarItem() { Text = "Add" };
            add.Clicked += Add_Clicked;
            ToolbarItems.Add(add);
            ToolbarItem paste = new ToolbarItem() { Text = "Paste" };
            paste.Clicked += Paste_Clicked;
            ToolbarItems.Add(paste);
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == Property) UpdateEntries();
        }

        private void UpdateEntries()
        {
            entries = (List<MonsterSaveBonus>)Model.GetType().GetRuntimeProperty(Property).GetValue(Model);
            Fill();
        }
        private void Fill() => Entries.ReplaceRange(entries.Select(f => new MonsterSaveViewModel(f)));

        private void Paste_Clicked(object sender, EventArgs e)
        {
            try
            {
                Model.MakeHistory();
                using (StringReader sr = new StringReader(DependencyService.Get<IClipboardService>().GetTextData()))
                {
                    object o = Serializer.Deserialize(sr);
                    if (o is MonsterSaveBonus msb) entries.Add(msb);
                    Fill();
                }
            } catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            Model.MakeHistory();
            MonsterSaveBonus t = new MonsterSaveBonus();
            MonsterSaveViewModel vm = new MonsterSaveViewModel(t);
            entries.Add(t);
            Entries.Add(vm);
            await Navigation.PushAsync(new MonsterSaveEditPage(vm));
        }

        private async void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MonsterSaveViewModel fvm) {
                if (move >= 0)
                {
                    Model.MakeHistory();
                    foreach (MonsterSaveViewModel ff in Entries) ff.Moving = false;
                    int target = entries.FindIndex(ff => fvm.Entry == ff);
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
                    Model.MakeHistory();
                    await Navigation.PushAsync(new MonsterSaveEditPage(fvm));
                    (sender as ListView).SelectedItem = null;
                }
            }
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is MonsterSaveViewModel f)
            {
                Model.MakeHistory();
                int i = entries.FindIndex(ff => f.Entry == ff);
                entries.RemoveAt(i);
                Fill();
            }
        }

        private async void Info_Clicked(object sender, EventArgs e)
        {
            if ((sender as MenuItem).BindingContext is MonsterSaveViewModel f) await Navigation.PushAsync(InfoPage.Show(new Feature(f.Text, f.Detail)));
        }

        private void Move_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is MonsterSaveViewModel f)
            {
                foreach (MonsterSaveViewModel ff in Entries) ff.Moving = false;
                f.Moving = true;
                move = entries.FindIndex(ff => f.Entry == ff);
            }
            
        }

        private void Cut_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is MonsterSaveViewModel f)
            {
                Model.MakeHistory();
                using (StringWriter sw = new StringWriter())
                {
                    Serializer.Serialize(sw, f.Entry);
                    DependencyService.Get<IClipboardService>().PutTextData(sw.ToString(), f.Detail);
                }
                int i = entries.FindIndex(ff => f.Entry == ff);
                entries.RemoveAt(i);
                Fill();
            }
        }

        private void Copy_Clicked(object sender, EventArgs e)
        {
            if (((MenuItem)sender).BindingContext is MonsterSaveViewModel f)
            {
                using (StringWriter sw = new StringWriter())
                {
                    Serializer.Serialize(sw, f.Entry);
                    DependencyService.Get<IClipboardService>().PutTextData(sw.ToString(), f.Detail);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (Modal)
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
            return Modal;
        }
    }
}