using OGL;
using OGL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
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
            return plusMinus(Roll) + " " + Skill.ToString() + " (" + Enum.GetName(typeof(Ability), Base) + ")";
        }
        public int CompareTo(SkillInfo other)
        {
            return Skill.Name.CompareTo(other.Skill.Name);
        }
        public static string plusMinus(int x)
        {
            if (x < 0) return x.ToString();
            return "+" + x;
        }
    }
}
