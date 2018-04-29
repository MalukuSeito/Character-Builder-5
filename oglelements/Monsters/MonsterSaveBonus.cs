using OGL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL.Monsters
{
    public class MonsterSaveBonus
    {
        public Ability Ability { get; set; }
        public int Bonus { get; set; }
        public String Text { get; set; }
        public override string ToString()
        {
            return Ability.ToString() + " " + Bonus.ToString("+#;-#;+0") + (Text != null && Text != "" ? " (" + Text + ")" : "");
        }
    }
}
