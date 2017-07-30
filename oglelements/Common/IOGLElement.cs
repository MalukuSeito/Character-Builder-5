using System;

namespace OGL.Common
{
    public interface IOGLElement<T>: IXML
    {
        bool ShowSource { get; set; }
        T Clone();
    }

    public interface IOGLElement : IXML
    {
        bool ShowSource { get; set; }
    }
}
