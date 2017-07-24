using OGL.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace OGL
{
    public class Language : IComparable<Language>, IXML, IOGLElement<Language>, IOGLElement
    {
        [XmlIgnore]
        public string filename;
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Language));
        [XmlIgnore]
        static public Dictionary<String, Language> languages = new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static public Dictionary<String, Language> simple = new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase);
        public String Name { get; set; }
        public String Description { get; set; }
        public String Skript { get; set; }
        public String TypicalSpeakers { get; set; }
        public String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;

        public byte[] ImageData { get; set; }
        public void Register(string file)
        {
            filename = file;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (languages.ContainsKey(full)) throw new Exception("Duplicate Language: " + full);
            languages.Add(full, this);
            if (simple.ContainsKey(Name))
            {
                simple[Name].ShowSource = true;
                ShowSource = true;
            }
            else simple.Add(Name, this);
        }
        public Language() 
        {
            Skript = "";
            TypicalSpeakers = "";
            Source = ConfigManager.DefaultSource;
        }
        public Language(String name, String description, String skript, String speakers)
        {
            Name = name;
            Description = description;
            Skript = skript;
            TypicalSpeakers = speakers;
            Source = ConfigManager.DefaultSource;
            Register(null);
        }
        public static Language Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (languages.ContainsKey(name)) return languages[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && languages.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return languages[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            ConfigManager.LogError("Unknown Language: " + name);
            return new Language(name, "Missing Entry", "", "");
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
        public int CompareTo(Language other)
        {
            return Name.CompareTo(other.Name);
        }
        public Language Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Language r = (Language)Serializer.Deserialize(mem);
                r.filename = filename;
                return r;
            }
        }
    }
}
