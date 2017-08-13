using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL
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
            if (x != null && x.Contains(ConfigManager.SourceSeperatorString)) x = x.Split(ConfigManager.SourceSeperator)[0].TrimEnd(' ');
            if (y != null && y.Contains(ConfigManager.SourceSeperatorString)) y = y.Split(ConfigManager.SourceSeperator)[0].TrimEnd(' ');
            return s.Equals(x, y);
        }

        public bool Equals(string x, IXML y)
        {
            return Equals(x, y.Name);
        }

        public bool Equals(IXML x, IXML y)
        {
            return s.Equals(x.Name, y.Name);
        }

        public bool Equals(IXML x, string y)
        {
            return Equals(x.Name, y);
        }

        public override int GetHashCode(string obj)
        {
            if (obj != null && obj.Contains(ConfigManager.SourceSeperatorString)) obj = obj.Split(ConfigManager.SourceSeperator)[0].TrimEnd(' ');
            return s.GetHashCode(obj);
        }

        public static string NoSource(string x)
        {
            if (x != null && x.Contains(ConfigManager.SourceSeperatorString)) return x.Split(ConfigManager.SourceSeperator)[0].TrimEnd(' ');
            return x;
        }
    }
}
