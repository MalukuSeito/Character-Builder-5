using OGL.Common;
using OGL.Features;
using System.IO;
using OGL.Descriptions;

namespace CB_5e.ViewModels
{
    public class TableValue : IXML
    {

        public TableEntry Entry { get; set; }
        public string Name { get => Entry.ToString(); }
        public string Source => "";

        public string ToXML()
        {
            return new Feature(Name, Name).ToXML();
        }

        public MemoryStream ToXMLStream()
        {
            return new Feature(Name, Name).ToXMLStream();
        }
        public override string ToString()
        {
            return Entry.ToString();
        }
    }
}
