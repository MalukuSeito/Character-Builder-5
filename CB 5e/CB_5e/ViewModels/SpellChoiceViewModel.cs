using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;
using Xamarin.Forms;
using System.Globalization;
using CB_5e.Helpers;
using OGL;
using OGL.Spells;
using OGL.Base;
using Character_Builder;
using CB_5e.Views;

namespace CB_5e.ViewModels
{
    class SpellChoiceViewModel : SpellbookViewModel
    {
        public SpellChoiceViewModel(PlayerModel model, SpellcastingFeature spellcasting, SpellChoiceFeature choice) : base(model, spellcasting, (spellcasting.DisplayName ?? spellcasting.SpellcastingID) + " - " + choice?.Name ?? choice?.UniqueID ?? "Additional")
        {
            Choice = choice;
            OnPrepare = new Command((par) =>
            {
                if (par is SpellViewModel svm && SpellcastingFeature != null && Choice != null)
                {
                    if (!svm.Prepared)
                    {
                        if (Count < Able)
                        {
                            svm.Prepared = true;
                            Model.MakeHistory();
                            Model.Context.Player.GetSpellChoice(SpellcastingID, UniqueID).Choices.Add(svm.Name + " " + ConfigManager.SourceSeperator + " " + svm.Source);
                            Model.Save();
                        }
                    } else
                    {
                        svm.Prepared = false;
                        Model.MakeHistory();
                        string r = svm.Name + " " + ConfigManager.SourceSeperator + " " + svm.Source;
                        Model.Context.Player.GetSpellChoice(SpellcastingID, UniqueID).Choices.RemoveAll(s => ConfigManager.SourceInvariantComparer.Equals(s, r));
                        Model.Save();
                        if (svm.BadChoice) Spells.Remove(svm);
                        if (svm.BadChoice) spells.Remove(svm);
                    }
                    OnPropertyChanged("Count");
                    OnPropertyChanged("Prepared");
                    ChangedSelectedSpells(SpellcastingID);
                }
            }, (par) => par is SpellViewModel svm && choice != null);
            ShowInfo = new Command(async (par) =>
            {
                if (par is SpellViewModel svm)
                {
                    if (svm.Spell is ModifiedSpell ms)
                    {
                        ms.Info = Model.Context.Player.GetAttack(ms, ms.differentAbility == Ability.None ? SpellcastingFeature.SpellcastingAbility : ms.differentAbility);
                        ms.Modifikations.AddRange(from f in Model.Context.Player.GetFeatures() where f is SpellModifyFeature && Utils.Matches(Model.Context, ms, ((SpellModifyFeature)f).Spells, null) select f);
                        ms.Modifikations = ms.Modifikations.Distinct().ToList();
                    }
                    await Navigation.PushAsync(InfoPage.Show(svm.Spell));
                }
            });
            ResetPrepared = new Command(() =>
            {
                IsBusy = true;
                Model.MakeHistory();
                Model.Context.Player.GetSpellChoice(SpellcastingID, UniqueID).Choices.Clear();
                Model.Save();
                foreach (SpellViewModel s in spells)
                {
                    s.Prepared = false;
                }
                spells.RemoveAll(s => s.BadChoice);
                UpdateSpells();
                OnPropertyChanged("Count");
                OnPropertyChanged("Selected");
                ChangedSelectedSpells(SpellcastingID);
                IsBusy = false;
            });
            AddSpells();
        }
        public void AddSpells()
        {
            if (Choice != null)
            {
                spells.Clear();
                Spells.ReplaceRange(spells);
                int classlevel = Model.Context.Player.GetClassLevel(SpellcastingID);
                List<Spell> available = new List<Spell>(Utils.FilterSpell(Context, Choice.AvailableSpellChoices, SpellcastingID, classlevel));
                List<Feature> spellfeatures = new List<Feature>(from f in Model.Context.Player.GetFeatures() where f is ModifySpellChoiceFeature select f);
                List<string> chosen = Model.Context.Player.GetSpellChoice(SpellcastingID, UniqueID).Choices;
                foreach (Feature f in spellfeatures)
                {
                    if (f is ModifySpellChoiceFeature msf && msf.UniqueID == UniqueID)
                    {
                        if (msf.AdditionalSpellChoices != "false") available.AddRange(Utils.FilterSpell(Context, msf.AdditionalSpellChoices, SpellcastingID, classlevel));
                        if (msf.AdditionalSpells != null && msf.AdditionalSpells.Count > 0) available.AddRange(Context.Spells.Values.Where(s => msf.AdditionalSpells.FirstOrDefault(ss => StringComparer.OrdinalIgnoreCase.Equals(s.Name, ss)) != null));
                    }
                }
                spells.AddRange(from s in available
                                select new SpellViewModel(s)
                                {
                                    Prepared = chosen.Exists(ss => ConfigManager.SourceInvariantComparer.Equals(s, ss)),
                                    Prepare = OnPrepare,
                                    ShowInfo = ShowInfo
                                });

                spells.AddRange(from s in chosen
                                where !spells.Exists(t => ConfigManager.SourceInvariantComparer.Equals(t.Spell, s))
                                select new SpellViewModel(Context.GetSpell(s, null))
                                {
                                    Prepared = false,
                                    Prepare = OnPrepare,
                                    ShowInfo = ShowInfo,
                                    BadChoice = true
                                });
            }
            spells.Sort();
            UpdateSpells();
        }

