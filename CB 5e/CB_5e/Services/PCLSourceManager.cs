using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLStorage;
using OGL;

namespace CB_5e.Services
{
    public class PCLSourceManager
    {
        public static IFolder Data;
        private static IList<IFolder> Sources;
        public static async Task<bool> InitAsync(bool skipInsteadOfExit = true)
        {
            Data = await App.Storage.CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists);
            Sources = await Data.GetFoldersAsync();
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
            if (!path.Contains("/"))
            {
                return "";
            }
            return path.Substring(0, path.LastIndexOf(PortablePath.DirectorySeparatorChar));
        }
    }
}
