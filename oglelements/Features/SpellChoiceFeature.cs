using OGL.Base;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OGL.Features
{
    public class SpellChoiceFeature : Feature
    {
        public String AvailableSpellChoices { get; set; }
        public string SpellcastingID { get; set; }
        public string UniqueID { get; set; }
        public PreparationMode AddTo { get; set; }
        public int Amount { get; set; }
        [XmlArrayItem(Type = typeof(Keyword)),
        XmlArrayItem(Type = typeof(Save)),
        XmlArrayItem(Type = typeof(Material)),
        XmlArrayItem(Type = typeof(Royalty))]
        public List<Keyword> KeywordsToAdd { get; set; }
        public SpellChoiceFeature()
            : base()
        {
            Action = Base.ActionType.ForceHidden;
            SpellcastingID = "";
            UniqueID = "";
            AddTo = PreparationMode.LearnSpells;
            AvailableSpellChoices = "false";
            KeywordsToAdd = new List<Keyword>();
            Amount = 1;
        }
        public SpellChoiceFeature(string name, string text, string spellcastingID, string uniqueID, string availableSpellChoices, int amount = 1, PreparationMode addTo = PreparationMode.LearnSpells, List<Keyword> kwToAdd = null, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            SpellcastingID = spellcastingID;
            UniqueID = uniqueID;
            AddTo = addTo;
            AvailableSpellChoices = availableSpellChoices;
            if (kwToAdd == null) KeywordsToAdd = new List<Keyword>();
            else KeywordsToAdd = kwToAdd;
            Amount = amount;
        }
        public string info()
        {
            if (AddTo == PreparationMode.ClassList) return "to class spell list";
            if (AddTo == PreparationMode.Spellbook) return "to spellbook";
            return "learned";
        }
        public override string Displayname()
        {
            return "Spellchoice Feature";
        }
    }
}
