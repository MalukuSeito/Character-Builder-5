using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class SubClassFeature: Feature
    {
        public String ParentClass { get; set; }
        public SubClassFeature() : base() 
        {
            ParentClass = "";
        }
        public SubClassFeature(string name, string text, String Class, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            ParentClass = Class;
        }

        public override string Displayname()
        {
            return "Subclass Feature";
        }
    }
}
