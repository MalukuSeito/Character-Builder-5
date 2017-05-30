using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_Builder_5
{
    public class SpellChoicePerLevel
    {
        public int Level { get; set; }
        public List<SpellChoice> Choices = new List<SpellChoice>();
        public SpellChoicePerLevel(int level, List<SpellChoice> choices)
        {
            this.Level = level;
            this.Choices = choices;
        }

        public SpellChoicePerLevel(int level)
        {
            this.Level = level;
            Choices = new List<SpellChoice>();
        }

        public SpellChoicePerLevel()
        {
            this.Level = 0;
            Choices = new List<SpellChoice>();
        }
    }
}
