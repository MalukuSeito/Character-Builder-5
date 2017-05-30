using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_Builder_5
{
    public class SpellPerLevel
    {
        public int Level { get; set; }
        public List<string> Spells = new List<string>();
        public SpellPerLevel()
        {
            this.Level = 0;
        }
        public SpellPerLevel(int level, List<string> spells)
        {
            this.Level = level;
            this.Spells = spells;
        }
    }
}
