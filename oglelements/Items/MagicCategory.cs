using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class MagicCategory : IComparable<MagicCategory>
    {
        public string Name;
        public List<MagicProperty> Contents;
        public MagicCategory(string name)
        {
            Name = name;
            Contents = new List<MagicProperty>();
        }
        public override string ToString()
        {
            string path = System.IO.Path.GetFileName(Name);
            if (path == null) path = Name;
            int count = 0;
            foreach (char c in Name)
                if (c == System.IO.Path.AltDirectorySeparatorChar) count++;
            return new String(' ', count) + path;
        }
        public int CompareTo(MagicCategory other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
