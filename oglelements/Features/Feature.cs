using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.IO;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class Feature : IComparable<Feature>, IHTML
    {
        public static bool DETAILED_TO_STRING = false;
        [XmlArrayItem(Type = typeof(Keyword))]
        public List<Keyword> Keywords;
        [XmlIgnore]
        private string name;

        public String Name { get { return name; } set { name = value == null ? null : value.Replace(ConfigManager.SourceSeperator, '-'); } }
        public String Text { get; set; }
        [XmlIgnore]
        public String Category { get; set; }
        public int Level { get; set; }
        public bool Hidden { get; set; }
        public String Prerequisite { get; set; }
        [XmlIgnore]
        public bool KWChanged = false;
        [XmlIgnore]
        public virtual String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        public Feature() 
        {
            Level = 1;
            Name = "";
            Text = "";
            Hidden = false;
            Prerequisite = "";
            Keywords = new List<Keyword>();
        }
        public Feature(string name, string text, int level=1, bool hidden=false)
        {
            Name = name;
            Text = text;
            Level = level;
            Hidden = hidden;
            Keywords = new List<Keyword>();
        }
        public Feature AssignKeywords(Keyword kw1, Keyword kw2 = null, Keyword kw3 = null, Keyword kw4 = null, Keyword kw5 = null, Keyword kw6 = null, Keyword kw7 = null, Keyword kw8 = null)
        {
            List<Keyword> kws = new List<Keyword>() { kw1, kw2, kw3, kw4, kw5, kw6, kw7, kw8 };
            kws.RemoveAll(k => k == null || Keywords.Exists(kk => k.Equals(kk)));
            Keywords.AddRange(kws);
            if (kws.Count > 0) KWChanged = true;
            return this;
        }
        public Feature AssignCategory(string cat)
        {
            Category = cat;
            if (!FeatureCollection.Categories.ContainsKey(cat)) FeatureCollection.Categories.Add(cat,new List<Feature>());
            FeatureCollection.Categories[cat].Add(this);
            FeatureCollection.Features.Add(this);
            return this;
        }
        public virtual List<Feature> Collect(int level, IChoiceProvider choiceProvider)
        {
            if (Level > level) return new List<Feature>();
            return new List<Feature>() { this };
        }
        public String toHTML()
        {
            return new FeatureContainer(this).toHTML();
        }
        public override string ToString()
        {
            string n = Name;
            if (ShowSource || ConfigManager.AlwaysShowSource) n = n + " " + ConfigManager.SourceSeperator + " " + Source;
            if (DETAILED_TO_STRING) return Name + (Level > 0 ? " (Level " + Level + "): " : ": ") + Displayname();
            return n;
        }
        public virtual string ShortDesc()
        {
            return Name + ": " + Text;
        }
        public static List<Feature> Load(String path)
        {
            return FeatureContainer.Load(path).Features;
        }
        public static List<Feature> LoadString(String text)
        {
            return FeatureContainer.LoadString(text).Features;
        }
        public void Save(String path)
        {
            new FeatureContainer(this).Save(path);
        }
        public string Save()
        {
            return new FeatureContainer(this).Save();
        }
        public int CompareTo (Feature other) {
            return Name.CompareTo(other.Name);
        }
        public bool Test()
        {
            if (Name.ToLowerInvariant().Contains(Item.Search)) return true;
            if (Text.ToLowerInvariant().Contains(Item.Search)) return true;
            if (Keywords.Exists(k => k.Name == Item.Search)) return true;
            return false;
        }
        public virtual string Displayname()
        {
            return "Basic Feature";
        }

        public Feature SetPrerequisite(string v)
        {
            this.Prerequisite = v;
            return this;
        }
    }
}
