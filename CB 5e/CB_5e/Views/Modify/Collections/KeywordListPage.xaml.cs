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

namespace CB_5e.Views.Modify.Collections
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KeywordListPage : ContentPage
    {
        public Command Undo { get => Model.Undo; }
        public Command Redo { get => Model.Redo; }
        public enum KeywordGroup
        {
            NONE, FEAT, ITEM, SPELL
        }
        private List<Keyword> entries;
        public ObservableRangeCollection<KeywordViewModel> Entries { get; set; } = new ObservableRangeCollection<KeywordViewModel>();
        public IEditModel Model { get; private set; }
        public string Property { get; private set; }
        private bool Modal = true;
        private Dictionary<KeywordGroup, List<Keyword>> Groups = new Dictionary<KeywordGroup, List<Keyword>>();

        private static HashSet<String> userAdded = new HashSet<string>();
        private KeywordGroup group = KeywordGroup.NONE;

        public KeywordListPage(IEditModel parent, string property, KeywordGroup keywords = KeywordGroup.NONE, bool modal = true)
        {
            if (Groups.Count == 0)
            {
                List<Keyword> none = new List<Keyword>();
                Groups.Add(KeywordGroup.NONE, none);

                List<Keyword> feat = new List<Keyword>();
                foreach (ClassDefinition c in parent.Context.ClassesSimple?.Values) feat.Add(new Keyword(c.Name));
                Groups.Add(KeywordGroup.FEAT, feat);
                List<Keyword> item = new List<Keyword>();
                foreach (String s in "Unarmed, Simple, Martial, Finesse, Thrown, Loading, Reach, Melee, Ranged, Bludgeoning, Piercing, Slashing".Split(',')) item.Add(new Keyword(s.Trim()));
                item.Add(new Range(0, 0));
                item.Add(new Versatile("0"));
                foreach (String s in "Ammunition, Two-handed, Special, Light, Medium, Heavy, Instrument, Trinket, Game, Artisan, Focus, Arcane, Divine, Druidic".Split(',')) item.Add(new Keyword(s.Trim()));
                Groups.Add(KeywordGroup.ITEM, item);

                List<Keyword> spell = new List<Keyword>();
                foreach (String s in "Abjuration, Conjuration, Divination, Evocation, Enchantment, Illusion, Necromancy, Transmutation".Split(',')) spell.Add(new Keyword(s.Trim()));
                spell.Add(new Material(""));
                foreach (String s in "Somatic, Verbal, Attack".Split(',')) spell.Add(new Keyword(s.Trim()));
                spell.Add(new Save(Ability.None));
                foreach (String s in "Healing, Cantrip, Ritual, Ranged, Melee, Touch, Self, Cone, Cube, Cylinder, Line, Sphere, Wall, Instantaneous, Concentration".Split(',')) spell.Add(new Keyword(s.Trim()));
                foreach (String s in "Acid, Cold, Fire, Force, Lightning, Necrotic, Poison, Psychic, Radiant, Thunder".Split(',')) spell.Add(new Keyword(s.Trim()));
                foreach (ClassDefinition c in parent.Context.ClassesSimple.Values) spell.Add(new Keyword(c.Name));
                Groups.Add(KeywordGroup.SPELL, spell);
            }
            Model = parent;
            Modal = modal;
            parent.PropertyChanged += Parent_PropertyChanged;
            Property = property;
            UpdateEntries();
            InitializeComponent();
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
            ToolbarItem add = new ToolbarItem() { Text = "Custom" };
            add.Clicked += Add_Clicked;
            ToolbarItems.Add(add);
        }

        private void Parent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == null || e.PropertyName == Property) UpdateEntries();
        }

        private void UpdateEntries()
        {
            entries = (List<Keyword>)Model.GetType().GetRuntimeProperty(Property).GetValue(Model);
            Fill();
        }
        private void Fill() {
            List<KeywordViewModel> res = new List<KeywordViewModel>(entries.Select(f => new KeywordViewModel(f)));
            res.AddRange(userAdded.Where(f => !entries.Exists(ff=>StringComparer.OrdinalIgnoreCase.Equals(ff.Name, f))).Select(f => new KeywordViewModel(new Keyword(f))));
            res.AddRange(Groups[group].Where(f=>!entries.Contains(f)).Select(f => new KeywordViewModel(f)));
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

        private async void Entries_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
                    if (fvm.Value is Material)
                    {
                        await Navigation.PushAsync(new CustomTextEntryPage("Material", new Command((par) =>
                        {
                            if (par is string s) {
                                entries.Add(new Material(s));
                                Fill();
                            }
                        })));
                    }
                    else if(fvm.Value is Range)
                    {
                        await Navigation.PushAsync(new CustomDualTextEntryPage("Range", new Command((par) =>
                        {
                            if (par is string[] s && s.Length == 2)
                            {
                                if (int.TryParse(s[0], out int min) && int.TryParse(s[1], out int max))
                                entries.Add(new Range(min, max));
                                Fill();
                            }
                        }), "Normal Range", "Long Range", Keyboard.Numeric, Keyboard.Numeric, "10", "20"));
                    }
                    else if(fvm.Value is Versatile)
                    {
                        await Navigation.PushAsync(new CustomTextEntryPage("Versatile Damage", new Command((par) =>
                        {
                            if (par is string s)
                            {
                                entries.Add(new Versatile(s));
                                Fill();
                            }
                        }), null, "1d8"));
                    }
                    else if(fvm.Value is Save)
                    {
                        await Navigation.PushAsync(new CustomPickerPage("Saving Throw", new Command((par) =>
                        {
                            if (par is string s && Enum.TryParse(s, true, out Ability a))
                            {
                                entries.Add(new Save(a));
                                Fill();
                            }
                        }), Enum.GetNames(typeof(Ability)).Select(s=>s.ToString()).ToList(), Ability.None.ToString()));
                    }
                    else
                    {
                        entries.Add(fvm.Value);
                        fvm.Selected = !fvm.Selected;
                    }
                }
                (sender as ListView).SelectedItem = null;
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