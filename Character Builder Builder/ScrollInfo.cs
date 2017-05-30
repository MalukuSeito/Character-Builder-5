using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_Builder
{
    struct ScrollInfo
    {
        public int index;
        public int top;

        public ScrollInfo(int selectedIndex, int topIndex) : this()
        {
            index = selectedIndex;
            top = topIndex;
        }
    }
}
