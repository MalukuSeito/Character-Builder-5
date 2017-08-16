using System;
using System.IO;

namespace OGL.Common
{
    public interface IOGLElement<T>: IXML
    {
        bool ShowSource { get; set; }
        T Clone();
        string FileName { get; set; }
        void Write(Stream stream);
    }

    public interface IOGLElement : IXML
    {
        bool ShowSource { get; set; }
    }
}
