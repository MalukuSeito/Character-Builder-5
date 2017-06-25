using OGL.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace OGL
{
    public class Condition : IComparable<Condition>, IHTML, OGLElement<Condition>
    {
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(Condition));
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        [XmlIgnore]
        static public Dictionary<String, Condition> conditions = new Dictionary<string, Condition>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static private Dictionary<String, Condition> simple = new Dictionary<string, Condition>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        string filename;
        public String Name { get; set; }
        public String Description { get; set; }
        public String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        [XmlIgnore]
        public Bitmap Image
        {
            set
            { // serialize
                if (value == null) ImageData = null;
                else using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    ImageData = ms.ToArray();
                }
            }
            get
            { // deserialize
                if (ImageData == null)
                {
                    return null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(ImageData))
                    {
                        return new Bitmap(ms);
                    }
                }
            }
        }

        public byte[] ImageData { get; set; }
        public void register(string file)
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
            register(null);
        }
        public static Condition Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperator))
            {
                if (conditions.ContainsKey(name)) return conditions[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && conditions.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return conditions[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            return new Condition(name);
        }
        public static void ExportAll()
        {
            foreach (Condition s in conditions.Values)
            {
                FileInfo file = SourceManager.getFileName(s.Name, s.Source, ConfigManager.Directory_Conditions);
                using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, s);
            }
        }
        public static void ImportAll()
        {
            conditions.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Conditions, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Condition s = (Condition)serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public String toHTML()
        {
            try
            {
                if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_Conditions.FullName);
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
        public int CompareTo(Condition other)
        {
            return Name.CompareTo(other.Name);
        }
        public bool save(Boolean overwrite)
        {
            Name = Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.getFileName(Name, Source, ConfigManager.Directory_Conditions);
            if (file.Exists && (filename == null || !filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, this);
            this.filename = file.FullName;
            return true;
        }
        public Condition clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Condition r = (Condition)serializer.Deserialize(mem);
                r.filename = filename;
                return r;
            }
        }
    }
}
