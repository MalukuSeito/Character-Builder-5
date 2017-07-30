using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;

namespace CB_5e.ViewModels
{
    public class CollectionChoice : ChoiceViewModel
    {
        public CollectionChoice(PlayerModel model, CollectionChoiceFeature feature, int level) : base(model, feature.UniqueID, feature.Amount, feature, from f in model.Context.GetFeatureCollection(((CollectionChoiceFeature)feature).Collection) where f.Level <= level select f, feature.AllowSameChoice)
        {
        }

        public override void Refresh(Feature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = ((CollectionChoiceFeature)feature).Amount;
            Multiple = ((CollectionChoiceFeature)feature).AllowSameChoice;
            int level = Model.Context.Player.GetLevel();
            Options = from f in Model.Context.GetFeatureCollection(((CollectionChoiceFeature)feature).Collection) where f.Level <= level select f;
            UpdateOptions();
        }
    }
}
