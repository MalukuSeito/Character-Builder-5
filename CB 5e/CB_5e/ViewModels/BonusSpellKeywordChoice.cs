using Character_Builder;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels
{
    public class BonusSpellKeywordChoice : ChoiceViewModel<BonusSpellKeywordChoiceFeature>
    {
        public BonusSpellKeywordChoice(PlayerModel model, BonusSpellKeywordChoiceFeature feature) 
            : base(model, feature.UniqueID, feature.Amount, feature, false)
        {
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetSpell(nameWithSource, Feature.Source);
        }

        public override void Refresh(BonusSpellKeywordChoiceFeature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = feature.Amount;
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return Utils.FilterSpell(Model.Context, Feature.Condition, null);
        }
    }
}
