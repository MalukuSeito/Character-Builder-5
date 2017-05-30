using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Character_Builder_5
{
    public class BonusSpellPrepareFeature : Feature
    {
        public List<string> Spells { get; set; }
        public string SpellcastingID { get; set; }
        [XmlArrayItem(Type = typeof(Keyword)),
        XmlArrayItem(Type = typeof(Save)),
        XmlArrayItem(Type = typeof(Material))]
        public List<Keyword> KeywordsToAdd { get; set; }
        public BonusSpellPrepareFeature()
            : base()
        {
            SpellcastingID = "";
            Spells = new List<string>();
            KeywordsToAdd = new List<Keyword>();
        }
        public BonusSpellPrepareFeature(string name, string text, string spellcastingID, List<Spell> spells, List<Keyword> kwToAdd = null, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            SpellcastingID = spellcastingID;
            Spells = new List<string>(from s in spells select s.Name);
            if (kwToAdd == null) KeywordsToAdd = new List<Keyword>();
            else KeywordsToAdd = kwToAdd;
        }
        public override string Displayname()
        {
            return "Bonus Always Prepared Spell Feature";
        }
    }
}
