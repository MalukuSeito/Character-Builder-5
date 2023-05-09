using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using Character_Builder_Forms;
using System.IO.Compression;

namespace OGL
{
    public class SourceManager
    {
        public static List<string> Sources { get; private set; }
        public static string AppPath = null;
        public static bool Init(OGLContext context, string path, bool skipInsteadOfExit = false)
        {
            Sources = new List<string>();
            AppPath = path;
            if (AppPath.EndsWith("/") || AppPath.EndsWith("\\"))
            {
                AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            ConfigManager.InvalidChars = (new string(Path.GetInvalidFileNameChars()) + ConfigManager.SourceSeperator).ToCharArray();
            foreach (string s in Directory.EnumerateDirectories(path))
            {
                if (s.Equals(context.Config.Plugins_Directory)) continue;
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
            foreach (string s in Directory.EnumerateFiles(path, "*.zip"))
            {
                string n = Path.GetFileName(s).Substring(0, Path.GetFileName(s).Length - 4);
                if (!Sources.Contains(n)) Sources.Add(n);
            }
            return true;
        }

        public static DirectoryInfo GetDirectory(string source, string type) {
            if (source == null || source == "") source = "No Source";
            String isource = string.Join("_", source.Split(Path.GetInvalidFileNameChars()));
            if (!Sources.Contains(isource)) Sources.Add(isource);
            DirectoryInfo res = new DirectoryInfo(Path.Combine(AppPath, isource, type));
            return res;
        }

        public static Dictionary<DirectoryInfo, string> GetAllDirectories(OGLContext context, string type, bool withZips = false)
        {
            Dictionary<DirectoryInfo, string> result = new Dictionary<DirectoryInfo, string>();
            foreach (string s in Sources)
            {
                if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                DirectoryInfo res = new DirectoryInfo(Path.Combine(AppPath, s, type));
                if (res.Exists) result.Add(res, s);
            }
            if (withZips) {
                foreach (KeyValuePair<FileInfo, string> zip in SourceManager.GetAllZips(context).AsEnumerable())
                {
                    ZipArchive archive = ZipFile.OpenRead(zip.Key.FullName);
                    string f = zip.Value.ToLowerInvariant() + "/";
                    string ff = zip.Value.ToLowerInvariant() + "\\";
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string name = entry.FullName.ToLowerInvariant();
                        if ((name.StartsWith(f) || name.StartsWith(ff)) && name.EndsWith(".xml"))
                        {
                            string path = Path.Combine(AppPath, entry.FullName);
                            DirectoryInfo res = new DirectoryInfo(Path.GetDirectoryName(path));
                            if (!result.ContainsKey(res)) result.Add(res, zip.Value);
                        }
                        else if (name.EndsWith(".xml"))
                        {
                            string path = Path.Combine(AppPath, zip.Value, entry.FullName);
                            DirectoryInfo res = new DirectoryInfo(Path.GetDirectoryName(path));
                            if (!result.ContainsKey(res)) result.Add(res, zip.Value);
                        } 
                    }
                }
            }
            return result;
        }

        public static Dictionary<FileInfo, string> GetAllZips(OGLContext context)
        {
            Dictionary<FileInfo, string> result = new Dictionary<FileInfo, string>();
            foreach (string s in Sources)
            {
                if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                FileInfo res = new FileInfo(Path.Combine(AppPath, s + ".zip"));
                if (res.Exists) {
                    result.Add(res, s);
                }
            }
            return result;
        }

        public static Dictionary<ZipArchive, string> GetAllZips(OGLContext context, string type)
        {
            Dictionary<ZipArchive, string> result = new Dictionary<ZipArchive, string>();
            string t = type.TrimEnd('/', '\\').ToLowerInvariant() + "/";
            string tt = type.TrimEnd('/', '\\').ToLowerInvariant() + "\\";
            foreach (string s in Sources)
            {
                if (context.ExcludedSources.Contains(s, StringComparer.OrdinalIgnoreCase)) continue;
                FileInfo res = new FileInfo(Path.Combine(AppPath, s + ".zip"));
                if (res.Exists) {
                    string f = s.ToLowerInvariant() + "/" + t;
                    string ff = s.ToLowerInvariant() + "\\" + tt;
                    try
                    {
                        ZipArchive archive = ZipFile.OpenRead(res.FullName);
                        foreach (var e in archive.Entries)
                        {
                            string name = e.FullName.ToLowerInvariant();
                            if (name.StartsWith(t) || name.StartsWith(f) || name.StartsWith(tt) || name.StartsWith(ff))
                            {
                                result.Add(archive, s);
                                break;
                            }
                        }
                    } catch (Exception e)
                    {
                        ConfigManager.LogError(e);
                    }
                }
            }
            return result;
        }

        public static Dictionary<string, FileInfoSource> EnumerateFiles(OGLContext context, string type, bool withZips, SearchOption option = SearchOption.AllDirectories, string pattern = "*.xml")
        {
            Dictionary<string, FileInfoSource> result = new Dictionary<string, FileInfoSource>(StringComparer.InvariantCultureIgnoreCase);
            if (withZips)
            {
                try
                {
                string t = type.TrimEnd('/', '\\').ToLowerInvariant() + "/";
                string tt = type.TrimEnd('/', '\\').ToLowerInvariant() + "\\";
                string p = pattern.StartsWith("*") ? pattern.Substring(1, pattern.Length - 1).ToLowerInvariant() : pattern.ToLowerInvariant();
                    foreach (var z in GetAllZips(context, type))
                    {
                        string s = z.Value;
                        foreach (ZipArchiveEntry e in z.Key.Entries)
                        {
                            string f = s.ToLowerInvariant() + "/" + t;
                            string ff = s.ToLowerInvariant() + "\\" + tt;
                            string name = e.FullName.ToLowerInvariant();
                            if (name.StartsWith(t) && name.EndsWith(p)) {
                                if (option == SearchOption.AllDirectories || !name.Substring(t.Length).Contains("/"))
                                {
                                    FileInfoSource fis = new FileInfoSource()
                                    {
                                        Archive = e,
                                        FullName = Path.Combine(AppPath, s, type, e.FullName.Substring(t.Length).Replace('/', Path.DirectorySeparatorChar)),
                                        Source = z.Value
                                    };
                                    if (result.ContainsKey(fis.FullName))
                                    {
                                        ConfigManager.LogError(fis.FullName + "already exists");
                                    }
                                    result.Add(fis.FullName, fis);
                                }
                            }
                            else if (name.StartsWith(f) && name.EndsWith(p))
                            {
                                if (option == SearchOption.AllDirectories || !name.Substring(f.Length).Contains("/"))
                                {
                                    FileInfoSource fis = new FileInfoSource()
                                    {
                                        Archive = e,
                                        FullName = Path.Combine(AppPath, s, type, e.FullName.Substring(f.Length).Replace('/', Path.DirectorySeparatorChar)),
                                        Source = z.Value
                                    };
                                    if (result.ContainsKey(fis.FullName)) {
                                        ConfigManager.LogError(fis.FullName + "already exists");
                                    }
                                    result.Add(fis.FullName, fis);
                                }
                            }
                            else if (name.StartsWith(tt) && name.EndsWith(p))
                            {
                                if (option == SearchOption.AllDirectories || !name.Substring(tt.Length).Contains("\\"))
                                {
                                    FileInfoSource fis = new FileInfoSource()
                                    {
                                        Archive = e,
                                        FullName = Path.Combine(AppPath, s, type, e.FullName.Substring(tt.Length).Replace('\\', Path.DirectorySeparatorChar)),
                                        Source = z.Value
                                    };
                                    if (result.ContainsKey(fis.FullName))
                                    {
                                        ConfigManager.LogError(fis.FullName + "already exists");
                                    }
                                    result.Add(fis.FullName, fis);
                                }
                            }
                            else if (name.StartsWith(ff) && name.EndsWith(p))
                            {
                                if (option == SearchOption.AllDirectories || !name.Substring(ff.Length).Contains("/"))
                                {
                                    FileInfoSource fis = new FileInfoSource()
                                    {
                                        Archive = e,
                                        FullName = Path.Combine(AppPath, s, type, e.FullName.Substring(ff.Length).Replace('/', Path.DirectorySeparatorChar)),
                                        Source = z.Value
                                    };
                                    if (result.ContainsKey(fis.FullName))
                                    {
                                        ConfigManager.LogError(fis.FullName + "already exists");
                                    }
                                    result.Add(fis.FullName, fis);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError(e);
                }
            }
            try
            {
                foreach (var f in GetAllDirectories(context, type))
                {
                    foreach (FileInfo file in f.Key.EnumerateFiles(pattern, option))
                    {
                        FileInfoSource fis = new FileInfoSource()
                        {
                            File = file,
                            FullName = file.FullName,
                            Source = f.Value
                        };
                        result[fis.FullName] = fis;
                    }
                }
            }
            catch (Exception e)
            {
                ConfigManager.LogError(e);
            }
            return result;
        }

        public static FileInfo GetFileName(string name, string source, string type, string extension = ".xml")
        {
            String iname = string.Join("_", name.Split(ConfigManager.InvalidChars));
            DirectoryInfo r = GetDirectory(source, type);
            Directory.CreateDirectory(r.FullName);
            return new FileInfo(Path.Combine(r.FullName, iname + extension));
        }

        public static string Cleanname(string path, string cut)
        {
            string cat = path;
            if (!cat.StartsWith(cut)) cat = Path.Combine(cut, path);
            cat = cat.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return cat;
        }
    }
}
