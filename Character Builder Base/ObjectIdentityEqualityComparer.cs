using OGL.Features;
using System.Collections.Generic;

namespace Character_Builder
{
    public class ObjectIdentityEqualityComparer : IEqualityComparer<Feature>
    {
        public int GetHashCode(Feature o)
        {
            return o.GetHashCode();
        }

        public bool Equals(Feature o1, Feature o2)
        {
            return object.ReferenceEquals(o1, o2);
        }
    }
}
