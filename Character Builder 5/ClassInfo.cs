using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class ClassInfo
    {
        public int Level;
        public int Hp;
        public int ClassLevel;
        public ClassDefinition Class;
        public ClassInfo(ClassDefinition c, int level, int hp, int classlevel)
        {
            Class = c;
            Level = level;
            Hp = hp;
            ClassLevel = classlevel;
        }
        public override string ToString()
        {
            if (Class != null) return "Level " + Level + ": " + Class.Name + " " + ClassLevel + " ( " + Hp + " HP )";
            return "Level " + Level + ": Unassigned";
        }
    }
}
