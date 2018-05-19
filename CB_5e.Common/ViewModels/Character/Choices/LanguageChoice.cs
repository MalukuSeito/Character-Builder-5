using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;

namespace CB_5e.ViewModels.Character.Choices
{
    public class LanguageChoice : ChoiceViewModel<LanguageChoiceFeature>
    {
        public LanguageChoice(PlayerModel model, LanguageChoiceFeature feature) 
            : base(model, feature.UniqueID, feature.Amount, feature, false)
        {
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetLanguage(nameWithSource, Feature.Source);
        }

        public override void Refresh(LanguageChoiceFeature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = feature.Amount;
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return 
                Model.Context.Languages.Values.OrderBy(s => s.Name).ToList();
        }
    }
}
