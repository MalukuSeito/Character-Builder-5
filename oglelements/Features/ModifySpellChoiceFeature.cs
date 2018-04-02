using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OGL.Features
{
    public class ModifySpellChoiceFeature : Feature
    {
        public String AdditionalSpellChoices { get; set; } = "false";
        public List<string> AdditionalSpells;
        public string UniqueID { get; set; }
        [XmlArrayItem(Type = typeof(Keyword)),
        XmlArrayItem(Type = typeof(Save)),
        XmlArrayItem(Type = typeof(Material))]
        public List<Keyword> KeywordsToAdd;
        public ModifySpellChoiceFeature()
            : base()
        {
            Action = Base.ActionType.ForceHidden;
            UniqueID = "";
            AdditionalSpellChoices = "false";
            KeywordsToAdd = new List<Keyword>();
            AdditionalSpells = new List<string>();
        }
        public ModifySpellChoiceFeature(string name, string text, string uniqueID, string additionalSpellChoices, List<Keyword> kwToAdd = null, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            UniqueID = uniqueID;
            AdditionalSpellChoices = additionalSpellChoices;
            if (kwToAdd == null) KeywordsToAdd = new List<Keyword>();
            else KeywordsToAdd = kwToAdd;
            AdditionalSpells = new List<string>();
        }
        public override string Displayname()
        {
            return "Modify Spellchoice Feature";
        }
    }
}
