using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class ToolProficiencyChoiceConditionFeature : Feature
    {
        public int Amount { get; set; }
        public String UniqueID { get; set; }
        public String Condition { get; set; }
        public ToolProficiencyChoiceConditionFeature()
            : base()
        {
            Amount = 1;
            Condition = "";
        }
        public ToolProficiencyChoiceConditionFeature(string name, string text, string uniqueID, String condition, int amount = 1, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Amount = amount;
            UniqueID = uniqueID;
            Condition = condition;
        }
        public List<Tool> getTools(IChoiceProvider provider)
        {
            List<Tool> res = new List<Tool>();
            int offset = provider.getChoiceOffset(this, UniqueID, Amount);
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = provider.getChoice(UniqueID + counter);
                if (cho != null && cho.Value != "") res.Add(Item.Get(cho.Value, Source).asTool());
            }
            return res;
        }
        public override string Displayname()
        {
            return "Tool Proficiency Choice Feature";
        }
    }
}
