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
    public class CustomBackground : IPlugin, IEqualityComparer<Feature>
    {
        public int ExecutionOrdering { get => 0; }
        public string Name
        {
            get
            {
                return "Official - Custom Background";
            }
        }

        private static Feature Skills = new SkillProficiencyChoiceFeature()
        {
            Action = OGL.Base.ActionType.ForceHidden,
            UniqueID = "CUSTOM_PROFICIENCY_SKILL",
            Amount = 2,
            Text = "Choose any two skills",
            ProficiencyMultiplier = 1.0f,
            Name = "Skill Proficiencies",
            BonusType = OGL.Base.ProficiencyBonus.AddOnlyIfNotProficient,
            NoDisplay = true,
            Hidden = true,
            Level = 0,
            Source = "System Reference Document 5.1"
        };
        private static Feature Backtool1 = new ChoiceFeature()
        {
            Amount = 1,
            Action = OGL.Base.ActionType.ForceHidden,
            UniqueID = "CUSTOM_PROFICIENCY1",
            Name = "Other Proficiency",
            Text = "Choose a total of two tool proficiencies or languages",
            Level = 0,
            NoDisplay = true,
            Hidden = true,
            Choices = new List<Feature>() {
                new LanguageChoiceFeature() {
                    Action = OGL.Base.ActionType.ForceHidden,
                    UniqueID = "CUSTOM_PROFICIENCY1_LANGUAGE",
                    Amount = 1,
                    Text = "Choose any language",
                    Name = "Language Proficiency",
                    NoDisplay = true,
                    Hidden = true,
                    Level = 0,
                    Source = "System Reference Document 5.1"
                },
                new ToolProficiencyChoiceConditionFeature() {
                    Action = OGL.Base.ActionType.ForceHidden,
                    UniqueID = "CUSTOM_PROFICIENCY1_TOOL",
                    Amount = 1,
                    Text = "Choose any tool",
                    Name = "Tool Proficiency",
                    NoDisplay = true,
                    Hidden = true,
                    Condition = "Tool and not Weapon and not Armor and not Shield",
                    Level = 0,
                    Source = "System Reference Document 5.1"
                },
            }
        };
        private static Feature Backtool2 = new ChoiceFeature()
        {
            Amount = 1,
            Action = OGL.Base.ActionType.ForceHidden,
            UniqueID = "CUSTOM_PROFICIENCY2",
            Name = "Other Proficiency",
            Text = "Choose a total of two tool proficiencies or languages",
            Level = 0,
            NoDisplay = true,
            Hidden = true,
            Choices = new List<Feature>() {
                new LanguageChoiceFeature() {
                    Action = OGL.Base.ActionType.ForceHidden,
                    UniqueID = "CUSTOM_PROFICIENCY2_LANGUAGE",
                    Amount = 1,
                    Text = "Choose any language",
                    Name = "Language Proficiency",
                    NoDisplay = true,
                    Hidden = true,
                    Level = 0,
                    Source = "System Reference Document 5.1"
                },
                new ToolProficiencyChoiceConditionFeature() {
                    Action = OGL.Base.ActionType.ForceHidden,
                    UniqueID = "CUSTOM_PROFICIENCY2_TOOL",
                    Amount = 1,
                    Text = "Choose any tool",
                    Name = "Tool Proficiency",
                    NoDisplay = true,
                    Hidden = true,
                    Condition = "Tool and not Weapon and not Armor and not Shield",
                    Level = 0,
                    Source = "System Reference Document 5.1"
                },
            }
        };

        private ChoiceFeature feature = new ChoiceFeature()
        {
            Action = OGL.Base.ActionType.ForceHidden,
            UniqueID = "CUSTOM_BACKGROUND_FEATURE",
            Amount = 1,
            Text = "Choose a feature from any background",
            Name = "Background Feature",
            NoDisplay = true,
            Hidden = true,
            Level = 0,
            Source = "System Reference Document 5.1",
            AllowSameChoice = false
        };

        public List<Feature> FilterBackgroundFeatures(Background b, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            var feats = features.Where(f => IsFeatFeature(f, Context)).SelectMany(f=>f.Collect(level, provider, Context)).ToList();
            var l = features.Where(f=>!feats.Contains(f)).Where(f => !(f is LanguageProficiencyFeature || f is LanguageChoiceFeature || f is OtherProficiencyFeature || f is SkillProficiencyFeature || f is SkillProficiencyChoiceFeature || f is ToolKWProficiencyFeature || f is ToolProficiencyChoiceConditionFeature || f is ToolProficiencyFeature || typeof(Feature).Equals(f?.GetType()) || IsChoiceFeature(f, Context) || IsFeatFeature(f, Context) || CheckMulti(f, Context) || CheckChoiceFeature(f, Context))).ToList();
            l.AddRange(Skills.Collect(level, provider, Context));
            l.AddRange(Backtool1.Collect(level, provider, Context));
            l.AddRange(Backtool2.Collect(level, provider, Context));
            feature.Choices = Context.Backgrounds.Values.SelectMany(bb => bb.Features.Where(f => typeof(Feature).Equals(f?.GetType()) || IsMulti(f, Context) || IsFeatFeature(f, Context) || IsChoiceFeature(f, Context)).SelectMany(f => f is ChoiceFeature cf ? cf.Choices as IEnumerable<Feature> : new Feature[] { f })).OrderBy(f=>f.Name).Distinct(this).ToList();
            l.AddRange(feature.Collect(level, provider, Context));
            return l;
        }

        private bool IsFeatFeature(Feature ff, OGLContext context)
        {
            if (ff is CollectionChoiceFeature ccf)
            {
                var options = context.GetFeatureCollection(ccf.Collection);
                return context.GetFeatureCollection(null).Any(a => options.Contains(a));
                //Utils.Matches(context, ccf.Collection) context.GetFeatureCollection("Feats");
                //return ccf.Collection.ToLowerInvariant().Contains("Category = 'Feats'".ToLowerInvariant()) || ccf.Collection.ToLowerInvariant().Contains("Category = \"Feats\"".ToLowerInvariant()); ;
            }
            return false;
        }

        private bool IsMulti(Feature ff, OGLContext Context)
        {
            if (ff is MultiFeature mf)
                return mf.Features.Where(f => !(typeof(Feature).Equals(f?.GetType()) || IsMulti(f, Context) || IsFeatFeature(f, Context) || IsChoiceFeature(f, Context))).Count() == 0;
            return false;
        }

        private bool IsChoiceFeature(Feature ff, OGLContext Context)
        {
            if (ff is ChoiceFeature mf)
                return mf.Choices.Where(f => !(typeof(Feature).Equals(f?.GetType()) || IsMulti(f, Context) || IsFeatFeature(f, Context) || IsChoiceFeature(f, Context))).Count() == 0;
            return false;
        }

        private bool CheckMulti(Feature ff, OGLContext Context)
        {
            if (ff is MultiFeature mf)
                return mf.Features.Where(f => !(f is LanguageProficiencyFeature || f is LanguageChoiceFeature || f is OtherProficiencyFeature || f is SkillProficiencyFeature || f is SkillProficiencyChoiceFeature || f is ToolKWProficiencyFeature || f is ToolProficiencyChoiceConditionFeature || f is ToolProficiencyFeature || typeof(Feature).Equals(f?.GetType()) || IsChoiceFeature(f, Context) || IsFeatFeature(f, Context) || CheckMulti(f, Context) || CheckChoiceFeature(f, Context))).Count() == 0;
            return false;
        }

        private bool CheckChoiceFeature(Feature ff, OGLContext Context)
        {
            if (ff is ChoiceFeature mf)
                return mf.Choices.Where(f => !(f is LanguageProficiencyFeature || f is LanguageChoiceFeature || f is OtherProficiencyFeature || f is SkillProficiencyFeature || f is SkillProficiencyChoiceFeature || f is ToolKWProficiencyFeature || f is ToolProficiencyChoiceConditionFeature || f is ToolProficiencyFeature || typeof(Feature).Equals(f?.GetType()) || IsChoiceFeature(f, Context) || IsFeatFeature(f, Context) || CheckMulti(f, Context) || CheckChoiceFeature(f, Context))).Count() == 0;
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
