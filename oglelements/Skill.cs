using OGL.Base;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace OGL
{
    public class Skill : IComparable<Skill>, IHTML, OGLElement<Skill>
    {
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(Skill));
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        [XmlIgnore]
        static public Dictionary<String, Skill> skills = new Dictionary<string, Skill>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static public Dictionary<String, Skill> simple = new Dictionary<string, Skill>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        string filename;
        public String Name { get; set; }
        public String Description { get; set; }
        public Ability Base { get; set; }
        public String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        public void register(String file)
        {
            filename = file;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (skills.ContainsKey(full)) throw new Exception("Duplicate Skill: " + full);
            skills.Add(full, this);
            if (simple.ContainsKey(Name))
            {
                simple[Name].ShowSource = true;
                ShowSource = true;
            }
            else simple.Add(Name, this);
        }
        public Skill() 
        {
            Base = Ability.None;
            Source = ConfigManager.DefaultSource;
        }
        public Skill(String name, String description, Ability basedOn)
        {
            Name = name;
            Description = description;
            Base = basedOn;
            Source = ConfigManager.DefaultSource;
            register(null);
        }
        public static Skill Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperator))
            {
                if (skills.ContainsKey(name)) return skills[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && skills.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return skills[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            ConfigManager.LogError("Unknown Skill: " + name);
            return new Skill(name, "Missing Entry", Ability.None);
        }
        public static void ExportAll()
        {
            foreach (Skill s in skills.Values)
            {
                FileInfo file = SourceManager.getFileName(s.Name, s.Source, ConfigManager.Directory_Skills);
                using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, s);
            }
        }
        public static void ImportAll()
        {
            skills.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Skills, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Skill s = (Skill)serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading "+ f.ToString(), e);
                }
            }
        }
        public String toHTML()
        {
            try
            {
                if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_Skills.FullName);
                using (MemoryStream mem = new MemoryStream())
                {
                    serializer.Serialize(mem, this);
                    ConfigManager.RemoveDescription(mem);
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
        public int CompareTo(Skill other)
        {
            if (Base.CompareTo(other.Base) != 0) return Base.CompareTo(other.Base);
            return Name.CompareTo(other.Name);
        }
        public bool save(Boolean overwrite)
        {
            Name = Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.getFileName(Name, Source, ConfigManager.Directory_Skills);
            if (file.Exists && (filename == null || !filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, this);
            this.filename = file.FullName;
            return true;
        }
        public Skill clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Skill r = (Skill)serializer.Deserialize(mem);
                r.filename = filename;
                return r;
            }
        }
    }
}
