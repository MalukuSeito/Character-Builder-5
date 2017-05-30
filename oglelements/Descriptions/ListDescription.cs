using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class ListDescription: Description
    {
        public List<Names> Names;
        public ListDescription()
        {
            Names = new List<Names>();
        }
        public ListDescription(String name, String text)
            : base(name, text)
        {
            Names = new List<Names>();
        }
        public ListDescription(String name, String text, List<Names> names)
            : base(name, text)
        {
            Names = names;
        }
    }
}
