using Character_Builder;
using OGL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class AttackRow
    {
        public AttackInfo Attack { get; set; }
        public Possession Possession { get; set; }

        public override string ToString()
        {
            if (Attack == null) return (Possession?.ToString() ?? "");
            if (Attack.SaveDC != null && Attack.SaveDC != "") return (Possession?.ToString() ?? "") + ": DC " + Attack.SaveDC + ", " + Attack.Damage + " " + Attack.DamageType + " damage";
            return (Possession?.ToString() ?? "" + (Attack.AttackOptions.Count > 0 ? " (" + string.Join(", ", Attack.AttackOptions) + " )" : "")) + ": +" + Attack.AttackBonus + ", " + Attack.Damage + " " + Attack.DamageType + " damage";
        }
    }
}
