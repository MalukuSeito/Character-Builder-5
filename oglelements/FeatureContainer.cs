using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace OGL
{
    public class FeatureContainer: IXML, IOGLElement<FeatureContainer>, IOGLElement
    {
        [XmlIgnore]
        public string FileName { get; set; }
        [XmlIgnore]
        public string category;
        [XmlIgnore]
        public string Name { get; set; }
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(FeatureContainer));
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
        public String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        public FeatureContainer()
        {
            Features = new List<Feature>();
            Source = "";
        }
        public FeatureContainer(Feature f)
        {
            Source = "";
            Features = new List<Feature>
            {
                f
            };
            if (f.Source != null) Source = f.Source;
        }

        public FeatureContainer(IEnumerable<Feature> features)
        {
            Source = "";
            Features = new List<Feature>(features);
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
        public static FeatureContainer LoadString(string text)
        {
            using (TextReader reader = new StringReader(text))
            {
                return ((FeatureContainer)Serializer.Deserialize(reader));
            }
        }
        public string Save()
        {
            using (StringWriter writer = new StringWriter())
            {
                Serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }
        public override string ToString()
        {
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
        }
        public FeatureContainer Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                FeatureContainer r = (FeatureContainer)Serializer.Deserialize(mem);
                r.FileName = FileName;
                r.category = category;
                r.Name = Name;
                return r;
            }
        }
        public static List<Feature> MakeCopy(List<Feature> features)
        {
            FeatureContainer fc = new FeatureContainer(features);
            return fc.Clone().Features;
        }

        public bool Matches(string text, bool nameOnly)
        {
            return Features.Exists(s => s.Matches(text, nameOnly));
        }
    }
}
