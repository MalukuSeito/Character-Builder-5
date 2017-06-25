using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Items;
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
    public class MagicProperty : IComparable<MagicProperty>, IHTML, OGLElement<MagicProperty>
    {
        [XmlIgnore]
        public static Dictionary<string, MagicCategory> Categories = new Dictionary<string, MagicCategory>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(MagicProperty));
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        [XmlIgnore]
        static public Dictionary<String, MagicProperty> properties = new Dictionary<string, MagicProperty>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        static public Dictionary<String, MagicProperty> simple = new Dictionary<string, MagicProperty>(StringComparer.OrdinalIgnoreCase);
        [XmlIgnore]
        public string Category;
        [XmlIgnore]
        private string filename;
        public String Requirement { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Slot Slot { get; set; }
        public String Base { get; set; }
        public String Source { get; set; }
        public String PrependName { get; set; }
        public String PostName { get; set; }
        public Rarity Rarity { get; set; }
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
        XmlArrayItem(Type = typeof(SubRaceFeature)), XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)), XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)), XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> AttunementFeatures = new List<Feature>();
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
        XmlArrayItem(Type = typeof(SubRaceFeature)), XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)), XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)), XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> CarryFeatures = new List<Feature>();
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
        XmlArrayItem(Type = typeof(SubRaceFeature)), XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)), XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)), XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> EquipFeatures = new List<Feature>();
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
        XmlArrayItem(Type = typeof(SubRaceFeature)), XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)), XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)), XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> AttunedEquipFeatures = new List<Feature>();
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
        XmlArrayItem(Type = typeof(SubRaceFeature)), XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)), XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)), XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> OnUseFeatures = new List<Feature>();
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
        XmlArrayItem(Type = typeof(SubRaceFeature)), XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)), XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)), XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> AttunedOnUseFeatures = new List<Feature>();

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
        public MagicProperty()
        {
            Source = ConfigManager.DefaultSource;
        }

        public MagicProperty(string name, string desc)
        {
            Name = name;
            Description = desc;
            simple[Name] = this;
        }

        public int CompareTo(MagicProperty other)
        {
            return Name.CompareTo(other.Name);
        }
        public static void ExportAll()
        {
            //foreach (KeyValuePair<string, FeatureCollection> fc in Collections)
            //{
            //    string path = Path.Combine(ConfigManager.Directory_Features.FullName, fc.Key);
            //    foreach (Feature i in fc.Value.Features)
            foreach (MagicCategory c in Categories.Values)
            {
                foreach (MagicProperty i in c.Contents)
                {
                    FileInfo file = SourceManager.getFileName(i.Name, i.Source, cleanname(i.Category));
                    file.Directory.Create();
                    using (TextWriter writer = new StreamWriter(file.FullName))
                    {
                        serializer.Serialize(writer, i);
                    }
                }
            }
            //}
        }
        public static void ImportAll()
        {
            properties.Clear();
            Categories.Clear();
            Categories.Add("Magic", new MagicCategory("Magic"));
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Magic, SearchOption.AllDirectories);
            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(SourceManager.getDirectory(f.Value, ConfigManager.Directory_Magic).FullName);
                    Uri target = new Uri(f.Key.DirectoryName);
                    string cat = cleanname(Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                    if (!Categories.ContainsKey(cat)) Categories.Add(cat, new MagicCategory(cat));
                    String parent = System.IO.Path.GetDirectoryName(cat);
                    while (parent.IsSubPathOf(ConfigManager.Directory_Magic) && !Categories.ContainsKey(parent))
                    {
                        Categories.Add(parent, new MagicCategory(parent));
                        parent = System.IO.Path.GetDirectoryName(parent);
                    }
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        MagicProperty mp = ((MagicProperty)serializer.Deserialize(reader));
                        mp.filename = f.Key.FullName;
                        mp.Source = f.Value;
                        foreach (Feature fea in mp.AttunementFeatures) fea.Source = f.Value;
                        foreach (Feature fea in mp.CarryFeatures) fea.Source = f.Value;
                        foreach (Feature fea in mp.OnUseFeatures) fea.Source = f.Value;
                        foreach (Feature fea in mp.EquipFeatures) fea.Source = f.Value;
                        mp.Category = cat;
                        Categories[cat].Contents.Add(mp);
                        if (properties.ContainsKey(mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source))
                        {
                            throw new Exception("Duplicate Magic Property: " + mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source);
                        }
                        if (simple.ContainsKey(mp.Name))
                        {
                            simple[mp.Name].ShowSource = true;
                            mp.ShowSource = true;
                        }
                        properties.Add(mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source, mp);
                        simple[mp.Name] = mp;
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }

                //Collections[].AddRange(feats);
            }
        }
        public static string cleanname(string path)
        {
            string cat = path;
            if (!cat.StartsWith(ConfigManager.Directory_Magic)) cat = Path.Combine(ConfigManager.Directory_Magic, path);
            cat = cat.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            //if (!Collections.ContainsKey(cat)) Collections.Add(cat, new FeatureCollection());
            return cat;
        }

        public static string path(string path)
        {
            string cat = cleanname(path);
            return cat.Remove(0, ConfigManager.Directory_Magic.Length + 1);
        }
        public static MagicProperty Get(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperator))
            {
                if (properties.ContainsKey(name)) return properties[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && properties.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return properties[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            ConfigManager.LogError("Unknown property: " + name);
            return new MagicProperty(name, "Missing Entry");
        }
        public bool Test()
        {
            if (Name != null && Name.ToLowerInvariant().Contains(Item.Search)) return true;
            if (Description != null && Description.ToLowerInvariant().Contains(Item.Search)) return true;
            if (Requirement != null && Requirement.ToLowerInvariant().Contains(Item.Search)) return true;
            return false;
        }
        public override string ToString()
        {
            if (ShowSource || ConfigManager.AlwaysShowSource) return Name + " " + ConfigManager.SourceSeperator + " " + Source;
            return Name;
        }
        public String toHTML()
        {
            try
            {
                if (transform.OutputSettings == null) transform.Load(ConfigManager.Transform_Magic.FullName);
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
        public static IEnumerable<MagicCategory> Section()
        {
            if (Item.Search == null) return from mc in Categories.Values orderby mc select mc;
            List<MagicCategory> res=new List<MagicCategory>();
            foreach (MagicCategory mc in Categories.Values) {
                MagicCategory copy = new MagicCategory(mc.Name);
                copy.Contents = new List<MagicProperty>(from mp in mc.Contents where mp.Test() orderby mp select mp);
                res.Add(copy);
            }
            return res;
        }
        public string getName(string oldname)
        {
            if (oldname != null && oldname != "") return (PrependName != null && PrependName != "" ? PrependName + " " : "") + oldname + (PostName != null && PostName != "" ? " " + PostName : "");
            return Name;
        }
        public IEnumerable<Feature> Collect(int level, bool equipped, bool attuned, IChoiceProvider choiceProvider)
        {
            List<Feature> res = new List<Feature>();
            foreach (Feature f in CarryFeatures)
            {
                f.Source = Source;
                res.AddRange(f.Collect(level, choiceProvider));
            }
            if (equipped) foreach (Feature f in EquipFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, choiceProvider));
                }
            if (attuned) foreach (Feature f in AttunementFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, choiceProvider));
                }
            if (attuned && equipped) foreach (Feature f in AttunedEquipFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, choiceProvider));
                }
            return res;
        }
        public IEnumerable<Feature> CollectOnUse(int level, IChoiceProvider choiceProvider, bool attuned)
        {
            List<Feature> res = new List<Feature>();
            foreach (Feature f in OnUseFeatures)
            {
                f.Source = Source;
                res.AddRange(f.Collect(level, choiceProvider));
            }
            if (attuned)
                foreach (Feature f in AttunedOnUseFeatures)
                {
                    f.Source = Source;
                    res.AddRange(f.Collect(level, choiceProvider));
                }
            return res;
        }

        public bool save(Boolean overwrite)
        {
            Name = Name.Replace(ConfigManager.SourceSeperator, '-');
            MagicProperty o = null;
            if (properties.ContainsKey(Name + " " + ConfigManager.SourceSeperator + " " + Source)) o = properties[Name + " " + ConfigManager.SourceSeperator + " " + Source];
            if (o != null && o.Category != Category)
            {
                throw new Exception("Magic Property needs a unique name");
            }
            FileInfo file = SourceManager.getFileName(Name, Source, cleanname(Category));
            if (file.Exists && (filename == null || !filename.Equals(file.FullName)) && !overwrite) return false;
            using (TextWriter writer = new StreamWriter(file.FullName)) serializer.Serialize(writer, this);
            this.filename = file.FullName;
            return true;
        }
        public MagicProperty clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                MagicProperty r = (MagicProperty)serializer.Deserialize(mem);
                r.filename = filename;
                r.Category = Category;
                r.Name = Name;
                return r;
            }
        }
    }
}