        public override void Refresh(SpellcastingFeature feature)
        {
            Title = (feature.DisplayName ?? feature.SpellcastingID) + " - " + Choice?.Name ?? Choice?.UniqueID ?? "Additional";
            SpellcastingFeature = feature;
            AddSpells();
            OnPropertyChanged(null);
        }

        public void Refresh(SpellcastingFeature feature, SpellChoiceFeature choice)
        {
            Choice = choice;
            SpellcastingFeature = feature;
            Title = (feature.DisplayName ?? feature.SpellcastingID) + " - " + Choice?.Name ?? Choice?.UniqueID ?? "Additional";
            AddSpells();
            OnPropertyChanged(null);
        }

        private List<SpellViewModel> spells = new List<SpellViewModel>();
        public ObservableRangeCollection<SpellViewModel> Spells { get; private set; } = new ObservableRangeCollection<SpellViewModel>();
        private string spellSearch;
        public String SpellSearch
        {
            get => spellSearch; set
            {
                SetProperty(ref spellSearch, value);
                UpdateSpells();
            }
        }

        public Command ShowInfo { get; private set; }
        public Command OnPrepare { get; private set; }

        public void UpdateSpells() => Spells.ReplaceRange(from f in spells where spellSearch == null || spellSearch == ""
            || Culture.CompareInfo.IndexOf(f.Spell.Name ?? "", spellSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(f.Spell.Description ?? "", spellSearch, CompareOptions.IgnoreCase) >= 0
            || f.Spell.GetKeywords().Exists(k => Culture.CompareInfo.IndexOf(k.Name ?? "", spellSearch, CompareOptions.IgnoreCase) >= 0)
            || f.Spell.Descriptions.Exists(k => Culture.CompareInfo.IndexOf(k.Text ?? "", spellSearch, CompareOptions.IgnoreCase) >= 0)
            orderby f.Name select f);

        public int Count { get => Choice == null ? spells.Count : Model.Context.Player.GetSpellChoice(SpellcastingID, UniqueID).Choices.Count; }
        public int Able
        {
            get
            {
                if (Choice == null) return spells.Count;
                List<Feature> spellfeatures = new List<Feature>(from f in Model.Context.Player.GetFeatures() where f is IncreaseSpellChoiceAmountFeature select f);
                int amount = Choice.Amount;
                foreach (Feature f in spellfeatures) if (f is IncreaseSpellChoiceAmountFeature && ((IncreaseSpellChoiceAmountFeature)f).UniqueID == UniqueID) amount += ((IncreaseSpellChoiceAmountFeature)f).Amount;
                return amount;
            }
        }

        public void SetSpells(List<ModifiedSpell> bonusprepared)
        {
            spells.AddRange(from ms in bonusprepared
                            select new SpellViewModel(ms)
                            {
                                Prepared = true,
                                AddAlwaysPreparedToName = true,
                                ShowInfo = ShowInfo,
                                Prepare = OnPrepare
                            });
            UpdateSpells();
        }

        public string Prepared { get => Count + "/" + Able + (Choice == null ? "Spells Known":" Spells Choosen"); }
        public Command ResetPrepared { get; private set; }

        public SpellChoiceFeature Choice { get; private set; }
        public string UniqueID { get => Choice?.UniqueID; }

    }
}
