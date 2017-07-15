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
        public static StringComparer SourceInvariantComparer = new SourceInvariantComparer();
        public static string Directory_Items = "Items";
        public static FileInfo Transform_Items = new FileInfo("Items.xsl");
        public static string Directory_Skills = "Skills";
        public static FileInfo Transform_Skills = new FileInfo("Skills.xsl");
        public static string Directory_Languages = "Languages";
        public static FileInfo Transform_Languages = new FileInfo("Languages.xsl");
        public static string Directory_Features = "Feats";
        public static FileInfo Transform_Features = new FileInfo("Features.xsl");
        public static string Directory_Backgrounds = "Backgrounds";
        public static FileInfo Transform_Backgrounds = new FileInfo("Backgrounds.xsl");
        public static string Directory_Classes = "Classes";
        public static FileInfo Transform_Classes = new FileInfo("Classes.xsl");
        public static string Directory_SubClasses = "SubClasses/";
        public static FileInfo Transform_SubClasses = new FileInfo("SubClasses.xsl");
        public static string Directory_Races = "Races";
        public static FileInfo Transform_Races = new FileInfo("Races.xsl");
        public static string Directory_SubRaces = "SubRaces";
        public static FileInfo Transform_SubRaces = new FileInfo("SubRaces.xsl");
        public static string Directory_Spells = "Spells";
        public static FileInfo Transform_Spells = new FileInfo("Spells.xsl");
        public static string Directory_Magic = "Magic";
        public static FileInfo Transform_Magic = new FileInfo("Magic.xsl");
        public static string Directory_Conditions = "Conditions";
        public static FileInfo Transform_Conditions = new FileInfo("Conditions.xsl");
        public static FileInfo Transform_Possession = new FileInfo("Possession.xsl");
        public static FileInfo Transform_Description = new FileInfo("Descriptions.xsl");
        public static FileInfo Transform_Scroll = new FileInfo("Scroll.xsl");
        public static string Directory_Plugins = "Plugins";
        public static FileInfo Transform_RemoveDescription = new FileInfo("NoDescription.xsl");
        public static Boolean AlwaysShowSource = false;

        public static char SourceSeperator = '\u2014';
        public static char[] InvalidChars = (new string(Path.GetInvalidFileNameChars()) + SourceSeperator).ToCharArray();
        public static bool Description = true;
        public static int MultiClassTarget
        {
            get
            {
                if (Loaded!=null && Loaded.MultiClassingAbilityScoreRequirement > 0) return Loaded.MultiClassingAbilityScoreRequirement;
                return 13;
            }
            set
            {
                Loaded.MultiClassingAbilityScoreRequirement = value;
            }
        }
        public static string DefaultSource
        {
            get
            {
                return Loaded.Source;
            }
            set
            {
                Loaded.Source = value;
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

        public static ConfigManager Loaded { get; set; } = null;
        

        public int MultiClassingAbilityScoreRequirement = 13;
        public string Items_Directory = "Items/";
        public string Items_Transform = "Items.xsl";
        public string Skills_Directory = "Skills/";
        public string Skills_Transform = "Skills.xsl";
        public string Languages_Directory = "Languages/";
        public string Languages_Transform = "Languages.xsl";
        public string Features_Directory = "Feats/";
        public string Features_Transform = "Features.xsl";
        public string Backgrounds_Directory = "Backgrounds/";
        public string Backgrounds_Transform = "Backgrounds.xsl";
        public string Classes_Directory = "Classes/";
        public string Classes_Transform = "Classes.xsl";
        public string SubClasses_Directory = "SubClasses/";
        public string Plugins_Directory = "Plugins/";
        public string SubClasses_Transform = "SubClasses.xsl";
        public string Races_Directory = "Races/";
        public string Races_Transform = "Races.xsl";
        public string SubRaces_Directory = "SubRaces/";
        public string SubRaces_Transform = "SubRaces.xsl";
        public string Spells_Directory = "Spells/";
        public string Spells_Transform = "Spells.xsl";
        public string Magic_Directory = "Magic/";
        public string Magic_Transform = "Magic.xsl";
        public string Conditions_Directory = "Conditions/";
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
        
        public static List<string> PDFExporters { get; set; }
        public static List<Feature> CommonFeatures
        {
            get
            {
                return Loaded.FeaturesForAll;
            }
        }
        public static List<Feature> MultiClassFeatures
        {
            get
            {
                return Loaded.FeaturesForMulticlassing;
            }
        }
        private static Regex quotes = new Regex("([\\\"])(?:\\\\\\1|.)*?\\1", RegexOptions.Compiled);

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
