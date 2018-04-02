using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace OGL
{
    public class SubRace: IXML, IOGLElement<SubRace>, IOGLElement
    {
        [XmlIgnore]
        public static bool DETAILED_TOSTRING = false;
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(SubRace));
        [XmlIgnore]
        public string FileName { get; set; }
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
        XmlArrayItem(Type = typeof(ResistanceFeature)),
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
        public byte[] ImageData { get; set; }
        //[XmlIgnore]
        //public Race ParentRace
        //{
        //    get
        //    {
        //        if (RaceName == null || RaceName == "" || RaceName == "*") return null;
        //        return Race.Get(RaceName, Source);
        //    }
        //    set
        //    {
        //        if (value == null) RaceName = "";
        //        else RaceName = value.Name;
        //    }
        //}
        
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;

        public void Register(OGLContext context, string file)
        {
            FileName = file;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (context.SubRaces.ContainsKey(full)) throw new Exception("Duplicate Subrace: " + full);
            context.SubRaces.Add(full, this);
            if (context.SubRacesSimple.ContainsKey(Name))
            {
                context.SubRacesSimple[Name].ShowSource = true;
                ShowSource = true;
            }
            else context.SubRacesSimple.Add(Name, this);
        }
        public SubRace() : base() {
            Features = new List<Feature>();
            Descriptions = new List<Description>();
        }
        public SubRace(OGLContext context, String name, Race race)
        {
            Name = name;
            if (race != null) RaceName = race.Name;
            Features = new List<Feature>();
            Descriptions = new List<Description>();
            Source = context.Config.DefaultSource;
            Register(context, null);
        }
        public String ToXML()
        {
            using (StringWriter mem = new StringWriter())
            {
                Serializer.Serialize(mem, this);
                return mem.ToString();
            }
        }
        public void Write(Stream stream)
        {
            Serializer.Serialize(stream, this);
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
        public List<Feature> CollectFeatures(int level, IChoiceProvider choiceProvider, OGLContext context)
        {
            List<Feature> res = new List<Feature>();
            foreach (Feature f in Features)
            {
                f.Source = Source;
                res.AddRange(f.Collect(level, choiceProvider, context));
            }
            return res;
        }
        public SubRace Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                SubRace r = (SubRace)Serializer.Deserialize(mem);
                r.FileName = FileName;
                return r;
            }
        }
        public bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Source ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Description ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Flavour ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Features.Exists(s => s.Matches(text, nameOnly))
                || Descriptions.Exists(s => s.Matches(text, nameOnly));
        }
    }
}
