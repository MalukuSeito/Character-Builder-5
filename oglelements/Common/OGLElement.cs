using System;

namespace OGL.Common
{
    public interface OGLElement<T>: IHTML
    {
        bool ShowSource { get; set; }
        String Name { get; set; }
        String Source { get; set; }
        bool save(Boolean overwrite);
        T clone();
    }
}
