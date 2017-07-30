using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;
using Character_Builder;

namespace CB_5e.ViewModels
{
    public class ItemConditionChoice : ChoiceViewModel
    {
        public ItemConditionChoice(PlayerModel model, ItemChoiceConditionFeature feature) : base(model, feature.UniqueID, feature.Amount, feature, Utils.Filter(model.Context, feature.Condition))
        {
        }

        public override void Refresh(Feature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = ((ItemChoiceConditionFeature)feature).Amount;
            Options = Utils.Filter(Model.Context, ((ItemChoiceConditionFeature)feature).Condition);
            UpdateOptions();
        }
    }
}
