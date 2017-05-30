using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
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
            return Player.current.getAbilityFeatChoice(ASFF).ToString();
        }
    }
}
