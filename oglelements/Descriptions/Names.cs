using OGL.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OGL.Descriptions
{
    public class Names: IMatchable
    {
        public String Title { get; set; }
        public List<string> ListOfNames;
        public Names()
        {
            ListOfNames = new List<string>();
        }
        public Names(string title, List<string> names)
        {
            Title = title;
            ListOfNames = names;
        }
        public override string ToString()
        {
            return Title;
        }

        public bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return Culture.CompareInfo.IndexOf(Title ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Title ?? "", text, CompareOptions.IgnoreCase) >= 0
                || ListOfNames.Exists(s => Culture.CompareInfo.IndexOf(s ?? "", text, CompareOptions.IgnoreCase) >= 0);
        }
    }
}
