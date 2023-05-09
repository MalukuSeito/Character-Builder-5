using OGL.Features;
using OGL.Items;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL
{
    public class OGLImport
    {

        public void Merge(OGLContext context, OGLContext other, bool applyKeywords = false)
        {
            foreach (var b in other.Skills.Values) b.Register(context, b.FileName);
            foreach (var b in other.Languages.Values) b.Register(context, b.FileName);
            foreach (var b in other.Spells.Values) b.Register(context, b.FileName);

            foreach (var b in other.Items.Values)
            {
                if (b.Category != null && !Category.Categories.ContainsKey(b.Category.Path))
                {
                    Category.Categories.Add(b.Category.Path, new Category(b.Category.Path, b.Category.CategoryPath, context));
                }
                b.Register(context, b.FileName);
            }
            foreach (var b in other.Backgrounds.Values) b.Register(context, b.FileName);
            foreach (var b in other.Races.Values) b.Register(context, b.FileName);
            foreach (var b in other.SubRaces.Values) b.Register(context, b.FileName);
            foreach (var b in other.FeatureCategories)
            {
                if (!context.FeatureCategories.ContainsKey(b.Key)) context.FeatureCategories.Add(b.Key, new List<Feature>(b.Value));
                else context.FeatureCategories[b.Key].AddRange(b.Value);
            }
            foreach (var b in other.FeatureContainers)
            {
                if (!context.FeatureContainers.ContainsKey(b.Key)) context.FeatureContainers.Add(b.Key, new List<FeatureContainer>(b.Value));
                else context.FeatureContainers[b.Key].AddRange(b.Value);
            }
            foreach (var b in other.Boons.Values)
            {
                if (context.BoonsSimple.ContainsKey(b.Name))
                {
                    context.BoonsSimple[b.Name].ShowSource = true;
                    b.ShowSource = true;
                }
                else context.BoonsSimple.Add(b.Name, b);
                if (context.Boons.ContainsKey(b.Name + " " + ConfigManager.SourceSeperator + " " + b.Source)) ConfigManager.LogError("Duplicate Boon: " + b.Name + " " + ConfigManager.SourceSeperator + " " + b.Source);
                else context.Boons[b.Name + " " + ConfigManager.SourceSeperator + " " + b.Source] = b;
            }
            foreach (var b in other.Features) context.Features.Add(b);
            foreach (var b in other.Conditions.Values) b.Register(context, b.FileName);
            foreach (var b in other.MagicCategories.Values) if (!context.MagicCategories.ContainsKey(b.Name)) context.MagicCategories.Add(b.Name, new MagicCategory(b.Name, b.DisplayName, b.Indent));
            foreach (var mp in other.Magic.Values)
            {
                string cat = mp.Category;
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
            foreach (var b in other.Classes.Values) b.Register(context, b.FileName, applyKeywords);
            foreach (var b in other.SubClasses.Values) b.Register(context, b.FileName, applyKeywords);
            foreach (var b in other.Monsters.Values) b.Register(context, b.FileName);
        }

        public static void Import(Stream reader, String fullpath, String source, String basepath, OGLContext context, bool applyKeywords = false)
        {
            IEnumerable<String> path = GetPath(fullpath, basepath, source, out String type);
            if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Monster_Directory)) ImportMonster(reader, fullpath, source, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Features_Directory)) ImportFeatureContainer(reader, fullpath, source, context, path);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Items_Directory)) ImportItem(reader, fullpath, source, context, path);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Magic_Directory)) ImportMagicItem(reader, fullpath, source, context, path);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Spells_Directory)) ImportSpell(reader, fullpath, source, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Backgrounds_Directory)) ImportBackground(reader, fullpath, source, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Races_Directory)) ImportRace(reader, fullpath, source, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.SubRaces_Directory)) ImportSubRace(reader, fullpath, source, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.SubClasses_Directory)) ImportSubClass(reader, fullpath, source, context, applyKeywords);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Classes_Directory)) ImportClass(reader, fullpath, source, context, applyKeywords);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Languages_Directory)) ImportLanguage(reader, fullpath, source, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Skills_Directory)) ImportSkill(reader, fullpath, source, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Conditions_Directory)) ImportCondition(reader, fullpath, source, context);
            else throw new Exception("Unknown Type: " + type);
        }

        public static void ImportBackground(Stream reader, string fullpath, string source, OGLContext context)
        {
            {
                Background s = (Background)Background.Serializer.Deserialize(reader);
                s.Source = source;
                foreach (Feature fea in s.Features) fea.Source = source;
                s.Register(context, fullpath);
            }
        }

        public static void ImportClass(Stream reader, string fullpath, string source, OGLContext context, bool applyKeywords = false)
        {
            ClassDefinition s = (ClassDefinition)ClassDefinition.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath, applyKeywords);
        }

        public static void ImportCondition(Stream reader, string fullpath, string source, OGLContext context)
        {
            Condition s = (Condition)Condition.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportMonster(Stream reader, string fullpath, string source, OGLContext context)
        {
            Monster s = (Monster)Monster.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportLanguage(Stream reader, string fullpath, string source, OGLContext context)
        {
            Language s = (Language)Language.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportRace(Stream reader, string fullpath, string source, OGLContext context)
        {
            Race s = (Race)Race.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportSkill(Stream reader, string fullpath, string source, OGLContext context)
        {
            Skill s = (Skill)Skill.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportSpell(Stream reader, string fullpath, string source, OGLContext context)
        {
            Spell s = (Spell)Spell.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }
        public static void ImportSubClass(Stream reader, string fullpath, string source, OGLContext context, bool applyKeywords = false)
        {
            SubClass s = (SubClass)SubClass.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath, applyKeywords);
        }

        public static void ImportSubRace(Stream reader, string fullpath, string source, OGLContext context)
        {
            SubRace s = (SubRace)SubRace.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportMagicItem(Stream reader, string fullpath, string source, OGLContext context, IEnumerable<String> path)
        {
            String cat = context.Config.Magic_Directory;
            List<String> p = new List<string>() { cat };
            p.AddRange(path);
            for (int i = 1; i < p.Count; i++)
            {
                cat = String.Join("/", p.Take(i)); 
                if (!context.MagicCategories.ContainsKey(cat)) {
                    context.MagicCategories.Add(cat, MakeMagicCategory(p.Take(i)));
                }
            }

            MagicProperty mp = ((MagicProperty)MagicProperty.Serializer.Deserialize(reader));
            mp.FileName = fullpath;
            mp.Source = source;
                foreach (Feature fea in mp.AttunementFeatures) fea.Source = source;
            foreach (Feature fea in mp.CarryFeatures) fea.Source = source;
            foreach (Feature fea in mp.OnUseFeatures) fea.Source = source;
            foreach (Feature fea in mp.EquipFeatures) fea.Source = source;
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

        public static void ImportItem(Stream reader, string fullpath, string source, OGLContext context, IEnumerable<String> path)
        {
            String cat = context.Config.Items_Directory;
            List<String> p = new List<string>() { cat };
            p.AddRange(path);
            for (int i = 1; i < p.Count; i++)
            {
                cat = String.Join("/", p.Take(i));
                if (!Category.Categories.ContainsKey(cat))
                {
                    Category.Categories.Add(cat, new Category(cat, p.Take(i), context));
                }
            }

            Item s = (Item)Item.Serializer.Deserialize(reader);
            s.Category = Category.Categories[cat];
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportFeatureContainer(Stream reader, string fullpath, string source, OGLContext context, IEnumerable<String> path)
        {
            String cat = context.Config.Features_Directory;
            List<String> p = new List<string>() { cat };
            p.AddRange(path);
            for (int i = 1; i < p.Count; i++)
            {
                cat = String.Join("/", p.Take(i));
            }
            FeatureContainer cont = FeatureContainer.Serializer.Deserialize(reader) as FeatureContainer;
            List<Feature> feats = cont.Features;
            if (!context.FeatureContainers.ContainsKey(cat)) context.FeatureContainers.Add(cat, new List<FeatureContainer>());
            cont.FileName = fullpath;
            cont.category = cat;
            cont.Name = Path.GetFileNameWithoutExtension(fullpath);
            cont.Source = source;
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

        private static MagicCategory MakeMagicCategory(IEnumerable<String> fullpath)
        {
            return new MagicCategory(String.Join("/", fullpath), fullpath.Last(), fullpath.Count() - 1);
        }

        public static IEnumerable<String> GetPath(string fullpath, string basepath, string source, out string type)
        {
            List<String> full = fullpath.Split('/', '\\').ToList();
            List<String> basep = basepath.Split('/', '\\').ToList();
            int i = 0;
            for (i = 0; i < basep.Count; i++)
            {
                if (!StringComparer.OrdinalIgnoreCase.Equals(full[0], basep[0]))
                {
                    throw new Exception("Unmatched paths: " + fullpath + " vs. " + basepath);
                }
            }
            if (full.Count > i + 2 && StringComparer.OrdinalIgnoreCase.Equals(source, full[i])) i++;
            else throw new Exception("Source: " + source + " not found in " + fullpath);
            type = full[i++].ToLower();
            return full.Skip(i);
        }

        public static IEnumerable<String> GetPath(string fullpath, string basepath, string source)
        {
            List<String> full = fullpath.Split('/', '\\').ToList();
            List<String> basep = basepath.Split('/', '\\').ToList();
            int i = 0;
            for (i = 0; i < basep.Count; i++)
            {
                if (!StringComparer.OrdinalIgnoreCase.Equals(full[0], basep[0]))
                {
                    throw new Exception("Unmatched paths: " + fullpath + " vs. " + basepath);
                }
            }
            if (full.Count > i + 2 && StringComparer.OrdinalIgnoreCase.Equals(source, full[i])) i++;
            else throw new Exception("Source: " + source + " not found in " + fullpath);
            return full.Skip(i + 1);
        }
    }
}
