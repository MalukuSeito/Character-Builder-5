using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;

namespace CB_5e.ViewModels
{
    public class ChoiceFeatureChoice : ChoiceViewModel
    {
        public ChoiceFeatureChoice(PlayerModel model, ChoiceFeature feature) : base(model, feature.UniqueID, feature.Amount, feature, feature.Choices, feature.AllowSameChoice)
        {
        }
        public override void Refresh(Feature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = ((ChoiceFeature)feature).Amount;
            Multiple = ((ChoiceFeature)feature).AllowSameChoice;
            Options = ((ChoiceFeature)feature).Choices;
            UpdateOptions();
        }
    }
}
