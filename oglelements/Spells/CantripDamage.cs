using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class CantripDamage
    {
        public int Level { get; set; }
        public string Damage { get; set; }
        public CantripDamage()
        {
            Level = 0;
            Damage = "";
        }
        public CantripDamage(int level, string damage)
        {
            Level = level;
            Damage = damage;
        }
    }
}
