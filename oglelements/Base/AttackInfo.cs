namespace OGL.Base
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
