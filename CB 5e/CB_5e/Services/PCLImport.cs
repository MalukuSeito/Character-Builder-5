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
using ICSharpCode.SharpZipLib.Zip;

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

        public static async Task ImportZips(this OGLContext context, bool applyKeywords = false, bool cleanup = true)
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
            String basepath = PCLSourceManager.Data.Path;
            foreach (IFile z in PCLSourceManager.Zips)
            {

                String s = System.IO.Path.ChangeExtension(z.Name, null);
                if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                {
                    string f = s.ToLowerInvariant() + "/";
                    string ff = s.ToLowerInvariant() + "\\";
                    String basesource = PCLSourceManager.Sources.Select(ss => ss.Name).FirstOrDefault(ss => StringComparer.OrdinalIgnoreCase.Equals(ss, s));
                    bool overridden = basesource != null;
                    foreach (ZipEntry entry in zf)
                    {
                        if (!entry.IsFile) continue;
                        string name = entry.Name.ToLowerInvariant();
                        if ((name.StartsWith(f) || name.StartsWith(ff)) && name.EndsWith(".xml"))
                        {
                            String path = System.IO.Path.Combine(basepath, name);
                            if (overridden && (await FileSystem.Current.GetFileFromPathAsync(path)) != null) continue;
                            using (Stream st = zf.GetInputStream(entry)) OGLImport.Import(st, path, s , basepath, context, applyKeywords);
                        }
                        else if (name.EndsWith(".xml"))
                        {
                            String path = System.IO.Path.Combine(basepath, basesource, name);
                            if (overridden && (await FileSystem.Current.GetFileFromPathAsync(path)) != null) continue;
                            using (Stream st = zf.GetInputStream(entry)) OGLImport.Import(st, path, s, basepath, context, applyKeywords);
                        }
                    }
                }
            }
        }

        public static async Task ImportClassesAsync(this OGLContext context, bool withZips = true, bool applyKeywords = false)
        {
            if (withZips)
            {
                context.Classes.Clear();
                context.ClassesSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Classes_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportClass(reader, s, f.Key, context, applyKeywords); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Classes_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportClass(reader, f.Value, f.Key.Path, context, applyKeywords);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportSubClassesAsync(this OGLContext context, bool withZips = true, bool applyKeywords = false)
        {
            if (withZips) { 
                context.SubClasses.Clear();
                context.SubClassesSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.SubClasses_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportSubClass(reader, s, f.Key, context, applyKeywords); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.SubClasses_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportSubClass(reader, f.Value, f.Key.Path, context, applyKeywords);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }
        public async static Task ImportSubRacesAsync(this OGLContext context, bool withZips = true)
        {
            if (withZips) { 
                context.SubRaces.Clear();
                context.SubRacesSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.SubRaces_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportSubRace(reader, s, f.Key, context); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.SubRaces_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportSubRace(reader, f.Value, f.Key.Path, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }
        public async static Task ImportRacesAsync(this OGLContext context, bool withZips = true)
        {
            if (withZips) {
                context.Races.Clear();
                context.RacesSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Races_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportRace(reader, s, f.Key, context); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Races_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportRace(reader, f.Value, f.Key.Path, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportSkillsAsync(this OGLContext context, bool withZips = true)
        {
            if (withZips)
            {
                context.Skills.Clear();
                context.SkillsSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Skills_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportSkill(reader, s, f.Key, context); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Skills_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportSkill(reader, f.Value, f.Key.Path, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportLanguagesAsync(this OGLContext context, bool withZips = true)
        {
            if (withZips)
            {
                context.Languages.Clear();
                context.LanguagesSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Languages_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportLanguage(reader, s, f.Key, context); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Languages_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportLanguage(reader, f.Value, f.Key.Path, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportMonstersAsync(this OGLContext context, bool withZips = true)
        {
            if (withZips)
            {
                context.Monsters.Clear();
                context.MonstersSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Monster_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportMonster(reader, s, f.Key, context); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Monster_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportMonster(reader, f.Value, f.Key.Path, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportSpellsAsync(this OGLContext context, bool withZips = true)
        {
            if (withZips)
            {
                context.Spells.Clear();
                context.SpellLists.Clear();
                context.SpellsSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Spells_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportSpell(reader, s, f.Key, context); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Spells_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportSpell(reader, f.Value, f.Key.Path, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportItemsAsync(this OGLContext context, bool withZips = true)
        {
            String basepath = PCLSourceManager.Data.Path;
            if (withZips)
            {
                context.Items.Clear();
                context.ItemLists.Clear();
                context.ItemsSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Items_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportItem(reader, s, f.Key, context, OGLImport.GetPath(f.Key, basepath, s)); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Items_Directory, true).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportItem(reader, f.Value, f.Key.Path, context, OGLImport.GetPath(f.Key.Path, basepath, f.Value));
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
        }

        public static Category Make(this OGLContext context, String path)
        {
            String p = path;
            if (!p.StartsWith(context.Config.Items_Directory)) p = PortablePath.Combine(context.Config.Items_Directory, path);
            p = p.Replace(PortablePath.DirectorySeparatorChar, '/');
            if (!Category.Categories.ContainsKey(p))
            {
                Category.Categories.Add(p.ToString(), new Category(p, path.Split(new[] { PortablePath.DirectorySeparatorChar }).ToList(), context));
            }
            String parent = PCLSourceManager.Parent(p);
            if (parent.StartsWith(context.Config.Items_Directory) && !parent.Equals(context.Config.Items_Directory)) Make(context, parent);
            return Category.Categories[p];
        }


        public static async Task ImportBackgroundsAsync(this OGLContext context, bool withZips = true)
        {
            if (withZips)
            {
                context.Backgrounds.Clear();
                context.BackgroundsSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Backgrounds_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportBackground(reader, s, f.Key, context); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Backgrounds_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportBackground(reader, f.Value, f.Key.Path, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportStandaloneFeaturesAsync(this OGLContext context, bool withZips = true)
        {
            String basepath = PCLSourceManager.Data.Path;
            if (withZips)
            {
                context.FeatureCollections.Clear();
                context.FeatureContainers.Clear();
                context.FeatureCategories.Clear();
                context.Boons.Clear();
                context.Features.Clear();
                context.BoonsSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Features_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportFeatureContainer(reader, s, f.Key, context, OGLImport.GetPath(f.Key, basepath, s)); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Features_Directory, true).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportFeatureContainer(reader, f.Value, f.Key.Path, context, OGLImport.GetPath(f.Key.Path, basepath, f.Value));
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }

        public static async Task ImportConditionsAsync(this OGLContext context, bool withZips = true)
        {
            if (withZips) { 
                context.Conditions.Clear();
                context.ConditionsSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Conditions_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportCondition(reader, s, f.Key, context); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Conditions_Directory).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportCondition(reader, f.Value, f.Key.Path, context);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + Path(f.Key.Path), e);
                }
            }
        }
        public static async Task ImportMagicAsync(this OGLContext context, bool withZips = true)
        {
            String basepath = PCLSourceManager.Data.Path;
            if (withZips)
            {
                context.Magic.Clear();
                context.MagicCategories.Clear();
                context.MagicCategories.Add("Magic", new MagicCategory("Magic", "Magic", 0));
                context.MagicSimple.Clear();
                foreach (IFile z in PCLSourceManager.Zips)
                {
                    String s = System.IO.Path.ChangeExtension(z.Name, null);
                    if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                    using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                    {
                        var zfiles = await PCLSourceManager.EnumerateZipFilesAsync(zf, s, context.Config.Magic_Directory).ConfigureAwait(false);
                        foreach (var f in zfiles) try { using (Stream reader = zf.GetInputStream(f.Value)) OGLImport.ImportMagicItem(reader, s, f.Key, context, OGLImport.GetPath(f.Key, basepath, s)); }
                            catch (Exception e) { ConfigManager.LogError("Error reading " + Path(f.Key), e); }
                    }
                }
            }
            var files = await PCLSourceManager.EnumerateFilesAsync(context, context.Config.Magic_Directory, true).ConfigureAwait(false);
            foreach (var f in files)
            {
                try
                {
                    using (Stream reader = await f.Key.OpenAsync(FileAccess.Read).ConfigureAwait(false)) OGLImport.ImportMagicItem(reader, f.Value, f.Key.Path, context, OGLImport.GetPath(f.Key.Path, basepath, f.Value));
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error reading " + f.ToString(), e);
                }
            }
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
            context.Config.Monster_Directory = MakeRelative(context.Config.Monster_Directory);
            context.Config.Plugins_Directory = MakeRelative(context.Config.Plugins_Directory);
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
            if (dir.Contains("\\") || dir.Contains("/"))
            {
                return System.IO.Path.GetDirectoryName(dir);
            }
            return dir;
            
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

        public static async Task<IEnumerable<string>> EnumerateCategories(this OGLContext context, string type)
        {
            HashSet<string> result = new HashSet<string>();
            foreach (var f in await PCLSourceManager.GetAllDirectoriesAsync(context, type))
            {
                result.UnionWith(await GetSubDirectoriesAsync(f.Key, type).ConfigureAwait(false));
            }
            string t = type.TrimEnd('/', '\\').ToLowerInvariant() + "/";
            string tt = type.TrimEnd('/', '\\').ToLowerInvariant() + "\\";
            foreach (IFile z in PCLSourceManager.Zips)
            {
                String s = System.IO.Path.ChangeExtension(z.Name, null);
                using (ZipFile zf = new ZipFile(await z.OpenAsync(FileAccess.Read)))
                {
                    string f = s.ToLowerInvariant() + "/" + t;
                    string ff = s.ToLowerInvariant() + "\\" + tt;
                    zf.IsStreamOwner = true;
                    foreach (ZipEntry entry in zf)
                    {
                        String name = entry.IsFile ? System.IO.Path.GetFileName(entry.Name) : entry.Name;
                        String n = name.ToLowerInvariant();
                        if (n.StartsWith(t)) result.Add(name.Substring(t.Length));
                        else if (n.StartsWith(tt)) result.Add(name.Substring(tt.Length));
                        else if (n.StartsWith(f)) result.Add(name.Substring(f.Length));
                        else if (n.StartsWith(ff)) result.Add(name.Substring(ff.Length));
                    }
                }
            }
            return from s in result orderby s select s;
        }

        private static async Task<IEnumerable<string>> GetSubDirectoriesAsync(IFolder key, string path = "")
        {
            List<string> res = new List<string>();
            foreach (var f in await key.GetFoldersAsync().ConfigureAwait(false) )
            {
                string p = path == "" ? f.Name : path + "/" + f.Name;
                res.Add(p);
                res.AddRange(await GetSubDirectoriesAsync(f, p).ConfigureAwait(false));
            }
            return res;
        }
    }
}
