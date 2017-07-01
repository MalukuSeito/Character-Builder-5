using System;

namespace OGL.Common
{
    public interface IOGLElement<T>: IHTML
    {
        bool ShowSource { get; set; }
        String Name { get; set; }
        String Source { get; set; }
        T Clone();
    }
}
