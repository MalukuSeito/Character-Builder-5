using OGL.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL
{
    public class SourceInvariantComparer : StringComparer
    {
        private readonly StringComparer s = StringComparer.OrdinalIgnoreCase;
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

    public class SourceAwareComparer: Comparer<IXML>, IEqualityComparer<IXML>
	{
        private readonly StringComparer comparer = StringComparer.OrdinalIgnoreCase;

		public override int Compare(IXML x, IXML y)
		{
			int i = comparer.Compare(x?.Name, y?.Name);
            if (i == 0) return comparer.Compare(x?.Source, y?.Source);
            return i;
		}

		public bool Equals(IXML x, IXML y)
		{
			return comparer.Equals(x?.Name, y?.Name) && comparer.Equals(x?.Source, y?.Source);
		}

		public int GetHashCode([DisallowNull] IXML obj)
		{
            return comparer.GetHashCode(obj.Name + ConfigManager.SourceSeperatorString + obj.Source);
		}
	}

	public class InvariantComparer : Comparer<IXML>, IEqualityComparer<IXML>
	{
		private readonly StringComparer comparer = StringComparer.OrdinalIgnoreCase;

		public override int Compare(IXML x, IXML y)
		{
			return comparer.Compare(x?.Name, y?.Name);
		}

		public bool Equals(IXML x, IXML y)
		{
            return comparer.Equals(x?.Name, y?.Name);
		}

		public int GetHashCode([DisallowNull] IXML obj)
		{
			return comparer.GetHashCode(obj.Name);
		}
	}
}
