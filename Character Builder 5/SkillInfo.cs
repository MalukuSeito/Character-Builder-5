using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class SkillInfo : IComparable<SkillInfo>
    {
        public Skill Skill { get; set; }
        public int Roll { get; set; }
        public Ability Base { get; set; }
        public SkillInfo(Skill skill, int roll, Ability baseability)
        {
            Skill = skill;
            Base = baseability;
            Roll = roll;
        }
        public override string ToString()
        {
            return Form1.plusMinus(Roll) + " " + Skill.ToString() + " (" + Enum.GetName(typeof(Ability), Base) + ")";
        }
        public int CompareTo(SkillInfo other)
        {
            return Skill.Name.CompareTo(other.Skill.Name);
        }
    }
}
