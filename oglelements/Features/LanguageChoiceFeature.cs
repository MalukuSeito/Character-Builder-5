using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class LanguageChoiceFeature : Feature
    {
        public int Amount { get; set; }
        public String UniqueID { get; set; }
        public LanguageChoiceFeature()
            : base(null, null, 1, true)
        {
            Amount = 1;
        }
        public LanguageChoiceFeature(string name, string text, string uniqueID, int amount = 1, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Amount = amount;
            UniqueID = uniqueID;
        }
        
        public List<Language> getLanguages(IChoiceProvider provider)
        {
            List<Language> res = new List<Language>();
            int offset = provider.getChoiceOffset(this, UniqueID, Amount);
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = provider.getChoice(UniqueID + counter);
                if (cho != null && cho.Value != "") res.Add(Language.Get(cho.Value, Source));
            }
            return res;
        }
        public override string Displayname()
        {
            return "Language Choice Feature";
        }
    }
}
