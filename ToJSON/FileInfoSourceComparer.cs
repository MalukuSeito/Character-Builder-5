using System;
using System.Collections.Generic;

namespace Character_Builder_Forms
{
    public class FileInfoSourceComparer : IEqualityComparer<FileInfoSource>
    {
        public bool Equals(FileInfoSource x, FileInfoSource y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals(x.FullName, y.FullName);
        }

        public int GetHashCode(FileInfoSource obj)
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.FullName);
        }
    }
}