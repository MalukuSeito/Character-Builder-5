using OGL.Base;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace OGL
{
    public class Skill : IComparable<Skill>, IXML, IOGLElement<Skill>, IOGLElement
    {
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Skill));
        [XmlIgnore]
        public string FileName { get; set; }
        public bool ShouldSerializeFileName() => false;
        public String Name { get; set; }
        public String Description { get; set; }
        public Ability Base { get; set; }
        public String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        public void Register(OGLContext context, String file)
        {
            FileName = file;
            string full = Name + " " + ConfigManager.SourceSeperator + " " + Source;
            if (context.Skills.ContainsKey(full)) throw new Exception("Duplicate Skill: " + full);
            context.Skills.Add(full, this);
            if (context.SkillsSimple.ContainsKey(Name))
            {
                context.SkillsSimple[Name].ShowSource = true;
                ShowSource = true;
            }
            else context.SkillsSimple.Add(Name, this);
        }
        public Skill() 
        {
            Base = Ability.None;
        }
        public Skill(OGLContext context, String name, String description, Ability basedOn)
        {
            Name = name;
            Description = description;
            Base = basedOn;
            Source = context.Config.DefaultSource;
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
        public int CompareTo(Skill other)
        {
            if (Base.CompareTo(other.Base) != 0) return Base.CompareTo(other.Base);
            return Name.CompareTo(other.Name);
        }
        public Skill Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Skill r = (Skill)Serializer.Deserialize(mem);
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
