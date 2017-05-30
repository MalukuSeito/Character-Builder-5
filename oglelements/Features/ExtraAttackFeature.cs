using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class ExtraAttackFeature: Feature
    {
        public int ExtraAttacks { get; set; }
        public ExtraAttackFeature():base()
        {
            ExtraAttacks = 1;
        }
        public ExtraAttackFeature(string name, string text, int extraAttacks, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            ExtraAttacks = extraAttacks;
        }

        public override string Displayname()
        {
            return "Extra Attack Feature";
        }
    }
}
