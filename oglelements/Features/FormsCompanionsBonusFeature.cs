using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL.Features
{
    public class FormsCompanionsBonusFeature: Feature
    {
        public FormsCompanionsBonusFeature()
        {
            Action = Base.ActionType.ForceHidden;
        }
        public string UniqueID { get; set; }
        public string HPBonus { get; set; }
        public string ACBonus { get; set; }
        public string SavesBonus { get; set; }
        public string SkillsBonus { get; set; }
        public string StrengthBonus { get; set; }
        public string DexterityBonus { get; set; }
        public string ConstitutionBonus { get; set; }
        public string IntelligenceBonus { get; set; }
        public string WisdomBonus { get; set; }
        public string CharismaBonus { get; set; }
        public string AttackBonus { get; set; }
        public string DamageBonus { get; set; }
        public List<string> Senses { get; set; } = new List<string>();
        public List<string> Speed { get; set; } = new List<string>();
        public List<string> Languages { get; set; } = new List<string>();
        public string TraitBonusName { get; set; }
        public string TraitBonusText { get; set; }
        public override string Displayname()
        {
            return "Forms/Companions Bonus Feature";
        }
    }
}
