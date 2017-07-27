using CB_5e.Helpers;
using CB_5e.Views;
using Character_Builder;
using OGL;
using OGL.Features;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class SpellbookSpellsViewModel : SpellbookViewModel
    {
        private static CultureInfo culture = CultureInfo.InvariantCulture;
        private int last = -1;
        public Spellcasting Spellcasting { get; private set; }
        public SpellbookSpellsViewModel(PlayerViewModel model, SpellcastingFeature spellcastingFeature) : base(model, spellcastingFeature)
        {
            Title = spellcastingFeature.DisplayName;
            if (Title == null || Title == "") Title = SpellcastingID;
            Spellcasting = Model.Context.Player.GetSpellcasting(SpellcastingID);
            OnHighlight = new Command((par) =>
            {

                Model.MakeHistory("Highlight");
                Spellcasting.Highlight = (par as SpellViewModel)?.Name;
                foreach (SpellViewModel s in Spells) s.IsHightlighted = false;
                if (par is SpellViewModel svm) svm.IsHightlighted = true;
                OnPropertyChanged("Highlight");
                Model.Save();
            });
            RemoveHighlight = new Command(() =>
            {

                Model.MakeHistory("Highlight");
                Spellcasting.Highlight = null;
                foreach (SpellViewModel s in Spells) s.IsHightlighted = false;
                OnPropertyChanged("Highlight");
                Model.Save();
            });
            OnReduce = new Command((par) =>
            {
                if (par is SpellSlotInfo s && s.Used < s.Slots) {
                    if (last == s.Level)
                    {
                        s.Used++;
                        Model.MakeHistory();
                        Model.Context.Player.SetSpellSlot(s.SpellcastingID, s.Level, s.Used);
                        Model.Save();
                        (s as SpellSlotViewModel)?.UpdateUsed();
                        if (selected == s)
                        {
                            used = s.Used;
                            OnPropertyChanged("Used");
                        }
                        Model.UpdateSlots(this);
                    }
                    last = s.Level;
                }
            });
            OnReset = new Command((par) =>
            {
                if (par is SpellSlotInfo s)
                {
                    s.Used = 0;
                    Model.MakeHistory(s.SpellcastingID + "Slots" + s.Level);
                    Model.Context.Player.SetSpellSlot(s.SpellcastingID, s.Level, s.Used);
                    Model.Save();
                    (s as SpellSlotViewModel)?.UpdateUsed();
                    if (selected == s)
                    {
                        used = s.Used;
                        OnPropertyChanged("Used");
                    }
                    Model.UpdateSlots(this);
                }
            });
            ResetAll = new Command(() =>
            {
                IsBusy = true;
                Model.MakeHistory();
                Model.Context.Player.ResetSpellSlots(SpellcastingID);
                Model.Save();
                UpdateSlots();
                used = 0;
                OnPropertyChanged("Used");
                Model.UpdateSlots(this);
                IsBusy = false;
            });
            ShowInfo = new Command(async (par) =>
            {
                if (par is SpellViewModel svm)
                {
                    if (svm.Spell is ModifiedSpell ms)
                    {
                        ms.Info = Model.Context.Player.GetAttack(ms, ms.differentAbility == OGL.Base.Ability.None ? SpellcastingFeature.SpellcastingAbility : ms.differentAbility);
                        ms.Modifikations.AddRange(from f in Model.Context.Player.GetFeatures() where f is SpellModifyFeature && Utils.Matches(Model.Context, ms, ((SpellModifyFeature)f).Spells, null) select f);
                        ms.Modifikations = ms.Modifikations.Distinct().ToList();
                    }
                    await Navigation.PushAsync(InfoPage.Show(svm.Spell));
                }
            });
            spells = new List<SpellViewModel>();
            spells.AddRange(from s in Spellcasting.GetLearned(Model.Context.Player, Model.Context) select new SpellViewModel(s)
            {
                Highlight = OnHighlight,
                ShowInfo = ShowInfo,
                AddAlwaysPreparedToName = false
            });
            spells.AddRange(from s in Spellcasting.GetPrepared(Model.Context.Player, Model.Context) select new SpellViewModel(s)
            {
                Highlight = OnHighlight,
                ShowInfo = ShowInfo,
                AddAlwaysPreparedToName = false
            });
            spells.Sort();
            UpdateSlots();
            UpdateSpells();
        }

        public void UpdateSlots()
        {
            Selected = null;
            SpellSlots.ReplaceRange(from s in Model.Context.Player.GetSpellSlotInfo(SpellcastingID) select new SpellSlotViewModel(s)
            {
                Reset = OnReset,
                Reduce = OnReduce
            }
            .UpdateUsed());
        }

        public override void Refresh()
        {
            Spellcasting = Model.Context.Player.GetSpellcasting(SpellcastingID);
            UpdateSlots();
            spells.Clear();
            spells.AddRange(from s in Spellcasting.GetLearned(Model.Context.Player, Model.Context) select new SpellViewModel(s)
            {
                Highlight = OnHighlight,
                ShowInfo = ShowInfo,
                AddAlwaysPreparedToName = false
            });
            spells.AddRange(from s in Spellcasting.GetPrepared(Model.Context.Player, Model.Context) select new SpellViewModel(s)
            {
                Highlight = OnHighlight,
                ShowInfo = ShowInfo,
                AddAlwaysPreparedToName = false
            });
            spells.Sort();
            UpdateSpells();
            base.Refresh();
        }

        public string Highlight { get => Spellcasting.Highlight; }
        public string Attack { get => Model.Context.Player.GetSpellAttack(SpellcastingID, SpellcastingFeature.SpellcastingAbility).ToString("+#;-#;0"); }
        public int DC { get => Model.Context.Player.GetSpellSaveDC(SpellcastingID, SpellcastingFeature.SpellcastingAbility); }
        public string Ability { get => SpellcastingFeature.SpellcastingAbility.ToString(); }
        public Command OnHighlight { get; private set; }
        public Command OnReduce { get; private set; }
        public Command OnReset { get; private set; }
        public Command ResetAll { get; private set; }
        public ObservableRangeCollection<SpellSlotViewModel> SpellSlots { get; private set; } = new ObservableRangeCollection<SpellSlotViewModel>();
        private List<SpellViewModel> spells;
        public ObservableRangeCollection<SpellViewModel> Spells { get; private set; } = new ObservableRangeCollection<SpellViewModel>();
        private string spellSearch;
        public String SpellSearch { get => spellSearch; set
            {
                SetProperty(ref spellSearch, value);
                UpdateSpells();
            }
        }

        public Command ShowInfo { get; private set; }

        public void UpdateSpells() => Spells.ReplaceRange(from f in spells where spellSearch == null || spellSearch == ""
            || culture.CompareInfo.IndexOf(f.Spell.Name, spellSearch, CompareOptions.IgnoreCase) >= 0
            || culture.CompareInfo.IndexOf(f.Spell.Description, spellSearch, CompareOptions.IgnoreCase) >= 0
            || f.Spell.GetKeywords().Exists(k=>culture.CompareInfo.IndexOf(k.Name, spellSearch, CompareOptions.IgnoreCase) >= 0)
            || f.Spell.Descriptions.Exists(k=>culture.CompareInfo.IndexOf(k.Text, spellSearch, CompareOptions.IgnoreCase) >= 0)
            orderby f.Name select f);

        private SpellSlotViewModel selected;
        public SpellSlotViewModel Selected {
            get => selected; set
            {
                SetProperty(ref selected, value);
                if (selected != null) used = selected.Used;
                else used = 0;
                OnPropertyChanged("Used");
            }
        }

        private int used;
        public int Used {
            get => used;
            set
            {
                if (used != value && selected != null)
                {
                    used = value;
                    if (used < 0) used = 0;
                    if (used > selected.Slots) used = selected.Slots;
                    selected.Used = used;
                    Model.MakeHistory(selected.SpellcastingID + "Slots" + selected.Level);
                    Model.Context.Player.SetSpellSlot(selected.SpellcastingID, selected.Level, selected.Used);
                    Model.Save();
                    Model.UpdateSlots(this);
                    selected.UpdateUsed();
                }
                OnPropertyChanged("Used");
            }
        }

        public Command RemoveHighlight { get; private set; }
        public Color Accent { get => Color.Accent; }
    }
}
