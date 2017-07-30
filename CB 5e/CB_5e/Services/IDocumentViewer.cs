using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.Services
{
    public interface IDocumentViewer
    {
        void ShowDocumentFile(string name, Stream content, string mimeType);
    }
}
