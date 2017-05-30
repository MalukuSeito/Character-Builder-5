using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class SourceInvariantComparer : StringComparer
    {
        StringComparer s = StringComparer.OrdinalIgnoreCase;
        public override int Compare(string x, string y)
        {
            return s.Compare(x, y);
        }

        public override bool Equals(string x, string y)
        {
            if (x != null && x.Contains(ConfigManager.SourceSeperator)) x = x.Split(ConfigManager.SourceSeperator)[0].TrimEnd(' ');
            if (y != null && y.Contains(ConfigManager.SourceSeperator)) y = y.Split(ConfigManager.SourceSeperator)[0].TrimEnd(' ');
            return s.Equals(x, y);
        }

        public override int GetHashCode(string obj)
        {
            if (obj != null && obj.Contains(ConfigManager.SourceSeperator)) obj = obj.Split(ConfigManager.SourceSeperator)[0].TrimEnd(' ');
            return s.GetHashCode(obj);
        }

        public static string NoSource(string x)
        {
            if (x != null && x.Contains(ConfigManager.SourceSeperator)) return x.Split(ConfigManager.SourceSeperator)[0].TrimEnd(' ');
            return x;
        }
    }
}
