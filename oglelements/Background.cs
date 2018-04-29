using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace OGL
{
    public class Background : IComparable<Background>, IXML, IOGLElement<Background>, IOGLElement
    {
        [XmlIgnore]
        public string FileName { get; set; }
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Background));
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
        XmlArrayItem(Type = typeof(ResistanceFeature)),
        XmlArrayItem(Type = typeof(FormsCompanionsFeature)),
        XmlArrayItem(Type = typeof(FormsCompanionsBonusFeature)),
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
        public bool ShowSource { get; set; } = false;
        [XmlElement(Order = 11)]
        public byte[] ImageData { get; set; }
        public void Register(OGLContext context, string file)
        {
            FileName = file;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (context.Backgrounds.ContainsKey(full)) throw new Exception("Duplicate Background: " + full);
            context.Backgrounds.Add(full, this);
            if (context.BackgroundsSimple.ContainsKey(Name))
            {
                context.BackgroundsSimple[Name].ShowSource = true;
                ShowSource = true;
            }
            else context.BackgroundsSimple.Add(Name, this);
            
        }
        public Background()
        {
            Descriptions = new List<Description>();
            Features = new List<Feature>();
            PersonalityTrait = new List<TableEntry>();
            Ideal = new List<TableEntry>();
            Bond = new List<TableEntry>();
            Flaw = new List<TableEntry>();
        }
        public Background(OGLContext context, String name, String description)
        {
            Name = name;
            Description = description;
            Descriptions = new List<Description>();
            Source = context.Config.DefaultSource;
            Features = new List<Feature>();
            PersonalityTrait = new List<TableEntry>();
            Ideal = new List<TableEntry>();
            Bond = new List<TableEntry>();
            Flaw = new List<TableEntry>();
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
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
        }
        public int CompareTo(Background other)
        {
            return Name.CompareTo(other.Name);
        }
        public List<Feature> CollectFeatures(int level, IChoiceProvider provider, OGLContext context)
        {
            List<Feature> res=new List<Feature>();
            foreach (Feature f in Features)
            {
                res.AddRange(f.Collect(level, provider, context));
            }
            return res;
        }
        public Background Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Background r = (Background)Serializer.Deserialize(mem);
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
                || PersonalityTrait.Exists(s => s.Matches(text, nameOnly))
                || Ideal.Exists(s => s.Matches(text, nameOnly))
                || Bond.Exists(s => s.Matches(text, nameOnly))
                || Flaw.Exists(s => s.Matches(text, nameOnly))
                || Descriptions.Exists(s => s.Matches(text, nameOnly))
                || Features.Exists(s => s.Matches(text, nameOnly));
        }
    }
}
