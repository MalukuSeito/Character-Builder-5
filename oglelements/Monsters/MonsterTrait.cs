using OGL.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL.Monsters
{
    public class MonsterTrait : IMatchable
    {
        public MonsterTrait(string name, string text)
        {
            Name = name;
            Text = text;
        }
        public MonsterTrait ()
        {

        }
        public string Name { get; set; }
        public string Text { get; set; }
        public virtual bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Text ?? "", text, CompareOptions.IgnoreCase) >= 0;
        }

        public override string ToString()
        {
            if (Name != null) return Name;
            return "";
        }
    }
}
