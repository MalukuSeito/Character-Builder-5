using OGL.Common;
using OGL.Descriptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OGL
{
    public class DescriptionContainer: IXML
    {
        [XmlIgnore]
        private static XmlSerializer Serializer = new XmlSerializer(typeof(DescriptionContainer));
        [XmlArrayItem(Type = typeof(Description)),
        XmlArrayItem(Type = typeof(TableDescription)),
        XmlArrayItem(Type = typeof(ListDescription))]
        public List<Description> Descriptions;

        public string Name { get => "Descriptions"; }

        public string Source { get => "No Source"; }

        public DescriptionContainer()
        {
            Descriptions = new List<Description>();
        }
        public DescriptionContainer(Description d)
        {
            Descriptions = new List<Description>
            {
                d
            };
        }
        public DescriptionContainer(IEnumerable<Description> descs)
        {
            Descriptions = new List<Description>(descs);
        }
        public String ToXML()
        {
            using (StringWriter mem = new StringWriter())
            {
                Serializer.Serialize(mem, this);
                return mem.ToString();
            }
        }

        public MemoryStream ToXMLStream()
        {
            MemoryStream mem = new MemoryStream();
            Serializer.Serialize(mem, this);
            return mem;
        }
        public string Save()
        {
            using (StringWriter writer = new StringWriter())
            {
                Serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }
        public static DescriptionContainer LoadString(string text)
        {
            using (TextReader reader = new StringReader(text))
            {
                return ((DescriptionContainer)Serializer.Deserialize(reader));
            }
        }
    }
}
