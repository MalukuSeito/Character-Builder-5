using Character_Builder_Plugin;
using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class LanguageChoice : IPlugin
    {
        public int ExecutionOrdering { get => 0; }
        public string Name => "Customizing Your Origin - Choose Language";

        private Dictionary<LanguageProficiencyFeature, LanguageChoiceFeature> cache = new Dictionary<LanguageProficiencyFeature, LanguageChoiceFeature>();

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
            List<Feature> result = new List<Feature>();
            int counter = 0;
            foreach (Feature f in features)
            {
                if (f is LanguageProficiencyFeature lf)
                {
                    counter++;
                    if (!cache.ContainsKey(lf)) cache.Add(lf, MakeFeature(race.Name, lf.Languages, counter));
                    result.AddRange(cache[lf].Collect(level, provider, Context));
                }
                else result.Add(f);
                
            }
            return result;
        }

        private LanguageChoiceFeature MakeFeature(string name, List<string> langs, int counter)
        {
            return new LanguageChoiceFeature()
            {
                Action = OGL.Base.ActionType.ForceHidden,
                Amount = langs.Count,
                Hidden = true,
                NoDisplay = true,
                Level = 0,
                UniqueID = "PLUGIN_LANGUAGE_CHOICE_" + name.ToUpperInvariant() + "_" + counter,
                Name = "Choose your language - " + name,
                Source = "Language Choice " + name,
                Text = "Choose language(s) to replace " + string.Join(", ", langs),
                ShowSource = false
            };
        }

        public List<Feature> FilterSubClassFeatures(SubClass subcls, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterSubRaceFeatures(SubRace subrace, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            List<Feature> result = new List<Feature>();
            int counter = 0;
            foreach (Feature f in features)
            {

                if (f is LanguageProficiencyFeature lf)
                {
                    counter++;
                    if (!cache.ContainsKey(lf)) cache.Add(lf, MakeFeature(subrace.Name, lf.Languages, counter));
                    result.AddRange(cache[lf].Collect(level, provider, Context));
                }
                else result.Add(f);

            }
            return result;
        }
    }
}
