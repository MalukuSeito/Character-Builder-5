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
    public class FormsCompanionsBonusFeatureEditModel : FeatureEditModel<FormsCompanionsBonusFeature>
    {
        public FormsCompanionsBonusFeatureEditModel(FormsCompanionsBonusFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }

        public string UniqueID
        {
            get => Feature.UniqueID;
            set
            {
                if (value == UniqueID) return;
                MakeHistory("UniqueID");
                Feature.UniqueID = value;
                OnPropertyChanged("UniqueID");
            }
        }
        public string HPBonus
        {
            get => Feature.HPBonus;
            set
            {
                if (value == HPBonus) return;
                MakeHistory("HPBonus");
                Feature.HPBonus = value;
                OnPropertyChanged("HPBonus");
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
        public string SavesBonus
        {
            get => Feature.SavesBonus;
            set
            {
                if (value == SavesBonus) return;
                MakeHistory("SavesBonus");
                Feature.SavesBonus = value;
                OnPropertyChanged("SavesBonus");
            }
        }
        public string SkillsBonus
        {
            get => Feature.SkillsBonus;
            set
            {
                if (value == SkillsBonus) return;
                MakeHistory("SkillsBonus");
                Feature.SkillsBonus = value;
                OnPropertyChanged("SkillsBonus");
            }
        }
        public string StrengthBonus
        {
            get => Feature.StrengthBonus;
            set
            {
                if (value == StrengthBonus) return;
                MakeHistory("StrengthBonus");
                Feature.StrengthBonus = value;
                OnPropertyChanged("StrengthBonus");
            }
        }
        public string DexterityBonus
        {
            get => Feature.DexterityBonus;
            set
            {
                if (value == DexterityBonus) return;
                MakeHistory("DexterityBonus");
                Feature.DexterityBonus = value;
                OnPropertyChanged("DexterityBonus");
            }
        }
        public string ConstitutionBonus
        {
            get => Feature.ConstitutionBonus;
            set
            {
                if (value == ConstitutionBonus) return;
                MakeHistory("ConstitutionBonus");
                Feature.ConstitutionBonus = value;
                OnPropertyChanged("ConstitutionBonus");
            }
        }
        public string IntelligenceBonus
        {
            get => Feature.IntelligenceBonus;
            set
            {
                if (value == IntelligenceBonus) return;
                MakeHistory("IntelligenceBonus");
                Feature.IntelligenceBonus = value;
                OnPropertyChanged("IntelligenceBonus");
            }
        }
        public string WisdomBonus
        {
            get => Feature.WisdomBonus;
            set
            {
                if (value == WisdomBonus) return;
                MakeHistory("WisdomBonus");
                Feature.WisdomBonus = value;
                OnPropertyChanged("WisdomBonus");
            }
        }
        public string CharismaBonus
        {
            get => Feature.CharismaBonus;
            set
            {
                if (value == CharismaBonus) return;
                MakeHistory("CharismaBonus");
                Feature.CharismaBonus = value;
                OnPropertyChanged("CharismaBonus");
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
        public List<string> Senses { get => Feature.Senses; }
        public List<string> Speed { get => Feature.Speed; }
        public List<string> Languages { get => Feature.Languages; }
        public string TraitBonusName
        {
            get => Feature.TraitBonusName;
            set
            {
                if (value == TraitBonusName) return;
                MakeHistory("TraitBonusName");
                Feature.TraitBonusName = value;
                OnPropertyChanged("TraitBonusName");
            }
        }
        public string TraitBonusText
        {
            get => Feature.TraitBonusText;
            set
            {
                if (value == TraitBonusText) return;
                MakeHistory("TraitBonusText");
                Feature.TraitBonusText = value;
                OnPropertyChanged("TraitBonusText");
            }
        }


    }
}
