using System;
using System.Collections.Generic;

namespace OGL.Items
{
    public class MagicCategory : IComparable<MagicCategory>
    {
        public string Name;
        public string DisplayName;
        public int Indent;
        public List<MagicProperty> Contents;
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
    }
}
