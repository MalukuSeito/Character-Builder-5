using OGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class MonsterInfo: IEqualityComparer<Monster>
    {
        public Monster Monster { get; set; }
        public List<string> Sources { get; set; } = new List<string>();

        public bool Equals(Monster x, Monster y)
        {
            return x == y;
        }

        public int GetHashCode(Monster obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
