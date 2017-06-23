using OGL.Base;
using OGL.Keywords;
using System;

namespace OGL.Items
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
