using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class SpellEqualityComparer : IEqualityComparer<Spell>
    {
        public bool Equals(Spell x, Spell y)
        {
            if (x == null) return y == null;
            if (y == null) return false;
            return String.Equals(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase)
                && String.Equals(x.Source, y.Source, StringComparison.InvariantCultureIgnoreCase)
                && int.Equals(x.Level, y.Level);
        }

        public int GetHashCode(Spell obj)
        {
            return obj != null
                ? (obj.Name != null ? obj.Name.ToLowerInvariant().GetHashCode() : 0)
                ^ (obj.Source != null ? obj.Source.ToLowerInvariant().GetHashCode() : 0)
                ^ obj.Level.GetHashCode()
                : 0;
        }
    }
}
