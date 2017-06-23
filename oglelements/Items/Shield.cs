using OGL.Base;
using OGL.Keywords;
using System;

namespace OGL.Items
{
    public class Shield : Tool
    {
        public int ACBonus { get; set; }
        public Shield()
            : base()
        {
        }
        public Shield(Tool t): base (t)
        {
            ACBonus=2;
        }
        public Shield(String name, String description, Price price, int acBonus, double weight, Keyword kw1 = null, Keyword kw2 = null, Keyword kw3 = null, Keyword kw4 = null, Keyword kw5 = null, Keyword kw6 = null, Keyword kw7 = null)
            : base(name, description, price, weight, kw1, kw2, kw3, kw4, kw5, kw6, kw7)
        {
            ACBonus = acBonus;
        }
    }
}
