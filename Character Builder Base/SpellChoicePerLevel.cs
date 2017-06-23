using System.Collections.Generic;

namespace Character_Builder
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
