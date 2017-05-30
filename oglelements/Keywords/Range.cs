using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class Range: Keyword
    {
        public int Short {get; set;}
        public int Long {get; set;}
        public Range(): base("range")
        {
            Short = 0;
            Long= 0;
        }
        public Range(int shortrange, int longrange)
            : base("range")
        {
            Short = shortrange;
            Long = longrange;
        }
        public override int CompareTo(Keyword other)
        {
            if (Name!=other.Name) return Name.CompareTo(other.Name);
            if (other is Range)
            {
                Range o = (Range)other;
                if (Short != o.Short) return Short.CompareTo(o.Short);
                return Long.CompareTo(o.Long);
            }
            return 1;
            
        }
        public override bool Equals(object other)
        {
            if (other is Range) return Name.Equals(((Range)other).Name) && Short == ((Range)other).Short && Long == ((Range)other).Long;
            return false;
        }
        public override int GetHashCode()
        {
            return (Name + "_" + Short + "_" + Long).GetHashCode();
        }
        public override string ToString()
        {
            return "Range (" + Short + "/" + Long + ")";
        }
    }
}
