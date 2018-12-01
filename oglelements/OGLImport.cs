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
        public static void Import(Stream reader, String fullpath, String source, String basepath, OGLContext context, bool applyKeywords = false)
        {
            IEnumerable<String> path = GetPath(fullpath, basepath, source, out String type);
            if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Monster_Directory)) ImportMonster(reader, source, fullpath, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Features_Directory)) ImportFeatureContainer(reader, source, fullpath, context, path);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Items_Directory)) ImportItem(reader, source, fullpath, context, path);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Magic_Directory)) ImportMagicItem(reader, source, fullpath, context, path);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Spells_Directory)) ImportSpell(reader, source, fullpath, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Backgrounds_Directory)) ImportBackground(reader, source, fullpath, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Races_Directory)) ImportRace(reader, source, fullpath, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.SubRaces_Directory)) ImportSubRace(reader, source, fullpath, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.SubClasses_Directory)) ImportSubClass(reader, source, fullpath, context, applyKeywords);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Classes_Directory)) ImportClass(reader, source, fullpath, context, applyKeywords);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Languages_Directory)) ImportLanguage(reader, source, fullpath, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Skills_Directory)) ImportSkill(reader, source, fullpath, context);
            else if (StringComparer.OrdinalIgnoreCase.Equals(type, context.Config.Conditions_Directory)) ImportCondition(reader, source, fullpath, context);
            else throw new Exception("Unknown Type: " + type);
        }

        public static void ImportBackground(Stream reader, string source, string fullpath, OGLContext context)
        {
            {
                Background s = (Background)Background.Serializer.Deserialize(reader);
                s.Source = source;
                foreach (Feature fea in s.Features) fea.Source = source;
                s.Register(context, fullpath);
            }
        }

        public static void ImportClass(Stream reader, string source, string fullpath, OGLContext context, bool applyKeywords = false)
        {
            ClassDefinition s = (ClassDefinition)ClassDefinition.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath, applyKeywords);
        }

        public static void ImportCondition(Stream reader, string source, string fullpath, OGLContext context)
        {
            Condition s = (Condition)Condition.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportMonster(Stream reader, string source, string fullpath, OGLContext context)
        {
            Monster s = (Monster)Monster.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportLanguage(Stream reader, string source, string fullpath, OGLContext context)
        {
            Language s = (Language)Language.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportRace(Stream reader, string source, string fullpath, OGLContext context)
        {
            Race s = (Race)Race.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportSkill(Stream reader, string source, string fullpath, OGLContext context)
        {
            Skill s = (Skill)Skill.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportSpell(Stream reader, string source, string fullpath, OGLContext context)
        {
            Spell s = (Spell)Spell.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }
        public static void ImportSubClass(Stream reader, string source, string fullpath, OGLContext context, bool applyKeywords = false)
        {
            SubClass s = (SubClass)SubClass.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath, applyKeywords);
        }

        public static void ImportSubRace(Stream reader, string source, string fullpath, OGLContext context)
        {
            SubRace s = (SubRace)SubRace.Serializer.Deserialize(reader);
            s.Source = source;
            s.Register(context, fullpath);
        }

        public static void ImportMagicItem(Stream reader, string source, string fullpath, OGLContext context, IEnumerable<String> path)
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

        public static void ImportItem(Stream reader, string source, string fullpath, OGLContext context, IEnumerable<String> path)
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

        public static void ImportFeatureContainer(Stream reader, string source, string fullpath, OGLContext context, IEnumerable<String> path)
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
