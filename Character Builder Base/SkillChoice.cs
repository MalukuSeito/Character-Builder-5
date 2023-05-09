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
    public class SkillChoice: IPlugin
    {
        public int ExecutionOrdering { get => 0; }
        public string Name => "Customizing Your Origin - Choose Skill Proficiency";

        private Dictionary<SkillProficiencyFeature, SkillProficiencyChoiceFeature> cache = new Dictionary<SkillProficiencyFeature, SkillProficiencyChoiceFeature>();
        private Dictionary<SkillProficiencyChoiceFeature, SkillProficiencyChoiceFeature> cache2 = new Dictionary<SkillProficiencyChoiceFeature, SkillProficiencyChoiceFeature>();

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
                if (f is SkillProficiencyFeature sf)
                {
                    counter++;
                    if (!cache.ContainsKey(sf)) cache.Add(sf, MakeFeature(race.Name, sf.Skills, counter));
                    result.AddRange(cache[sf].Collect(level, provider, Context));
                }
                else if (f is SkillProficiencyChoiceFeature scf && scf.ProficiencyMultiplier > 0.9d && scf.ProficiencyMultiplier < 1.1d && !scf.OnlyAlreadyKnownSkills && scf.Skills.Count > 0)
                {
                    counter++;
                    if (!cache2.ContainsKey(scf)) cache2.Add(scf, Convert(scf));
                    result.Add(cache2[scf]);
                }
                else result.Add(f);
                
            }
            return result;
        }

        private SkillProficiencyChoiceFeature Convert(SkillProficiencyChoiceFeature scf)
        {
            return new SkillProficiencyChoiceFeature()
            {
                Action = scf.Action,
                Amount = scf.Amount,
                BonusType = scf.BonusType,
                NoDisplay = scf.NoDisplay,
                Category = scf.Category,
                Hidden = scf.Hidden,
                ProficiencyMultiplier = 1.0d,
                OnlyAlreadyKnownSkills = false,
                Level = scf.Level,
                Keywords = new List<OGL.Keywords.Keyword>(scf.Keywords),
                KWChanged = scf.KWChanged,
                Name = scf.Name,
                Prerequisite = scf.Prerequisite,
                ShowSource = scf.ShowSource,
                Source = scf.Source,
                Skills = new List<string>(),
                Text = scf.Text,
                UniqueID = scf.UniqueID
            };
        }

        private SkillProficiencyChoiceFeature MakeFeature(string name, List<string> skills, int counter)
        {
            return new SkillProficiencyChoiceFeature()
            {
                Action = OGL.Base.ActionType.ForceHidden,
                Amount = skills.Count,
                Hidden = true,
                NoDisplay = true,
                Level = 0,
                UniqueID = "PLUGIN_SKILL_CHOICE_" + name.ToUpperInvariant() + "_" + counter,
                Name = "Choose your skill proficiency - " + name,
                Source = "Skill Proficiency Choice " + name,
                Text = "skill proficienc(y/ies) to replace " + string.Join(", ", skills),
                ProficiencyMultiplier = 1,
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
                if (f is SkillProficiencyFeature sf)
                {
                    counter++;
                    if (!cache.ContainsKey(sf)) cache.Add(sf, MakeFeature(subrace.Name, sf.Skills, counter));
                    result.AddRange(cache[sf].Collect(level, provider, Context));
                }
                else if (f is SkillProficiencyChoiceFeature scf && scf.ProficiencyMultiplier > 0.9d && scf.ProficiencyMultiplier < 1.1d && !scf.OnlyAlreadyKnownSkills && scf.Skills.Count > 0)
                {
                    counter++;
                    if (!cache2.ContainsKey(scf)) cache2.Add(scf, Convert(scf));
                    result.Add(cache2[scf]);
                }
                else result.Add(f);

            }
            return result;
        }
    }
}
