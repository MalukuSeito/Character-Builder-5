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
    public class SkillProficiencyFeatureEditModel : FeatureEditModel<SkillProficiencyFeature>
    {
        public SkillProficiencyFeatureEditModel(SkillProficiencyFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }

        public List<string> Skills { get => Feature.Skills; }
        public string ProficiencyMultiplier
        {
            get => Feature.ProficiencyMultiplier.ToString();
            set
            {
                double parsed = 0;
                if (value == "" || value == "-" || double.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, null, out parsed))
                {
                    if (Feature.ProficiencyMultiplier == parsed) return;
                    MakeHistory("ProficiencyMultiplier");
                    Feature.ProficiencyMultiplier = parsed;
                    if (value != "" && value != "-") OnPropertyChanged("ProficiencyMultiplier");
                }
                if (value != "" && value != "-") OnPropertyChanged("ProficiencyMultiplier");
            }
        }
        public string BonusType
        {
            get => Feature.BonusType.ToString();
            set
            {
                ProficiencyBonus parsed = ProficiencyBonus.AddOnlyIfNotProficient;
                if (Enum.TryParse(value, out parsed))
                {
                    if (Feature.BonusType == parsed) return;
                    MakeHistory("BonusType");
                    Feature.BonusType = parsed;
                    OnPropertyChanged("BonusType");
                }
                OnPropertyChanged("BonusType");
            }
        }
        public List<string> BonusTypes {
            get => Enum.GetNames(typeof(ProficiencyBonus)).ToList();
        }
    }
}
