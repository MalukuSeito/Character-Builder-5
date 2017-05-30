using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class AttackInfo
    {
        public int AttackBonus { get; set; }
        public string Damage { get; set; }
        public string DamageType { get; set; }
        public string SaveDC { get; set; }
        public AttackInfo()
        {

        }
        public AttackInfo(int attack, string damage, string type)
        {
            AttackBonus = attack;
            Damage = damage;
            DamageType = type;
            SaveDC = "";
        }
        public AttackInfo(string saveDC, string damage, string type)
        {
            AttackBonus = 0;
            Damage = damage;
            DamageType = type;
            SaveDC = saveDC;
        }
    }
}
