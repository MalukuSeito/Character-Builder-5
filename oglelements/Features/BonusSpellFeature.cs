using OGL.Base;
using OGL.Keywords;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OGL.Features
{
    public class BonusSpellFeature :Feature 
    {
        public string Spell {get; set;}
        public Ability SpellCastingAbility { get; set; }
        public RechargeModifier SpellCastModifier { get; set; }
        [XmlArrayItem(Type = typeof(Keyword)),
        XmlArrayItem(Type = typeof(Save)),
        XmlArrayItem(Type = typeof(Material))]
        public List<Keyword> KeywordsToAdd { get; set; }
        public BonusSpellFeature()
            : base()
        {
            Action = Base.ActionType.DetectAction;
            KeywordsToAdd = new List<Keyword>();
            SpellCastingAbility = Ability.None;
        }
        public BonusSpellFeature(string name, string text, Ability spellability, Spell spell, RechargeModifier spellmodifier=RechargeModifier.Unmodified, List<Keyword> kwToAdd=null, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.DetectAction;
            Spell = spell.Name;
            KeywordsToAdd = new List<Keyword>();
            SpellCastingAbility = spellability;
            SpellCastModifier = spellmodifier;
            if (kwToAdd == null) KeywordsToAdd = new List<Keyword>();
            else KeywordsToAdd = kwToAdd;
        }
        public override string Displayname()
        {
            return "Bonus Spell Feature";
        }
    }
}
