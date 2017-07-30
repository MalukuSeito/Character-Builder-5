using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL.Common
{
    public interface IXML
    {
        String Name { get; }
        String Source { get; }
        String ToXML();
        MemoryStream ToXMLStream();
    }
}
