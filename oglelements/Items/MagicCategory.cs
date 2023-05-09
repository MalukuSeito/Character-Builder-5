using System;
using System.Collections.Generic;
using System.Linq;

namespace OGL.Items
{
    public class MagicCategory : IComparable<MagicCategory>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Indent { get; set; }
        public List<MagicProperty> Contents;
        public MagicCategory() { }
        public MagicCategory(string name, string display, int indent)
        {
            Name = name;
            DisplayName = display;
            Indent = indent;
            Contents = new List<MagicProperty>();
        }
        public override string ToString()
        {
            return new String(' ', Indent) + DisplayName;
        }
        public int CompareTo(MagicCategory other)
        {
            return Name.CompareTo(other.Name);
        }

        public IEnumerable<MagicProperty> SubSection(string search)
        {
            string st = search.ToLowerInvariant();
            return from mp in Contents where mp.Name.ToLowerInvariant().Contains(st) || mp.Description.ToLowerInvariant().Contains(st) select mp;
        }
    }
}
