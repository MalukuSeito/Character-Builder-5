using OGL.Features;

namespace Character_Builder
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
            if (Spellcastingfeature == null) return "Additional Spells";
            return Spellcastingfeature.DisplayName;
        }
    }
}
