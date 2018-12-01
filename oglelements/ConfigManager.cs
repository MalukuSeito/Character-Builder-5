using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml;
using OGL.Features;
using OGL.Base;
using OGL.Common;

namespace OGL
{
    public delegate void LogEvent(object sender, string message, Exception e);
    public class ConfigManager
    {
        public static XmlSerializer Serializer = new XmlSerializer(typeof(ConfigManager));
        public static SourceInvariantComparer SourceInvariantComparer = new SourceInvariantComparer(); 
        public static Boolean AlwaysShowSource = false;

        public static char SourceSeperator = '\u2014';
        public static string SourceSeperatorString = "\u2014";
        public static char[] InvalidChars = "\u2014".ToCharArray();
        public static bool Description = true;
        [XmlIgnore]
        public int MultiClassTarget
        {
            get
            {
                if (MultiClassingAbilityScoreRequirement > 0) return MultiClassingAbilityScoreRequirement;
                return 13;
            }
            set
            {
                MultiClassingAbilityScoreRequirement = value;
            }
        }

        public ConfigManager Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                ConfigManager r = (ConfigManager)Serializer.Deserialize(mem);
                return r;
            }
        }
        [XmlIgnore]
        public string DefaultSource
        {
            get
            {
                return Source;
            }
            set
            {
                Source = value;
            }
        }
        public static ILicense LicenseProvider;

        public static event LogEvent LogEvents;

        public static void LogError(Exception e) => LogEvents.Invoke(null, e.Message, e);
        public static void LogError(object source, Exception e) => LogEvents.Invoke(source, e.Message, e);
        public static void LogError(object source, string text, Exception e = null) => LogEvents.Invoke(source, text, e);
        public static void LogError(string text, Exception e = null) => LogEvents.Invoke(null, text, e);


        public ConfigManager()
        {
            WeightOfACoin = 1.0 / 50.0;
            Source = "My Source";
        }
       

        public int MultiClassingAbilityScoreRequirement = 13;
        public string Items_Directory = "Items";
        public string Items_Transform = "Items.xsl";
        public string Skills_Directory = "Skills";
        public string Skills_Transform = "Skills.xsl";
        public string Monster_Directory = "Monsters";
        public string Monster_Transform = "Monster.xsl";
        public string Languages_Directory = "Languages";
        public string Languages_Transform = "Languages.xsl";
        public string Features_Directory = "Feats";
        public string Features_Transform = "Features.xsl";
        public string Backgrounds_Directory = "Backgrounds";
        public string Backgrounds_Transform = "Backgrounds.xsl";
        public string Classes_Directory = "Classes";
        public string Classes_Transform = "Classes.xsl";
        public string SubClasses_Directory = "SubClasses";
        public string Plugins_Directory = "Plugins";
        public string SubClasses_Transform = "SubClasses.xsl";
        public string Races_Directory = "Races";
        public string Races_Transform = "Races.xsl";
        public string SubRaces_Directory = "SubRaces";
        public string SubRaces_Transform = "SubRaces.xsl";
        public string Spells_Directory = "Spells";
        public string Spells_Transform = "Spells.xsl";
        public string Magic_Directory = "Magic";
        public string Magic_Transform = "Magic.xsl";
        public string Conditions_Directory = "Conditions";
        public string Conditions_Transform = "Conditions.xsl";
        public string Possessions_Transform = "Possession.xsl";
        public string RemoveDescription_Transform = "NoDescription.xsl";
        public List<string> PDF = new List<string>() {};
        public List<string> Slots = new List<string>() { };
        public string Levels = "Levels.xml";
        public string AbilityScores = "AbilityScores.xml";
        public string Source { get; set; }
        public double WeightOfACoin { get; set; }
        [XmlArrayItem(Type = typeof(AbilityScoreFeature)),
        XmlArrayItem(Type = typeof(BonusSpellKeywordChoiceFeature)),
        XmlArrayItem(Type = typeof(ChoiceFeature)),
        XmlArrayItem(Type = typeof(CollectionChoiceFeature)),
        XmlArrayItem(Type = typeof(Feature)),
        XmlArrayItem(Type = typeof(FreeItemAndGoldFeature)),
        XmlArrayItem(Type = typeof(ItemChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(ItemChoiceFeature)),
        XmlArrayItem(Type = typeof(HitPointsFeature)),
        XmlArrayItem(Type = typeof(LanguageProficiencyFeature)),
        XmlArrayItem(Type = typeof(LanguageChoiceFeature)),
        XmlArrayItem(Type = typeof(MultiFeature)),
        XmlArrayItem(Type = typeof(OtherProficiencyFeature)),
        XmlArrayItem(Type = typeof(ResistanceFeature)),
        XmlArrayItem(Type = typeof(FormsCompanionsFeature)),
        XmlArrayItem(Type = typeof(FormsCompanionsBonusFeature)),
        XmlArrayItem(Type = typeof(SaveProficiencyFeature)),
        XmlArrayItem(Type = typeof(SpeedFeature)),
        XmlArrayItem(Type = typeof(SkillProficiencyChoiceFeature)),
        XmlArrayItem(Type = typeof(SkillProficiencyFeature)),
        XmlArrayItem(Type = typeof(SubRaceFeature)), XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)), XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)), XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> FeaturesForAll = new List<Feature>();

        [XmlArrayItem(Type = typeof(AbilityScoreFeature)),
        XmlArrayItem(Type = typeof(BonusSpellKeywordChoiceFeature)),
        XmlArrayItem(Type = typeof(ChoiceFeature)),
        XmlArrayItem(Type = typeof(CollectionChoiceFeature)),
        XmlArrayItem(Type = typeof(Feature)),
        XmlArrayItem(Type = typeof(FreeItemAndGoldFeature)),
        XmlArrayItem(Type = typeof(ItemChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(ItemChoiceFeature)),
        XmlArrayItem(Type = typeof(HitPointsFeature)),
        XmlArrayItem(Type = typeof(LanguageProficiencyFeature)),
        XmlArrayItem(Type = typeof(LanguageChoiceFeature)),
        XmlArrayItem(Type = typeof(MultiFeature)),
        XmlArrayItem(Type = typeof(OtherProficiencyFeature)),
        XmlArrayItem(Type = typeof(ResistanceFeature)),
        XmlArrayItem(Type = typeof(FormsCompanionsFeature)),
        XmlArrayItem(Type = typeof(FormsCompanionsBonusFeature)),
        XmlArrayItem(Type = typeof(SaveProficiencyFeature)),
        XmlArrayItem(Type = typeof(SpeedFeature)),
        XmlArrayItem(Type = typeof(SkillProficiencyChoiceFeature)),
        XmlArrayItem(Type = typeof(SkillProficiencyFeature)),
        XmlArrayItem(Type = typeof(SubRaceFeature)), XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)), XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)), XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> FeaturesForMulticlassing = new List<Feature>() {};
        
        [XmlIgnore]
        public List<string> PDFExporters { get; set; }
        [XmlIgnore]
        public List<Feature> CommonFeatures
        {
            get
            {
                return FeaturesForAll;
            }
        }
        [XmlIgnore]
        public List<Feature> MultiClassFeatures
        {
            get
            {
                return FeaturesForMulticlassing;
            }
        }
        private static Regex quotes = new Regex("([\\\"])(?:\\\\\\1|.)*?\\1");

        public static string FixQuotes(string exp)
        {
            if (exp == null) return "true";
            StringBuilder res = new StringBuilder();
            int last = 0;
            for (Match m = quotes.Match(exp); m.Success; m = m.NextMatch())
            {
                res.Append(exp.Substring(last, m.Index - last));
                last = m.Index + m.Length;
                res.Append("'");
                res.Append(m.Value.Substring(1, m.Length - 2).Replace("\\\"", "\"").Replace("'", "\\'"));
                res.Append("'");
            }
            if (last == 0) return exp;
            if (last < exp.Length) res.Append(exp.Substring(last));
            return res.ToString();
        }
    }
}
