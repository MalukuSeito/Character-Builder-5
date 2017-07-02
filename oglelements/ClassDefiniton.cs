using OGL.Base;
using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using OGL.Keywords;
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
    public class ClassDefinition : IComparable<ClassDefinition>, IHTML, IOGLElement<ClassDefinition>
    {
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(ClassDefinition));
        [XmlIgnore]
        public string filename;
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        public String Name { get; set; }
        public String Description { get; set; }
        public String Flavour { get; set; }
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
        public List<Feature> Features;
        public List<int> MulticlassingSpellLevels;
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
        public List<Feature> MulticlassingFeatures;
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
        public List<Feature> FirstClassFeatures;
        public Ability MulticlassingAbilityScores;
        public List<string> FeaturesToAddClassKeywordTo;
        public List<string> SpellsToAddClassKeywordTo;
        public int HitDie { get; set; }
        public int HitDieCount { get; set; } = 1;
        public int HPFirstLevel { get; set; }
        public int AverageHPPerLevel { get; set; }
        public bool AvailableAtFirstLevel { get; set; } = true;
        public String Source { get; set; }
        public String MulticlassingCondition {get; set; }
        [XmlIgnore]
        static public Dictionary<String, ClassDefinition> classes = new Dictionary<string, ClassDefinition>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static public Dictionary<String, ClassDefinition> simple = new Dictionary<string, ClassDefinition>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
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
        public void register(string filename, bool applyKeywords)
        {
            this.filename = filename;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (classes.ContainsKey(full)) throw new Exception("Duplicate Class: " + full);
            classes.Add(full, this);
            if (simple.ContainsKey(Name))
            {
                simple[Name].ShowSource = true;
                ShowSource = true;
            }
            else simple.Add(Name, this);
            Keyword me = new Keyword(Name);
            if (applyKeywords)
            {
                if (FeaturesToAddClassKeywordTo != null && FeaturesToAddClassKeywordTo.Count > 0) foreach (Feature f in FeatureCollection.Features) if (FeaturesToAddClassKeywordTo.Contains(f.Name, ConfigManager.SourceInvariantComparer)) f.AssignKeywords(me);
                if (SpellsToAddClassKeywordTo != null && SpellsToAddClassKeywordTo.Count > 0) foreach (Spell s in Spell.spells.Values) if (SpellsToAddClassKeywordTo.Contains(s.Name + " " + ConfigManager.SourceSeperator + " " + s.Source, ConfigManager.SourceInvariantComparer)) s.AssignKeywords(me);
            }
        }
        public ClassDefinition()
        {
            Descriptions = new List<Description>();
            Source = ConfigManager.DefaultSource;
            Features = new List<Feature>();
            MulticlassingSpellLevels = new List<int>();
            MulticlassingFeatures = new List<Feature>();
            FirstClassFeatures = new List<Feature>();
            MulticlassingAbilityScores = Ability.None;
            FeaturesToAddClassKeywordTo = new List<string>();
            SpellsToAddClassKeywordTo = new List<string>();
            HPFirstLevel = 4;
            HitDie = 4;
            AverageHPPerLevel = 2;
        }
        public ClassDefinition(String name, String description, int hitdie, List<string> featuresToAddClassKeywordTo = null, List<List<string>> spellsToAddClassKeywordTo = null)
        {
            Name = name;
            Description = description;
            Descriptions = new List<Description>();
            Source = ConfigManager.DefaultSource;
            Features = new List<Feature>();
            MulticlassingSpellLevels = new List<int>();
            MulticlassingFeatures = new List<Feature>();
            FirstClassFeatures = new List<Feature>();
            MulticlassingAbilityScores = Ability.None;
            HPFirstLevel = hitdie;
            HitDie = hitdie;
            AverageHPPerLevel = (hitdie / 2) + 1;
            FeaturesToAddClassKeywordTo = featuresToAddClassKeywordTo;
            SpellsToAddClassKeywordTo = new List<string>();
            int level=0;
            if (spellsToAddClassKeywordTo != null)
            {
                foreach (List<string> ls in spellsToAddClassKeywordTo)
                {
                    foreach (string s in ls) if (!Spell.simple.ContainsKey(s))
                        {
                            Spell sp = new Spell(level, s, "", "", "", "Missing Entry");
                            if (level == 0) sp.AssignKeywords(new Keyword("Cantrip"));

                        }
                    level++;
                    SpellsToAddClassKeywordTo.AddRange(ls);
                }
            }
            register(null, false);
        }
        public static ClassDefinition Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperator))
            {
                if (classes.ContainsKey(name)) return classes[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && classes.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return classes[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            ConfigManager.LogError("Unknown Class: " + name);
            return new ClassDefinition(name, "Missing Entry", 4);
        }
        //public static void ExportAll()
        //{
        //    foreach (ClassDefinition i in classes.Values)
        //    {
        //        FileInfo file = SourceManager.getFileName(i.Name, i.Source, ConfigManager.Directory_Classes);
        //        using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, i);
        //    }
        //}
        public String ToHTML()
        {
            try
            {
                if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_Classes.FullName);
                using (MemoryStream mem = new MemoryStream())
                {
                    Serializer.Serialize(mem, this);
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
        public static IEnumerable<ClassDefinition> GetClasses(int level, IChoiceProvider provider)
        {
            if (level > 1)
            {
                return from c in classes.Values where provider.CanMulticlass(c, level) select c;
            }
            return from c in classes.Values where c.AvailableAtFirstLevel select c;
        }
        public override string ToString()
        {
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
        }
        public int CompareTo(ClassDefinition other)
        {
            return Name.CompareTo(other.Name);
        }
        public List<Feature> CollectFeatures(int level, bool secondClass, IChoiceProvider provider)
        {
            List<Feature> res=new List<Feature>();
            foreach (Feature f in Features)
            {
                f.Source = Source;
                res.AddRange(f.Collect(level, provider));
            }
            if (secondClass) foreach (Feature f in MulticlassingFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, provider));
                }
            else foreach (Feature f in FirstClassFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, provider));
                }
            return res;
        }

        
        public ClassDefinition Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                ClassDefinition r = (ClassDefinition)Serializer.Deserialize(mem);
                r.filename = filename;
                return r;
            }
        }

    }
}
