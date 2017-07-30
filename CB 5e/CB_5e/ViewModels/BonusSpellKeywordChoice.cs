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
    public class BonusSpellKeywordChoice : ChoiceViewModel
    {
        public BonusSpellKeywordChoice(PlayerModel model, BonusSpellKeywordChoiceFeature feature) 
            : base(model, feature.UniqueID, feature.Amount, feature, Utils.FilterSpell(model.Context, feature.Condition, null), false)
        {
        }
        public override void Refresh(Feature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = ((BonusSpellKeywordChoiceFeature)feature).Amount;
            Options = Utils.FilterSpell(Model.Context, ((BonusSpellKeywordChoiceFeature)feature).Condition, null);
            UpdateOptions();
        }
    }
}
