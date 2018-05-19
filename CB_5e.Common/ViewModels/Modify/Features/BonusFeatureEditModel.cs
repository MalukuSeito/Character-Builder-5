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
    public class BonusFeatureEditModel : FeatureEditModel<BonusFeature>
    {
        public BonusFeatureEditModel(BonusFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
            if (f.DamageBonusModifier != Ability.None)
            {
                f.DamageBonus = ResourceFeatureEditModel.convert(f.DamageBonusModifier, f.DamageBonus);
                f.DamageBonusModifier = Ability.None;
            }
        }

        public string Condition
        {
            get => Feature.Condition;
            set
            {
                if (value == Condition) return;
                MakeHistory("Condition");
                Feature.Condition = value;
                OnPropertyChanged("Condition");
            }
        }

        public List<string> Skills { get => Feature.Skills; }
        public List<string> ProficiencyOptions { get => Feature.ProficiencyOptions; }
        public string SkillBonus
        {
            get => Feature.SkillBonus;
            set
            {
                if (value == SkillBonus) return;
                MakeHistory("SkillBonus");
                Feature.SkillBonus = value;
                OnPropertyChanged("SkillBonus");
            }
        }

        public bool SkillPassive
        {
            get => Feature.SkillPassive;
            set
            {
                if (value == SkillPassive) return;
                MakeHistory("SkillPassive");
                Feature.SkillPassive = value;
                OnPropertyChanged("SkillPassive");
            }
        }

        public string AttackBonus
        {
            get => Feature.AttackBonus;
            set
            {
                if (value == AttackBonus) return;
                MakeHistory("AttackBonus");
                Feature.AttackBonus = value;
                OnPropertyChanged("AttackBonus");
            }
        }
        public string DamageBonus
        {
            get => Feature.DamageBonus;
            set
            {
                if (value == DamageBonus) return;
                MakeHistory("DamageBonus");
                Feature.DamageBonus = value;
                OnPropertyChanged("DamageBonus");
            }
        }
        public string SaveDCBonus
        {
            get => Feature.SaveDCBonus;
            set
            {
                if (value == SaveDCBonus) return;
                MakeHistory("SaveDCBonus");
                Feature.SaveDCBonus = value;
                OnPropertyChanged("SaveDCBonus");
            }
        }
        public string ACBonus
        {
            get => Feature.ACBonus;
            set
            {
                if (value == ACBonus) return;
                MakeHistory("ACBonus");
                Feature.ACBonus = value;
                OnPropertyChanged("ACBonus");
            }
        }
        public string InitiativeBonus
        {
            get => Feature.InitiativeBonus;
            set
            {
                if (value == InitiativeBonus) return;
                MakeHistory("InitiativeBonus");
                Feature.InitiativeBonus = value;
                OnPropertyChanged("InitiativeBonus");
            }
        }
        public string DamageBonusText
        {
            get => Feature.DamageBonusText;
            set
            {
                if (value == DamageBonusText) return;
                MakeHistory("DamageBonusText");
                Feature.DamageBonusText = value;
                OnPropertyChanged("DamageBonusText");
            }
        }
        public string SavingThrowBonus
        {
            get => Feature.SavingThrowBonus;
            set
            {
                if (value == SavingThrowBonus) return;
                MakeHistory("SavingThrowBonus");
                Feature.SavingThrowBonus = value;
                OnPropertyChanged("SavingThrowBonus");
            }
        }
        public string ProficiencyBonus
        {
            get => Feature.ProficiencyBonus;
            set
            {
                if (value == ProficiencyBonus) return;
                MakeHistory("ProficiencyBonus");
                Feature.ProficiencyBonus = value;
                OnPropertyChanged("ProficiencyBonus");
            }
        }
        public string BaseItemChange
        {
            get => Feature.BaseItemChange;
            set
            {
                if (value == BaseItemChange) return;
                MakeHistory("BaseItemChange");
                Feature.BaseItemChange = value;
                OnPropertyChanged("BaseItemChange");
            }
        }
        public string SizeChange
        {
            get => Feature.SizeChange.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.SizeChange == parsedInt) return;
                    MakeHistory("SizeChange");
                    Feature.SizeChange = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("SizeChange");
                }
                if (value != "" && value != "-") OnPropertyChanged("SizeChange");
            }
        }

        public bool BaseStrength
        {
            get => Feature.BaseAbility.HasFlag(Ability.Strength);
            set
            {
                MakeHistory("BaseAbility");
                if (value) Feature.BaseAbility |= Ability.Strength;
                else Feature.BaseAbility &= ~Ability.Strength;
                OnPropertyChanged("BaseStrength");
            }
        }
        public bool BaseDexterity
        {
            get => Feature.BaseAbility.HasFlag(Ability.Dexterity);
            set
            {
                MakeHistory("BaseAbility");
                if (value) Feature.BaseAbility |= Ability.Dexterity;
                else Feature.BaseAbility &= ~Ability.Dexterity;
                OnPropertyChanged("BaseDexterity");
            }
        }
        public bool BaseConstitution
        {
            get => Feature.BaseAbility.HasFlag(Ability.Constitution);
            set
            {
                MakeHistory("BaseAbility");
                if (value) Feature.BaseAbility |= Ability.Constitution;
                else Feature.BaseAbility &= ~Ability.Constitution;
                OnPropertyChanged("BaseConstitution");
            }
        }
        public bool BaseIntelligence
        {
            get => Feature.BaseAbility.HasFlag(Ability.Intelligence);
            set
            {
                MakeHistory("BaseAbility");
                if (value) Feature.BaseAbility |= Ability.Intelligence;
                else Feature.BaseAbility &= ~Ability.Intelligence;
                OnPropertyChanged("BaseIntelligence");
            }
        }
        public bool BaseWisdom
        {
            get => Feature.BaseAbility.HasFlag(Ability.Wisdom);
            set
            {
                MakeHistory("BaseAbility");
                if (value) Feature.BaseAbility |= Ability.Wisdom;
                else Feature.BaseAbility &= ~Ability.Wisdom;
                OnPropertyChanged("BaseWisdom");
            }
        }
        public bool BaseCharisma
        {
            get => Feature.BaseAbility.HasFlag(Ability.Charisma);
            set
            {
                MakeHistory("BaseAbility");
                if (value) Feature.BaseAbility |= Ability.Charisma;
                else Feature.BaseAbility &= ~Ability.Charisma;
                OnPropertyChanged("BaseCharisma");
            }
        }
        public bool SaveStrength
        {
            get => Feature.SavingThrowAbility.HasFlag(Ability.Strength);
            set
            {
                MakeHistory("SavingThrowAbility");
                if (value) Feature.SavingThrowAbility |= Ability.Strength;
                else Feature.SavingThrowAbility &= ~Ability.Strength;
                OnPropertyChanged("SaveStrength");
            }
        }
        public bool SaveDexterity
        {
            get => Feature.SavingThrowAbility.HasFlag(Ability.Dexterity);
            set
            {
                MakeHistory("SavingThrowAbility");
                if (value) Feature.SavingThrowAbility |= Ability.Dexterity;
                else Feature.SavingThrowAbility &= ~Ability.Dexterity;
                OnPropertyChanged("SaveDexterity");
            }
        }
        public bool SaveConstitution
        {
            get => Feature.SavingThrowAbility.HasFlag(Ability.Constitution);
            set
            {
                MakeHistory("SavingThrowAbility");
                if (value) Feature.SavingThrowAbility |= Ability.Constitution;
                else Feature.SavingThrowAbility &= ~Ability.Constitution;
                OnPropertyChanged("SaveConstitution");
            }
        }
        public bool SaveIntelligence
        {
            get => Feature.SavingThrowAbility.HasFlag(Ability.Intelligence);
            set
            {
                MakeHistory("SavingThrowAbility");
                if (value) Feature.SavingThrowAbility |= Ability.Intelligence;
                else Feature.SavingThrowAbility &= ~Ability.Intelligence;
                OnPropertyChanged("SaveIntelligence");
            }
        }
        public bool SaveWisdom
        {
            get => Feature.SavingThrowAbility.HasFlag(Ability.Wisdom);
            set
            {
                MakeHistory("SavingThrowAbility");
                if (value) Feature.SavingThrowAbility |= Ability.Wisdom;
                else Feature.SavingThrowAbility &= ~Ability.Wisdom;
                OnPropertyChanged("SaveWisdom");
            }
        }
        public bool SaveCharisma
        {
            get => Feature.SavingThrowAbility.HasFlag(Ability.Charisma);
            set
            {
                MakeHistory("SavingThrowAbility");
                if (value) Feature.SavingThrowAbility |= Ability.Charisma;
                else Feature.SavingThrowAbility &= ~Ability.Charisma;
                OnPropertyChanged("SaveCharisma");
            }
        }
    }
}
