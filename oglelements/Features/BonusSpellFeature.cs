using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
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
            KeywordsToAdd = new List<Keyword>();
            SpellCastingAbility = Ability.None;
        }
        public BonusSpellFeature(string name, string text, Ability spellability, Spell spell, RechargeModifier spellmodifier=RechargeModifier.Unmodified, List<Keyword> kwToAdd=null, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
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
