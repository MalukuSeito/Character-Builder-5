using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class IncreaseSpellChoiceAmountFeature : Feature
    {
        public string UniqueID { get; set; }
        public int Amount { get; set; }
        public IncreaseSpellChoiceAmountFeature()
            : base()
        {
            Amount = 1;
        }
        public IncreaseSpellChoiceAmountFeature(int level, string uniqueID, int amount = 1)
            : base("", "", level, true)
        {
            UniqueID = uniqueID;
            Amount = amount;
        }
        public override string Displayname()
        {
            return "Increase Spellchoices Feature";
        }
    }
}
