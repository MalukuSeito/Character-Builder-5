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
    public class ItemConditionChoice : ChoiceViewModel<ItemChoiceConditionFeature>
    {
        public ItemConditionChoice(PlayerModel model, ItemChoiceConditionFeature feature) : base(model, feature.UniqueID, feature.Amount, feature)
        {
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetItem(nameWithSource, Feature.Source);
        }

        public override void Refresh(ItemChoiceConditionFeature feature)
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
