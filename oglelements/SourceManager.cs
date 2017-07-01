using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace OGL
{
    public class SourceManager
    {
        public static List<string> Sources { get; private set; }
        private static string AppPath = null;
        public static bool init(string path, bool skipInsteadOfExit = false)
        {
            Sources = new List<string>();
            AppPath = path;
            foreach (string s in Directory.EnumerateDirectories(path))
            {
                if (s.Equals(ConfigManager.Directory_Plugins)) continue;
                string f = Path.Combine(s, "LICENSE");
                if (File.Exists(f))
                {
                    if (ConfigManager.LicenseProvider.ShowLicense(Path.GetFileName(s), File.ReadAllLines(f)))
                    {
                        Sources.Add(Path.GetFileName(s));
                        File.Move(f, f + ".txt");
                    } else if (!skipInsteadOfExit) 
                    {
                        return false;
                    }
                } else
                {
                    Sources.Add(Path.GetFileName(s));
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
            try
            {
                foreach (var f in getAllDirectories(type))
                {
                    foreach (FileInfo file in f.Key.EnumerateFiles(pattern, option))
                    {
                        result.Add(file, f.Value);
                    }
                }
            }
            catch (Exception e)
            {
                ConfigManager.LogError(e);
            }
            return result;
        }

        public static FileInfo getFileName(string name, string source, string type, string extension = ".xml")
        {
            String iname = string.Join("_", name.Split(ConfigManager.InvalidChars));
            return new FileInfo(Path.Combine(getDirectory(source, type).FullName, iname + extension));
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
