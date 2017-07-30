using Character_Builder;
using OGL;
using OGL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels
{
    public class ScoresModelViewModel : BaseViewModel
    {
        public AbilityScoreArray Current;
        public AbilityScoreArray Max;
        public Dictionary<Ability, int> Saves;
        public ScoresModelViewModel(BuilderContext Context)
        {
            Update(Context.Player);
        }
        public void Update(Player p)
        {
            Saves = p.GetSaves();
            Current = p.GetFinalAbilityScores(out Max);
            OnPropertyChanged(null);
        }

        public int StrengthValue { get { return Current.Strength; } }
        public int StrengthMax { get { return Max.Strength; } }
        public string StrengthMod { get { return Current.StrMod.ToString("+#;-#;0"); } }
        public string StrengthSave { get { return Saves[Ability.Strength].ToString("+#;-#;0"); } }

        public int DexterityValue { get { return Current.Dexterity; } }
        public int DexterityMax { get { return Max.Dexterity; } }
        public string DexterityMod { get { return Current.DexMod.ToString("+#;-#;0"); } }
        public string DexteritySave { get { return Saves[Ability.Dexterity].ToString("+#;-#;0"); } }

        public int ConstitutionValue { get { return Current.Constitution; } }
        public int ConstitutionMax { get { return Max.Constitution; } }
        public string ConstitutionMod { get { return Current.ConMod.ToString("+#;-#;0"); } }
        public string ConstitutionSave { get { return Saves[Ability.Constitution].ToString("+#;-#;0"); } }

        public int IntelligenceValue { get { return Current.Intelligence; } }
        public int IntelligenceMax { get { return Max.Intelligence; } }
        public string IntelligenceMod { get { return Current.IntMod.ToString("+#;-#;0"); } }
        public string IntelligenceSave { get { return Saves[Ability.Intelligence].ToString("+#;-#;0"); } }

        public int WisdomValue { get { return Current.Wisdom; } }
        public int WisdomMax { get { return Max.Wisdom; } }
        public string WisdomMod { get { return Current.WisMod.ToString("+#;-#;0"); } }
        public string WisdomSave { get { return Saves[Ability.Wisdom].ToString("+#;-#;0"); } }

        public int CharismaValue { get { return Current.Charisma; } }
        public int CharismaMax { get { return Max.Charisma; } }
        public string CharismaMod { get { return Current.ChaMod.ToString("+#;-#;0"); } }
        public string CharismaSave { get { return Saves[Ability.Charisma].ToString("+#;-#;0"); } }
    }
}
