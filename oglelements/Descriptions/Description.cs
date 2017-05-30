using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class Description
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public Description() { }
        public Description(String name, String text)
        {
            Name = name;
            Text = text;
        }
        public bool Test()
        {
            if (Name != null && Name.ToLowerInvariant().Contains(Item.Search)) return true;
            if (Text != null && Text.ToLowerInvariant().Contains(Item.Search)) return true;
            return false;
        }
        public override string ToString()
        {
            if (Name != null) return Name;
            return "";
        }

        public string Save()
        {
            return new DescriptionContainer(this).Save();
        }
    }
}
