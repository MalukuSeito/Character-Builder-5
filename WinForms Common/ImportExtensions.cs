using OGL;
using OGL.Base;
using OGL.Features;
using OGL.Items;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Character_Builder_Forms
{
    public static class ImportExtensions
    {
        
        public static AbilityScores LoadAbilityScores(String file)
        {
            using (TextReader reader = new StreamReader(file)) AbilityScores.Current = (AbilityScores)AbilityScores.Serializer.Deserialize(reader);
            AbilityScores.Current.Filename = file;
            return AbilityScores.Current;
        }
        public static void ImportBackgrounds()
        {
            Background.backgrounds.Clear();
            Background.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Backgrounds, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Background s = (Background)Background.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        foreach (Feature fea in s.Features) fea.Source = f.Value;
                        s.Register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportClasses(bool applyKeywords = false)
        {
            ClassDefinition.classes.Clear();
            ClassDefinition.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Classes, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        ClassDefinition s = (ClassDefinition)ClassDefinition.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.FullName, applyKeywords);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportConditions()
        {
            Condition.conditions.Clear();
            Condition.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Conditions, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Condition s = (Condition)Condition.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportStandaloneFeatures()
        {
            FeatureCollection.Collections.Clear();
            FeatureCollection.Container.Clear();
            FeatureCollection.Categories.Clear();
            FeatureCollection.Boons.Clear();
            FeatureCollection.Features.Clear();
            FeatureCollection.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Features);
            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(SourceManager.GetDirectory(f.Value, ConfigManager.Directory_Features).FullName);
                    Uri target = new Uri(f.Key.DirectoryName);
                    FeatureContainer cont = LoadFeatureContainer(f.Key.FullName);
                    List<Feature> feats = cont.Features;
                    string cat = FeatureCleanname(Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                    if (!FeatureCollection.Container.ContainsKey(cat)) FeatureCollection.Container.Add(cat, new List<FeatureContainer>());
                    cont.filename = f.Key.FullName;
                    cont.category = cat;
                    cont.Name = Path.GetFileNameWithoutExtension(f.Key.FullName);
                    cont.Source = f.Value;
                    FeatureCollection.Container[cat].Add(cont);
                    foreach (Feature feat in feats)
                    {
                        feat.Source = cont.Source;
                        foreach (Keyword kw in feat.Keywords) kw.check();
                        feat.Category = cat;
                        if (!FeatureCollection.Categories.ContainsKey(cat)) FeatureCollection.Categories.Add(cat, new List<Feature>());
                        Feature other = FeatureCollection.Categories[cat].Where(ff => string.Equals(ff.Name, feat.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (other != null)
                        {
                            other.ShowSource = true;
                            feat.ShowSource = true;
                        }
                        FeatureCollection.Categories[cat].Add(feat);
                        if (cat.Equals("Boons", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (FeatureCollection.simple.ContainsKey(feat.Name))
                            {
                                FeatureCollection.simple[feat.Name].ShowSource = true;
                                feat.ShowSource = true;
                            }
                            else FeatureCollection.simple.Add(feat.Name, feat);
                            if (FeatureCollection.Boons.ContainsKey(feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source)) ConfigManager.LogError("Duplicate Boon: " + feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source);
                            else FeatureCollection.Boons[feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source] = feat;
                        }
                    }
                    foreach (Feature feat in feats)
                    {
                        FeatureCollection.Features.Add(feat);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static string FeatureCleanname(string path)
        {
            string cat = path;
            if (!cat.StartsWith(ConfigManager.Directory_Features)) cat = Path.Combine(ConfigManager.Directory_Features, path);
            cat = cat.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            //if (!Collections.ContainsKey(cat)) Collections.Add(cat, new FeatureCollection());
            return cat;
        }
        public static FeatureContainer LoadFeatureContainer(String path)
        {
            using (TextReader reader = new StreamReader(path))
            {
                return ((FeatureContainer)FeatureContainer.Serializer.Deserialize(reader));
            }
        }
        public static IEnumerable<string> EnumerateCategories(string type)
        {
            HashSet<string> result = new HashSet<string>();
            foreach (var f in SourceManager.GetAllDirectories(type))
            {
                Uri source = new Uri(f.Key.FullName);
                ImportStandaloneFeatures();
                var cats = f.Key.EnumerateDirectories("*", SearchOption.AllDirectories);
                foreach (DirectoryInfo d in cats) result.Add(SourceManager.Cleanname(Uri.UnescapeDataString(source.MakeRelativeUri(new Uri(d.FullName)).ToString()), type));
            }
            return from s in result orderby s select s;
        }
        public static void ImportItems()
        {
            Item.items.Clear();
            Item.ItemLists.Clear();
            Item.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Items);

            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(SourceManager.GetDirectory(f.Value, ConfigManager.Directory_Items).FullName);
                    Uri target = new Uri(f.Key.DirectoryName);
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Item s = (Item)Item.Serializer.Deserialize(reader);
                        s.Category = Category.Make(source.MakeRelativeUri(target));
                        s.Source = f.Value;
                        s.Register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportLanguages()
        {
            Language.languages.Clear();
            Language.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Languages, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Language s = (Language)Language.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static Level LoadLevel(String file)
        {
            using (TextReader reader = new StreamReader(file)) Level.Current = (Level)Level.Serializer.Deserialize(reader);
            Level.Current.Sort();
            return Level.Current;
        }
        public static void ImportMagic()
        {
            MagicProperty.properties.Clear();
            MagicProperty.Categories.Clear();
            MagicProperty.Categories.Add("Magic", new MagicCategory("Magic"));
            MagicProperty.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Magic, SearchOption.AllDirectories);
            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(SourceManager.GetDirectory(f.Value, ConfigManager.Directory_Magic).FullName);
                    Uri target = new Uri(f.Key.DirectoryName);
                    string cat = MagicPropertyCleanname(Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                    if (!MagicProperty.Categories.ContainsKey(cat)) MagicProperty.Categories.Add(cat, new MagicCategory(cat));
                    String parent = System.IO.Path.GetDirectoryName(cat);
                    while (parent.IsSubPathOf(ConfigManager.Directory_Magic) && !MagicProperty.Categories.ContainsKey(parent))
                    {
                        MagicProperty.Categories.Add(parent, new MagicCategory(parent));
                        parent = System.IO.Path.GetDirectoryName(parent);
                    }
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        MagicProperty mp = ((MagicProperty)MagicProperty.Serializer.Deserialize(reader));
                        mp.Filename = f.Key.FullName;
                        mp.Source = f.Value;
                        foreach (Feature fea in mp.AttunementFeatures) fea.Source = f.Value;
                        foreach (Feature fea in mp.CarryFeatures) fea.Source = f.Value;
                        foreach (Feature fea in mp.OnUseFeatures) fea.Source = f.Value;
                        foreach (Feature fea in mp.EquipFeatures) fea.Source = f.Value;
                        mp.Category = cat;
                        MagicProperty.Categories[cat].Contents.Add(mp);
                        if (MagicProperty.properties.ContainsKey(mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source))
                        {
                            throw new Exception("Duplicate Magic Property: " + mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source);
                        }
                        if (MagicProperty.simple.ContainsKey(mp.Name))
                        {
                            MagicProperty.simple[mp.Name].ShowSource = true;
                            mp.ShowSource = true;
                        }
                        MagicProperty.properties.Add(mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source, mp);
                        MagicProperty.simple[mp.Name] = mp;
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }

                //Collections[].AddRange(feats);
            }
        }
        public static string MagicPropertyCleanname(string path)
        {
            string cat = path;
            if (!cat.StartsWith(ConfigManager.Directory_Magic)) cat = System.IO.Path.Combine(ConfigManager.Directory_Magic, path);
            cat = cat.Replace(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
            //if (!Collections.ContainsKey(cat)) Collections.Add(cat, new FeatureCollection());
            return cat;
        }

        public static string MagicPropertyPath(string path)
        {
            string cat = MagicPropertyCleanname(path);
            return cat.Remove(0, ConfigManager.Directory_Magic.Length + 1);
        }
        public static void ImportRaces()
        {
            Race.races.Clear();
            Race.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Races, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Race s = (Race)Race.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSkills()
        {
            Skill.skills.Clear();
            Skill.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Skills, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Skill s = (Skill)Skill.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSpells()
        {
            Spell.spells.Clear();
            Spell.SpellLists.Clear();
            Spell.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Spells, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Spell s = (Spell)Spell.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSubClasses(bool applyKeywords = false)
        {
            SubClass.subclasses.Clear();
            SubClass.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_SubClasses, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        SubClass s = (SubClass)SubClass.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.FullName, applyKeywords);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSubRaces()
        {
            SubRace.subraces.Clear();
            SubRace.simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_SubRaces, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        SubRace s = (SubRace)SubRace.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static ConfigManager LoadConfig(string path)
        {
            if (!File.Exists(Path.Combine(path, "Config.xml")))
            {
                ConfigManager cm = new ConfigManager()
                {
                    FeaturesForAll = new List<Feature>() {
                        new ACFeature("Normal AC Calculation", "Wearing Armor","if(Armor,if(Light,BaseAC+DexMod,if(Medium, BaseAC+Min(DexMod,2),BaseAC)),10+DexMod)+ShieldBonus",1,true)
                    },
                    PDF = new List<string>() { "DefaultPDF.xml", "AlternatePDF.xml" }
                };
                cm.Save(Path.Combine(path, "Config.xml"));
            }
            ConfigManager Loaded = ConfigManager.Loaded = LoadConfigManager(Path.Combine(path, "Config.xml"));
            if (ConfigManager.Loaded.Slots.Count == 0)
            {
                ConfigManager.Loaded.Slots = new List<string>() { EquipSlot.MainHand, EquipSlot.OffHand, EquipSlot.Armor };
            }
            ConfigManager.Directory_Items = MakeRelative(Loaded.Items_Directory);
            ConfigManager.Transform_Items = new FileInfo(Fullpath(path, Loaded.Items_Transform));
            ConfigManager.Directory_Skills = MakeRelative(Loaded.Skills_Directory);
            ConfigManager.Transform_Skills = new FileInfo(Fullpath(path, Loaded.Skills_Transform));
            ConfigManager.Directory_Languages = MakeRelative(Loaded.Languages_Directory);
            ConfigManager.Transform_Languages = new FileInfo(Fullpath(path, Loaded.Languages_Transform));
            ConfigManager.Directory_Features = MakeRelative(Loaded.Features_Directory);
            ConfigManager.Transform_Features = new FileInfo(Fullpath(path, Loaded.Features_Transform));
            ConfigManager.Directory_Backgrounds = MakeRelative(Loaded.Backgrounds_Directory);
            ConfigManager.Transform_Backgrounds = new FileInfo(Fullpath(path, Loaded.Backgrounds_Transform));
            ConfigManager.Directory_Classes = MakeRelative(Loaded.Classes_Directory);
            ConfigManager.Transform_Classes = new FileInfo(Fullpath(path, Loaded.Classes_Transform));
            ConfigManager.Directory_SubClasses = MakeRelative(Loaded.SubClasses_Directory);
            ConfigManager.Transform_SubClasses = new FileInfo(Fullpath(path, Loaded.SubClasses_Transform));
            ConfigManager.Directory_Races = MakeRelative(Loaded.Races_Directory);
            ConfigManager.Transform_Races = new FileInfo(Fullpath(path, Loaded.Races_Transform));
            ConfigManager.Directory_SubRaces = MakeRelative(Loaded.SubRaces_Directory);
            ConfigManager.Transform_SubRaces = new FileInfo(Fullpath(path, Loaded.SubRaces_Transform));
            ConfigManager.Directory_Spells = MakeRelative(Loaded.Spells_Directory);
            ConfigManager.Transform_Spells = new FileInfo(Fullpath(path, Loaded.Spells_Transform));
            ConfigManager.Directory_Magic = MakeRelative(Loaded.Magic_Directory);
            ConfigManager.Transform_Magic = new FileInfo(Fullpath(path, Loaded.Magic_Transform));
            ConfigManager.Directory_Conditions = MakeRelative(Loaded.Conditions_Directory);
            ConfigManager.Transform_Conditions = new FileInfo(Fullpath(path, Loaded.Conditions_Transform));
            ConfigManager.Transform_Possession = new FileInfo(Fullpath(path, Loaded.Possessions_Transform));
            ConfigManager.Transform_RemoveDescription = new FileInfo(Fullpath(path, Loaded.RemoveDescription_Transform));
            ConfigManager.Directory_Plugins = MakeRelative(Loaded.Plugins_Directory);
            ConfigManager.PDFExporters = new List<string>();
            foreach (string s in ConfigManager.Loaded.PDF) ConfigManager.PDFExporters.Add(Fullpath(path, s));
            //for (int i = 0; i < loaded.PDF.Count; i++)
            //{
            //    loaded.PDF[i] = Fullpath(path, loaded.PDF[i]);
            //}


            return ConfigManager.Loaded;

        }
        public static string Fullpath(string basepath, string path)
        {
            if (Path.IsPathRooted(path)) return path;
            return Path.GetFullPath(Path.Combine(basepath, path));
        }
        //public static PDF PDFExporter = PDF.Load("AlternatePDF.xml");


        private static string MakeRelative(string dir)
        {
            return Path.GetDirectoryName(dir);
        }

        public static ConfigManager LoadConfigManager(string file)
        {
            using (TextReader reader = new StreamReader(file))
            {
                ConfigManager cm = (ConfigManager)ConfigManager.Serializer.Deserialize(reader);
                return cm;
            }
        }
        public static void Save(this ConfigManager m, string file)
        {
            using (TextWriter writer = new StreamWriter(file)) ConfigManager.Serializer.Serialize(writer, m);
        }
    }
}
