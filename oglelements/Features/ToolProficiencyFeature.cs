using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class ToolProficiencyFeature: Feature
    {
        public List<String> Tools;
        public ToolProficiencyFeature()
            : base()
        {
            Tools = new List<string>();
        }
        public ToolProficiencyFeature(string name, string text, Tool tool, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Tools = new List<string>();
            Tools.Add(tool.Name);
        }
        public ToolProficiencyFeature(string name, string text, Tool tool, Tool tool2, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Tools = new List<string>();
            Tools.Add(tool.Name);
            Tools.Add(tool2.Name);
        }
        public ToolProficiencyFeature(string name, string text, Tool tool, Tool tool2, Tool tool3, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Tools = new List<string>();
            Tools.Add(tool.Name);
            Tools.Add(tool2.Name);
            Tools.Add(tool3.Name);
        }
        public ToolProficiencyFeature(string name, string text, Tool tool, Tool tool2, Tool tool3, Tool tool4, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Tools = new List<string>();
            Tools.Add(tool.Name);
            Tools.Add(tool2.Name);
            Tools.Add(tool3.Name);
            Tools.Add(tool4.Name);
        }
        public ToolProficiencyFeature(string name, string text, Tool tool, Tool tool2, Tool tool3, Tool tool4,Tool tool5, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Tools = new List<string>();
            Tools.Add(tool.Name);
            Tools.Add(tool2.Name);
            Tools.Add(tool3.Name);
            Tools.Add(tool4.Name);
            Tools.Add(tool5.Name);
        }
        public ToolProficiencyFeature(string name, string text, Tool tool, Tool tool2, Tool tool3, Tool tool4, Tool tool5, Tool tool6, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Tools = new List<string>();
            Tools.Add(tool.Name);
            Tools.Add(tool2.Name);
            Tools.Add(tool3.Name);
            Tools.Add(tool4.Name);
            Tools.Add(tool5.Name);
            Tools.Add(tool6.Name);
        }
        public override string Displayname()
        {
            return "Tool Proficiency Feature";
        }
    }
}
