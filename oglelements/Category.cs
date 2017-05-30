using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace Character_Builder_5
{
    public class Category: IComparable<Category>
    {
        [XmlIgnore]
        public static string regex2 = String.Format("[{0}]", Regex.Escape(new string(System.IO.Path.GetInvalidPathChars())));
        [XmlIgnore]
        public static Regex removeInvalidPathChars = new Regex(regex2, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        [XmlIgnore]
        public List<String> CategoryPath { get; private set; }
        public string Path { get; private set; }
        [XmlIgnore]
        public static Dictionary<String, Category> categories = new Dictionary<string, Category>();

        public static Category Make()
        {
            if (!categories.ContainsKey(ConfigManager.Directory_Items)) categories.Add(ConfigManager.Directory_Items, new Category());
            return categories[ConfigManager.Directory_Items];
        }
        public static Category Make(Uri path)
        {
            return Make(Uri.UnescapeDataString(path.ToString()));
        }
        public static Category Make(String path)
        {
            String p=path;
            if (!p.StartsWith(ConfigManager.Directory_Items)) p = System.IO.Path.Combine(ConfigManager.Directory_Items, path);
            p = p.Replace(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
            if (!categories.ContainsKey(p)) categories.Add(p.ToString(), new Category(p));
            return categories[p];
        }
        private Category(String path)
        {
            
            CategoryPath = new List<string>();
            CategoryPath.AddRange(path.Split(new[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar }));
            Path = path;
            //if (CategoryPath.First<String>() != ConfigManager.Directory_Items.Directory.Name) CategoryPath.Insert(0, ConfigManager.Directory_Items.Directory.Name);
        }
        private Category()
        {
            CategoryPath = new List<string>();
            CategoryPath.Add(ConfigManager.Directory_Items);
        }
        public string makePath()
        {
            if (CategoryPath.Count > 1 && CategoryPath.Last<String>() == ConfigManager.Directory_Items) return ".";
            List<String> cnames = new List<string>();
            foreach (string s in CategoryPath) 
            {
                removeInvalidPathChars.Replace(s, "_");
                cnames.Add(s);
            }
            cnames.Remove(ConfigManager.Directory_Items);
            return System.IO.Path.Combine(cnames.ToArray());
        }
        public int CompareTo (Category other) {
            return makePath().CompareTo(other.makePath());
        }
        public override string ToString()
        {
            return new String(' ', CategoryPath.Count - 1) + CategoryPath.Last<String>();
        }
        public static IOrderedEnumerable<Category> Section()
        {
            return (from c in categories.Values orderby c select c);
        }
    }
}
