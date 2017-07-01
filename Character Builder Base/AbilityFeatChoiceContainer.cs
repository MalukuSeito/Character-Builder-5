using OGL.Features;

namespace Character_Builder
{
    public class AbilityFeatChoiceContainer
    {
        public AbilityScoreFeatFeature ASFF { get; set; }
        public AbilityFeatChoiceContainer(AbilityScoreFeatFeature asff)
        {
            ASFF = asff;
        }
        public override string ToString()
        {
            return Player.Current.GetAbilityFeatChoice(ASFF).ToString();
        }
    }
}
