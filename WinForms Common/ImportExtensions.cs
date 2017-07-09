using OGL;
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
                        s.register(f.Key.FullName, applyKeywords);
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
                        s.register(f.Key.FullName);
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
                        s.register(f.Key.FullName);
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
                        s.register(f.Key.FullName);
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
                        s.register(f.Key.FullName);
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
    }
}
