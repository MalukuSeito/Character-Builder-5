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
        
        public static AbilityScores LoadAbilityScores(this OGLContext context, String file)
        {
            if (context == null || context.Config == null) return null;
            using (TextReader reader = new StreamReader(file)) context.Scores = (AbilityScores)AbilityScores.Serializer.Deserialize(reader);
            context.Scores.Filename = file;
            return context.Scores;
        }
        public static void ImportBackgrounds(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.Backgrounds.Clear();
            context.BackgroundsSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Backgrounds_Directory, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Background s = (Background)Background.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        foreach (Feature fea in s.Features) fea.Source = f.Value;
                        s.Register(context, f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportClasses(this OGLContext context, bool applyKeywords = false)
        {
            if (context == null || context.Config == null) return;
            context.Classes.Clear();
            context.ClassesSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Classes_Directory, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        ClassDefinition s = (ClassDefinition)ClassDefinition.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.FullName, applyKeywords);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportConditions(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.Conditions.Clear();
            context.ConditionsSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Conditions_Directory, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Condition s = (Condition)Condition.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportStandaloneFeatures(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.FeatureCollections.Clear();
            context.FeatureContainers.Clear();
            context.FeatureCategories.Clear();
            context.Boons.Clear();
            context.Features.Clear();
            context.BoonsSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Features_Directory);
            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(SourceManager.GetDirectory(f.Value, context.Config.Features_Directory).FullName);
                    Uri target = new Uri(f.Key.DirectoryName);
                    FeatureContainer cont = LoadFeatureContainer(f.Key.FullName);
                    List<Feature> feats = cont.Features;
                    string cat = FeatureCleanname(context, Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                    if (!context.FeatureContainers.ContainsKey(cat)) context.FeatureContainers.Add(cat, new List<FeatureContainer>());
                    cont.FileName = f.Key.FullName;
                    cont.category = cat;
                    cont.Name = Path.GetFileNameWithoutExtension(f.Key.FullName);
                    cont.Source = f.Value;
                    context.FeatureContainers[cat].Add(cont);
                    foreach (Feature feat in feats)
                    {
                        feat.Source = cont.Source;
                        foreach (Keyword kw in feat.Keywords) kw.check();
                        feat.Category = cat;
                        if (!context.FeatureCategories.ContainsKey(cat)) context.FeatureCategories.Add(cat, new List<Feature>());
                        Feature other = context.FeatureCategories[cat].Where(ff => string.Equals(ff.Name, feat.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (other != null)
                        {
                            other.ShowSource = true;
                            feat.ShowSource = true;
                        }
                        context.FeatureCategories[cat].Add(feat);
                        if (cat.Equals("Feats/Boons", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (context.BoonsSimple.ContainsKey(feat.Name))
                            {
                                context.BoonsSimple[feat.Name].ShowSource = true;
                                feat.ShowSource = true;
                            }
                            else context.BoonsSimple.Add(feat.Name, feat);
                            if (context.Boons.ContainsKey(feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source)) ConfigManager.LogError("Duplicate Boon: " + feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source);
                            else context.Boons[feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source] = feat;
                        }
                    }
                    foreach (Feature feat in feats)
                    {
                        context.Features.Add(feat);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static string FeatureCleanname(OGLContext context, string path)
        {
            string cat = path;
            if (!cat.StartsWith(context.Config.Features_Directory)) cat = Path.Combine(context.Config.Features_Directory, path);
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
        public static IEnumerable<string> EnumerateCategories(this OGLContext context, string type)
        {
            HashSet<string> result = new HashSet<string>();
            foreach (var f in SourceManager.GetAllDirectories(context, type))
            {
                Uri source = new Uri(f.Key.FullName);
                //context.ImportStandaloneFeatures();
                var cats = f.Key.EnumerateDirectories("*", SearchOption.AllDirectories);
                foreach (DirectoryInfo d in cats) result.Add(SourceManager.Cleanname(Uri.UnescapeDataString(source.MakeRelativeUri(new Uri(d.FullName)).ToString()), type));
            }
            return from s in result orderby s select s;
        }
        public static void ImportItems(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.Items.Clear();
            context.ItemLists.Clear();
            context.ItemsSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Items_Directory);

            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(SourceManager.GetDirectory(f.Value, context.Config.Items_Directory).FullName);
                    Uri target = new Uri(f.Key.DirectoryName);
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Item s = (Item)Item.Serializer.Deserialize(reader);
                        s.Category = Make(context, source.MakeRelativeUri(target));
                        s.Source = f.Value;
                        s.Register(context, f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportLanguages(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.Languages.Clear();
            context.LanguagesSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Languages_Directory, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Language s = (Language)Language.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static Level LoadLevel(this OGLContext context, String file)
        {
            if (context == null || context.Config == null) return null;
            using (TextReader reader = new StreamReader(file)) context.Levels = (Level)Level.Serializer.Deserialize(reader);
            context.Levels.Sort();
            return context.Levels;
        }
        private static MagicCategory MakeMagicCategory(string Name)
        {
            string path = Path.GetFileName(Name);
            if (path == null) path = Name;
            int count = 0;
            foreach (char c in Name)
                if (c == Path.AltDirectorySeparatorChar) count++;
            return new MagicCategory(Name, path, count);
        }
        public static void ImportMagic(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.Magic.Clear();
            context.MagicCategories.Clear();
            context.MagicCategories.Add("Magic", new MagicCategory("Magic", "Magic", 0));
            context.MagicSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Magic_Directory, SearchOption.AllDirectories);
            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(SourceManager.GetDirectory(f.Value, context.Config.Magic_Directory).FullName);
                    Uri target = new Uri(f.Key.DirectoryName);
                    string cat = MagicPropertyCleanname(context, Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                    if (!context.MagicCategories.ContainsKey(cat)) context.MagicCategories.Add(cat, MakeMagicCategory(cat));
                    String parent = Path.GetDirectoryName(cat);
                    while (parent.IsSubPathOf(context.Config.Magic_Directory) && !context.MagicCategories.ContainsKey(parent))
                    {
                        context.MagicCategories.Add(parent, MakeMagicCategory(parent));
                        parent = Path.GetDirectoryName(parent);
                    }
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        MagicProperty mp = ((MagicProperty)MagicProperty.Serializer.Deserialize(reader));
                        mp.FileName = f.Key.FullName;
                        mp.Source = f.Value;
                        foreach (Feature fea in mp.AttunementFeatures) fea.Source = f.Value;
                        foreach (Feature fea in mp.CarryFeatures) fea.Source = f.Value;
                        foreach (Feature fea in mp.OnUseFeatures) fea.Source = f.Value;
                        foreach (Feature fea in mp.EquipFeatures) fea.Source = f.Value;
                        mp.Category = cat;
                        context.MagicCategories[cat].Contents.Add(mp);
                        if (context.Magic.ContainsKey(mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source))
                        {
                            throw new Exception("Duplicate Magic Property: " + mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source);
                        }
                        if (context.MagicSimple.ContainsKey(mp.Name))
                        {
                            context.MagicSimple[mp.Name].ShowSource = true;
                            mp.ShowSource = true;
                        }
                        context.Magic.Add(mp.Name + " " + ConfigManager.SourceSeperator + " " + mp.Source, mp);
                        context.MagicSimple[mp.Name] = mp;
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }

                //Collections[].AddRange(feats);
            }
        }
        public static string MagicPropertyCleanname(this OGLContext context, string path)
        {
            string cat = path;
            if (!cat.StartsWith(context.Config.Magic_Directory)) cat = Path.Combine(context.Config.Magic_Directory, path);
            cat = cat.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            //if (!Collections.ContainsKey(cat)) Collections.Add(cat, new FeatureCollection());
            return cat;
        }

        public static string MagicPropertyPath(this OGLContext context, string path)
        {
            string cat = MagicPropertyCleanname(context, path);
            return cat.Remove(0, context.Config.Magic_Directory.Length + 1);
        }
        public static void ImportRaces(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.Races.Clear();
            context.RacesSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Races_Directory, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Race s = (Race)Race.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSkills(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.Skills.Clear();
            context.SkillsSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Skills_Directory, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Skill s = (Skill)Skill.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSpells(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.Spells.Clear();
            context.SpellLists.Clear();
            context.SpellsSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.Spells_Directory, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        Spell s = (Spell)Spell.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSubClasses(this OGLContext context, bool applyKeywords = false)
        {
            if (context == null || context.Config == null) return;
            context.SubClasses.Clear();
            context.SubClassesSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.SubClasses_Directory, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        SubClass s = (SubClass)SubClass.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.FullName, applyKeywords);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSubRaces(this OGLContext context)
        {
            if (context == null || context.Config == null) return;
            context.SubRaces.Clear();
            context.SubRacesSimple.Clear();
            var files = SourceManager.EnumerateFiles(context, context.Config.SubRaces_Directory, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (TextReader reader = new StreamReader(f.Key.FullName))
                    {
                        SubRace s = (SubRace)SubRace.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.FullName);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static ConfigManager LoadConfig(this OGLContext context, string path)
        {
            if (!File.Exists(Path.Combine(path, "Config.xml")))
            {
                context.Config = new ConfigManager()
                {
                    FeaturesForAll = new List<Feature>() {
                        new ACFeature("Normal AC Calculation", "Wearing Armor","if(Armor,if(Light,BaseAC+DexMod,if(Medium, BaseAC+Min(DexMod,2),BaseAC)),10+DexMod)+ShieldBonus",1,true)
                    },
                    PDF = new List<string>() { "DefaultPDF.xml", "AlternatePDF.xml" }
                };
                context.Config.Save(Path.Combine(path, "Config.xml"));
            }
            context.Config = LoadConfigManager(Path.Combine(path, "Config.xml"));
            if (context.Config.Slots.Count == 0)
            {
                context.Config.Slots = new List<string>() { EquipSlot.MainHand, EquipSlot.OffHand, EquipSlot.Armor };
            }
            context.Config.Items_Directory = MakeRelative(context.Config.Items_Directory);
            HTMLExtensions.Transform_Items = new FileInfo(Fullpath(path, context.Config.Items_Transform));
            context.Config.Skills_Directory = MakeRelative(context.Config.Skills_Directory);
            HTMLExtensions.Transform_Skills = new FileInfo(Fullpath(path, context.Config.Skills_Transform));
            context.Config.Languages_Directory = MakeRelative(context.Config.Languages_Directory);
            HTMLExtensions.Transform_Languages = new FileInfo(Fullpath(path, context.Config.Languages_Transform));
            context.Config.Features_Directory = MakeRelative(context.Config.Features_Directory);
            HTMLExtensions.Transform_Features = new FileInfo(Fullpath(path, context.Config.Features_Transform));
            context.Config.Backgrounds_Directory = MakeRelative(context.Config.Backgrounds_Directory);
            HTMLExtensions.Transform_Backgrounds = new FileInfo(Fullpath(path, context.Config.Backgrounds_Transform));
            context.Config.Classes_Directory = MakeRelative(context.Config.Classes_Directory);
            HTMLExtensions.Transform_Classes = new FileInfo(Fullpath(path, context.Config.Classes_Transform));
            context.Config.SubClasses_Directory = MakeRelative(context.Config.SubClasses_Directory);
            HTMLExtensions.Transform_SubClasses = new FileInfo(Fullpath(path, context.Config.SubClasses_Transform));
            context.Config.Races_Directory = MakeRelative(context.Config.Races_Directory);
            HTMLExtensions.Transform_Races = new FileInfo(Fullpath(path, context.Config.Races_Transform));
            context.Config.SubRaces_Directory = MakeRelative(context.Config.SubRaces_Directory);
            HTMLExtensions.Transform_SubRaces = new FileInfo(Fullpath(path, context.Config.SubRaces_Transform));
            context.Config.Spells_Directory = MakeRelative(context.Config.Spells_Directory);
            HTMLExtensions.Transform_Spells = new FileInfo(Fullpath(path, context.Config.Spells_Transform));
            context.Config.Magic_Directory = MakeRelative(context.Config.Magic_Directory);
            HTMLExtensions.Transform_Magic = new FileInfo(Fullpath(path, context.Config.Magic_Transform));
            context.Config.Conditions_Directory = MakeRelative(context.Config.Conditions_Directory);
            HTMLExtensions.Transform_Conditions = new FileInfo(Fullpath(path, context.Config.Conditions_Transform));
            HTMLExtensions.Transform_Possession = new FileInfo(Fullpath(path, context.Config.Possessions_Transform));
            HTMLExtensions.Transform_RemoveDescription = new FileInfo(Fullpath(path, context.Config.RemoveDescription_Transform));
            context.Config.Plugins_Directory = MakeRelative(context.Config.Plugins_Directory);
            context.Config.PDFExporters = new List<string>();
            foreach (string s in context.Config.PDF) context.Config.PDFExporters.Add(Fullpath(path, s));
            //for (int i = 0; i < loaded.PDF.Count; i++)
            //{
            //    loaded.PDF[i] = Fullpath(path, loaded.PDF[i]);
            //}


            return context.Config;

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
        public static Category Make(OGLContext context, Uri path)
        {
            return Make(context, Uri.UnescapeDataString(path.ToString()));
        }
        public static Category Make(OGLContext context, String path)
        {
            String p = path;
            if (!p.StartsWith(context.Config.Items_Directory)) p = Path.Combine(context.Config.Items_Directory, path);
            p = p.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (!Category.Categories.ContainsKey(p))
            {
                Category.Categories.Add(p.ToString(), new Category(p, path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }).ToList()));
            }
            String parent = Path.GetDirectoryName(p);
            if (parent.IsSubPathOf(context.Config.Items_Directory)) Make(context, parent);
            return Category.Categories[p];
        }

    }
}
