using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class SpellModifyFeature: Feature
    {
        public string Spells { get; set; }
        public SpellModifyFeature():base()
        {
            Spells = "";
        }
        public SpellModifyFeature(string name, string text, string spells, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Spells = spells;
        }
        public override string Displayname()
        {
            return "Spell Modify Feature";
        }
    }
}
