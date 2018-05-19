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
    public class CollectionChoice : ChoiceViewModel<CollectionChoiceFeature>
    {
        public CollectionChoice(PlayerModel model, CollectionChoiceFeature feature) : base(model, feature.UniqueID, feature.Amount, feature, feature.AllowSameChoice)
        {
        }

        public override IXML GetValue(string nameWithSource)
        {
            Feature res = Model.Context.GetFeatureCollection(Feature.Collection).Find(feat => feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source == nameWithSource);
            if (res == null) res = Model.Context.GetFeatureCollection(Feature.Collection).Find(feat => ConfigManager.SourceInvariantComparer.Equals(feat, nameWithSource));
            return res;
        }

        public override void Refresh(CollectionChoiceFeature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = feature.Amount;
            Multiple = feature.AllowSameChoice;
            int level = Model.Context.Player.GetLevel();
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            int level = Model.Context.Player.GetLevel();
            return from f in Model.Context.GetFeatureCollection(Feature.Collection) where f.Level <= level select f;
        }
    }
}
