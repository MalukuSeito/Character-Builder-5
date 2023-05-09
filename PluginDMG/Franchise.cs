using Character_Builder_Plugin;
using OGL;
using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDMG
{
    public class Franchise : IPlugin
    {
        public int ExecutionOrdering { get => 0; }
        private Dictionary<string, MultiFeature> Previews;
        private ChoiceFeature RankChoice;
        private ChoiceFeature FranchiseChoice;
        private Dictionary<SubClass, List<Feature>> cache = new Dictionary<SubClass, List<Feature>>();
        private const string FRANCHISE = "FRANCHISE";
        private const string RANK = "FRANCHISE_RANK";
        private const string SOURCE = "Acquisitions Incorporated";

        public string Name => "Acquisitions Incorporated - Franchises";

        public List<Feature> FilterBackgroundFeatures(Background background, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterBoons(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            if (FranchiseChoice == null)
            {
                Previews = Context.SubClassFor("FRANCHISE").Select(s => Map(s)).ToDictionary(m => m.Name);
                FranchiseChoice = new ChoiceFeature()
                {
                    Name = "Franchise",
                    Action = OGL.Base.ActionType.ForceHidden,
                    Amount = 1,
                    AllowSameChoice = false,
                    Source = SOURCE,
                    Choices = Previews.Values.OrderBy(s => s.Name).ToList<Feature>(),
                    Hidden = true,
                    Text = "Select a franchise",
                    NoDisplay = true,
                    Level = 0,
                    UniqueID = FRANCHISE
                };
                RankChoice = new ChoiceFeature()
                {
                    Name = "Franchise Rank",
                    Action = OGL.Base.ActionType.ForceHidden,
                    Amount = 1,
                    AllowSameChoice = false,
                    Source = SOURCE,
                    Choices = Enumerable.Range(0, 5).Select(s => Rank(s)).ToList(),
                    Hidden = true,
                    Text = "Franchise Rank",
                    NoDisplay = true,
                    Level = 0,
                    UniqueID = RANK
                };
            }
            List<Feature> result = new List<Feature>(features)
                {
                    RankChoice,
                    FranchiseChoice
                };
            if (provider.GetChoice(FRANCHISE) is Choice choice && choice.Value != null && Context.GetSubClass(choice.Value, null) is SubClass sub && int.TryParse(SourceInvariantComparer.NoSource(provider.GetChoice(RANK)?.Value ?? "0"), out int rank))
            {
                if (!cache.ContainsKey(sub)) {
                    List<Feature> dfeatures = new List<Feature>();
                    foreach (Description d in sub.Descriptions) if (d is TableDescription td) dfeatures.Add(ToChoice(td, sub.Source));
                    cache[sub] = dfeatures;
                }
                result.AddRange(cache[sub]);
                result.AddRange(sub.CollectFeatures(rank, false, provider, Context));
            }
            return result;
        }

        private Feature ToChoice(TableDescription td, string source)
        {
            return new ChoiceFeature()
            {
                Name = td.Name,
                Action = OGL.Base.ActionType.ForceHidden,
                Amount = td.Amount,
                UniqueID = td.UniqueID,
                Choices = td.Entries.Select(e => new Feature() {
                    Name = e.ToString(),
                    Action = OGL.Base.ActionType.ForceHidden,
                    NoDisplay = true,
                    Hidden = true,
                    Source = source,
                    Level = 0,
                    Text = e.ToFullString()
                }).ToList(),
                Source = source,
                NoDisplay = true,
                Hidden = true,
                Level = 0,
                Text = td.Text
            };
        }

        private Feature Rank(int s)
        {
            return new Feature()
            {
                Name = s.ToString(),
                Source = SOURCE,
                Action = OGL.Base.ActionType.ForceHidden,
                Hidden = true,
                Text = "Rank " + s.ToString(),
                Level = 0
            };
        }

        private MultiFeature Map(SubClass s)
        {
            return new MultiFeature()
            {
                Name = s.Name,
                Source = s.Source,
                Hidden = true,
                Features = s.Descriptions.Select(d => Map(d, s.Source)).Concat(s.Features).ToList(),
                Text = s.Description,
                Action = OGL.Base.ActionType.ForceHidden,
                Condition = "false",
                Level = 0,
                Prerequisite = s.Flavour,
                NoDisplay = true
            };
        }

        private Feature Map(Description d, string source)
        {
            return new Feature()
            {
                Name = d.Name,
                Source = source,
                Action = OGL.Base.ActionType.ForceHidden,
                Hidden = true,
                Text = d.Text + (d is TableDescription td && td.Entries.Count > 0 ?  "\n" + string.Join("\n", td.Entries.Select(entry => entry.ToFullString())) : ""),
                Level = 0
            };
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
            return features;
        }

        public List<Feature> FilterSubClassFeatures(SubClass subcls, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterSubRaceFeatures(SubRace subrace, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }
    }
}
