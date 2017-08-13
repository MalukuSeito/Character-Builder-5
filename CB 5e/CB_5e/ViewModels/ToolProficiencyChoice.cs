using Character_Builder;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;

namespace CB_5e.ViewModels
{
    public class ToolProficiencyChoice: ChoiceViewModel<ToolProficiencyChoiceConditionFeature>
    {
        public ToolProficiencyChoice(PlayerModel model, ToolProficiencyChoiceConditionFeature feature) : base(model, feature.UniqueID, feature.Amount, feature)
        {
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetItem(nameWithSource, Feature.Source);
        }

        public override void Refresh(ToolProficiencyChoiceConditionFeature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = feature.Amount;
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return Utils.Filter(Model.Context, Feature.Condition);
        }
    }
}
