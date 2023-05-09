using Character_Builder_Plugin;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using System.Collections.Generic;

namespace PluginDMG
{
    public class SpellPoints : IPlugin
    {

        public int ExecutionOrdering { get => 0; }

        public string Name
        {
            get
            {
                return "Spell Points Instead of Spell Slots";
            }
        }

        public List<Feature> FilterBackgroundFeatures(Background background, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterBoons(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterClassFeatures(ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            if (cls != null && cls.MulticlassingSpellLevels != null && cls.MulticlassingSpellLevels.Count >= classlevel)
            {
                features.RemoveAll(f => f is SpellSlotsFeature);
                features.AddRange(GetFeature("CLS_" + cls.Name, cls.MulticlassingSpellLevels[classlevel - 1]));
            } else if (cls == null)
            {
                features.AddRange(GetFeature("MULTICLASSING", classlevel));
            }
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

        public List<Feature> FilterRaceFeatures(Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterSubClassFeatures(SubClass sc, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            if (sc != null && sc.MulticlassingSpellLevels != null && sc.MulticlassingSpellLevels.Count >= classlevel)
            {
                features.RemoveAll(f => f is SpellSlotsFeature);
                features.AddRange(GetFeature("SUB_" + sc.Name, sc.MulticlassingSpellLevels[classlevel - 1]));
            }
            return features;
        }

        public List<Feature> FilterSubRaceFeatures(SubRace subrace, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }
        private List<ResourceFeature> GetFeature(string i, int level)
        {
            List<ResourceFeature> features = new List<ResourceFeature>();
            ResourceFeature feature = new ResourceFeature("Spell Points", "Spell Level: Point Cost\n1st: 2\n2nd: 3\n3rd: 5 \n4th: 6\n5th: 7\n6th: 9\n7th: 10\n8th: 11\n9th: 13", "VARIANT_SPELL_POINTS_" + i.ToUpperInvariant(), "VARIANT_SPELL_POINTS", 0, RechargeModifier.LongRest, 1, false);
            switch (level)
            {
                case 1: feature.Value = "4"; break;
                case 2: feature.Value = "6"; break;
                case 3: feature.Value = "14"; break;
                case 4: feature.Value = "17"; break;
                case 5: feature.Value = "27"; break;
                case 6: feature.Value = "32"; break;
                case 7: feature.Value = "38"; break;
                case 8: feature.Value = "44"; break;
                case 9: feature.Value = "57"; break;
                case 10: feature.Value = "64"; break;
                case 11: feature.Value = "73"; break;
                case 12: feature.Value = "73"; break;
                case 13: feature.Value = "83"; break;
                case 14: feature.Value = "83"; break;
                case 15: feature.Value = "94"; break;
                case 16: feature.Value = "94"; break;
                case 17: feature.Value = "107"; break;
                case 18: feature.Value = "114"; break;
                case 19: feature.Value = "123"; break;
                default: feature.Value = "133"; break;
            }
            if (level < 3) feature.Text = "Max Spell Level 1st\n" + feature.Text;
            else if (level < 5) feature.Text = "Max Spell Level 2nd\n" + feature.Text;
            else if (level < 7) feature.Text = "Max Spell Level 3rd\n" + feature.Text;
            else if (level < 9) feature.Text = "Max Spell Level 4th\n" + feature.Text;
            else feature.Text = "Max Spell Level 5th\n" + feature.Text;
            features.Add(feature);
            if (level >= 11) features.Add(new ResourceFeature("6th Level Spell", "One spell of 6th level", "VARIANT_SPELL_POINTS_LEVEL6_" + i.ToUpperInvariant(), "VARIANT_SPELL_POINTS_LEVEL6", 1, RechargeModifier.LongRest));
            if (level >= 13) features.Add(new ResourceFeature("7th Level Spell", "One spell of 7th level", "VARIANT_SPELL_POINTS_LEVEL7_" + i.ToUpperInvariant(), "VARIANT_SPELL_POINTS_LEVEL7", 1, RechargeModifier.LongRest));
            if (level >= 15) features.Add(new ResourceFeature("8th Level Spell", "One spell of 8th level", "VARIANT_SPELL_POINTS_LEVEL8_" + i.ToUpperInvariant(), "VARIANT_SPELL_POINTS_LEVEL8", 1, RechargeModifier.LongRest));
            if (level >= 17) features.Add(new ResourceFeature("9th Level Spell", "One spell of 9th level", "VARIANT_SPELL_POINTS_LEVEL9_" + i.ToUpperInvariant(), "VARIANT_SPELL_POINTS_LEVEL9", 1, RechargeModifier.LongRest));
            return features;
        }
    }
}
