using OGL.Base;
using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
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

    public class Race: IHTML, OGLElement<Race>
    {
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(Race));
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        public String Name { get; set; }
        public String Flavour { get; set; }
        public Base.Size Size { get; set; }
        public String Description { get; set; }
        [XmlArrayItem(Type = typeof(Description)),
        XmlArrayItem(Type = typeof(ListDescription)),
        XmlArrayItem(Type = typeof(TableDescription))]
        public List<Description> Descriptions { get; set; }
        [XmlIgnore]
        static public Dictionary<String, Race> races = new Dictionary<string, Race>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static public Dictionary<String, Race> simple = new Dictionary<string, Race>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        string filename;
        [XmlIgnore]
        public Bitmap Image {
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
            if (races.ContainsKey(full)) throw new Exception("Duplicate Race: " + full);
            races.Add(full, this);
            if (simple.ContainsKey(Name))
            {
                simple[Name].ShowSource = true;
                ShowSource = true;
            }
            else simple.Add(Name, this);

        }
        [XmlArrayItem(Type = typeof(AbilityScoreFeature)),
        XmlArrayItem(Type = typeof(BonusSpellKeywordChoiceFeature)),
        XmlArrayItem(Type = typeof(ChoiceFeature)),
        XmlArrayItem(Type = typeof(CollectionChoiceFeature)),
        XmlArrayItem(Type = typeof(Feature)),
        XmlArrayItem(Type = typeof(FreeItemAndGoldFeature)),
        XmlArrayItem(Type = typeof(ItemChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(ItemChoiceFeature)),
        XmlArrayItem(Type = typeof(HitPointsFeature)),
        XmlArrayItem(Type = typeof(LanguageProficiencyFeature)),
        XmlArrayItem(Type = typeof(LanguageChoiceFeature)),
        XmlArrayItem(Type = typeof(MultiFeature)),
        XmlArrayItem(Type = typeof(OtherProficiencyFeature)),
        XmlArrayItem(Type = typeof(SaveProficiencyFeature)),
        XmlArrayItem(Type = typeof(SpeedFeature)),
        XmlArrayItem(Type = typeof(SkillProficiencyChoiceFeature)),
        XmlArrayItem(Type = typeof(SkillProficiencyFeature)),
        XmlArrayItem(Type = typeof(SubRaceFeature)),XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)),XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)),XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
       
        //
        public List<Feature> Features;
        public String Source { get; set; }
        [XmlIgnore]
        public bool ShowSource { get; set; } = false;
        public static Race Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperator))
            {
                if (races.ContainsKey(name)) return races[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && races.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return races[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            ConfigManager.LogError("Unknown Race: " + name);
            Race r = new Race(name);
            r.Description = "Missing Entry";
            return r;
        }
        public Race()
        {
            Features = new List<Feature>();
            Descriptions = new List<Description>();
            Source = ConfigManager.DefaultSource;
        }
        public Race(String name)
        {
            Name=name;
            Features = new List<Feature>();
            Descriptions = new List<Description>();
            Source = ConfigManager.DefaultSource;
            register(null);
        }
        public static void ExportAll()
        {
            foreach (Race i in races.Values)
            {
                FileInfo file = SourceManager.getFileName(i.Name, i.Source, ConfigManager.Directory_Races);
                using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, i);
            }
        }



        public bool save(Boolean overwrite)
        {
            Name = Name.Replace(ConfigManager.SourceSeperator, '-');
            FileInfo file = SourceManager.getFileName(Name, Source, ConfigManager.Directory_Races);
            if (file.Exists && (filename == null || !filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, this);
            this.filename = file.FullName;
            return true;
        }
        public static void ImportAll()
        {
            races.Clear();
            simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Races, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Race s = (Race)serializer.Deserialize(reader);
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
        public virtual String toHTML()
        {
            try
            {
                if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_Races.FullName);
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
        public int CompareTo(Race other)
        {
            return Name.CompareTo(other.Name);
        }
        public List<Feature> CollectFeatures(int level, IChoiceProvider choiceProvider)
        {
            List<Feature> res = new List<Feature>();
            foreach (Feature f in Features)
            {
                f.Source = Source;
                res.AddRange(f.Collect(level, choiceProvider));
            }
            return res;
        }

        public Race clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Race r = (Race)serializer.Deserialize(mem);
                r.filename = filename;
                return r;
            }
        }
    }
}
