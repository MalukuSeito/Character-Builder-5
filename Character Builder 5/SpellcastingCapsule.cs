using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class SpellcastingCapsule
    {
        public SpellcastingFeature Spellcastingfeature;
        public SpellcastingCapsule(SpellcastingFeature scf)
        {
            Spellcastingfeature = scf;
        }
        public override string ToString()
        {
            return Spellcastingfeature.DisplayName;
        }
    }
}
