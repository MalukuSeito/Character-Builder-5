using Character_Builder_Plugin;
using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PluginDMG
{
    public class PlaneTouchedWings : IPlugin
    {
        public int ExecutionOrdering { get => 0; }
        private static Feature WingedTiefling = new Feature()
        {
            Name = "Plane-Touched Wings",
            Text = "You have bat-like wings sprouting from your shoulder blades. You have a ﬂying speed of 30 feet while you aren’t wearing heavy armor.",
            Action = OGL.Base.ActionType.ForceHidden,
            Hidden = false,
            Source = "Sword Coast Adventurer's Guide",
            Level = 5,

        };
        private static Feature WingedAasimar = new Feature()
        {
            Name = "Plane-Touched Wings",
            Text = "You can choose aasimar (Volo’s Guide to Monsters) as your character’s race.Additionally, at 5th level, you can permanently replace the Light Bearer trait and racial trait they gain at 3rd level to sprout feathered wings—gaining a fly speed of 30 ft while not wearing heavy armor.",
            Action = OGL.Base.ActionType.ForceHidden,
            Hidden = false,
            Source = "AL Player's Guide 9",
            Level = 5,

        };
        private static SkillProficiencyChoiceFeature boon = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature cls = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature subcls = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature race = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature subrace = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature feat = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature common = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature possesion = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);

        public string Name => "D&D AL Season 9 - Plane Touched Wings";

        public List<Feature> FilterBackgroundFeatures(Background b, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
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
            if (level >= 5 && "Tiefling".Equals(r.Name, StringComparison.OrdinalIgnoreCase))
            {
                List<Feature> res = new List<Feature>();
                bool found = false;
                foreach (Feature f in features)
                {
                    if ("Infernal Legacy".Equals(f.Name, StringComparison.OrdinalIgnoreCase)) found = true;
                    else res.Add(f);
                }
                if (found) res.Add(WingedTiefling);
                return res;
            }
            if (level >= 5 && r.Name.StartsWith("Aasimar", StringComparison.OrdinalIgnoreCase))
            {
                List<Feature> res = new List<Feature>();
                bool found = false;
                foreach (Feature f in features)
                {
                    if ("Light Bearer".Equals(f.Name, StringComparison.OrdinalIgnoreCase)) found = true;
                    else if (f.Level != 3) res.Add(f);
                }
                if (found) res.Add(WingedAasimar);
                return res;
            }
            return features;
            
        }

        public List<Feature> FilterSubClassFeatures(SubClass sc, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterSubRaceFeatures(SubRace sr, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            if (level >= 5 && "Aasimar".Equals(race.Name, StringComparison.OrdinalIgnoreCase))
            {
                List<Feature> res = new List<Feature>();
                foreach (Feature f in features) if (f.Level != 3) res.Add(f);
                return res;
            }
            return features;
        }
    }
}
