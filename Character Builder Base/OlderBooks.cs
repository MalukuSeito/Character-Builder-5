using Character_Builder_Plugin;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_Builder
{
    public class OlderBooks : IPlugin, IEqualityComparer<Feature>
    {

        public int ExecutionOrdering { get => 2; }
        public string Name
        {
            get
            {
                return "Player's Handbook (2024) - Backgrounds and Species from Older Books";
            }
        }

        private readonly CollectionChoiceFeature feature = new ()
        {
            Action = OGL.Base.ActionType.ForceHidden,
            UniqueID = "ORIGIN_FEAT",
            Amount = 1,
            Text = "If the background you choose doesn't provide a feat, you gain an Origin feat of your choice.",
            Name = "Origin Feat",
            NoDisplay = true,
            Hidden = true,
            Level = 0,
            Source = "Player's Handbook (2024)",
            AllowSameChoice = false,
            Collection = "Category = 'Feats' and Origin"
        };

        private readonly ChoiceFeature abilityScores = new()
        {
            Action = OGL.Base.ActionType.ForceHidden,
            Name = "Ability Scores",
            Level = 0,
            Amount = 1,
            Text = "When determining your character's ability scores, increase one score by 2 and increase a different score by 1, or increase three different scores by 1.",
            NoDisplay = true,
            Hidden = true,
            UniqueID = "ABILITY_SCORES",
            AllowSameChoice = false,
			Choices = new()
            {
                new ChoiceFeature()
                {
					Action = OGL.Base.ActionType.ForceHidden,
					Name = "One score by 2 and increase a different score by 1",
					Level = 0,
					Amount = 1,
					Text = "When determining your character's ability scores, increase one score by 2 and increase a different score by 1",
					NoDisplay = false,
					Hidden = true,
					UniqueID = "ABILITY_SCORES_TWO_ONE",
					AllowSameChoice = false,
					Choices = new()
					{
						ByTwo(Ability.Strength),
						ByTwo(Ability.Dexterity),
						ByTwo(Ability.Constitution),
						ByTwo(Ability.Intelligence),
						ByTwo(Ability.Wisdom),   
						ByTwo(Ability.Charisma)
					}
				},
                new ChoiceFeature()
                {
					Action = OGL.Base.ActionType.ForceHidden,
			        Name = "Three different scores by 1",
			        Level = 0,
			        Amount = 3,
			        Text = "When determining your character's ability scores increase three different scores by 1.",
			        NoDisplay = false,
			        Hidden = true,
			        UniqueID = "ABILITY_SCORES_THREE_DIFFERENT",
			        AllowSameChoice = false,
                    Choices = new()
                    {
                        Score(Ability.Strength, 1),
						Score(Ability.Dexterity, 1),
						Score(Ability.Constitution, 1),
						Score(Ability.Intelligence, 1),
						Score(Ability.Wisdom, 1),
						Score(Ability.Charisma, 1)
					}
				}
            }
        };

        private static MultiFeature ByTwo(Ability score) => new()
        {
            Action = OGL.Base.ActionType.ForceHidden,
            Name = score.ToString() + " by 2 and a different score by 1",
            Text = "Increase score.ToString() by 2 and increase a different score by 1.",
            Level = 0,
            NoDisplay = false,
            Hidden = true,
            Condition = null,
            Features = new()
            {
                Score(score, 2),
                new ChoiceFeature()
                {
					Action = OGL.Base.ActionType.ForceHidden,
					Name = "Increase a different scores by 1",
					Level = 0,
					Amount = 1,
					Text = "Increase a different scores by 1.",
					NoDisplay = false,
					Hidden = true,
					UniqueID = "ABILITY_SCORES_" + score.ToString().ToUpperInvariant(),
					AllowSameChoice = false,
					Choices = Scores(score)
				}
            }
        };

        private static List<Feature> Scores(Ability excluded)
        {
            List<Feature> list = new();
            if (excluded != Ability.Strength) list.Add(Score(Ability.Strength, 1));
			if (excluded != Ability.Dexterity) list.Add(Score(Ability.Dexterity, 1));
			if (excluded != Ability.Constitution) list.Add(Score(Ability.Constitution, 1));
			if (excluded != Ability.Intelligence) list.Add(Score(Ability.Intelligence, 1));
			if (excluded != Ability.Wisdom) list.Add(Score(Ability.Wisdom, 1));
			if (excluded != Ability.Charisma) list.Add(Score(Ability.Charisma, 1));
			return list;
        }

        private static AbilityScoreFeature Score(Ability score, int value) => new()
        {
			Action = OGL.Base.ActionType.ForceHidden,
			Name = score.ToString(),
			Level = 0,
            NoDisplay = false,
            Hidden = true,
            Text = "Your " + score.ToString() + " score increases by " + value,
            Strength = score == Ability.Strength ? value : 0,
            Dexterity = score == Ability.Dexterity ? value : 0,
			Constitution = score == Ability.Constitution ? value : 0,
			Intelligence = score == Ability.Intelligence ? value : 0,
			Wisdom = score == Ability.Wisdom ? value : 0,
			Charisma = score == Ability.Charisma ? value : 0,
            Modifier = AbilityScoreModifikation.AddScore,
            
		};

        public bool IsAbilityScoreFeature(Feature f)
        {
            if (f is AbilityScoreFeature) return true;
            if (f is MultiFeature mf) return mf.Features.All(IsAbilityScoreFeature);
            if (f is ChoiceFeature cf) return cf.Choices.All(IsAbilityScoreFeature);
            return false;
        }

        public List<Feature> FilterBackgroundFeatures(Background b, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            var l = new List<Feature>(features);
            if (!features.Any(f => IsFeatFeature(f, Context))) l.AddRange(feature.Collect(level, provider, Context));
            l.RemoveAll(IsAbilityScoreFeature);
            l.AddRange(abilityScores.Collect(level, provider, Context));
			return l;
        }

        private bool IsFeatFeature(Feature ff, OGLContext context)
        {
            if (ff is CollectionChoiceFeature ccf)
            {
                var options = context.GetFeatureCollection(ccf.Collection);
                return context.GetFeatureCollection(null).Any(options.Contains);
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
            List<Feature> l = new(features);
			l.RemoveAll(IsAbilityScoreFeature);
			return l;
        }

        public List<Feature> FilterSubClassFeatures(SubClass sc, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterSubRaceFeatures(SubRace sr, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
			List<Feature> l = new(features);
			l.RemoveAll(IsAbilityScoreFeature);
			return l;
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
