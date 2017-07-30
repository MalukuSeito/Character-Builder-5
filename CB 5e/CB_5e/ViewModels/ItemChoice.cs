using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;

namespace CB_5e.ViewModels
{
    public class ItemChoice : ChoiceViewModel
    {
        public ItemChoice(PlayerModel model, ItemChoiceFeature feature) : base(model, feature.UniqueID, feature.Amount, feature, (from i in feature.Items select model.Context.GetItem(i, feature.Source)).ToList())
        {
        }

        public override void Refresh(Feature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = ((ItemChoiceFeature)feature).Amount;
            Options = (from i in ((ItemChoiceFeature)feature).Items select Model.Context.GetItem(i, feature.Source)).ToList();
            UpdateOptions();
        }
    }
}
