using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;
using OGL;

namespace CB_5e.ViewModels.Character.Choices
{
    public class ChoiceFeatureChoice : ChoiceViewModel<ChoiceFeature>
    {
        public ChoiceFeatureChoice(PlayerModel model, ChoiceFeature feature) : base(model, feature.UniqueID, feature.Amount, feature, feature.AllowSameChoice)
        {
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Feature.Choices.FirstOrDefault(s => ConfigManager.SourceInvariantComparer.Equals(nameWithSource, s));
        }

        public override void Refresh(ChoiceFeature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = feature.Amount;
            Multiple = feature.AllowSameChoice;
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return Feature.Choices;
        }
    }
}
