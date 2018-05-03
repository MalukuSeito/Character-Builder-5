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
        public string ToString(Monster monster, OGLContext context)
        {
            if (Skill != null) try
                {
                    Skill s = context.GetSkill(Skill, monster.Source);
                    if (s.Base != Ability.None)
                    {
                        return Skill + " " + (monster.getAbility(s.Base) / 2 - 5 + Bonus).ToString("+#;-#;+0") + (Text != null && Text != "" ? " (" + Text + ")" : "");
                    }

                }
                catch (Exception) { }
            if (Ability != Ability.None) return Skill + " " + (monster.getAbility(Ability) / 2 - 5 + Bonus).ToString("+#;-#;+0") + (Text != null && Text != "" ? " (" + Text + ")" : "");
            return ToString();
        }
    }
}
