using OGL;
using System;
using PCLStorage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OGL.Base;
using Character_Builder;
using Xamarin.Forms;
using OGL.Items;
using OGL.Features;
using OGL.Keywords;

namespace CB_5e.Services
{
    public class PCLImport
    {
        public static async Task<AbilityScores> LoadAbilityScoresAsync(IFile file)
        {
            using (Stream fs = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false)) AbilityScores.Current = (AbilityScores)AbilityScores.Serializer.Deserialize(fs);
            AbilityScores.Current.Filename = file.Path;
            return AbilityScores.Current;
        }

        public static async Task ImportClassesAsync(bool applyKeywords = false)
        {
            ClassDefinition.classes.Clear();
            ClassDefinition.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Classes).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) 
                    {
                        ClassDefinition s = (ClassDefinition)ClassDefinition.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.Path, applyKeywords);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportSubClassesAsync(bool applyKeywords = false)
        {
            SubClass.subclasses.Clear();
            SubClass.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_SubClasses).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        SubClass s = (SubClass)SubClass.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.Path, applyKeywords);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }
        public async static Task ImportSubRacesAsync()
        {
            SubRace.subraces.Clear();
            SubRace.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_SubRaces).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        SubRace s = (SubRace)SubRace.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }
        public async static Task ImportRacesAsync()
        {
            Race.races.Clear();
            Race.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Races).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Race s = (Race)Race.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportSkillsAsync()
        {
            Skill.skills.Clear();
            Skill.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Skills).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Skill s = (Skill)Skill.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportLanguagesAsync()
        {
            Language.languages.Clear();
            Language.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Languages).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Language s = (Language)Language.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportSpellsAsync()
        {
            Spell.spells.Clear();
            Spell.SpellLists.Clear();
            Spell.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Spells).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Spell s = (Spell)Spell.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportItemsAsync()
        {
            Item.items.Clear();
            Item.ItemLists.Clear();
            Item.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Items, true).ConfigureAwait(false);

            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(PCLSourceManager.GetDirectory(f.Value, ConfigManager.Directory_Items));
                    Uri target = new Uri(PCLSourceManager.Parent(f.Key));
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Item s = (Item)Item.Serializer.Deserialize(reader);
                        s.Category = Make(source.MakeRelativeUri(target));
                        s.Source = f.Value;
                        s.Register(f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }

        public static Category Make(Uri path)
        {
            return Make(Uri.UnescapeDataString(path.ToString()));
        }

        public static Category Make(String path)
        {
            String p = path;
            if (!p.StartsWith(ConfigManager.Directory_Items)) p = PortablePath.Combine(ConfigManager.Directory_Items, path);
            p = p.Replace(PortablePath.DirectorySeparatorChar, '/');
            if (!Category.Categories.ContainsKey(p))
            {
                Category.Categories.Add(p.ToString(), new Category(p, path.Split(new[] { PortablePath.DirectorySeparatorChar }).ToList()));
            }
            String parent = PCLSourceManager.Parent(p);
            if (parent.StartsWith(ConfigManager.Directory_Items) && !parent.Equals(ConfigManager.Directory_Items)) Make(parent);
            return Category.Categories[p];
        }


        public static async Task ImportBackgroundsAsync()
        {
            Background.backgrounds.Clear();
            Background.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Backgrounds).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Background s = (Background)Background.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        foreach (Feature fea in s.Features) fea.Source = f.Value;
                        s.Register(f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportStandaloneFeaturesAsync()
        {
            FeatureCollection.Collections.Clear();
            FeatureCollection.Container.Clear();
            FeatureCollection.Categories.Clear();
            FeatureCollection.Boons.Clear();
            FeatureCollection.Features.Clear();
            FeatureCollection.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Features, true).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(PCLSourceManager.GetDirectory(f.Value, ConfigManager.Directory_Features));
                    Uri target = new Uri(PCLSourceManager.Parent(f.Key));
                    FeatureContainer cont = await LoadFeatureContainerAsync(f.Key);
                    List<Feature> feats = cont.Features;
                    string cat = FeatureCleanname(Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                    if (!FeatureCollection.Container.ContainsKey(cat)) FeatureCollection.Container.Add(cat, new List<FeatureContainer>());
                    cont.filename = f.Key.Path;
                    cont.category = cat;
                    cont.Name = f.Key.Name;
                    int i = cont.Name.LastIndexOf('.');
                    if (i > 0)
                    {
                        cont.Name = cont.Name.Substring(0, i);
                    }
                    cont.Source = f.Value;
                    FeatureCollection.Container[cat].Add(cont);
                    foreach (Feature feat in feats)
                    {
                        feat.Source = cont.Source;
                        foreach (Keyword kw in feat.Keywords) kw.check();
                        feat.Category = cat;
                        if (!FeatureCollection.Categories.ContainsKey(cat)) FeatureCollection.Categories.Add(cat, new List<Feature>());
                        Feature other = FeatureCollection.Categories[cat].Where(ff => string.Equals(ff.Name, feat.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (other != null)
                        {
                            other.ShowSource = true;
                            feat.ShowSource = true;
                        }
                        FeatureCollection.Categories[cat].Add(feat);
                        if (cat.Equals("Boons", StringComparison.OrdinalIgnoreCase))
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
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task<FeatureContainer> LoadFeatureContainerAsync(IFile path)
        {
            using (Stream reader = await path.OpenAsync(FileAccess.Read).ConfigureAwait(false))
            {
                return ((FeatureContainer)FeatureContainer.Serializer.Deserialize(reader));
            }
        }

        public static string FeatureCleanname(string path)
        {
            string cat = path;
            if (!cat.StartsWith(ConfigManager.Directory_Features)) cat = PortablePath.Combine(ConfigManager.Directory_Features, path);
            cat = cat.Replace(PortablePath.DirectorySeparatorChar, '/');
            //if (!Collections.ContainsKey(cat)) Collections.Add(cat, new FeatureCollection());
            return cat;
        }

        public static async Task ImportConditionsAsync()
        {
            OGL.Condition.conditions.Clear();
            OGL.Condition.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Conditions).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        OGL.Condition s = (OGL.Condition)OGL.Condition.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }
        private static MagicCategory MakeMagicCategory(string Name)
        {
            string path = Name.TrimEnd('/');
            int i = path.LastIndexOf('/');
            if (i >= 0) path = path.Substring(i + 1);
            int count = 0;
            foreach (char c in Name)
                if (c == '/') count++;
            return new MagicCategory(Name, path, count);
        }
        public static async Task ImportMagicAsync()
        {
            MagicProperty.properties.Clear();
            MagicProperty.Categories.Clear();
            MagicProperty.Categories.Add("Magic", new MagicCategory("Magic", "Magic", 0));
            MagicProperty.simple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(ConfigManager.Directory_Magic, true).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(PCLSourceManager.GetDirectory(f.Value, ConfigManager.Directory_Features));
                    Uri target = new Uri(PCLSourceManager.Parent(f.Key));
                    string cat = MagicPropertyCleanname(Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                    if (!MagicProperty.Categories.ContainsKey(cat)) MagicProperty.Categories.Add(cat, MakeMagicCategory(cat));
                    String parent = PCLSourceManager.Parent(cat);
                    while (parent.StartsWith(ConfigManager.Directory_Magic) && !MagicProperty.Categories.ContainsKey(parent))
                    {
                        MagicProperty.Categories.Add(parent, MakeMagicCategory(parent));
                        parent = PCLSourceManager.Parent(parent);
                    }
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        MagicProperty mp = ((MagicProperty)MagicProperty.Serializer.Deserialize(reader));
                        mp.Filename = f.Key.Path;
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
            if (!cat.StartsWith(ConfigManager.Directory_Magic)) cat = PortablePath.Combine(ConfigManager.Directory_Magic, path);
            cat = cat.Replace(PortablePath.DirectorySeparatorChar, '/');
            //if (!Collections.ContainsKey(cat)) Collections.Add(cat, new FeatureCollection());
            return cat;
        }

        public static string MagicPropertyPath(string path)
        {
            string cat = MagicPropertyCleanname(path);
            return cat.Remove(0, ConfigManager.Directory_Magic.Length + 1);
        }

        public static string Path(string path)
        {
            return path.Replace(PCLSourceManager.Data.Path, "");
        }
        public async static Task<ConfigManager> LoadConfigAsync(IFile file)
        {
            ConfigManager Loaded = ConfigManager.Loaded = await LoadConfigManagerAsync(file);
            if (ConfigManager.Loaded.Slots.Count == 0)
            {
                ConfigManager.Loaded.Slots = new List<string>() { EquipSlot.MainHand, EquipSlot.OffHand, EquipSlot.Armor };
            }
            ConfigManager.Directory_Items = MakeRelative(Loaded.Items_Directory);
            ConfigManager.Directory_Skills = MakeRelative(Loaded.Skills_Directory);
            ConfigManager.Directory_Languages = MakeRelative(Loaded.Languages_Directory);
            ConfigManager.Directory_Features = MakeRelative(Loaded.Features_Directory);
            ConfigManager.Directory_Backgrounds = MakeRelative(Loaded.Backgrounds_Directory);
            ConfigManager.Directory_Classes = MakeRelative(Loaded.Classes_Directory);
            ConfigManager.Directory_SubClasses = MakeRelative(Loaded.SubClasses_Directory);
            ConfigManager.Directory_Races = MakeRelative(Loaded.Races_Directory);
            ConfigManager.Directory_SubRaces = MakeRelative(Loaded.SubRaces_Directory);
            ConfigManager.Directory_Spells = MakeRelative(Loaded.Spells_Directory);
            ConfigManager.Directory_Magic = MakeRelative(Loaded.Magic_Directory);
            ConfigManager.Directory_Conditions = MakeRelative(Loaded.Conditions_Directory);
            ConfigManager.PDFExporters = new List<string>();
            foreach (string s in ConfigManager.Loaded.PDF) ConfigManager.PDFExporters.Add(s);
            //for (int i = 0; i < loaded.PDF.Count; i++)
            //{
            //    loaded.PDF[i] = Fullpath(path, loaded.PDF[i]);
            //}


            return ConfigManager.Loaded;
        }


        private static string MakeRelative(string dir)
        {
            return System.IO.Path.GetDirectoryName(dir);
        }

        public async static Task<ConfigManager> LoadConfigManagerAsync(IFile file)
        {
            using (Stream reader = await file.OpenAsync(FileAccess.Read))
            {
                ConfigManager cm = (ConfigManager)ConfigManager.Serializer.Deserialize(reader);
                return cm;
            }
        }

        public async static Task<Player> LoadPlayerAsync(IFile file, bool postLoad = true)
        {
            try
            {
                using (Stream fs = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                {
                    Player p = (Player)Player.Serializer.Deserialize(fs);
                    p.FilePath = file;
                    //p.Allies = p.Allies.Replace("\n", Environment.NewLine);
                    //p.Backstory = p.Backstory.Replace("\n", Environment.NewLine);
                    //foreach (Possession pos in p.Possessions) if (pos.Description != null) pos.Description = pos.Description.Replace("\n", Environment.NewLine);
                    //for (int i = 0; i < p.Journal.Count; i++) p.Journal[i] = p.Journal[i].Replace("\n", Environment.NewLine);
                    //for (int i = 0; i < p.ComplexJournal.Count; i++) if (p.ComplexJournal[i].Text != null) p.ComplexJournal[i].Text = p.ComplexJournal[i].Text.Replace("\n", Environment.NewLine);
                    //if (p.Portrait == null && p.PortraitLocation != null && File.Exists(p.PortraitLocation)) p.SetPortrait(new Bitmap(p.PortraitLocation));
                    //if (p.FactionImage == null && p.FactionImageLocation != null && File.Exists(p.FactionImageLocation)) p.SetFactionImage(new Bitmap(p.FactionImageLocation));
                    //p.PortraitLocation = null;
                    //p.FactionImageLocation = null;
                    if (postLoad)
                    {
                        foreach (Spellcasting sc in p.Spellcasting)
                        {
                            sc.PostLoad(p.GetLevel());
                        }
                    }
                    return p;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Can't load " + file.Name + ": " + e.Message, e);
            }
        }
        public static async Task<Level> LoadLevelAsync(IFile file)
        {
            using (Stream fs = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false)) Level.Current = (Level)Level.Serializer.Deserialize(fs);
            Level.Current.Sort();
            return Level.Current;
        }
    }
}
