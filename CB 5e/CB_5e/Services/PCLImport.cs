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
    public static class PCLImport
    {
        public static async Task<AbilityScores> LoadAbilityScoresAsync(this OGLContext context, IFile file)
        {
            using (Stream fs = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false)) context.Scores = (AbilityScores)AbilityScores.Serializer.Deserialize(fs);
            context.Scores.Filename = file.Path;
            return context.Scores;
        }

        public static async Task ImportClassesAsync(this OGLContext context, bool applyKeywords = false)
        {
            context.Classes.Clear();
            context.ClassesSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Classes_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) 
                    {
                        ClassDefinition s = (ClassDefinition)ClassDefinition.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.Path, applyKeywords);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportSubClassesAsync(this OGLContext context, bool applyKeywords = false)
        {
            context.SubClasses.Clear();
            context.SubClassesSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.SubClasses_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        SubClass s = (SubClass)SubClass.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.Path, applyKeywords);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }
        public async static Task ImportSubRacesAsync(this OGLContext context)
        {
            context.SubRaces.Clear();
            context.SubRacesSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.SubRaces_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        SubRace s = (SubRace)SubRace.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }
        public async static Task ImportRacesAsync(this OGLContext context)
        {
            context.Races.Clear();
            context.RacesSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Races_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Race s = (Race)Race.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportSkillsAsync(this OGLContext context)
        {
            context.Skills.Clear();
            context.SkillsSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Skills_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Skill s = (Skill)Skill.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportLanguagesAsync(this OGLContext context)
        {
            context.Languages.Clear();
            context.LanguagesSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Languages_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Language s = (Language)Language.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportSpellsAsync(this OGLContext context)
        {
            context.Spells.Clear();
            context.SpellLists.Clear();
            context.SpellsSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Spells_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Spell s = (Spell)Spell.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportItemsAsync(this OGLContext context)
        {
            context.Items.Clear();
            context.ItemLists.Clear();
            context.ItemsSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Items_Directory, true).ConfigureAwait(false);

            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(PCLSourceManager.GetDirectory(f.Value, context.Config.Items_Directory));
                    Uri target = new Uri(PCLSourceManager.Parent(f.Key));
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Item s = (Item)Item.Serializer.Deserialize(reader);
                        s.Category = Make(context, source.MakeRelativeUri(target));
                        s.Source = f.Value;
                        s.Register(context, f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }

        public static Category Make(this OGLContext context, Uri path)
        {
            return Make(context, Uri.UnescapeDataString(path.ToString()));
        }

        public static Category Make(this OGLContext context, String path)
        {
            String p = path;
            if (!p.StartsWith(context.Config.Items_Directory)) p = PortablePath.Combine(context.Config.Items_Directory, path);
            p = p.Replace(PortablePath.DirectorySeparatorChar, '/');
            if (!Category.Categories.ContainsKey(p))
            {
                Category.Categories.Add(p.ToString(), new Category(p, path.Split(new[] { PortablePath.DirectorySeparatorChar }).ToList()));
            }
            String parent = PCLSourceManager.Parent(p);
            if (parent.StartsWith(context.Config.Items_Directory) && !parent.Equals(context.Config.Items_Directory)) Make(context, parent);
            return Category.Categories[p];
        }


        public static async Task ImportBackgroundsAsync(this OGLContext context)
        {
            context.Backgrounds.Clear();
            context.BackgroundsSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Backgrounds_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        Background s = (Background)Background.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        foreach (Feature fea in s.Features) fea.Source = f.Value;
                        s.Register(context, f.Key.Path);
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportStandaloneFeaturesAsync(this OGLContext context)
        {
            context.FeatureCollections.Clear();
            context.FeatureContainers.Clear();
            context.FeatureCategories.Clear();
            context.Boons.Clear();
            context.Features.Clear();
            context.BoonsSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Features_Directory, true).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(PCLSourceManager.GetDirectory(f.Value, context.Config.Features_Directory));
                    Uri target = new Uri(PCLSourceManager.Parent(f.Key));
                    FeatureContainer cont = await LoadFeatureContainerAsync(f.Key);
                    List<Feature> feats = cont.Features;
                    string cat = FeatureCleanname(context, Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                    if (!context.FeatureContainers.ContainsKey(cat)) context.FeatureContainers.Add(cat, new List<FeatureContainer>());
                    cont.filename = f.Key.Path;
                    cont.category = cat;
                    cont.Name = f.Key.Name;
                    int i = cont.Name.LastIndexOf('.');
                    if (i > 0)
                    {
                        cont.Name = cont.Name.Substring(0, i);
                    }
                    cont.Source = f.Value;
                    context.FeatureContainers[cat].Add(cont);
                    foreach (Feature feat in feats)
                    {
                        feat.Source = cont.Source;
                        foreach (Keyword kw in feat.Keywords) kw.check();
                        feat.Category = cat;
                        if (!context.FeatureCategories.ContainsKey(cat)) context.FeatureCategories.Add(cat, new List<Feature>());
                        Feature other = context.FeatureCategories[cat].Where(ff => string.Equals(ff.Name, feat.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (other != null)
                        {
                            other.ShowSource = true;
                            feat.ShowSource = true;
                        }
                        context.FeatureCategories[cat].Add(feat);
                        if (cat.Equals("Feats/Boons", StringComparison.OrdinalIgnoreCase))
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

        public static string FeatureCleanname(this OGLContext context, string path)
        {
            string cat = path;
            if (!cat.StartsWith(context.Config.Features_Directory)) cat = PortablePath.Combine(context.Config.Features_Directory, path);
            cat = cat.Replace(PortablePath.DirectorySeparatorChar, '/');
            //if (!Collections.ContainsKey(cat)) Collections.Add(cat, new FeatureCollection());
            return cat;
        }

        public static async Task ImportConditionsAsync(this OGLContext context)
        {
            context.Conditions.Clear();
            context.ConditionsSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Conditions_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                    {
                        OGL.Condition s = (OGL.Condition)OGL.Condition.Serializer.Deserialize(reader);
                        s.Source = f.Value;
                        s.Register(context, f.Key.Path);
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
        public static async Task ImportMagicAsync(this OGLContext context)
        {
            context.Magic.Clear();
            context.MagicCategories.Clear();
            context.MagicCategories.Add("Magic", new MagicCategory("Magic", "Magic", 0));
            context.MagicSimple.Clear();
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Magic_Directory, true).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    Uri source = new Uri(PCLSourceManager.GetDirectory(f.Value, context.Config.Magic_Directory));
                    Uri target = new Uri(PCLSourceManager.Parent(f.Key));
                    string cat = MagicPropertyCleanname(context, Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                    if (!context.MagicCategories.ContainsKey(cat)) context.MagicCategories.Add(cat, MakeMagicCategory(cat));
                    String parent = PCLSourceManager.Parent(cat);
                    while (parent.StartsWith(context.Config.Magic_Directory) && !context.MagicCategories.ContainsKey(parent))
                    {
                        context.MagicCategories.Add(parent, MakeMagicCategory(parent));
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
            if (!cat.StartsWith(context.Config.Magic_Directory)) cat = PortablePath.Combine(context.Config.Magic_Directory, path);
            cat = cat.Replace(PortablePath.DirectorySeparatorChar, '/');
            //if (!Collections.ContainsKey(cat)) Collections.Add(cat, new FeatureCollection());
            return cat;
        }

        public static string MagicPropertyPath(OGLContext context, string path)
        {
            string cat = MagicPropertyCleanname(context, path);
            return cat.Remove(0, context.Config.Magic_Directory.Length + 1);
        }

        public static string Path(string path)
        {
            return path.Replace(PCLSourceManager.Data.Path, "");
        }
        public async static Task<ConfigManager> LoadConfigAsync(this OGLContext context, IFile file)
        {
            context.Config = await LoadConfigManagerAsync(file);
            if (context.Config.Slots.Count == 0)
            {
                context.Config.Slots = new List<string>() { EquipSlot.MainHand, EquipSlot.OffHand, EquipSlot.Armor };
            }
            context.Config.Items_Directory = MakeRelative(context.Config.Items_Directory);
            context.Config.Skills_Directory = MakeRelative(context.Config.Skills_Directory);
            context.Config.Languages_Directory = MakeRelative(context.Config.Languages_Directory);
            context.Config.Features_Directory = MakeRelative(context.Config.Features_Directory);
            context.Config.Backgrounds_Directory = MakeRelative(context.Config.Backgrounds_Directory);
            context.Config.Classes_Directory = MakeRelative(context.Config.Classes_Directory);
            context.Config.SubClasses_Directory = MakeRelative(context.Config.SubClasses_Directory);
            context.Config.Races_Directory = MakeRelative(context.Config.Races_Directory);
            context.Config.SubRaces_Directory = MakeRelative(context.Config.SubRaces_Directory);
            context.Config.Spells_Directory = MakeRelative(context.Config.Spells_Directory);
            context.Config.Magic_Directory = MakeRelative(context.Config.Magic_Directory);
            context.Config.Conditions_Directory = MakeRelative(context.Config.Conditions_Directory);
            context.Config.PDFExporters = new List<string>();
            foreach (string s in context.Config.PDF) context.Config.PDFExporters.Add(s);
            //for (int i = 0; i < loaded.PDF.Count; i++)
            //{
            //    loaded.PDF[i] = Fullpath(path, loaded.PDF[i]);
            //}


            return context.Config;
        }


        public static string MakeRelative(string dir)
        {
            return System.IO.Path.GetDirectoryName(dir);
        }

        public static string MakeRelativeFile(string dir)
        {
            return System.IO.Path.GetFileName(dir);
        }

        public async static Task<ConfigManager> LoadConfigManagerAsync(IFile file)
        {
            using (Stream reader = await file.OpenAsync(FileAccess.Read))
            {
                ConfigManager cm = (ConfigManager)ConfigManager.Serializer.Deserialize(reader);
                return cm;
            }
        }

        public async static Task<Player> LoadPlayerAsync(this BuilderContext context, IFile file, bool postLoad = true)
        {
            try
            {
                using (Stream fs = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false))
                {
                    Player p = (Player)Player.Serializer.Deserialize(fs);
                    p.Context = context;
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
        public static async Task<Level> LoadLevelAsync(this OGLContext context, IFile file)
        {
            using (Stream fs = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false)) context.Levels = (Level)Level.Serializer.Deserialize(fs);
            context.Levels.Sort();
            return context.Levels;
        }
    }
}
