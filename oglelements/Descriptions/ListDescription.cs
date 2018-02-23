using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OGL.Descriptions
{
    public class ListDescription: Description
    {
        public List<Names> Names;
        public ListDescription()
        {
            Names = new List<Names>();
        }
        public ListDescription(String name, String text)
            : base(name, text)
        {
            Names = new List<Names>();
        }
        public ListDescription(String name, String text, List<Names> names)
            : base(name, text)
        {
            Names = names;
        }

        public override bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Text ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Names.Exists(s => s.Matches(text, nameOnly));
        }
    }
}
