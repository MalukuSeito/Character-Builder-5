using OGL;
using System;
using System.Collections.Generic;

namespace Character_Builder
{
    public class SpellEqualityComparer : IEqualityComparer<Spell>
    {
        public bool Equals(Spell x, Spell y)
        {
            if (x == null) return y == null;
            if (y == null) return false;
            return String.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
                && String.Equals(x.Source, y.Source, StringComparison.OrdinalIgnoreCase)
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
