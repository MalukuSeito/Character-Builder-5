using Character_Builder_Plugin;
using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_Builder
{
    public class BackgroundFeat : IPlugin, IEqualityComparer<Feature>
    {

        public int ExecutionOrdering { get => 1; }
        public string Name
        {
            get
            {
                return "Official - Background Feat";
            }
        }

        private CollectionChoiceFeature feature = new CollectionChoiceFeature()
        {
            Action = OGL.Base.ActionType.ForceHidden,
            UniqueID = "BACKGROUND_FEAT",
            Amount = 1,
            Text = "Choose a Feat if your background does not give a feat.",
            Name = "Background Feature",
            NoDisplay = true,
            Hidden = true,
            Level = 0,
            Source = "Astral Adventurers Guide Errata",
            AllowSameChoice = false,
            Collection = "Category = 'Feats' and Background"
        };

        public List<Feature> FilterBackgroundFeatures(Background b, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            var l = new List<Feature>(features);
            if (!features.Any(f => IsFeatFeature(f, Context))) l.AddRange(feature.Collect(level, provider, Context));
            return l;
        }

        private bool IsFeatFeature(Feature ff, OGLContext context)
        {
            if (ff is CollectionChoiceFeature ccf)
            {
                var options = context.GetFeatureCollection(ccf.Collection);
                return context.GetFeatureCollection(null).Any(a => options.Contains(a));
            }
            return false;
        }
        public List<Feature> FilterBoons(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterClassFeatures(ClassDefinition c, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterCommonFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterFeats(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterPossessionFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterRaceFeatures(Race r, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterSubClassFeatures(SubClass sc, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterSubRaceFeatures(SubRace sr, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public bool Equals(Feature x, Feature y)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(x.Name, y.Name);
        }

        public int GetHashCode(Feature obj)
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
        }
    }
}
