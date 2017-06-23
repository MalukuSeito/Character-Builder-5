using System.Collections.Generic;

namespace Character_Builder
{
    public class SpellChoice
    {
        public string UniqueID;
        public List<string> Choices = new List<string>();
        public SpellChoice()
        {

        }
        public SpellChoice(SpellChoice s)
        {
            if (s != null)
            {
                this.UniqueID = s.UniqueID;
                this.Choices = new List<string>(s.Choices);
            }
        }
    }
}
