using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class UsedResource
    {
        public string ResourceID;
        public int Used;
        public UsedResource()
        {
            ResourceID = "";
            Used = 0;
        }
        public UsedResource(string id, int value)
        {
            ResourceID = id;
            Used = value;
        }
    }
}
