using OGL.Common;
using System;
using System.Globalization;
using System.Linq;

namespace OGL.Descriptions
{
    public class Description: IMatchable
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

        public virtual bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Text ?? "", text, CompareOptions.IgnoreCase) >= 0;
        }
    }
}
