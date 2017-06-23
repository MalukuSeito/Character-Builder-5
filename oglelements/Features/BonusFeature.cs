using OGL.Base;
using OGL.Keywords;
using System;
using System.Collections.Generic;

namespace OGL.Features
{
    public class BonusFeature: Feature
    {
        /*[XmlArrayItem(Type = typeof(Keyword)),
        XmlArrayItem(Type = typeof(Versatile)),
        XmlArrayItem(Type = typeof(Range))]*/
        //public List<Keyword> Keywords;
        public String Condition { get; set; }
        public List<String> Skills { get; set; }
        public String SkillBonus { get; set; }
        public bool SkillPassive { get; set; }
        public String AttackBonus { get; set; }
        public String DamageBonus { get; set; }
        public String SaveDCBonus { get; set; }
        public String ACBonus { get; set; }
        public String InitiativeBonus { get; set; }
        public string DamageBonusText { get; set; }
        public Ability DamageBonusModifier { get; set; }
        public Ability BaseAbility { get; set; }
        public String SavingThrowBonus { get; set; }
        public String ProficiencyBonus { get; set; }
        public Ability SavingThrowAbility { get; set; }
        public String BaseItemChange { get; set; }
        public List<String> ProficiencyOptions { get; set; } = new List<string>();
        public BonusFeature():base()
        {
            Skills = new List<string>();
            SkillBonus = "0";
            AttackBonus = "0";
            DamageBonus = "0";
            SaveDCBonus = "0";
            SavingThrowBonus = "0";
            InitiativeBonus = "0";
            ProficiencyBonus = "0";
            Keywords = new List<Keyword>();
        }
        public BonusFeature(string name, string text, int skillbonus, List<String> skill, int attackBonus, int damageBonus, int saveDCBonus, int initiativeBonus, int acBonus, string expression, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Skills = skill;
            SkillBonus = skillbonus.ToString();
            AttackBonus = attackBonus.ToString();
            DamageBonus = damageBonus.ToString();
            SaveDCBonus = saveDCBonus.ToString();
            InitiativeBonus = initiativeBonus.ToString();
            ACBonus = acBonus.ToString();
            Condition = expression;
            //Keywords = new List<Keyword>() { kw1, kw2};
            //Keywords.RemoveAll(kw => kw == null);
        }
        public BonusFeature(string name, string text, int skillbonus, List<String> skill, int attackBonus, Ability damageBonusModifier, int saveDCBonus, int initiativeBonus, int acBonus, string expression, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Skills = skill;
            SkillBonus = skillbonus.ToString();
            AttackBonus = attackBonus.ToString();
            DamageBonus = "0";
            DamageBonusModifier = damageBonusModifier;
            SaveDCBonus = saveDCBonus.ToString();
            InitiativeBonus = initiativeBonus.ToString();
            ACBonus = acBonus.ToString();
            Condition = expression;
            //Keywords = new List<Keyword>() { kw1, kw2};
            //Keywords.RemoveAll(kw => kw == null);
        }

        public override string Displayname()
        {
            return "Stat Bonus Feature";
        }
    }
}

