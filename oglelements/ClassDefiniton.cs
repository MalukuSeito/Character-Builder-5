using OGL.Base;
using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace OGL
{
    public class ClassDefinition : IComparable<ClassDefinition>, IXML, IOGLElement<ClassDefinition>, IOGLElement
    {
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(ClassDefinition));
        [XmlIgnore]
        public string filename;
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
        public bool ShowSource { get; set; } = false;
        
        public byte[] ImageData { get; set; }
        public void Register(OGLContext context, string filename, bool applyKeywords)
        {
            this.filename = filename;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (context.Classes.ContainsKey(full)) throw new Exception("Duplicate Class: " + full);
            context.Classes.Add(full, this);
            if (context.ClassesSimple.ContainsKey(Name))
            {
                context.ClassesSimple[Name].ShowSource = true;
                ShowSource = true;
            }
            else context.ClassesSimple.Add(Name, this);
            Keyword me = new Keyword(Name);
            if (applyKeywords)
            {
                if (FeaturesToAddClassKeywordTo != null && FeaturesToAddClassKeywordTo.Count > 0) foreach (Feature f in context.Features) if (FeaturesToAddClassKeywordTo.Contains(f.Name, ConfigManager.SourceInvariantComparer)) f.AssignKeywords(me);
                if (SpellsToAddClassKeywordTo != null && SpellsToAddClassKeywordTo.Count > 0) foreach (Spell s in context.Spells.Values) if (SpellsToAddClassKeywordTo.Contains(s.Name + " " + ConfigManager.SourceSeperator + " " + s.Source, ConfigManager.SourceInvariantComparer)) s.AssignKeywords(me);
            }
        }
        public ClassDefinition()
        {
            Descriptions = new List<Description>();
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
        public ClassDefinition(OGLContext context, String name, String description, int hitdie, List<string> featuresToAddClassKeywordTo = null, List<List<string>> spellsToAddClassKeywordTo = null)
        {
            Name = name;
            Description = description;
            Descriptions = new List<Description>();
            Source = context.Config.DefaultSource;
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
                    foreach (string s in ls) if (!context.SpellsSimple.ContainsKey(s))
                        {
                            Spell sp = new Spell(context, level, s, "", "", "", "Missing Entry");
                            if (level == 0) sp.AssignKeywords(new Keyword("Cantrip"));

                        }
                    level++;
                    SpellsToAddClassKeywordTo.AddRange(ls);
                }
            }
            Register(context, null, false);
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
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
        }
        public int CompareTo(ClassDefinition other)
        {
            return Name.CompareTo(other.Name);
        }
        public List<Feature> CollectFeatures(int level, bool secondClass, IChoiceProvider provider, OGLContext context)
        {
            List<Feature> res=new List<Feature>();
            foreach (Feature f in Features)
            {
                f.Source = Source;
                res.AddRange(f.Collect(level, provider, context));
            }
            if (secondClass) foreach (Feature f in MulticlassingFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, provider, context));
                }
            else foreach (Feature f in FirstClassFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, provider, context));
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
