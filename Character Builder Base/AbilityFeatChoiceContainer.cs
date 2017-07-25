using OGL.Features;

namespace Character_Builder
{
    public class AbilityFeatChoiceContainer
    {
        public AbilityScoreFeatFeature ASFF { get; set; }
        private Player Player;
        public AbilityFeatChoiceContainer(Player player, AbilityScoreFeatFeature asff)
        {
            Player = player;
            ASFF = asff;
        }
        public override string ToString()
        {
            return Player.GetAbilityFeatChoice(ASFF).ToString();
        }
    }
}
