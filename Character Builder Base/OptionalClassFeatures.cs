using Character_Builder_Plugin;
using OGL;
using OGL.Common;
using OGL.Features;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class OptionalClassFeatures : IPlugin
    {
        public int ExecutionOrdering { get => 0; }
        public string Name => "Official - Optional Class Features";

        private Dictionary<ClassDefinition, List<ChoiceFeature>> cache = new Dictionary<ClassDefinition, List<ChoiceFeature>>();
        private Dictionary<ClassDefinition, List<Feature>> replacedFeatures = new Dictionary<ClassDefinition, List<Feature>>();
        private Dictionary<SubClass, List<ChoiceFeature>> cache2 = new Dictionary<SubClass, List<ChoiceFeature>>();
        private Dictionary<SubClass, List<Feature>> replacedFeatures2 = new Dictionary<SubClass, List<Feature>>();

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
            if (cls == null) return features;
            List<Feature> result = new List<Feature>();
            if (!replacedFeatures.ContainsKey(cls)) replacedFeatures.Add(cls, new List<Feature>());
            if (!cache.ContainsKey(cls)) {
                Dictionary<string, ChoiceFeature> optionalClassFeatures = new Dictionary<string, ChoiceFeature>();
                foreach (Feature f in Context.GetFeatureCollection("Category = 'Feats/Optional Class Features' and not Subclass and " + cls.Name))
                {
                    if (!optionalClassFeatures.ContainsKey(f.Name))
                    {
                        ChoiceFeature cf = new ChoiceFeature()
                        {
                            Amount = 1,
                            Level = f.Level,
                            Hidden = true,
                            NoDisplay = true,
                            Name = "Optional Class Feature - " + f.Name,
                            Action = OGL.Base.ActionType.ForceHidden,
                            Source = Name,
                            UniqueID = "OPTIONAL_CLASS_FEATURE_" + cls.Name + "_" + f.Name,
                            Text = "Optional Class Feature",
                            Choices = new List<Feature>()
                        };
                        optionalClassFeatures.Add(f.Name, cf);
                        List<Feature> replaces = matches(f.Keywords, cls.Features);
                        if (replaces.Count == 1)
                        {
                            cf.Name = "Optional Class Feature - " + replaces[0].Name;
                            replacedFeatures[cls].AddRange(replaces);
                            cf.Choices.Add(replaces[0]);
                        }
                        else if (replaces.Count > 1)
                        {
                            cf.Name = "Optional Class Feature - " + string.Join(", ", replaces.Select(s => s.Name));
                            replacedFeatures[cls].AddRange(replaces);
                            cf.Choices.Add(new MultiFeature()
                            {
                                Name = string.Join(", ", replaces.Select(s => s.Name)),
                                Text = string.Join("\n\n", replaces.Select(s => s.Text)),
                                Level = cf.Level,
                                Hidden = true,
                                NoDisplay = true,
                                Action = OGL.Base.ActionType.ForceHidden,
                                Source = Name,
                                Features = replaces
                            });
                        }
                        else
                        {
                            cf.Choices.Add(new Feature()
                            {
                                Name = "--Ignore--",
                                Text = "Do not select this optional feature",
                                Hidden = true,
                                Action = OGL.Base.ActionType.ForceHidden,
                                NoDisplay = true
                            });
                        }
                        cf.Choices.Add(f);
                    }
                    else optionalClassFeatures[f.Name].Choices.Add(f);
                }
                foreach (ChoiceFeature f in optionalClassFeatures.Values) result.AddRange(f.Collect(classlevel, provider, Context));
                cache.Add(cls, new List<ChoiceFeature>(optionalClassFeatures.Values.OrderBy(cf=>cf.Level).ThenBy(cf=>cf.Name)));
            }
            else
            {
                foreach (ChoiceFeature f in cache[cls]) result.AddRange(f.Collect(classlevel, provider, Context));
            }
            result.AddRange(features.Where(f => !replacedFeatures[cls].Exists(r=>f.Name.Equals(r.Name))));
            return result;
        }

        private List<Feature> matches(List<Keyword> keywords, List<Feature> features)
        {
            List<Feature> result = new List<Feature>();
            foreach (Feature f in features)
            {
                string kw = f.Name.Replace(" ", "_").ToLowerInvariant();
                if (keywords.Exists(k => k.Name.Equals(kw))) result.Add(f);
            }
            return result;
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

        public List<Feature> FilterSubClassFeatures(SubClass subcls, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            if (cls == null) return features;
            List<Feature> result = new List<Feature>();
            if (!replacedFeatures2.ContainsKey(subcls)) replacedFeatures2.Add(subcls, new List<Feature>());
            if (!cache2.ContainsKey(subcls))
            {
                Dictionary<string, ChoiceFeature> optionalClassFeatures = new Dictionary<string, ChoiceFeature>();
                foreach (Feature f in Context.GetFeatureCollection("Category = 'Feats/Optional Class Features' and Subclass and " + cls.Name + " and (" + new Regex("[^a-zA-Z]+").Replace(subcls.Name, "_") + " or Any_Subclass)"))
                {
                    if (!optionalClassFeatures.ContainsKey(f.Name))
                    {
                        ChoiceFeature cf = new ChoiceFeature()
                        {
                            Amount = 1,
                            Level = f.Level,
                            Hidden = true,
                            NoDisplay = true,
                            Name = "Optional Class Feature - " + f.Name,
                            Action = OGL.Base.ActionType.ForceHidden,
                            Source = Name,
                            UniqueID = "OPTIONAL_CLASS_FEATURE_" + cls.Name + "_" + f.Name,
                            Text = "Optional Class Feature",
                            Choices = new List<Feature>()
                        };
                        optionalClassFeatures.Add(f.Name, cf);
                        List<Feature> replaces = matches(f.Keywords, subcls.Features);
                        if (replaces.Count == 1)
                        {
                            cf.Name = "Optional Class Feature - " + replaces[0].Name;
                            replacedFeatures[cls].AddRange(replaces);
                            cf.Choices.Add(replaces[0]);
                        }
                        else if (replaces.Count > 1)
                        {
                            cf.Name = "Optional Class Feature - " + string.Join(", ", replaces.Select(s => s.Name));
                            replacedFeatures[cls].AddRange(replaces);
                            cf.Choices.Add(new MultiFeature()
                            {
                                Name = string.Join(", ", replaces.Select(s => s.Name)),
                                Text = string.Join("\n\n", replaces.Select(s => s.Text)),
                                Level = cf.Level,
                                Hidden = true,
                                NoDisplay = true,
                                Action = OGL.Base.ActionType.ForceHidden,
                                Source = Name,
                                Features = replaces
                            });
                        }
                        else
                        {
                            cf.Choices.Add(new Feature()
                            {
                                Name = "--Ignore--",
                                Text = "Do not select this optional feature",
                                Hidden = true,
                                Action = OGL.Base.ActionType.ForceHidden,
                                NoDisplay = true
                            });
                        }
                        cf.Choices.Add(f);
                    }
                    else optionalClassFeatures[f.Name].Choices.Add(f);
                }
                foreach (ChoiceFeature f in optionalClassFeatures.Values) result.AddRange(f.Collect(classlevel, provider, Context));
                cache2.Add(subcls, new List<ChoiceFeature>(optionalClassFeatures.Values.OrderBy(cf => cf.Level).ThenBy(cf => cf.Name)));
            }
            else
            {
                foreach (ChoiceFeature f in cache2[subcls]) result.AddRange(f.Collect(classlevel, provider, Context));
            }
            result.AddRange(features.Where(f => !replacedFeatures2[subcls].Exists(r => f.Name.Equals(r.Name))));
            return result;
        }

        public List<Feature> FilterSubRaceFeatures(SubRace subrace, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

    }
}
