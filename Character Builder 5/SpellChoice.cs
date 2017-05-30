using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
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
