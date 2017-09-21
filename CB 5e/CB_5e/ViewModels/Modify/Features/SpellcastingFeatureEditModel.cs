using OGL.Base;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class SpellcastingFeatureEditModel : FeatureEditModel<SpellcastingFeature>
    {
        public SpellcastingFeatureEditModel(SpellcastingFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
            if (f.PrepareCountAdditionalModifier != Ability.None)
            {
                f.PrepareCount = ResourceFeatureEditModel.convert(f.PrepareCountAdditionalModifier, f.PrepareCount);
                f.PrepareCountAdditionalModifier = Ability.None;
            }
            if (f.PrepareCountPerClassLevel != 0)
            {
                f.PrepareCount = (f.PrepareCount == null || f.PrepareCount.Trim() == "0" || f.PrepareCount.Trim() == "" ? "" : f.PrepareCount + " + ") + "ClassLevel" + (f.PrepareCountPerClassLevel > 1 ? " * " + f.PrepareCountPerClassLevel : "");
                f.PrepareCountPerClassLevel = 0;
            }
            if (f.PrepareCountAdditional != 0)
            {
                f.PrepareCount = (f.PrepareCount == null || f.PrepareCount.Trim() == "0" || f.PrepareCount.Trim() == "" ? "" : f.PrepareCount + " + ") + f.PrepareCountAdditional;
                f.PrepareCountAdditional = 0;
            }
        }

        public static HashSet<string> SpellcastingIDs = new HashSet<string>();

        public string SpellcastingID
        {
            get => Feature.SpellcastingID;
            set
            {
                if (value == SpellcastingID) return;
                MakeHistory("SpellcastingID");
                Feature.SpellcastingID = value;
                OnPropertyChanged("SpellcastingID");
            }
        }

        public bool IgnoreMulticlassing
        {
            get => Feature.IgnoreMulticlassing;
            set
            {
                if (value == IgnoreMulticlassing) return;
                MakeHistory("IgnoreMulticlassing");
                Feature.IgnoreMulticlassing = value;
                OnPropertyChanged("IgnoreMulticlassing");
            }
        }

        public string PrepareableSpells
        {
            get => Feature.PrepareableSpells;
            set
            {
                if (value == PrepareableSpells) return;
                MakeHistory("PrepareableSpells");
                Feature.PrepareableSpells = value;
                OnPropertyChanged("PrepareableSpells");
            }
        }

        public string PrepareCount
        {
            get => Feature.PrepareCount;
            set
            {
                if (value == PrepareCount) return;
                MakeHistory("PrepareCount");
                Feature.PrepareCount = value;
                OnPropertyChanged("PrepareCount");
            }
        }

        public string DisplayName
        {
            get => Feature.DisplayName;
            set
            {
                if (value == DisplayName) return;
                MakeHistory("DisplayName");
                Feature.DisplayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        public string Preparation
        {
            get => Feature.Preparation.ToString();
            set
            {
                PreparationMode parsed = PreparationMode.LearnSpells;
                if (Enum.TryParse(value, out parsed))
                {
                    if (Feature.Preparation == parsed) return;
                    MakeHistory("Preparation");
                    Feature.Preparation = parsed;
                    OnPropertyChanged("Preparation");
                }
                OnPropertyChanged("Preparation");
            }
        }
        public List<string> PreparationTypes
        {
            get => Enum.GetNames(typeof(PreparationMode)).ToList();
        }

        public string PreparationChange
        {
            get => Feature.PreparationChange.ToString();
            set
            {
                RechargeModifier parsed = RechargeModifier.LongRest;
                if (Enum.TryParse(value, out parsed))
                {
                    if (Feature.PreparationChange == parsed) return;
                    MakeHistory("PreparationChange");
                    Feature.PreparationChange = parsed;
                    OnPropertyChanged("PreparationChange");
                }
                OnPropertyChanged("PreparationChange");
            }
        }
        public List<string> RechargeTypes
        {
            get => Enum.GetNames(typeof(RechargeModifier)).ToList();
        }

        public bool BaseStrength
        {
            get => Feature.SpellcastingAbility.HasFlag(Ability.Strength);
            set
            {
                MakeHistory("SpellcastingAbility");
                if (value) Feature.SpellcastingAbility |= Ability.Strength;
                else Feature.SpellcastingAbility &= ~Ability.Strength;
                OnPropertyChanged("BaseStrength");
            }
        }
        public bool BaseDexterity
        {
            get => Feature.SpellcastingAbility.HasFlag(Ability.Dexterity);
            set
            {
                MakeHistory("SpellcastingAbility");
                if (value) Feature.SpellcastingAbility |= Ability.Dexterity;
                else Feature.SpellcastingAbility &= ~Ability.Dexterity;
                OnPropertyChanged("BaseDexterity");
            }
        }
        public bool BaseConstitution
        {
            get => Feature.SpellcastingAbility.HasFlag(Ability.Constitution);
            set
            {
                MakeHistory("SpellcastingAbility");
                if (value) Feature.SpellcastingAbility |= Ability.Constitution;
                else Feature.SpellcastingAbility &= ~Ability.Constitution;
                OnPropertyChanged("BaseConstitution");
            }
        }
        public bool BaseIntelligence
        {
            get => Feature.SpellcastingAbility.HasFlag(Ability.Intelligence);
            set
            {
                MakeHistory("SpellcastingAbility");
                if (value) Feature.SpellcastingAbility |= Ability.Intelligence;
                else Feature.SpellcastingAbility &= ~Ability.Intelligence;
                OnPropertyChanged("BaseIntelligence");
            }
        }
        public bool BaseWisdom
        {
            get => Feature.SpellcastingAbility.HasFlag(Ability.Wisdom);
            set
            {
                MakeHistory("SpellcastingAbility");
                if (value) Feature.SpellcastingAbility |= Ability.Wisdom;
                else Feature.SpellcastingAbility &= ~Ability.Wisdom;
                OnPropertyChanged("BaseWisdom");
            }
        }
        public bool BaseCharisma
        {
            get => Feature.SpellcastingAbility.HasFlag(Ability.Charisma);
            set
            {
                MakeHistory("SpellcastingAbility");
                if (value) Feature.SpellcastingAbility |= Ability.Charisma;
                else Feature.SpellcastingAbility &= ~Ability.Charisma;
                OnPropertyChanged("BaseCharisma");
            }
        }
    }
}
