using OGL.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace OGL
{
    public class Condition : IComparable<Condition>, IXML, IOGLElement<Condition>, IOGLElement
    {
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Condition));
        [XmlIgnore]
        public string FileName { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;

        public byte[] ImageData { get; set; }
        public void Register(OGLContext context, string file)
        {
            FileName = file;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (context.Conditions.ContainsKey(full)) throw new Exception("Duplicate Condition: " + full);
            context.Conditions.Add(full, this);
            if (context.ConditionsSimple.ContainsKey(Name))
            {
                context.ConditionsSimple[Name].ShowSource = true;
                ShowSource = true;
            }
            else context.ConditionsSimple.Add(Name, this);
        }
        public Condition() 
        {
        }
        public Condition(String name)
        {
            Name = name;
            Description = "Custom Condition";
            Source = "Custom";
        }
        public Condition(OGLContext context, String name, String description)
        {
            Name = name;
            Description = description;
            Source = "Custom Condition";
            Register(context, null);
        }
        public String ToXML()
        {
            using (StringWriter mem = new StringWriter())
            {
                Serializer.Serialize(mem, this);
                return mem.ToString();
            }
        }
        public void Write(Stream stream)
        {
            Serializer.Serialize(stream, this);
        }
        public MemoryStream ToXMLStream()
        {
            MemoryStream mem = new MemoryStream();
            Serializer.Serialize(mem, this);
            return mem;
        }
        public override string ToString()
        {
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
        }
        public int CompareTo(Condition other)
        {
            return Name.CompareTo(other.Name);
        }
        public Condition Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Condition r = (Condition)Serializer.Deserialize(mem);
                r.FileName = FileName;
                return r;
            }
        }
        public bool Matches(string text, bool nameOnly)
        {
            CultureInfo Culture = CultureInfo.InvariantCulture;
            if (nameOnly) return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Source ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(Description ?? "", text, CompareOptions.IgnoreCase) >= 0;
        }
    }
}
