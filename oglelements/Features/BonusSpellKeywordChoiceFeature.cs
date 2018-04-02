using OGL.Base;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OGL.Features
{
    public class BonusSpellKeywordChoiceFeature :Feature 
    {
       
        public string Condition { get; set; }
        public Ability SpellCastingAbility { get; set; }
        public String UniqueID { get; set; } //Also used as ResourceID with _Counter
        public RechargeModifier SpellCastModifier { get; set; }
        public int Amount { get; set; }
        [XmlArrayItem(Type = typeof(Keyword)),
        XmlArrayItem(Type = typeof(Save)),
        XmlArrayItem(Type = typeof(Material))]
        public List<Keyword> KeywordsToAdd { get; set; }
        public BonusSpellKeywordChoiceFeature()
            : base()
        {
            Action = Base.ActionType.DetectAction;
            KeywordsToAdd = new List<Keyword>();
            SpellCastingAbility = Ability.None;
            Amount = 1;
        }
        public BonusSpellKeywordChoiceFeature(string name, string text, string condition, String uniqueID, Ability spellability, RechargeModifier spellmodifier = RechargeModifier.Unmodified, List<Keyword> kwToAdd = null, int amount = 1, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.DetectAction;
            KeywordsToAdd = new List<Keyword>();
            Condition = condition;
            SpellCastingAbility = spellability;
            UniqueID = uniqueID;
            Amount = amount;
            SpellCastModifier = spellmodifier;
            if (kwToAdd == null) KeywordsToAdd = new List<Keyword>();
            else KeywordsToAdd = kwToAdd;
        }
        public override string Displayname()
        {
            return "Bonus Spell Choice Feature";
        }
    }
}
