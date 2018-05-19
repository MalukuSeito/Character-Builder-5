using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL;
using System.Reflection;
using System.IO;
using System.IO.Compression;

namespace CB_5e.Services
{
    public class PCLSourceManager
    {
        public static DirectoryInfo Data;
        public static IList<DirectoryInfo> Sources;

        //private static async Task Extract(Stream stream, DirectoryInfo target, string path)
        //{
        //    FileInfo file = await GetFile(target, path);
        //    if (file == null) return;
        //    using (Stream s = await file.OpenAsync(FileAccess.ReadAndWrite))
        //    {
        //        stream.CopyTo(s);
        //    }
        //}

        //public static async Task<FileInfo> GetFile(DirectoryInfo target, string path, CreationCollisionOption options = CreationCollisionOption.GenerateUniqueName)
        //{
        //    if (path == null || path == "" || path.StartsWith(".") || path.EndsWith("/")) return null;
        //    int i = path.IndexOf('/');
        //    if (i >= 0)
        //    {
        //        string folder = path.Substring(0, i);
        //        if (folder != "" && !folder.StartsWith(".") && !folder.StartsWith("/"))
        //        {
        //            return await GetFile(await target.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists), path.Substring(i + 1), options);
        //        }
        //        return null;
        //    }
        //    else
        //    {
        //        return await target.CreateFileAsync(path, options);
        //    }
        //}

        //public static async Task<DirectoryInfo> GetFolder(DirectoryInfo target, string path, CreationCollisionOption options = CreationCollisionOption.GenerateUniqueName)
        //{
        //    if (path == null || path == "" || path.StartsWith(".")) return null;
        //    if (path.EndsWith("/")) path = path.Substring(0, path.Length - 1);
        //    int i = path.IndexOf('/');
        //    if (i >= 0)
        //    {
        //        string folder = path.Substring(0, i);
        //        if (folder != "" && !folder.StartsWith(".") && !folder.StartsWith("/"))
        //        {
        //            return await GetFolder(await target.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists), path.Substring(i + 1), options);
        //        }
        //        return null;
        //    }
        //    else
        //    {
        //        return await target.CreateFolderAsync(path, options);
        //    }
        //}

        public static async Task<bool> InitAsync(bool skipInsteadOfExit = true)
        {
            Data = new DirectoryInfo(Path.Combine(App.Storage.FullName, "Data"));
            if (!Data.Exists)
            {
                Data.Create();
                var assembly = typeof(PCLSourceManager).GetTypeInfo().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream("CB_5e.Common.Data.zip"))
                {
                    using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read, false))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            using (Stream s = entry.Open())
                            {
                                using (Stream o = new FileStream(Path.Combine(Data.FullName, entry.FullName), FileMode.OpenOrCreate))
                                {
                                    await s.CopyToAsync(o);
                                }
                            }
                        }
                    }
                }
            }
            Sources = Data.GetDirectories();
            return true;
        }

        public static async Task<Dictionary<DirectoryInfo, string>> GetAllDirectoriesAsync(OGLContext context, string type)
        {
            Dictionary<DirectoryInfo, string> result = new Dictionary<DirectoryInfo, string>();
            foreach (DirectoryInfo s in Sources)
            {
                if (context.ExcludedSources.Contains(s.Name, StringComparer.OrdinalIgnoreCase)) continue;
                DirectoryInfo sub = new DirectoryInfo(Path.Combine(s.FullName, type));
                await Task.Run(() => {
                    if (sub.Exists) result.Add(sub, s.Name);
                });
                
            }
            return result;
        }

        public static async Task<Dictionary<FileInfo, string>> EnumerateFilesAsync(OGLContext context, string type, bool recurse = false, string pattern = "*.xml")
        {
            Dictionary<FileInfo, string> result = new Dictionary<FileInfo, string>();
            try
            {
                foreach (var f in await GetAllDirectoriesAsync(context, type))
                {
                    foreach (FileInfo file in await Task.Run(() => f.Key.EnumerateFiles(pattern, recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))) result.Add(file, f.Value);
                }
            }
            catch (Exception e)
            {
                ConfigManager.LogError(e);
            }
            return result;
        }

        public static string GetDirectory(string source, string type)
        {
            return Path.Combine(Data.FullName, source, type);
        }

        public static string Parent(FileInfo file)
        {
            return Parent(file.FullName);
        }

        public static string Parent(string path)
        {
            if (!path.Contains("/"))
            {
                return "";
            }
            return path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar));
        }
    }
}
