using OGL.Base;
using OGL.Common;
using OGL.Keywords;
using System;
using System.Globalization;

namespace OGL.Items
{
    public class Weapon: Tool, IOGLElement<Weapon>
    {
        public String Damage { get; set; }
        public String DamageType { get; set; }
        public Weapon()
            : base()
        {
           
        }
        public Weapon(OGLContext context, String name, String description, Price price, double weight, String damage, String damageType, Keyword kw1 = null, Keyword kw2 = null, Keyword kw3 = null, Keyword kw4 = null, Keyword kw5 = null, Keyword kw6 = null, Keyword kw7 = null)
            : base(context, name, description, price, weight, kw1, kw2, kw3, kw4, kw5, kw6, kw7)
        {
            Damage = damage;
            DamageType = damageType.ToLowerInvariant();
        }

        Weapon IOGLElement<Weapon>.Clone()
        {
            return base.Clone() as Weapon;
        }

        public override bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Source ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Description ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(DamageType ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Damage ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Category?.ToString() ?? "", text, CompareOptions.IgnoreCase) >= 0;
        }
    }
}
