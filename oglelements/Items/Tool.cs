using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class Tool: Item
    {
        public Tool()
            : base()
        {
        }
        public Tool(Item i): base (i.Name)
        {
        }
        public Tool(String name, String description, Price price, double weight, Keyword kw1 = null, Keyword kw2 = null, Keyword kw3 = null, Keyword kw4 = null, Keyword kw5 = null, Keyword kw6 = null, Keyword kw7 = null)
            : base(name, description, price, weight, 1, kw1, kw2, kw3, kw4, kw5, kw6, kw7)
        {
            
        }

    }
}
