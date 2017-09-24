using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace OGL.Items
{
    public class Category: IComparable<Category>
    {
        [XmlIgnore]
        private static string regex2 = String.Format("[{0}]", Regex.Escape(new string(ConfigManager.InvalidChars)));
        [XmlIgnore]
        private static Regex removeInvalidPathChars = new Regex(regex2, RegexOptions.Singleline | RegexOptions.CultureInvariant);
        [XmlIgnore]
        public List<String> CategoryPath { get; private set; }
        public string Path { get; private set; }
        [XmlIgnore]
        public static Dictionary<String, Category> Categories = new Dictionary<string, Category>();

        private string ItemsDir;
        public static Category Make(OGLContext context)
        {
            if (!Category.Categories.ContainsKey(context.Config.Items_Directory)) Category.Categories.Add(context.Config.Items_Directory, new Category(context));
            return Category.Categories[context.Config.Items_Directory];
        }
        public Category(String path, List<string> categorypath)
        {

            CategoryPath = categorypath;
            Path = path;
            //if (CategoryPath.First<String>() != ConfigManager.Directory_Items.Directory.Name) CategoryPath.Insert(0, ConfigManager.Directory_Items.Directory.Name);
        }
        public Category(OGLContext context)
        {
            ItemsDir = context.Config.Items_Directory;
            CategoryPath = new List<string>
            {
                context.Config.Items_Directory
            };
        }
        public List<String> MakePath()
        {
            if (CategoryPath.Count > 1 && CategoryPath.Last() == ItemsDir) return new List<string>() { "." };
            List<String> cnames = new List<string>();
            foreach (string s in CategoryPath) 
            {
                removeInvalidPathChars.Replace(s, "_");
                cnames.Add(s);
            }
            cnames.Remove(ItemsDir);
            return cnames;
        }
        public int CompareTo (Category other) {
            return String.Join("//", MakePath()).CompareTo(String.Join("//", other.MakePath()));
        }
        public override string ToString()
        {
            if (CategoryPath.Count < 2) return "";
            return new String(' ', CategoryPath.Count - 2) + CategoryPath.Last<String>();
        }
        public static IOrderedEnumerable<Category> Section()
        {
            return (from c in Categories.Values where c.ToString() != "Items" && c.ToString() != "" orderby c select c);
        }
    }
}
