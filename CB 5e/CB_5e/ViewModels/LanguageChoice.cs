using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels
{
    public class LanguageChoice : ChoiceViewModel
    {
        public LanguageChoice(PlayerModel model, LanguageChoiceFeature feature) 
            : base(model, feature.UniqueID, feature.Amount, feature, model.Context.Languages.Values.OrderBy(s=>s.Name).ToList(), false)
        {
        }
        public override void Refresh(Feature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = ((LanguageChoiceFeature)feature).Amount;
            Options = Model.Context.Languages.Values.OrderBy(s => s.Name).ToList();
            UpdateOptions();
        }
    }
}
