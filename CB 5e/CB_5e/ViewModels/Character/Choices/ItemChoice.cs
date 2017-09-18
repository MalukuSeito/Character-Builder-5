using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;

namespace CB_5e.ViewModels.Character.Choices
{
    public class ItemChoice : ChoiceViewModel<ItemChoiceFeature>
    {
        public ItemChoice(PlayerModel model, ItemChoiceFeature feature) : base(model, feature.UniqueID, feature.Amount, feature)
        {
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetItem(nameWithSource, Feature.Source);
        }

        public override void Refresh(ItemChoiceFeature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = feature.Amount;
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return (from i in Feature.Items select Model.Context.GetItem(i, Feature.Source)).ToList();
        }
    }
}
