using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class VisionFeature: Feature
    {
        public int Range { get; set; }
        public VisionFeature()
            : base()
        {
            Range = 0;
        }
        public VisionFeature(string name, string text, int range, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Range = range;
        }
        public override string Displayname()
        {
            return "Vision Feature";
        }
    }
    
}
