using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Xsl;

namespace Character_Builder_5
{
    public class Language : IComparable<Language>, IHTML
    {
        [XmlIgnore]
        string filename;
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(Language));
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        [XmlIgnore]
        static public Dictionary<String, Language> languages = new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static private Dictionary<String, Language> simple = new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase);
        public String Name { get; set; }
        public String Description { get; set; }
        public String Skript { get; set; }
        public String TypicalSpeakers { get; set; }
        public String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        public void register(string file)
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
            register(null);
        }
        public static Language Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperator))
            {
                if (languages.ContainsKey(name)) return languages[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && languages.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return languages[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            throw new Exception("Unknown Language: " + name);
        }
        public static void ExportAll()
        {
            foreach (Language s in languages.Values)
            {
                FileInfo file = SourceManager.getFileName(s.Name, s.Source, ConfigManager.Directory_Languages);
                using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, s);
            }
        }
        public static void ImportAll()
        {
            languages.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Languages, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                using (TextReader reader = new StreamReader(f.Key.FullName))
                {
                    Language s = (Language)serializer.Deserialize(reader);
                    s.Source = f.Value;
                    s.register(f.Key.FullName);
                }
            }
        }
        public String toHTML()
        {
            try
            {
                if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_Languages.FullName);
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
        public override string ToString()
        {
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
        }
        public int CompareTo(Language other)
        {
            return Name.CompareTo(other.Name);
        }

        public bool save(Boolean overwrite)
        {
            Name = Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.getFileName(Name, Source, ConfigManager.Directory_Languages);
            if (file.Exists && (filename == null || !filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, this);
            this.filename = file.FullName;
            return true;
        }
        public Language clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Language r = (Language)serializer.Deserialize(mem);
                r.filename = filename;
                return r;
            }
        }
    }
}
