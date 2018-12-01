using OGL;
using OGL.Base;
using OGL.Features;
using OGL.Items;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        public static void ImportZips(this OGLContext context, bool applyKeywords = false,  bool cleanup = true)
        {
            if (cleanup)
            {
                context.Backgrounds.Clear();
                context.BackgroundsSimple.Clear();
                context.Classes.Clear();
                context.ClassesSimple.Clear();
                context.Conditions.Clear();
                context.ConditionsSimple.Clear();
                context.FeatureCollections.Clear();
                context.FeatureContainers.Clear();
                context.FeatureCategories.Clear();
                context.Boons.Clear();
                context.Features.Clear();
                context.BoonsSimple.Clear();
                context.Items.Clear();
                context.ItemLists.Clear();
                context.ItemsSimple.Clear();
                context.Languages.Clear();
                context.LanguagesSimple.Clear();
                context.Magic.Clear();
                context.MagicCategories.Clear();
                context.MagicCategories.Add("Magic", new MagicCategory("Magic", "Magic", 0));
                context.MagicSimple.Clear();
                context.Monsters.Clear();
                context.MonstersSimple.Clear();
                context.Races.Clear();
                context.RacesSimple.Clear();
                context.Skills.Clear();
                context.SkillsSimple.Clear();
                context.Spells.Clear();
                context.SpellLists.Clear();
                context.SpellsSimple.Clear();
                context.SubClasses.Clear();
                context.SubClassesSimple.Clear();
                context.SubRaces.Clear();
                context.SubRacesSimple.Clear();
            }
            foreach (KeyValuePair<FileInfo, string> zip in SourceManager.GetAllZips(context).AsEnumerable())
            {
                ZipArchive archive = ZipFile.OpenRead(zip.Key.FullName);
                string f = zip.Value.ToLowerInvariant() + "/";
                string ff = zip.Value.ToLowerInvariant() + "\\";
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string name = entry.FullName.ToLowerInvariant();
                    if ((name.StartsWith(f) || name.StartsWith(ff)) && name.EndsWith(".xml"))
                    {
                        String path = Path.Combine(SourceManager.AppPath, name);
                        if (File.Exists(path)) continue;
                        using (Stream s = entry.Open()) OGLImport.Import(s, path, zip.Value, SourceManager.AppPath, context, applyKeywords);
                    }
                    else if (name.EndsWith(".xml"))
                    {
                        String path = Path.Combine(SourceManager.AppPath, zip.Value, name);
                        if (File.Exists(path)) continue;
                        using (Stream s = entry.Open()) OGLImport.Import(s, path, zip.Value, SourceManager.AppPath, context, applyKeywords);
                    }
                }
            }
        }

        public static void ImportBackgrounds(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Backgrounds.Clear();
                context.BackgroundsSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Backgrounds_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportBackground(reader, f.Value.FullName, f.Value.Source, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportClasses(this OGLContext context, bool withZips, bool applyKeywords = false)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Classes.Clear();
                context.ClassesSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Classes_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportClass(reader, f.Value.FullName, f.Value.Source, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportConditions(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Conditions.Clear();
                context.ConditionsSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Conditions_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportCondition(reader, f.Value.FullName, f.Value.Source, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportStandaloneFeatures(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.FeatureCollections.Clear();
                context.FeatureContainers.Clear();
                context.FeatureCategories.Clear();
                context.Boons.Clear();
                context.Features.Clear();
                context.BoonsSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Features_Directory, withZips);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportFeatureContainer(reader, f.Value.Source, f.Value.FullName, context, OGLImport.GetPath(f.Value.FullName, SourceManager.AppPath, f.Value.Source));
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
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
            string t = type.TrimEnd('/', '\\').ToLowerInvariant() + "/";
            string tt = type.TrimEnd('/', '\\').ToLowerInvariant() + "\\";
            foreach (var z in SourceManager.GetAllZips(context, type))
            {
                foreach (ZipArchiveEntry a in z.Key.Entries)
                {
                    string f = z.Value.ToLowerInvariant() + "/" + t;
                    string ff = z.Value.ToLowerInvariant() + "\\" + tt;
                    if (a.Length == 0)
                    {
                        string name = a.FullName.ToLowerInvariant();
                        if (a.FullName.StartsWith(t)) result.Add(a.FullName.Substring(t.Length).TrimEnd('/', '\\'));
                        else if (a.FullName.StartsWith(f)) result.Add(a.FullName.Substring(f.Length).TrimEnd('/', '\\'));
                        else if (a.FullName.StartsWith(tt)) result.Add(a.FullName.Substring(tt.Length).TrimEnd('/', '\\'));
                        else if (a.FullName.StartsWith(ff)) result.Add(a.FullName.Substring(ff.Length).TrimEnd('/', '\\'));
                    }
                }
            }
            return result.OrderBy(s => s).Distinct();
        }
        public static void ImportItems(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Items.Clear();
                context.ItemLists.Clear();
                context.ItemsSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Items_Directory, withZips);

            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportItem(reader, f.Value.Source, f.Value.FullName, context, OGLImport.GetPath(f.Value.FullName, SourceManager.AppPath, f.Value.Source));
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportLanguages(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Languages.Clear();
                context.LanguagesSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Languages_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportLanguage(reader, f.Value.Source, f.Value.FullName, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportMonsters(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Monsters.Clear();
                context.MonstersSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Monster_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportMonster(reader, f.Value.Source, f.Value.FullName, context);
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
        public static void ImportMagic(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Magic.Clear();
                context.MagicCategories.Clear();
                context.MagicCategories.Add("Magic", new MagicCategory("Magic", "Magic", 0));
                context.MagicSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Magic_Directory, withZips, SearchOption.AllDirectories);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportMagicItem(reader, f.Value.Source, f.Value.FullName, context, OGLImport.GetPath(f.Value.FullName, SourceManager.AppPath, f.Value.Source));
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportRaces(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Races.Clear();
                context.RacesSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Races_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportRace(reader, f.Value.Source, f.Value.FullName, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSkills(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Skills.Clear();
                context.SkillsSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Skills_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportSkill(reader, f.Value.Source, f.Value.FullName, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSpells(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.Spells.Clear();
                context.SpellLists.Clear();
                context.SpellsSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.Spells_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportSpell(reader, f.Value.Source, f.Value.FullName, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSubClasses(this OGLContext context, bool withZips, bool applyKeywords = false)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.SubClasses.Clear();
                context.SubClassesSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.SubClasses_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportSubClass(reader, f.Value.Source, f.Value.FullName, context, applyKeywords);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }
        public static void ImportSubRaces(this OGLContext context, bool withZips)
        {
            if (context == null || context.Config == null) return;
            if (withZips)
            {
                context.SubRaces.Clear();
                context.SubRacesSimple.Clear();
            }
            var files = SourceManager.EnumerateFiles(context, context.Config.SubRaces_Directory, withZips, SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = f.Value.GetReader()) OGLImport.ImportSubRace(reader, f.Value.Source, f.Value.FullName, context);
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
            HTMLExtensions.Transform_Monster = new FileInfo(Fullpath(path, context.Config.Monster_Transform));
            HTMLExtensions.Transform_Possession = new FileInfo(Fullpath(path, context.Config.Possessions_Transform));
            HTMLExtensions.Transform_RemoveDescription = new FileInfo(Fullpath(path, context.Config.RemoveDescription_Transform));
            context.Config.Plugins_Directory = MakeRelative(context.Config.Plugins_Directory);
            context.Config.Monster_Directory = MakeRelative(context.Config.Monster_Directory);
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
            if (dir.Contains(Path.DirectorySeparatorChar) || dir.Contains(Path.AltDirectorySeparatorChar))
            {
                return Path.GetDirectoryName(dir);
            }
            return dir;
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
                Category.Categories.Add(p.ToString(), new Category(p, path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }).ToList(), context));
            }
            String parent = Path.GetDirectoryName(p);
            if (parent.IsSubPathOf(context.Config.Items_Directory)) Make(context, parent);
            return Category.Categories[p];
        }
    }
}
