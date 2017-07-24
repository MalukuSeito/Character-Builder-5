using OGL.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace OGL
{
    public class Condition : IComparable<Condition>, IXML, IOGLElement<Condition>, IOGLElement
    {
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Condition));
        [XmlIgnore]
        static public Dictionary<String, Condition> conditions = new Dictionary<string, Condition>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static public Dictionary<String, Condition> simple = new Dictionary<string, Condition>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        public string filename;
        public String Name { get; set; }
        public String Description { get; set; }
        public String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;

        public byte[] ImageData { get; set; }
        public void Register(string file)
        {
            filename = file;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (conditions.ContainsKey(full)) throw new Exception("Duplicate Condition: " + full);
            conditions.Add(full, this);
            if (simple.ContainsKey(Name))
            {
                simple[Name].ShowSource = true;
                ShowSource = true;
            }
            else simple.Add(Name, this);
        }
        public Condition() 
        {
            Source = ConfigManager.DefaultSource;
        }
        public Condition(String name)
        {
            Name = name;
            Description = "Custom Condition";
            Source = ConfigManager.DefaultSource;
        }
        public Condition(String name, String description)
        {
            Name = name;
            Description = description;
            Source = ConfigManager.DefaultSource;
            Register(null);
        }
        public static Condition Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (conditions.ContainsKey(name)) return conditions[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && conditions.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return conditions[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            return new Condition(name);
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
                r.filename = filename;
                return r;
            }
        }
    }
}
