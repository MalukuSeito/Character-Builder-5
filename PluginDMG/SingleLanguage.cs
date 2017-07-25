using Character_Builder_Plugin;
using OGL;
using OGL.Common;
using OGL.Features;
using System.Collections.Generic;
using System.Linq;

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

        public List<Feature> FilterBackgroundFeatures(Background b, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features, background);
        }

        public List<Feature> FilterBoons(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features, boon);
        }

        public List<Feature> FilterClassFeatures(ClassDefinition c, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features, cls);
        }

        public List<Feature> FilterCommonFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features, common);
        }

        public List<Feature> FilterFeats(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features, feat);
        }

        public List<Feature> FilterPossessionFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features, possesion);
        }

        public List<Feature> FilterRaceFeatures(Race r, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            List<Feature> res = ScanReplace(features, race);
            if (Context.LanguagesSimple.ContainsKey("Common"))
                res.Add(new LanguageProficiencyFeature("", "", Context.LanguagesSimple["Common"], 0, true));
            return res;
        }

        public List<Feature> FilterSubClassFeatures(SubClass sc, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features, subcls);
        }

        public List<Feature> FilterSubRaceFeatures(SubRace sr, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return ScanReplace(features, subrace);
        }
        private List<Feature> ScanReplace(List<Feature> feats, SkillProficiencyChoiceFeature replacement)
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
