using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
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
