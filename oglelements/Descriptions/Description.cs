using System;
using System.Linq;

namespace OGL.Descriptions
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
        public bool Test(OGLContext context)
        {
            if (Name != null && Name.ToLowerInvariant().Contains(context.Search)) return true;
            if (Text != null && Text.ToLowerInvariant().Contains(context.Search)) return true;
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
