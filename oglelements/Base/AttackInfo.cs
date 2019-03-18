using System.Collections.Generic;

namespace OGL.Base
{
    public class AttackInfo
    {
        public int AttackBonus { get; set; }
        public string Damage { get; set; }
        public string DamageType { get; set; }
        public string SaveDC { get; set; }
        public List<string> AttackOptions { get; set; } = new List<string>();
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
        public AttackInfo(int attack, string damage, string type, List<string> options)
        {
            AttackBonus = attack;
            Damage = damage;
            DamageType = type;
            SaveDC = "";
            AttackOptions = options;
        }
        public AttackInfo(string saveDC, string damage, string type)
        {
            AttackBonus = 0;
            Damage = damage;
            DamageType = type;
            SaveDC = saveDC;
        }
        public AttackInfo(string saveDC, string damage, string type, List<string> options)
        {
            AttackBonus = 0;
            Damage = damage;
            DamageType = type;
            SaveDC = saveDC;
            AttackOptions = options;
        }
        public override string ToString()
        {
            if (SaveDC != "") return "DC " + SaveDC + ": " + Damage + " " + DamageType;
            return "+" + AttackBonus + ": " + Damage + " " + DamageType;
        }
    }
}
