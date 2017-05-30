using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Character_Builder_5
{
    public class ModifySpellChoiceFeature : Feature
    {
        public String AdditionalSpellChoices { get; set; }
        public string UniqueID { get; set; }
        [XmlArrayItem(Type = typeof(Keyword)),
        XmlArrayItem(Type = typeof(Save)),
        XmlArrayItem(Type = typeof(Material))]
        public List<Keyword> KeywordsToAdd;
        public ModifySpellChoiceFeature()
            : base()
        {
            UniqueID = "";
            AdditionalSpellChoices = "false";
            KeywordsToAdd = new List<Keyword>();
        }
        public ModifySpellChoiceFeature(string name, string text, string uniqueID, string additionalSpellChoices, List<Keyword> kwToAdd = null, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            UniqueID = uniqueID;
            AdditionalSpellChoices = additionalSpellChoices;
            if (kwToAdd == null) KeywordsToAdd = new List<Keyword>();
            else KeywordsToAdd = kwToAdd;
        }
        public override string Displayname()
        {
            return "Modify Spellchoice Feature";
        }
    }
}
