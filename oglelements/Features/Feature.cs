﻿using OGL.Base;
using OGL.Common;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace OGL.Features
{
    [JsonDerivedType(typeof(AbilityScoreFeature), "AbilityScoreFeature")]
    [JsonDerivedType(typeof(BonusSpellKeywordChoiceFeature), "BonusSpellKeywordChoiceFeature")]
    [JsonDerivedType(typeof(ChoiceFeature), "ChoiceFeature")]
    [JsonDerivedType(typeof(CollectionChoiceFeature), "CollectionChoiceFeature")]
    [JsonDerivedType(typeof(Feature), "Feature")]
    [JsonDerivedType(typeof(FreeItemAndGoldFeature), "FreeItemAndGoldFeature")]
    [JsonDerivedType(typeof(ItemChoiceConditionFeature), "ItemChoiceConditionFeature")]
    [JsonDerivedType(typeof(ItemChoiceFeature), "ItemChoiceFeature")]
    [JsonDerivedType(typeof(HitPointsFeature), "HitPointsFeature")]
    [JsonDerivedType(typeof(LanguageProficiencyFeature), "LanguageProficiencyFeature")]
    [JsonDerivedType(typeof(LanguageChoiceFeature), "LanguageChoiceFeature")]
    [JsonDerivedType(typeof(MultiFeature), "MultiFeature")]
    [JsonDerivedType(typeof(OtherProficiencyFeature), "OtherProficiencyFeature")]
    [JsonDerivedType(typeof(ResistanceFeature), "ResistanceFeature")]
    [JsonDerivedType(typeof(FormsCompanionsFeature), "FormsCompanionsFeature")]
    [JsonDerivedType(typeof(FormsCompanionsBonusFeature), "FormsCompanionsBonusFeature")]
    [JsonDerivedType(typeof(SaveProficiencyFeature), "SaveProficiencyFeature")]
    [JsonDerivedType(typeof(SpeedFeature), "SpeedFeature")]
    [JsonDerivedType(typeof(SkillProficiencyChoiceFeature), "SkillProficiencyChoiceFeature")]
    [JsonDerivedType(typeof(SkillProficiencyFeature), "SkillProficiencyFeature")]
    [JsonDerivedType(typeof(SubRaceFeature), "SubRaceFeature")]
    [JsonDerivedType(typeof(SubClassFeature), "SubClassFeature")]
    [JsonDerivedType(typeof(ToolProficiencyFeature), "ToolProficiencyFeature")]
    [JsonDerivedType(typeof(ToolKWProficiencyFeature), "ToolKWProficiencyFeature")]
    [JsonDerivedType(typeof(ToolProficiencyChoiceConditionFeature), "ToolProficiencyChoiceConditionFeature")]
    [JsonDerivedType(typeof(BonusFeature), "BonusFeature")]
    [JsonDerivedType(typeof(SpellcastingFeature), "SpellcastingFeature")]
    [JsonDerivedType(typeof(IncreaseSpellChoiceAmountFeature), "IncreaseSpellChoiceAmountFeature")]
    [JsonDerivedType(typeof(ModifySpellChoiceFeature), "ModifySpellChoiceFeature")]
    [JsonDerivedType(typeof(SpellChoiceFeature), "SpellChoiceFeature")]
    [JsonDerivedType(typeof(SpellSlotsFeature), "SpellSlotsFeature")]
    [JsonDerivedType(typeof(BonusSpellPrepareFeature), "BonusSpellPrepareFeature")]
    [JsonDerivedType(typeof(BonusSpellFeature), "BonusSpellFeature")]
    [JsonDerivedType(typeof(ACFeature), "ACFeature")]
    [JsonDerivedType(typeof(AbilityScoreFeatFeature), "AbilityScoreFeatFeature")]
    [JsonDerivedType(typeof(ExtraAttackFeature), "ExtraAttackFeature")]
    [JsonDerivedType(typeof(ResourceFeature), "ResourceFeature")]
    [JsonDerivedType(typeof(SpellModifyFeature), "SpellModifyFeature")]
    [JsonDerivedType(typeof(VisionFeature), "VisionFeature")]
    public class Feature : IComparable<Feature>, IXML, IInfoText
    {
        [XmlIgnore]
        public string InfoTitle => Name;
        [XmlIgnore]
        public string InfoText => Text;
        public static bool DETAILED_TO_STRING = false;
        [XmlArrayItem(Type = typeof(Keyword))]
        public List<Keyword> Keywords { get; set; }
        [XmlIgnore]
        private string name;

        public String Name { get { return name; } set { name = value?.Replace(ConfigManager.SourceSeperator, '-'); } }
        public String Text { get; set; }
        [XmlIgnore]
        public String Category { get; set; }
        public int Level { get; set; }
        public bool Hidden { get; set; }
        public bool NoDisplay { get; set; }
        public ActionType Action { get; set; }
        public String Prerequisite { get; set; }
        [XmlIgnore]
        public bool KWChanged = false;
        [XmlIgnore]
        public virtual String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        public Feature() 
        {
            Level = 1;
            Name = "";
            Text = "";
            Hidden = false;
            Prerequisite = "";
            Keywords = new List<Keyword>();
        }
        public Feature(string name, string text, int level=1, bool hidden=false)
        {
            Name = name;
            Text = text;
            Level = level;
            Hidden = hidden;
            Keywords = new List<Keyword>();
        }
        public Feature AssignKeywords(Keyword kw1, Keyword kw2 = null, Keyword kw3 = null, Keyword kw4 = null, Keyword kw5 = null, Keyword kw6 = null, Keyword kw7 = null, Keyword kw8 = null)
        {
            List<Keyword> kws = new List<Keyword>() { kw1, kw2, kw3, kw4, kw5, kw6, kw7, kw8 };
            kws.RemoveAll(k => k == null || Keywords.Exists(kk => k.Equals(kk)));
            Keywords.AddRange(kws);
            if (kws.Count > 0) KWChanged = true;
            return this;
        }
        public Feature AssignCategory(OGLContext context, string cat)
        {
            Category = cat;
            if (!context.FeatureCategories.ContainsKey(cat)) context.FeatureCategories.Add(cat,new List<Feature>());
            context.FeatureCategories[cat].Add(this);
            context.Features.Add(this);
            return this;
        }
        public virtual List<Feature> Collect(int level, IChoiceProvider choiceProvider, OGLContext context)
        {
            if (Level > level) return new List<Feature>();
            return new List<Feature>() { this };
        }
        public String ToXML()
        {
            return new FeatureContainer(this).ToXML();
        }

        public MemoryStream ToXMLStream()
        {
            return new FeatureContainer(this).ToXMLStream();
        }
        public override string ToString()
        {
            string n = Name;
            if (ShowSource || ConfigManager.AlwaysShowSource) n = n + " " + ConfigManager.SourceSeperator + " " + Source;
            if (DETAILED_TO_STRING) return Name + (Level > 0 ? " (Level " + Level + "): " : ": ") + Displayname();
            return n;
        }
        public virtual string ShortDesc()
        {
            return Name + ": " + (Text?.Trim(new char[] { ' ', '\r', '\n', '\t' }) ?? "");
        }
        public static List<Feature> LoadString(String text)
        {
            return FeatureContainer.LoadString(text).Features;
        }
        
        public int CompareTo (Feature other) {
            return Name.CompareTo(other.Name);
        }
        public bool Test(OGLContext context)
        {
            if (Name.ToLowerInvariant().Contains(context.Search)) return true;
            if (Text.ToLowerInvariant().Contains(context.Search)) return true;
            if (Keywords.Exists(k => k.Name == context.Search)) return true;
            return false;
        }
        public virtual string Displayname()
        {
            return "Basic Feature";
        }
        public string Save()
        {
            return new FeatureContainer(this).Save();
        }
        public Feature SetPrerequisite(string v)
        {
            this.Prerequisite = v;
            return this;
        }

        public virtual bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Text ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Prerequisite ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Category ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Keywords.Exists(s => Culture.CompareInfo.IndexOf(s.Name ?? "", text, CompareOptions.IgnoreCase) >= 0);
           
        }

        public string ToInfo(bool desc = false)
        {
            if (!desc) return Text;
            return ShortDesc();
        }

        [XmlIgnore]
        public string Desc { get => Prerequisite; }
    }
}
