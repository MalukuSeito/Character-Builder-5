using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Xml.Xsl;
using System.IO;

namespace Character_Builder_5
{
    public class DescriptionContainer: IHTML
    {
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(DescriptionContainer));
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
            Descriptions = new List<Description>();
            Descriptions.Add(d);
        }
        public DescriptionContainer(IEnumerable<Description> descs)
        {
            Descriptions = new List<Description>(descs);
        }
        public String toHTML()
        {
            try
            {
                if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_Description.FullName);
                using (MemoryStream mem = new MemoryStream())
                {
                    serializer.Serialize(mem, this);
                    mem.Seek(0, SeekOrigin.Begin);
                    XmlReader xr = XmlReader.Create(mem);
                    using (StringWriter textWriter = new StringWriter())
                    {
                        using (XmlWriter xw = XmlWriter.Create(textWriter))
                        {
                            transform.Transform(xr, xw);
                            return textWriter.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "<html><body><b>Error generating output:</b><br>" + ex.Message + "<br>" + ex.InnerException + "<br>" + ex.StackTrace + "</body></html>";
            }
        }
        public static FeatureContainer Load(String path)
        {
            using (TextReader reader = new StreamReader(path))
            {
                return ((FeatureContainer)serializer.Deserialize(reader));
            }
        }
        public void Save(String path)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, this);
            }

        }
        public string Save()
        {
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }
        public static DescriptionContainer LoadString(string text)
        {
            using (TextReader reader = new StringReader(text))
            {
                return ((DescriptionContainer)serializer.Deserialize(reader));
            }
        }
    }
}
