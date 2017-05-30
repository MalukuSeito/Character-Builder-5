using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class OtherProficiencyFeature: Feature
    {
        public OtherProficiencyFeature() : base() { }
        public OtherProficiencyFeature(string name, string text, int level = 1, bool hidden = false) : base(name, text, level, hidden) { }
        public override string Displayname()
        {
            return "Other Proficiency Feature";
        }
    }
}
