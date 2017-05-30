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
        public List<string> CalculatedChoices;
        public SpellChoiceCapsule(SpellChoiceFeature scf, int calculatedamount = 0, List<string> calculatedchoices = null)
        {
            Spellchoicefeature = scf;
            CalculatedAmount = calculatedamount;
            CalculatedChoices = calculatedchoices;
            if (CalculatedChoices == null) CalculatedChoices = new List<string>();
        }
        public override string ToString()
        {
            return Spellchoicefeature.Name + (CalculatedChoices != null ? (CalculatedAmount >= Spellchoicefeature.Amount ? " " + CalculatedChoices.Count + "/" + CalculatedAmount : "") + (CalculatedChoices.Count > 0 ? " (" + String.Join(", ", from s in CalculatedChoices orderby s select SourceInvariantComparer.NoSource(s)) + ")" : "") : "") + " " + Spellchoicefeature.info();
        }
    }
}
