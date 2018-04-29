using OGL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL.Monsters
{
    public class MonsterSkillBonus
    {
        public String Skill { get; set; }
        public Ability Ability { get; set; }
        public int Bonus { get; set; }
        public String Text { get; set; }
        public override string ToString()
        {
            return Skill + (Ability != Ability.None ? " [" + Ability.ToString()+"]" : "") + " " + Bonus.ToString("+#;-#;+0") + (Text != null && Text != "" ? " (" + Text + ")" : "");
        }
    }
}
