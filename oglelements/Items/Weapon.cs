using OGL.Base;
using OGL.Keywords;
using System;

namespace OGL.Items
{
    public class Weapon: Tool
    {
        public String Damage { get; set; }
        public String DamageType { get; set; }
        public Weapon()
            : base()
        {
           
        }
        public Weapon(String name, String description, Price price, double weight, String damage, String damageType, Keyword kw1 = null, Keyword kw2 = null, Keyword kw3 = null, Keyword kw4 = null, Keyword kw5 = null, Keyword kw6 = null, Keyword kw7 = null)
            : base(name, description, price, weight, kw1, kw2, kw3, kw4, kw5, kw6, kw7)
        {
            Damage = damage;
            DamageType = damageType.ToLowerInvariant();
        }
    }
}
