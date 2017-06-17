using Character_Builder_Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Character_Builder_5;

namespace PluginDMG
{
    public class SingleLanguage : IPlugin
    {
        private static SkillProficiencyChoiceFeature background = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature boon = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature cls = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature subcls = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature race = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature subrace = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature feat = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature common = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);
        private static SkillProficiencyChoiceFeature possesion = new SkillProficiencyChoiceFeature("Skill Proficiency", "", "VARIANT_SKILL_FOR_LANGUAGE", 0, 0, true);

        public string Name
        {
            get
            {
                return "Replace Languages with Skill Proficiencies";
            }
        }

        public List<Feature> filterBackgroundFeatures(Background b, List<Feature> features, int level, IChoiceProvider provider)
        {
            return scanReplace(features, background);
        }

        public List<Feature> filterBoons(List<Feature> features, int level, IChoiceProvider provider)
        {
            return scanReplace(features, boon);
        }

        public List<Feature> filterClassFeatures(ClassDefinition c, int classlevel, List<Feature> features, int level, IChoiceProvider provider)
        {
            return scanReplace(features, cls);
        }

        public List<Feature> filterCommonFeatures(List<Feature> features, int level, IChoiceProvider provider)
        {
            return scanReplace(features, common);
        }

        public List<Feature> filterFeats(List<Feature> features, int level, IChoiceProvider provider)
        {
            return scanReplace(features, feat);
        }

        public List<Feature> filterPossessionFeatures(List<Feature> features, int level, IChoiceProvider provider)
        {
            return scanReplace(features, possesion);
        }

        public List<Feature> filterRaceFeatures(Race r, List<Feature> features, int level, IChoiceProvider provider)
        {
            List<Feature> res = scanReplace(features, race);
            if (Language.simple.ContainsKey("Common"))
                res.Add(new LanguageProficiencyFeature("", "", Language.simple["Common"], 0, true));
            return res;
        }

        public List<Feature> filterSubClassFeatures(SubClass sc, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider)
        {
            return scanReplace(features, subcls);
        }

        public List<Feature> filterSubRaceFeatures(SubRace sr, Race race, List<Feature> features, int level, IChoiceProvider provider)
        {
            return scanReplace(features, subrace);
        }
        private List<Feature> scanReplace(List<Feature> feats, SkillProficiencyChoiceFeature replacement)
        {
            List<Feature> res = new List<Feature>(feats.Count);
            int count = 0;
            foreach (Feature f in feats)
            {
                if (f is LanguageChoiceFeature) count += (f as LanguageChoiceFeature).Amount;
                else if (f is LanguageProficiencyFeature) count += (f as LanguageProficiencyFeature).Languages.Where(s => !ConfigManager.SourceInvariantComparer.Equals(s, "Common")).Distinct().Count();
                else res.Add(f);
            }
            replacement.Amount = count;
            if (count >= 0) res.Add(replacement);
            return res;
        }
    }
}
