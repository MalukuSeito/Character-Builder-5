﻿using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace OGL
{
    public class MagicProperty : IComparable<MagicProperty>, IXML, IOGLElement<MagicProperty>, IOGLElement
    {
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(MagicProperty));        
        [XmlIgnore]
        public string Category;
        [XmlIgnore]
        public string FileName { get; set; }
        public String Requirement { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Slot Slot { get; set; }
        public String Base { get; set; }
        public String Source { get; set; }
        public String PrependName { get; set; }
        public String PostName { get; set; }
        public Rarity Rarity { get; set; }
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
        public List<Feature> AttunementFeatures = new List<Feature>();
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
        public List<Feature> CarryFeatures = new List<Feature>();
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
        public List<Feature> EquipFeatures = new List<Feature>();
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
        public List<Feature> AttunedEquipFeatures = new List<Feature>();
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
        public List<Feature> OnUseFeatures = new List<Feature>();
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
        public List<Feature> AttunedOnUseFeatures = new List<Feature>();

        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        public byte[] ImageData { get; set; }
        public MagicProperty()
        {
            Source = "";
        }

        public MagicProperty(OGLContext context, string name, string desc)
        {
            Name = name;
            Description = desc;
            context.MagicSimple[Name] = this;
        }

        public int CompareTo(MagicProperty other)
        {
            return Name.CompareTo(other.Name);
        }        
        public bool Test(OGLContext context)
        {
            if (Name != null && Name.ToLowerInvariant().Contains(context.Search)) return true;
            if (Description != null && Description.ToLowerInvariant().Contains(context.Search)) return true;
            if (Requirement != null && Requirement.ToLowerInvariant().Contains(context.Search)) return true;
            return false;
        }
        public override string ToString()
        {
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
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
        public string GetName(string oldname)
        {
            if (oldname != null && oldname != "") return (PrependName != null && PrependName != "" ? PrependName + " " : "") + oldname + (PostName != null && PostName != "" ? " " + PostName : "");
            return Name;
        }
        public IEnumerable<Feature> Collect(int level, bool equipped, bool attuned, IChoiceProvider choiceProvider, OGLContext context, bool includeOnUseFeatures = false)
        {
            List<Feature> res = new List<Feature>();
            foreach (Feature f in CarryFeatures)
            {
                f.Source = Source;
                res.AddRange(f.Collect(level, choiceProvider, context));
            }
            if (equipped) foreach (Feature f in EquipFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, choiceProvider, context));
                }
            if (attuned) foreach (Feature f in AttunementFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, choiceProvider, context));
                }
            if (attuned && equipped) foreach (Feature f in AttunedEquipFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, choiceProvider, context));
                }
            if (includeOnUseFeatures)
            {
                foreach (Feature f in OnUseFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, choiceProvider, context));
                }
                if (attuned)
                    foreach (Feature f in AttunedOnUseFeatures)
                    {
                        f.Source = Source;
                        res.AddRange(f.Collect(level, choiceProvider, context));
                    }
            }
            return res;
        }
        public IEnumerable<Feature> CollectOnUse(int level, IChoiceProvider choiceProvider, bool attuned, OGLContext context)
        {
            List<Feature> res = new List<Feature>();
            foreach (Feature f in OnUseFeatures)
            {
                f.Source = Source;
                res.AddRange(f.Collect(level, choiceProvider, context));
            }
            if (attuned)
                foreach (Feature f in AttunedOnUseFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, choiceProvider, context));
                }
            return res;
        }

        public MagicProperty Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                MagicProperty r = (MagicProperty)Serializer.Deserialize(mem);
                r.FileName = FileName;
                r.Category = Category;
                r.Name = Name;
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
                || Culture.CompareInfo.IndexOf(Requirement ?? "", text, CompareOptions.IgnoreCase) >= 0
                || AttunedEquipFeatures.Exists(s => s.Matches(text, nameOnly))
                || AttunedOnUseFeatures.Exists(s => s.Matches(text, nameOnly))
                || AttunementFeatures.Exists(s => s.Matches(text, nameOnly))
                || CarryFeatures.Exists(s => s.Matches(text, nameOnly))
                || EquipFeatures.Exists(s => s.Matches(text, nameOnly))
                || OnUseFeatures.Exists(s => s.Matches(text, nameOnly));
        }

        [XmlIgnore]
        public string Desc { get => Requirement; }

        [XmlIgnore]
        public string Text { get
            {
                List<String> s = new List<string>();
                if (Description != null && Description != "") s.Add(Description);
                foreach (Feature f in AttunementFeatures) if (f.Name != "" && f.Name != null && !f.NoDisplay) s.Add(f.ShortDesc());
                foreach (Feature f in CarryFeatures) if (f.Name != "" && f.Name != null && !f.NoDisplay) s.Add(f.ShortDesc());
                foreach (Feature f in EquipFeatures) if (f.Name != "" && f.Name != null && !f.NoDisplay) s.Add(f.ShortDesc());
                foreach (Feature f in OnUseFeatures) if (f.Name != "" && f.Name != null && !f.NoDisplay) s.Add(f.ShortDesc());
                foreach (Feature f in AttunedEquipFeatures) if (f.Name != "" && f.Name != null && !f.NoDisplay) s.Add(f.ShortDesc());
                foreach (Feature f in AttunedOnUseFeatures) if (f.Name != "" && f.Name != null && !f.NoDisplay) s.Add(f.ShortDesc());
                return String.Join("\n", s);
            }
        }

        [XmlIgnore]
        public String DisplayRequirement
        {
            get
            {
                if (Requirement != null && Requirement != "")
                {
                    if (Rarity != Rarity.None)
                    {
                        if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(Requirement, DisplayRarity) >= 0) {
                            if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(Requirement, "requires attunement") >= 0) return Requirement;
                            else if (AttunedEquipFeatures.Count > 0 || AttunementFeatures.Count > 0 || AttunedOnUseFeatures.Count > 0) return Requirement + " (requires attunment)";
                            else return Requirement;
                        } else
                        {
                            if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(Requirement, "requires attunement") >= 0) return Requirement + ", " + DisplayRarity;
                            else if (AttunedEquipFeatures.Count > 0 || AttunementFeatures.Count > 0 || AttunedOnUseFeatures.Count > 0) return Requirement + ", " + DisplayRarity + " (requires attunment)";
                            else return Requirement + ", " + DisplayRarity;
                        }
                    } else
                    {
                        if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(Requirement, "requires attunement") >= 0) return Requirement;
                        else if (AttunedEquipFeatures.Count > 0 || AttunementFeatures.Count > 0 || AttunedOnUseFeatures.Count > 0) return Requirement + " (requires attunment)";
                        else return Requirement;
                    }
                } else
                {
                    if (Rarity != Rarity.None)
                    {
                        if (AttunedEquipFeatures.Count > 0 || AttunementFeatures.Count > 0 || AttunedOnUseFeatures.Count > 0) return "(requires attunment)";
                        else return "";
                    } else
                    {
                        if (AttunedEquipFeatures.Count > 0 || AttunementFeatures.Count > 0 || AttunedOnUseFeatures.Count > 0) return DisplayRarity + " (requires attunment)";
                        return DisplayRarity;
                    }
                }
            }
        }

        [XmlIgnore]
        public string DisplayRarity
        {
            get
            {
                if (Rarity == Rarity.None) return "";
                if (Rarity == Rarity.VeryRare) return "very rare";
                return Rarity.ToString().ToLowerInvariant();
            }
        }
    }
}
