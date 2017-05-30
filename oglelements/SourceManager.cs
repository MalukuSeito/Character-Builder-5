using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace Character_Builder_5
{
    public class SourceManager
    {
        public static List<string> Sources { get; private set; }
        private static string AppPath = null;
        public static bool init(string path)
        {
            Sources = new List<string>();
            AppPath = path;
            foreach (string s in Directory.EnumerateDirectories(path))
            {
                Sources.Add(Path.GetFileName(s));
                string f = Path.Combine(s, "LICENSE");
                Console.WriteLine(f);
                if (File.Exists(f))
                {
                    System.Windows.Forms.DialogResult r = new License(Path.GetFileName(s), File.ReadAllLines(f)).ShowDialog();
                    if (r == System.Windows.Forms.DialogResult.OK)
                    {
                        File.Move(f, f + ".txt");
                    } else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static DirectoryInfo getDirectory(string source, string type) {
            if (source == null || source == "") source = "No Source";
            String isource = string.Join("_", source.Split(Path.GetInvalidFileNameChars()));
            if (!Sources.Contains(isource)) Sources.Add(isource);
            DirectoryInfo res = new DirectoryInfo(Path.Combine(AppPath, isource, type));
            res.Create();
            return res;
        }

        public static Dictionary<DirectoryInfo, string> getAllDirectories(string type)
        {
            Dictionary<DirectoryInfo, string> result = new Dictionary<DirectoryInfo, string>();
            foreach (string s in Sources)
            {
                DirectoryInfo res = new DirectoryInfo(Path.Combine(AppPath, s, type));
                if (res.Exists) result.Add(res, s);
            }
            return result;
        }

        public static Dictionary<FileInfo, string> EnumerateFiles(string type, SearchOption option = SearchOption.AllDirectories, string pattern = "*.xml")
        {
            Dictionary<FileInfo, string> result = new Dictionary<FileInfo, string>();
            foreach (var f in getAllDirectories(type))
            {
                foreach (FileInfo file in f.Key.EnumerateFiles(pattern, option)) {
                    result.Add(file, f.Value);
                }
            }
            return result;
        }

        public static FileInfo getFileName(string name, string source, string type, string extension = ".xml")
        {
            String iname = string.Join("_", name.Split(ConfigManager.InvalidChars));
            return new FileInfo(Path.Combine(getDirectory(source, type).FullName, iname + extension));
        }

        public static IEnumerable<string> EnumerateCategories(string type)
        {
            HashSet<string> result = new HashSet<string>();
            foreach (var f in getAllDirectories(type))
            {
                Uri source = new Uri(f.Key.FullName);
                FeatureCollection.ImportAll();
                var cats = f.Key.EnumerateDirectories("*", SearchOption.AllDirectories);
                foreach (DirectoryInfo d in cats) result.Add(cleanname(Uri.UnescapeDataString(source.MakeRelativeUri(new Uri(d.FullName)).ToString()), type));
            }
            return from s in result orderby s select s;
        }

        public static string cleanname(string path, string cut)
        {
            string cat = path;
            if (!cat.StartsWith(cut)) cat = Path.Combine(cut, path);
            cat = cat.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return cat;
        }
    }
}
