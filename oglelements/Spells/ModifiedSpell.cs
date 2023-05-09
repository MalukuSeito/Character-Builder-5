﻿using OGL.Base;
using OGL.Features;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OGL.Spells
{
    public class ModifiedSpell : Spell
    {
        public List<Keyword> AdditionalKeywords {  get; set; }
        public Ability differentAbility { get; set; }
        public RechargeModifier RechargeModifier { get; set; }
        public bool AddAlwaysPreparedToName { get; set; }
        public bool displayShort { get; set; }
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
        public List<Feature> Modifikations { get; set; }
        [XmlIgnore]
        public int used { get; set; }
        [XmlIgnore]
        public int count { get; set; }
		[XmlIgnore]
        public bool includeResources { get; set; } = true;
        [XmlIgnore]
        public bool includeRecharge { get; set; } = true;
        [XmlIgnore]
        public bool OnlyAsRitual { get; set; } = false;
        public AttackInfo Info { get; set; }
        public ModifiedSpell()
        {
            AdditionalKeywords = new List<Keyword>();
            Modifikations = new List<Feature>();
        }
        public ModifiedSpell(Spell s, IEnumerable<Keyword> kwToAdd, Ability ability = Ability.None, RechargeModifier recharge = RechargeModifier.Unmodified)
        {
            Name = s.Name;
            Keywords = s.Keywords;
            if (kwToAdd != null) AdditionalKeywords = new List<Keyword>(kwToAdd);
            else AdditionalKeywords = new List<Keyword>();
            Level = s.Level;
            CastingTime = s.CastingTime;
            Range = s.Range;
            Duration = s.Duration;
            Description = s.Description;
            Descriptions = s.Descriptions;
            differentAbility = ability;
            RechargeModifier = recharge;
            AdditionalKeywords.RemoveAll(k => Keywords.Contains(k));
            AddAlwaysPreparedToName = false;
            Source = s.Source;
            CantripDamage = s.CantripDamage;
            used = 0;
            FormsCompanionsFilter = s.FormsCompanionsFilter;
            FormsCompanionsCount = s.FormsCompanionsCount;
            displayShort = false;
            Modifikations = new List<Feature>();
            count = 1;
        }
        public ModifiedSpell(Spell s, IEnumerable<Keyword> kwToAdd, bool addAlwaysPreparedToName, bool includeResources = true)
        {
            Name = s.Name;
            Keywords = s.Keywords;
            if (kwToAdd != null) AdditionalKeywords = new List<Keyword>(kwToAdd);
            else AdditionalKeywords = new List<Keyword>();
            Level = s.Level;
            CastingTime = s.CastingTime;
            Range = s.Range;
            Duration = s.Duration;
            Description = s.Description;
            Descriptions = s.Descriptions;
            differentAbility = Ability.None;
            RechargeModifier = RechargeModifier.Unmodified;
            AdditionalKeywords.RemoveAll(k => Keywords.Contains(k));
            AddAlwaysPreparedToName = addAlwaysPreparedToName;
            Source = s.Source;
            CantripDamage = s.CantripDamage;
            used = 0;
            displayShort = false;
            Modifikations = new List<Feature>();
            this.includeResources = includeResources;
            includeRecharge = includeResources;
            FormsCompanionsFilter = s.FormsCompanionsFilter;
            FormsCompanionsCount = s.FormsCompanionsCount;
            count = 1;
        }
        public ModifiedSpell(Spell s, bool onlyAsRitual)
        {
            Name = s.Name;
            Keywords = s.Keywords;
            AdditionalKeywords = new List<Keyword>();
            Level = s.Level;
            CastingTime = s.CastingTime;
            Range = s.Range;
            Duration = s.Duration;
            Description = s.Description;
            Descriptions = s.Descriptions;
            differentAbility = Ability.None;
            RechargeModifier = RechargeModifier.Unmodified;
            AdditionalKeywords.RemoveAll(k => Keywords.Contains(k));
            AddAlwaysPreparedToName = false;
            Source = s.Source;
            CantripDamage = s.CantripDamage;
            used = 0;
            displayShort = false;
            Modifikations = new List<Feature>();
            this.includeResources = false;
            includeRecharge = includeResources;
            OnlyAsRitual = true;
            FormsCompanionsFilter = s.FormsCompanionsFilter;
            FormsCompanionsCount = s.FormsCompanionsCount;
            count = 1;
        }
        public string Recharge(RechargeModifier r, RechargeModifier defaultRecharge = RechargeModifier.LongRest)
        {
            if (!includeRecharge) return "";
            if (r == RechargeModifier.Unmodified) return Recharge(defaultRecharge);
            return "(" + RechargeName(r) + ")";
            //if (r.HasFlag(RechargeModifier.AtWill)) return "";
            //if (r == RechargeModifier.Ritual) return "Ritual";
            //if (r == RechargeModifier.ShortRest) if (displayShort) return "(Short Rest)";
            //    else return "Recharge: Short Rest";
            //if (displayShort) return "(Long Rest)";
            //return "Recharge: Long Rest";
        }
        public static string RechargeName(RechargeModifier Recharge)
        {
            if (Recharge == RechargeModifier.LongRest) return "Long Rest";
            if (Recharge == RechargeModifier.ShortRest) return "Any Rest";
            if (Recharge == RechargeModifier.Dawn) return "At Dawn";
            if (Recharge == RechargeModifier.Dusk) return "At Dusk";
            if (Recharge == RechargeModifier.Special) return "Special";
            if (Recharge == RechargeModifier.Charges) return "Used with Charges";
            if (Recharge == RechargeModifier.NoRecharge) return "No Recharge";
            if (Recharge == RechargeModifier.Ritual) return "Ritual";
			if (Recharge == RechargeModifier.AtWill) return "At-Will";
			return "Other";
        }
        public override string ToString() {
            if (displayShort) return Name + ((RechargeModifier == RechargeModifier.Unmodified && Level == 0) || RechargeModifier == RechargeModifier.AtWill ? "" : (includeResources && RechargeModifier != RechargeModifier.Charges ? (": " + (count - used) + "/" + count + " ") : " ") + Recharge(RechargeModifier)) + (OnlyAsRitual ? " (Ritual only)": "");
            return Name + (AddAlwaysPreparedToName ? " (always prepared)" : "") + (differentAbility != Ability.None ? " (" + String.Join(", ",differentAbility.GetIndividualFlags()) + ")" : "") + ((RechargeModifier == RechargeModifier.Unmodified && Level == 0) || RechargeModifier == RechargeModifier.AtWill ? "" : (includeResources && RechargeModifier != RechargeModifier.Charges ? (": " + (count - used) + "/" + count + " ") : " ") + Recharge(RechargeModifier)) + (OnlyAsRitual ? " (Ritual only)" : "");
        }
        public override List<Keyword> GetKeywords()
        {
            List<Keyword> res = new List<Keyword>(Keywords);
            res.AddRange(AdditionalKeywords);
            return res;
        }

        public string getResourceID()
        {
            return ConfigManager.SourceSeperator + "BONUS_SPELL " + Name + " " + ConfigManager.SourceSeperator + " " + Source + "|" + String.Join(",", differentAbility.GetIndividualFlags()) + RechargeModifier;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {


            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ModifiedSpell o = (ModifiedSpell)obj;
            return String.Equals(Name, o.Name, StringComparison.OrdinalIgnoreCase)
                && differentAbility == o.differentAbility
                && RechargeModifier == o.RechargeModifier
                && String.Equals(Source, o.Source, StringComparison.OrdinalIgnoreCase);

        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0)
                ^ differentAbility.GetHashCode()
                ^ RechargeModifier.GetHashCode()
                ^ (Source != null ? Source.GetHashCode() : 0);
        }

        public string Text
        {
            get => displayShort ? Name : Name + (AddAlwaysPreparedToName ? " (always prepared)" : "") + (differentAbility != Ability.None ? " (" + String.Join(", ", differentAbility.GetIndividualFlags()) + ")" : "");
        }
        public override string Desc
        {
            get
            {
                if (displayShort) return Name + ((RechargeModifier == RechargeModifier.Unmodified && Level == 0) || RechargeModifier == RechargeModifier.AtWill ? "" : (includeResources && RechargeModifier < RechargeModifier.Charges ? (": " + (count - used) + "/" + count + " ") : " ") + Recharge(RechargeModifier));
                if ((RechargeModifier == RechargeModifier.Unmodified && Level == 0) || RechargeModifier == RechargeModifier.AtWill) return "";
                return (includeResources && RechargeModifier < RechargeModifier.Charges ? ((count - used) + "/" + count + " ") : "") + Recharge(RechargeModifier);
            }
        }
    }
}
