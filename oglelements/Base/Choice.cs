using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class Choice
    {
        public String UniqueID { get; set; }
        public String Value { get; set; }
        public Choice() {
        }
        public Choice(String ID, String value)
        {
            UniqueID = ID;
            Value = value;
        }

    }
}
