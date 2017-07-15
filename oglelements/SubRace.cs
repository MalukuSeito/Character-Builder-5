using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace OGL
{
    public class SubRace: IXML, IOGLElement<SubRace>
    {
        [XmlIgnore]
        public static bool DETAILED_TOSTRING = false;
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(SubRace));
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        [XmlIgnore]
        public string Filename { get; set; }
        [XmlArrayItem(Type = typeof(Description)),
        XmlArrayItem(Type = typeof(ListDescription)),
        XmlArrayItem(Type = typeof(TableDescription))]
        public List<Description> Descriptions { get; set; }
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
        public String Flavour { get; set; }
        public String Source { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        [XmlElement(ElementName = "ParentRace")]
        public String RaceName { get; set; }
        [XmlIgnore]
        public Bitmap Image
        {
            set
            { // serialize
                if (value == null) ImageData = null;
                else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    ImageData = ms.ToArray();
                }
            }
            get
            { // deserialize
                if (ImageData == null)
                {
                    return null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(ImageData))
                    {
                        return new Bitmap(ms);
                    }
                }
            }
        }

        public byte[] ImageData { get; set; }
        [XmlIgnore]
        public Race ParentRace
        {
            get
            {
                if (RaceName == null || RaceName == "" || RaceName == "*") return null;
                return Race.Get(RaceName, Source);
            }
            set
            {
                if (value == null) RaceName = "";
                else RaceName = value.Name;
            }
        }
        [XmlIgnore]
        static public Dictionary<String, SubRace> subraces = new Dictionary<string, SubRace>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        [XmlIgnore]
        static public Dictionary<String, SubRace> simple = new Dictionary<string, SubRace>(StringComparer.OrdinalIgnoreCase);
        public void Register(string file)
        {
            Filename = file;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (subraces.ContainsKey(full)) throw new Exception("Duplicate Subrace: " + full);
            subraces.Add(full, this);
            if (simple.ContainsKey(Name))
            {
                simple[Name].ShowSource = true;
                ShowSource = true;
            }
            else simple.Add(Name, this);
        }
        public SubRace() : base() {
            Features = new List<Feature>();
            Descriptions = new List<Description>();
            Source = ConfigManager.DefaultSource;
        }
        public SubRace(String name, Race race)
        {
            Name = name;
            if (race != null) RaceName = race.Name;
            Features = new List<Feature>();
            Descriptions = new List<Description>();
            Source = ConfigManager.DefaultSource;
            Register(null);
        }
          public static SubRace Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperator))
            {
                if (subraces.ContainsKey(name)) return subraces[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && subraces.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return subraces[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            ConfigManager.LogError("Unknown Subrace: " + name);
            SubRace sr = new SubRace(name, null)
            {
                Description = "Missing Entry"
            };
            return sr;
        }
        public String ToXML()
        {
            using (StringWriter mem = new StringWriter())
            {
                Serializer.Serialize(mem, this);
                return mem.ToString();
            }
        }

        public MemoryStream ToXMLStream()
        {
            MemoryStream mem = new MemoryStream();
            Serializer.Serialize(mem, this);
            return mem;
        }
        public override string ToString()
        {
            string n = Name;
            if (ShowSource || ConfigManager.AlwaysShowSource) n = n + " " + ConfigManager.SourceSeperator + " " + Source;
            if (DETAILED_TOSTRING) return n + " (" + RaceName + ")";
            return n;
        }
        public int CompareTo(SubRace other)
        {
            return Name.CompareTo(other.Name);
        }
        public List<Feature> CollectFeatures(int level, IChoiceProvider choiceProvider)
        {
            List<Feature> res = new List<Feature>();
            foreach (Feature f in Features)
            {
                f.Source = Source;
                res.AddRange(f.Collect(level, choiceProvider));
            }
            return res;
        }
        public static IEnumerable<SubRace> For(List<String> races)
        {
            return (from s in subraces.Values where races.Contains(s.RaceName) || s.RaceName == "*" select s);
        }
        public SubRace Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                SubRace r = (SubRace)Serializer.Deserialize(mem);
                r.Filename = Filename;
                return r;
            }
        }
    }
}
