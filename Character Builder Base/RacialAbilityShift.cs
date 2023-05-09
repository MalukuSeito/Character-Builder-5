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
    public class RacialAbilityShift : IPlugin
    {
        public int ExecutionOrdering { get => 0; }
        public string Name => "Customizing Your Origin - Choose Racial Ability Score Increases";

        private Dictionary<AbilityScoreFeature, List<ChoiceFeature>> cache = new Dictionary<AbilityScoreFeature, List<ChoiceFeature>>();

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
                if (f is AbilityScoreFeature af)
                {
                    if (af.Modifier != AbilityScoreModifikation.AddScore || af.Strength == af.Dexterity && af.Strength == af.Constitution && af.Strength == af.Intelligence && af.Strength == af.Wisdom && af.Strength == af.Charisma)
                    {
                        result.Add(f);
                    }
                    else
                    {
                        counter++;
                        if (!cache.ContainsKey(af))
                        {
                            cache.Add(af, MakeFeatures(af, race.Name, counter));
                        }
                        foreach (ChoiceFeature cf in cache[af])
                        {
                            result.AddRange(cf.Collect(level, provider, Context));
                        }
                    }
                }
                else result.Add(f);
                
            }
            return result;
        }

        private List<ChoiceFeature> MakeFeatures(AbilityScoreFeature af, string name, int counter)
        {
            List<ChoiceFeature> result = new List<ChoiceFeature>();
            if (af.Strength != 0) result.Add(MakeFeature(name, "Strength", af.Strength, counter));
            if (af.Dexterity != 0) result.Add(MakeFeature(name, "Dexterity", af.Dexterity, counter));
            if (af.Constitution != 0) result.Add(MakeFeature(name, "Constitution", af.Constitution, counter));
            if (af.Intelligence != 0) result.Add(MakeFeature(name, "Intelligence", af.Intelligence, counter));
            if (af.Wisdom != 0) result.Add(MakeFeature(name, "Wisdom", af.Wisdom, counter));
            if (af.Charisma != 0) result.Add(MakeFeature(name, "Charisma", af.Charisma, counter));
            return result;
        }

        private ChoiceFeature MakeFeature(string name, string ability, int value, int counter)
        {
            return new ChoiceFeature()
            {
                Action = OGL.Base.ActionType.ForceHidden,
                AllowSameChoice = false,
                Amount = 1,
                Hidden = true,
                NoDisplay = true,
                Level = 0,
                Name = "Racial Ability Choice " + name + " - " + ability + " (" + PlusMinus(value) + ")",
                Source = "Racial Ability Shift " + name,
                Text = "Choose an Ability Score to replace " + name + " - " + ability + "(" + PlusMinus(value) + ")",
                UniqueID = "PLUGIN_RACIAL_ABILITY_SHIFT_" + name.ToUpperInvariant() + "_" + ability.ToUpperInvariant() + "_" + counter,
                ShowSource = false,
                Choices = new List<Feature>()
                {
                    new AbilityScoreFeature()
                    {
                        Modifier = AbilityScoreModifikation.AddScore,
                        Level = 0,
                        Name = "Strength",
                        NoDisplay = true,
                        Hidden = true,
                        Action = OGL.Base.ActionType.ForceHidden,
                        Text = "Increases your strength ability score by " + value,
                        Strength = value,
                        ShowSource = false,
                        Source = "Racial Ability Shift - " + name
                    },
                    new AbilityScoreFeature()
                    {
                        Modifier = AbilityScoreModifikation.AddScore,
                        Level = 0,
                        Name = "Dexterity",
                        NoDisplay = true,
                        Hidden = true,
                        Action = OGL.Base.ActionType.ForceHidden,
                        Text = "Increases your dexterity ability score by " + value,
                        Dexterity = value,
                        ShowSource = false,
                        Source = "Racial Ability Shift - " + name
                    },
                    new AbilityScoreFeature()
                    {
                        Modifier = AbilityScoreModifikation.AddScore,
                        Level = 0,
                        Name = "Constitution",
                        NoDisplay = true,
                        Hidden = true,
                        Action = OGL.Base.ActionType.ForceHidden,
                        Text = "Increases your constitution ability score by " + value,
                        Constitution = value,
                        ShowSource = false,
                        Source = "Racial Ability Shift - " + name
                    },
                    new AbilityScoreFeature()
                    {
                        Modifier = AbilityScoreModifikation.AddScore,
                        Level = 0,
                        Name = "Intelligence",
                        NoDisplay = true,
                        Hidden = true,
                        Action = OGL.Base.ActionType.ForceHidden,
                        Text = "Increases your intelligence ability score by " + value,
                        Intelligence = value,
                        ShowSource = false,
                        Source = "Racial Ability Shift - " + name
                    },
                    new AbilityScoreFeature()
                    {
                        Modifier = AbilityScoreModifikation.AddScore,
                        Level = 0,
                        Name = "Wisdom",
                        NoDisplay = true,
                        Hidden = true,
                        Action = OGL.Base.ActionType.ForceHidden,
                        Text = "Increases your wisdom ability score by " + value,
                        Wisdom = value,
                        ShowSource = false,
                        Source = "Racial Ability Shift - " + name
                    },
                    new AbilityScoreFeature()
                    {
                        Modifier = AbilityScoreModifikation.AddScore,
                        Level = 0,
                        Name = "Charisma",
                        NoDisplay = true,
                        Hidden = true,
                        Action = OGL.Base.ActionType.ForceHidden,
                        Text = "Increases your charisma ability score by " + value,
                        Charisma = value,
                        ShowSource = false,
                        Source = "Racial Ability Shift - " + name
                    }
                }
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
                if (f is AbilityScoreFeature af)
                {
                    if (af.Modifier != AbilityScoreModifikation.AddScore || af.Strength == af.Dexterity && af.Strength == af.Constitution && af.Strength == af.Intelligence && af.Strength == af.Wisdom && af.Strength == af.Charisma)
                    {
                        result.Add(f);
                    }
                    else
                    {
                        counter++;
                        if (!cache.ContainsKey(af))
                        {
                            cache.Add(af, MakeFeatures(af, subrace.Name, counter));
                        }
                        foreach (ChoiceFeature cf in cache[af])
                        {
                            result.AddRange(cf.Collect(level, provider, Context));
                        }
                    }
                }
                else result.Add(f);

            }
            return result;
        }

        private string PlusMinus(int value, string zero = "+0")
        {
            if (value > 0) return "+" + value;
            if (value == 0) return zero;
            return value.ToString();
        }
    }
}
