using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Character_Builder_5
{
    public class ModifiedSpell : Spell
    {
        public List<Keyword> AdditionalKeywords;
        public Ability differentAbility;
        public RechargeModifier RechargeModifier;
        public bool AddAlwaysPreparedToName;
        public bool displayShort;
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
        public List<Feature> Modifikations;
        [XmlIgnore]
        public bool used;
        [XmlIgnore]
        public bool includeResources = true;
        [XmlIgnore]
        public bool includeRecharge = true;
        public AttackInfo Info;
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
            used = false;
            displayShort = false;
            Modifikations = new List<Feature>();
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
            used = false;
            displayShort = false;
            Modifikations = new List<Feature>();
            this.includeResources = includeResources;
            includeRecharge = includeResources;
        }
        public string recharge(RechargeModifier r, RechargeModifier defaultRecharge = RechargeModifier.LongRest)
        {
            if (!includeRecharge) return "";
            if (r == RechargeModifier.Unmodified) return recharge(defaultRecharge);
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
            return "Other";
        }
        public override string ToString() {
            if (displayShort) return Name + ((RechargeModifier == RechargeModifier.Unmodified && Level == 0) || RechargeModifier == RechargeModifier.AtWill ? "" : (includeResources && RechargeModifier != RechargeModifier.Charges ? (used ? ": 0/1 " : ": 1/1 ") : " ") + recharge(RechargeModifier));
            return Name + (AddAlwaysPreparedToName ? " (always prepared)" : "") + (differentAbility != Ability.None ? " (" + Enum.GetName(typeof(Ability), differentAbility) + ")" : "") + ((RechargeModifier == RechargeModifier.Unmodified && Level == 0) || RechargeModifier == RechargeModifier.AtWill ? "" : (includeResources && RechargeModifier != RechargeModifier.Charges ? (used ? ": 0/1 " : ": 1/1 ") : " ") + recharge(RechargeModifier));
        }
        public override List<Keyword> getKeywords()
        {
            List<Keyword> res = new List<Keyword>(Keywords);
            res.AddRange(AdditionalKeywords);
            return res;
        }
    }
}
