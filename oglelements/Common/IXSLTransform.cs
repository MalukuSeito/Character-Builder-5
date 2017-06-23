using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OGL.Common
{
    public interface IXSLTransform
    {
        void Transform(string file, XmlReader reader, XmlWriter writer);
    }
}
