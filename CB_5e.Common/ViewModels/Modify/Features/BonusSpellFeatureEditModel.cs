using CB_5e.Helpers;
using OGL.Base;
using OGL.Features;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class BonusSpellFeatureEditModel : FeatureEditModel<BonusSpellFeature>
    {
        public BonusSpellFeatureEditModel(BonusSpellFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }
        public string Spell
        {
            get => Feature.Spell;
            set
            {
                if (value == null) return;
                if (value == Spell) return;
                MakeHistory("Spell");
                Feature.Spell = value;
                OnPropertyChanged("Spell");
            }
        }
        public string Recharge
        {
            get => Feature.SpellCastModifier.ToString();
            set
            {
                RechargeModifier parsed = RechargeModifier.Unmodified;
                if (Enum.TryParse(value, out parsed))
                {
                    if (Feature.SpellCastModifier == parsed) return;
                    MakeHistory("SpellCastModifier");
                    Feature.SpellCastModifier = parsed;
                    OnPropertyChanged("Recharge");
                }
                OnPropertyChanged("Recharge");
            }
        }
        public List<string> RechargeTypes
        {
            get => Enum.GetNames(typeof(RechargeModifier)).ToList();
        }
        public List<Keyword> AdditionalKeywords { get => Feature.KeywordsToAdd; }
        public bool BaseStrength
        {
            get => Feature.SpellCastingAbility.HasFlag(Ability.Strength);
            set
            {
                MakeHistory("SpellCastingAbility");
                if (value) Feature.SpellCastingAbility |= Ability.Strength;
                else Feature.SpellCastingAbility &= ~Ability.Strength;
                OnPropertyChanged("BaseStrength");
            }
        }
        public bool BaseDexterity
        {
            get => Feature.SpellCastingAbility.HasFlag(Ability.Dexterity);
            set
            {
                MakeHistory("SpellCastingAbility");
                if (value) Feature.SpellCastingAbility |= Ability.Dexterity;
                else Feature.SpellCastingAbility &= ~Ability.Dexterity;
                OnPropertyChanged("BaseDexterity");
            }
        }
        public bool BaseConstitution
        {
            get => Feature.SpellCastingAbility.HasFlag(Ability.Constitution);
            set
            {
                MakeHistory("SpellCastingAbility");
                if (value) Feature.SpellCastingAbility |= Ability.Constitution;
                else Feature.SpellCastingAbility &= ~Ability.Constitution;
                OnPropertyChanged("BaseConstitution");
            }
        }
        public bool BaseIntelligence
        {
            get => Feature.SpellCastingAbility.HasFlag(Ability.Intelligence);
            set
            {
                MakeHistory("SpellCastingAbility");
                if (value) Feature.SpellCastingAbility |= Ability.Intelligence;
                else Feature.SpellCastingAbility &= ~Ability.Intelligence;
                OnPropertyChanged("BaseIntelligence");
            }
        }
        public bool BaseWisdom
        {
            get => Feature.SpellCastingAbility.HasFlag(Ability.Wisdom);
            set
            {
                MakeHistory("SpellCastingAbility");
                if (value) Feature.SpellCastingAbility |= Ability.Wisdom;
                else Feature.SpellCastingAbility &= ~Ability.Wisdom;
                OnPropertyChanged("BaseWisdom");
            }
        }
        public bool BaseCharisma
        {
            get => Feature.SpellCastingAbility.HasFlag(Ability.Charisma);
            set
            {
                MakeHistory("SpellCastingAbility");
                if (value) Feature.SpellCastingAbility |= Ability.Charisma;
                else Feature.SpellCastingAbility &= ~Ability.Charisma;
                OnPropertyChanged("BaseCharisma");
            }
        }

        public ObservableRangeCollection<string> Suggestions { get; private set; } = new ObservableRangeCollection<string>();
    }
}
