using Character_Builder_Plugin;
using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_Builder_Base
{
    public class NoFreeEquipment : IPlugin
    {
        public string Name
        {
            get
            {
                return "No Starting Equipment or Gold (Add your own starting wealth)";
            }
        }

        public List<Feature> FilterBackgroundFeatures(Background b, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features);
        }

        public List<Feature> FilterBoons(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features);
        }

        public List<Feature> FilterClassFeatures(ClassDefinition c, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features);
        }

        public List<Feature> FilterCommonFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features);
        }

        public List<Feature> FilterFeats(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features);
        }

        public List<Feature> FilterPossessionFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features);
        }

        public List<Feature> FilterRaceFeatures(Race r, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features);
        }

        public List<Feature> FilterSubClassFeatures(SubClass sc, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features);
        }

        public List<Feature> FilterSubRaceFeatures(SubRace sr, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features);
        }
        private List<Feature> ScanReplace(List<Feature> feats)
        {
            return feats.Where(f => !(f is FreeItemAndGoldFeature)).ToList();
        }
    }
}
