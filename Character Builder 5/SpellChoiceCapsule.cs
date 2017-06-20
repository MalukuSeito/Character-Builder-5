using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class SpellChoiceCapsule
    {
        public SpellChoiceFeature Spellchoicefeature;
        public int CalculatedAmount;
        public List<Spell> CalculatedChoices;
        public SpellChoiceCapsule(SpellChoiceFeature scf, int calculatedamount = 0, List<Spell> calculatedchoices = null)
        {
            Spellchoicefeature = scf;
            CalculatedAmount = calculatedamount;
            CalculatedChoices = calculatedchoices;
            if (CalculatedChoices == null) CalculatedChoices = new List<Spell>();
        }
        public override string ToString()
        {
            if (Spellchoicefeature == null) return "Additional Spells " + CalculatedAmount + " (" + (CalculatedChoices.Count > 0 ? " (" + String.Join(", ", from s in CalculatedChoices orderby s select s.Name) + ")" : "");
            return Spellchoicefeature.Name + (CalculatedChoices != null ? (CalculatedAmount >= Spellchoicefeature.Amount ? " " + CalculatedChoices.Count + "/" + CalculatedAmount : "") + (CalculatedChoices.Count > 0 ? " (" + String.Join(", ", from s in CalculatedChoices orderby s select s.Name) + ")" : "") : "") + " " + Spellchoicefeature.info();
        }
    }
}
