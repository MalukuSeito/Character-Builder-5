using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Xsl;
using XCalc;

namespace Character_Builder_5
{
    public class FeatureCollection
    {
        [XmlIgnore]
        private static Regex quotes = new Regex("([\\\"])(?:\\\\\\1|.)*?\\1", RegexOptions.Compiled);

        private static string fixQuotes(string exp)
        {
            StringBuilder res = new StringBuilder();
            int last = 0;
            for (Match m = quotes.Match(exp); m.Success; m = m.NextMatch())
            {
                res.Append(exp.Substring(last, m.Index - last));
                last = m.Index + m.Length;
                res.Append("'");
                res.Append(m.Value.Substring(1, m.Length - 2).Replace("\\\"", "\"").Replace("'", "\\'"));
                res.Append("'");
            }
            if (last == 0) return exp;
            if (last < exp.Length) res.Append(exp.Substring(last));
            return res.ToString();
        }


        [XmlIgnore]
        public static List<Dictionary<string, List<Feature>>> Collections = new List<Dictionary<string, List<Feature>>>();
        public static Dictionary<string, List<FeatureContainer>> Container = new Dictionary<string, List<FeatureContainer>>(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, List<Feature>> Categories = new Dictionary<string, List<Feature>>(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, Feature> Boons = new Dictionary<string, Feature>(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, Feature> simple = new Dictionary<string, Feature>(StringComparer.OrdinalIgnoreCase);
        public static List<Feature> Features = new List<Feature>();
        private static List<List<Feature>> copies = new List<List<Feature>>();
        /*public static List<Feature> Feats = new List<Feature>();
        public static List<string> Names = new List<string>();
        public static List<int> Levels = new List<int>();
        public static List<List<string>> Keywords = new List<List<string>>();
        public static List<string> Categories = new List<string>();*/
        public static void ExportAll()
        {
            //foreach (KeyValuePair<string, FeatureCollection> fc in Collections)
            //{
            //    string path = Path.Combine(ConfigManager.Directory_Features.FullName, fc.Key);
            //    foreach (Feature i in fc.Value.Features)
                foreach (Feature i in Features)
                {
                    string path = Path.Combine(SourceManager.getDirectory(i.Source, Path.Combine(ConfigManager.Directory_Features, i.Category)).FullName, i.Category);
                    String iname = string.Join("_", i.Name.Split(ConfigManager.InvalidChars));
                    FileInfo file = new FileInfo(Path.Combine(path, iname + ".xml"));
                    file.Directory.Create();
                    i.Save(file.FullName);
                }
            //}
        }
        public static void ImportAll()
        {
            Collections.Clear();
            Container.Clear();
            Categories.Clear();
            Boons.Clear();
            Features.Clear();
            simple.Clear();
            var files = SourceManager.EnumerateFiles(ConfigManager.Directory_Features);
            foreach (var f in files)
            {
                Uri source = new Uri(SourceManager.getDirectory(f.Value, ConfigManager.Directory_Features).FullName);
                Uri target = new Uri(f.Key.DirectoryName);
                FeatureContainer cont = FeatureContainer.Load(f.Key.FullName);
                List<Feature> feats = cont.Features;
                string cat=cleanname(Uri.UnescapeDataString(source.MakeRelativeUri(target).ToString()));
                if (!Container.ContainsKey(cat)) Container.Add(cat, new List<FeatureContainer>());
                cont.filename = f.Key.FullName;
                cont.category = cat;
                cont.Name = Path.GetFileNameWithoutExtension(f.Key.FullName);
                cont.Source = f.Value;
                Container[cat].Add(cont);
                foreach (Feature feat in feats)
                {
                    feat.Source = cont.Source;
                    foreach (Keyword kw in feat.Keywords) kw.check();
                    feat.Category = cat;
                    if (!Categories.ContainsKey(cat)) Categories.Add(cat, new List<Feature>());
                    Categories[cat].Add(feat);
                    if (cat.Contains("Boons"))
                    {
                        if (simple.ContainsKey(feat.Name))
                        {
                            simple[feat.Name].ShowSource = true;
                            feat.ShowSource = true;
                        }
                        else simple.Add(feat.Name, feat);
                        Boons[feat.Name + " " + ConfigManager.SourceSeperator + " " + feat.Source] = feat;
                    }
                }
                foreach (Feature feat in feats)
                {
                    Features.Add(feat);
                }
                //Collections[].AddRange(feats);
            }
        }
        public void Add(Feature f)
        {
            Features.Add(f);
        }
        public void AddRange(List<Feature> f)
        {
            Features.AddRange(f);
        }
        public static string cleanname(string path)
        {
            string cat = path;
            if (!cat.StartsWith(ConfigManager.Directory_Features)) cat = Path.Combine(ConfigManager.Directory_Features, path);
            cat = cat.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            //if (!Collections.ContainsKey(cat)) Collections.Add(cat, new FeatureCollection());
            return cat;
        }
        public static List<Feature> Get(string expression, int copy = 0)
        {
            while (Collections.Count <= copy) Collections.Add(new Dictionary<string, List<Feature>>(StringComparer.OrdinalIgnoreCase));
            int c = copy - 1;
            while (copies.Count <= c) copies.Add(MakeCopy(Features));
            List<Feature> features = Features;
            if (c >= 0) features = copies[c];
            if (expression == null || expression == "") expression = "Category = 'Feats'";
            if (expression == "Boons") expression = "Category = 'Boons'";
            if (Collections[copy].ContainsKey(expression)) return new List<Feature>(Collections[copy][expression]);
            try
            {
                Expression ex = new Expression(fixQuotes(expression));
                Feature current=null;
                ex.EvaluateParameter += delegate(string name, ParameterArgs args)
                {
                    name=name.ToLowerInvariant();
                    if (name == "category") args.Result = current.Category;
                    else if (name == "level") args.Result = current.Level;
                    else if (name == "name") args.Result = current.Name.ToLowerInvariant();
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k=>k.Name == name)) args.Result = true;
                    else args.Result = false;
                };
                List<Feature> res=new List<Feature>();
                foreach (Feature f in features)
                {
                    current=f;
                    object o = ex.Evaluate();
                    if (o is Boolean && (Boolean)o) res.Add(current);
                    
                }
                res.Sort();
                Collections[copy][expression] = res;
                return res;
            }
            catch (Exception e)
            {
                throw new Exception("Error while evaluating expression " + expression + ":" + e);
            }
        }

        internal static List<Feature> MakeCopy(List<Feature> features)
        {
            FeatureContainer fc = new FeatureContainer(features);
            return fc.clone().Features;
        }

        public static Feature getBoon(string name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperator))
            {
                if (Boons.ContainsKey(name)) return Boons[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Boons.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Boons[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (simple.ContainsKey(name)) return simple[name];
            throw new Exception("Unknown Boon: " + name);
        }
        public static IEnumerable<string> Section()
        {
            if (Item.Search == null) return from s in Categories.Keys where s.EndsWith("/Boons") orderby s select s;
            return from s in Categories where s.Key.EndsWith("/Boons") && s.Value.Exists(f => f.Test()) orderby s.Key select s.Key;
        }
        public static IEnumerable<Feature> Subsection(string s)
        {
            if (Item.Search == null) return from f in Categories[s] orderby f select f;
            else return from f in Categories[s] where f.Test() orderby f select f;
        }
    }

}
