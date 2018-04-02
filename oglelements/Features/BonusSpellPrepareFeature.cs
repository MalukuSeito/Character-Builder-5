using OGL.Base;
using OGL.Keywords;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OGL.Features
{
    public class BonusSpellPrepareFeature : Feature
    {
        public List<string> Spells { get; set; }
        public string Condition { get; set; } = "false";
        public string SpellcastingID { get; set; }
        [XmlArrayItem(Type = typeof(Keyword)),
        XmlArrayItem(Type = typeof(Save)),
        XmlArrayItem(Type = typeof(Material))]
        public List<Keyword> KeywordsToAdd { get; set; }
        public PreparationMode AddTo { get; set; } = PreparationMode.LearnSpells;
        public BonusSpellPrepareFeature()
            : base()
        {
            Action = Base.ActionType.ForceHidden;
            SpellcastingID = "";
            Spells = new List<string>();
            KeywordsToAdd = new List<Keyword>();
        }
        public BonusSpellPrepareFeature(string name, string text, string spellcastingID, List<Spell> spells, List<Keyword> kwToAdd = null, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            SpellcastingID = spellcastingID;
            Spells = new List<string>(from s in spells select s.Name);
            if (kwToAdd == null) KeywordsToAdd = new List<Keyword>();
            else KeywordsToAdd = kwToAdd;
        }
        public override string Displayname()
        {
            return "Add Spells Feature";
        }
    }
}
