using Character_Builder;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels
{
    public class ToolProficiencyChoice: ChoiceViewModel
    {
        public ToolProficiencyChoice(PlayerModel model, ToolProficiencyChoiceConditionFeature feature) : base(model, feature.UniqueID, feature.Amount, feature, Utils.Filter(model.Context, feature.Condition))
        {
        }

        public override void Refresh(Feature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = ((ToolProficiencyChoiceConditionFeature)feature).Amount;
            Options = Utils.Filter(Model.Context, ((ToolProficiencyChoiceConditionFeature)feature).Condition);
            UpdateOptions();
        }
    }
}
