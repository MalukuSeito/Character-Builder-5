using System;

namespace OGL.Common
{
    public interface IOGLElement<T>: IXML
    {
        bool ShowSource { get; set; }
        String Name { get; set; }
        String Source { get; set; }
        T Clone();
    }

    public interface IOGLElement : IXML
    {
        bool ShowSource { get; set; }
        String Name { get; set; }
        String Source { get; set; }
    }
}
