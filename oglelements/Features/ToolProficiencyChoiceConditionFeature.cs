using OGL.Common;
using OGL.Items;
using System;
using System.Collections.Generic;

namespace OGL.Features
{
    public class ToolProficiencyChoiceConditionFeature : Feature
    {
        public int Amount { get; set; }
        public String UniqueID { get; set; }
        public String Condition { get; set; }
        public ToolProficiencyChoiceConditionFeature()
            : base()
        {
            Action = Base.ActionType.ForceHidden;
            Amount = 1;
            Condition = "";
        }
        public ToolProficiencyChoiceConditionFeature(string name, string text, string uniqueID, String condition, int amount = 1, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            Amount = amount;
            UniqueID = uniqueID;
            Condition = condition;
        }
        public List<Tool> getTools(IChoiceProvider provider, OGLContext context)
        {
            List<Tool> res = new List<Tool>();
            int offset = provider.GetChoiceOffset(this, UniqueID, Amount);
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = provider.GetChoice(UniqueID + counter);
                if (cho != null && cho.Value != "") res.Add(context.GetItem(cho.Value, Source).AsTool());
            }
            return res;
        }
        public override string Displayname()
        {
            return "Tool Proficiency Choice Feature";
        }
    }
}
