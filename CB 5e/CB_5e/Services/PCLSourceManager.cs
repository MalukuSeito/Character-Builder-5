using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLStorage;
using OGL;
using ICSharpCode.SharpZipLib.Zip;
using System.Reflection;
using System.IO;

namespace CB_5e.Services
{
    public class PCLSourceManager
    {
        public static IFolder Data;
        public static IList<IFolder> Sources;
        public static IList<IFile> Zips;

        private static async Task Extract(Stream stream, IFolder target, string path)
        {
            IFile file = await GetFile(target, path);
            if (file == null) return;
            using (Stream s = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                stream.CopyTo(s);
            }
        }

        public static async Task<IFile> GetFile(IFolder target, string path, CreationCollisionOption options = CreationCollisionOption.GenerateUniqueName)
        {
            if (path == null || path == "" || path.StartsWith(".") || path.EndsWith("/")) return null;
            int i = path.IndexOf('/');
            if (i >= 0)
            {
                string folder = path.Substring(0, i);
                if (folder != "" && !folder.StartsWith(".") && !folder.StartsWith("/"))
                {
                    return await GetFile(await target.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists), path.Substring(i + 1), options);
                }
                return null;
            }
            else
            {
                return await target.CreateFileAsync(path, options);
            }
        }

        public static async Task<IFolder> GetFolder(IFolder target, string path, CreationCollisionOption options = CreationCollisionOption.GenerateUniqueName)
        {
            if (path == null || path == "" || path.StartsWith(".")) return null;
            if (path.EndsWith("/")) path = path.Substring(0, path.Length - 1);
            int i = path.IndexOf('/');
            if (i >= 0)
            {
                string folder = path.Substring(0, i);
                if (folder != "" && !folder.StartsWith(".") && !folder.StartsWith("/"))
                {
                    return await GetFolder(await target.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists), path.Substring(i + 1), options);
                }
                return null;
            }
            else
            {
                return await target.CreateFolderAsync(path, options);
            }
        }

        public static async Task<bool> InitAsync(bool skipInsteadOfExit = true)
        {
            ExistenceCheckResult res = await App.Storage.CheckExistsAsync("Data");
            Data = await App.Storage.CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists);
            if (res == ExistenceCheckResult.NotFound)
            {
                var assembly = typeof(PCLSourceManager).GetTypeInfo().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream("CB_5e.Data.zip"))
                {
                    using (ZipFile zf = new ZipFile(stream))
                    {
                        zf.IsStreamOwner = true;
                        foreach (ZipEntry entry in zf)
                        {
                            if (!entry.IsFile) continue;

                            using (Stream ss = zf.GetInputStream(entry)) await Extract(ss, Data, entry.Name);
                        }
                    }
                }
            }
            Sources = await Data.GetFoldersAsync();
            Zips = (await Data.GetFilesAsync()).Where(f => f.Name.ToLowerInvariant().EndsWith(".zip")).ToList();
            return true;
        }

        public static async Task<Dictionary<IFolder, string>> GetAllDirectoriesAsync(OGLContext context, string type)
        {
            Dictionary<IFolder, string> result = new Dictionary<IFolder, string>();
            foreach (IFolder s in Sources)
            {
                if (context.ExcludedSources.Contains(s.Name, StringComparer.OrdinalIgnoreCase)) continue;
                ExistenceCheckResult res = await s.CheckExistsAsync(type);
                if (res == ExistenceCheckResult.FolderExists) result.Add(await s.GetFolderAsync(type), s.Name);
            }
            return result;
        }

        public static async Task<Dictionary<IFile, string>> EnumerateFilesAsync(OGLContext context, string type, bool recurse = false, string pattern = ".xml")
        {
            Dictionary<IFile, string> result = new Dictionary<IFile, string>();
            try
            {
                foreach (var f in await GetAllDirectoriesAsync(context, type))
                {
                    foreach (IFile file in await GetAllFilesAsync(f.Key, recurse, pattern)) result.Add(file, f.Value);
                }
            }
            catch (Exception e)
            {
                ConfigManager.LogError(e);
            }
            return result;
        }

        public static async Task<Dictionary<string, ZipEntry>> EnumerateZipFilesAsync(ZipFile zf, String source, string type)
        {
            Dictionary<string, ZipEntry> result = new Dictionary<string, ZipEntry>();
            string t = type.TrimEnd('/', '\\').ToLowerInvariant() + "/";
            string tt = type.TrimEnd('/', '\\').ToLowerInvariant() + "\\";
            string f = source.ToLowerInvariant() + "/" + t;
            string ff = source.ToLowerInvariant() + "\\" + tt;
            String basepath = Data.Path;
            String basesource = Sources.Select(ss=>ss.Name).FirstOrDefault(ss => StringComparer.OrdinalIgnoreCase.Equals(ss, source));
            bool overridden = basesource != null;
            foreach (ZipEntry entry in zf)
            {
                if (!entry.IsFile) continue;
                string name = entry.Name.ToLowerInvariant();
                if ((name.StartsWith(f) || name.StartsWith(ff)) && name.EndsWith(".xml"))
                {
                    String path = Path.Combine(basepath, name);
                    if (overridden && (await FileSystem.Current.GetFileFromPathAsync(path)) != null) continue;
                    result.Add(path, entry);
                }
                else if ((name.StartsWith(t) || name.StartsWith(tt)) && name.EndsWith(".xml"))
                {
                    String path = Path.Combine(basepath, basesource, name);
                    if (overridden && (await FileSystem.Current.GetFileFromPathAsync(path)) != null) continue;
                    result.Add(path, entry);
                }
            }
            return result;
        }

        private static async Task<List<IFile>> GetAllFilesAsync(IFolder folder, bool recurse, string pattern)
        {
            List<IFile> result = new List<IFile>();
            foreach (IFile file in await folder.GetFilesAsync())
            {
                if (file.Name.EndsWith(pattern, StringComparison.OrdinalIgnoreCase)) result.Add(file);
            }
            if (recurse) foreach (IFolder f in await folder.GetFoldersAsync()) result.AddRange(await GetAllFilesAsync(f, recurse, pattern));
            return result;
        }

        public static string GetDirectory(string source, string type)
        {
            return PortablePath.Combine(Data.Path, source, type);
        }

        public static string Parent(IFile file)
        {
            return Parent(file.Path);
        }

        public static string Parent(string path)
        {
            if (!path.Contains(""+ PortablePath.DirectorySeparatorChar))
            {
                return "";
            }
            return path.Substring(0, path.LastIndexOf(PortablePath.DirectorySeparatorChar));
        }

        public static List<String> AllSources()
        {
            List<String> res = new List<string>(Sources.Count + Zips.Count);
            res.AddRange(Sources.Select(f => f.Name));
            res.AddRange(Zips.Select(z => Path.ChangeExtension(z.Name, null)));
            return res;
        }
    }
}
