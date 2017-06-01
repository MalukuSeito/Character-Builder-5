using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Xsl;

namespace Character_Builder_5
{
    public class Background : IComparable<Background>, IHTML
    {
        [XmlIgnore]
        string filename;
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(Background));
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        [XmlElement(Order = 1)] 
        public String Name { get; set; }
        [XmlElement(Order = 2)] 
        public String Description { get; set; }
        [XmlArray(Order = 3),
        XmlArrayItem(Type = typeof(Description)),
        XmlArrayItem(Type = typeof(ListDescription)),
        XmlArrayItem(Type = typeof(TableDescription))]
        public List<Description> Descriptions { get; set; }
        [XmlArray(Order = 4),
        XmlArrayItem(Type = typeof(AbilityScoreFeature)),
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
        XmlArrayItem(Type = typeof(SubRaceFeature)),XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)),XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)),XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> Features;
        [XmlArray(Order = 5)] 
        public List<TableEntry> PersonalityTrait;
        [XmlArray(Order = 6)] 
        public List<TableEntry> Ideal;
        [XmlArray(Order = 7)] 
        public List<TableEntry> Bond;
        [XmlArray(Order = 8)] 
        public List<TableEntry> Flaw;
        [XmlElement(Order = 9)] 
        public String Source { get; set; }
        [XmlElement(Order = 10)]
        public String Flavour { get; set; }
        [XmlIgnore]
        static public Dictionary<String, Background> backgrounds = new Dictionary<string, Background>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static private Dictionary<String, Background> simple= new Dictionary<string, Background>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;

        public void register(string file)
        {
            filename = file;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (backgrounds.ContainsKey(full)) throw new Exception("Duplicate Background: " + full);
            backgrounds.Add(full, this);
            if (simple.ContainsKey(Name))
            {
                simple[Name].ShowSource = true;
                ShowSource = true;
            }
            else simple.Add(Name, this);
            
        }
        public Background()
        {
            Descriptions = new List<Description>();
            Source = ConfigManager.DefaultSource;
            Features = new List<Feature>();
            PersonalityTrait = new List<TableEntry>();
            Ideal = new List<TableEntry>();
            Bond = new List<TableEntry>();
            Flaw = new List<TableEntry>();
        }
        public Background(String name, String description)
        {
            Name = name;
            Description = description;
            Descriptions = new List<Description>();
            Source = ConfigManager.DefaultSource;
            Features = new List<Feature>();
            PersonalityTrait = new List<TableEntry>();
            Ideal = new List<TableEntry>();
            Bond = new List<TableEntry>();
            Flaw = new List<TableEntry>();
            register(null);
        }
        public static Background Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperator))
            {
                if (backgrounds.ContainsKey(name)) return backgrounds[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && backgrounds.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return backgrounds[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            throw new Exception("Unknown Background: " + name);
        }
        public static void ExportAll()
        {
            foreach (Background i in backgrounds.Values)
            {
                FileInfo file = SourceManager.getFileName(i.Name, i.Source, ConfigManager.Directory_Backgrounds);
                using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, i);
            }
        }
        public static void ImportAll()
        {
            backgrounds.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Backgrounds, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                using (TextReader reader = new StreamReader(f.Key.FullName))
                {
                    Background s = (Background)serializer.Deserialize(reader);
                    s.Source = f.Value;
                    foreach (Feature fea in s.Features) fea.Source = f.Value;
                    s.register(f.Key.FullName);
                }
            }
        }
        public String toHTML()
        {
            try
            {
                if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_Backgrounds.FullName);
                using (MemoryStream mem = new MemoryStream())
                {
                    serializer.Serialize(mem, this);
                    ConfigManager.RemoveDescription(mem);
                    mem.Seek(0, SeekOrigin.Begin);
                    XmlReader xr = XmlReader.Create(mem);
                    using (StringWriter textWriter = new StringWriter())
                    {
                        using (XmlWriter xw = XmlWriter.Create(textWriter))
                        {
                            transform.Transform(xr, xw);
                            return textWriter.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "<html><body><b>Error generating output:</b><br>" + ex.Message + "<br>" + ex.InnerException + "<br>" + ex.StackTrace + "</body></html>";
            }
        }
        public override string ToString()
        {
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
        }
        public int CompareTo(Background other)
        {
            return Name.CompareTo(other.Name);
        }
        public List<Feature> CollectFeatures(int level, IChoiceProvider provider)
        {
            List<Feature> res=new List<Feature>();
            foreach (Feature f in Features)
            {
                res.AddRange(f.Collect(level, provider));
            }
            return res;
        }
        public bool save(Boolean overwrite)
        {
            Name = Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.getFileName(Name, Source, ConfigManager.Directory_Backgrounds);
            if (file.Exists && (filename == null || !filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, this);
            this.filename = file.FullName;
            return true;
        }
        public Background clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Background r = (Background)serializer.Deserialize(mem);
                r.filename = filename;
                return r;
            }
        }
    }
}
