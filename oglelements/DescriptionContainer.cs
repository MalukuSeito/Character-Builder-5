using OGL.Common;
using OGL.Descriptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace OGL
{
    public class DescriptionContainer: IXML
    {
        [XmlIgnore]
        private static XmlSerializer Serializer = new XmlSerializer(typeof(DescriptionContainer));
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        [XmlArrayItem(Type = typeof(Description)),
        XmlArrayItem(Type = typeof(TableDescription)),
        XmlArrayItem(Type = typeof(ListDescription))]
        public List<Description> Descriptions;
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
        public static FeatureContainer Load(String path)
        {
            using (TextReader reader = new StreamReader(path))
            {
                return ((FeatureContainer)Serializer.Deserialize(reader));
            }
        }
        public void Save(String path)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                Serializer.Serialize(writer, this);
            }

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
