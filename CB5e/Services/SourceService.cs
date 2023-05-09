using BlazorDB;
using Character_Builder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OGL;
using OGL.Features;
using OGL.Items;
using System.IO.Compression;

namespace CB5e.Services
{
    public class SourceService
    {
        public static readonly string Database = "StorageTest";
		public static readonly string Storage = "Test2";

		public List<ISource> Sources { get; private set; } = new List<ISource>();
        public SourceService (IWebAssemblyHostEnvironment hostEnvironment)
        {
            //Sources.Add(new UrlSource("Systems Reference Document v5.1", hostEnvironment.BaseAddress + "Systems%20Reference%20Document%20v5.1.zip"));
        }

        public async Task FindAsync(IBlazorDbFactory dbFactory)
        {
            var db = await dbFactory.GetDbManager(Database);
            db.ActionCompleted += (sender, e) =>
            {
                if (e.Failed) Console.WriteLine(e.Message);
            };
            var list = await db.ToArray<SourcePreview>(Storage);
            if (list is not null)
            {
                foreach (SourcePreview entry in list)
                {
                    Sources.Add(new CachedSource(dbFactory, entry.Name, null));
                }
            } else
            {
            }
        }

    }

    public interface ISource
    {
        string Name { get; }
        string Type { get; }
        string Status { get; }
        Task ImportClassesAsync(OGLContext context, bool applyKeywords = false);
        Task ImportSubClassesAsync(OGLContext context, bool applyKeywords = false);
        Task ImportSkillsAsync(OGLContext context);
        Task ImportLanguagesAsync(OGLContext context);
        Task ImportSpellsAsync(OGLContext context);
        Task ImportItemsAsync(OGLContext context);
        Task ImportBackgroundsAsync(OGLContext context);
        Task ImportRacesAsync(OGLContext context);
        Task ImportSubRacesAsync(OGLContext context);
        Task ImportStandaloneFeaturesAsync(OGLContext context);
        Task ImportConditionsAsync(OGLContext context);
        Task ImportMagicAsync(OGLContext context);
        Task ImportMonstersAsync(OGLContext context);
    }


	public class SourcePreview
	{
		public string Name { get; set; }
	}
	public class SourceEntry
    {
        public string Name { get; set; }
        public OGLContext Context { get; set; }
    }

    public class CachedSource : ISource
    {
        public string Name { get; private set; }

        public string Type => "Cache";

        public string Status => cached == null ? "Not Loaded" : "Loaded";

        private OGLContext? cached = null;
        private IBlazorDbFactory dbFactory;

        public CachedSource(IBlazorDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public CachedSource(IBlazorDbFactory dbFactory, string name, OGLContext cached)
        {
            this.dbFactory = dbFactory;
            Name = name;
            this.cached = cached;
        }
        public async Task Delete(IBlazorDbFactory dbFactory)
        {
			var db = await dbFactory.GetDbManager(SourceService.Database);
			var exist = await db.GetRecordByIdAsync<string, SourceEntry>(SourceService.Storage, Name);
			if (exist != null) await db.DeleteRecordAsync(SourceService.Storage, Name);
		}

		public async Task Import(ISource other, ConfigManager config)
        {
            Name = other.Name;
            cached = new BuilderContext
            {
                Config = config
            };
            await other.ImportSkillsAsync(cached);
            await other.ImportLanguagesAsync(cached);
            await other.ImportSpellsAsync(cached);
            await other.ImportItemsAsync(cached);
            await other.ImportBackgroundsAsync(cached);
            await other.ImportRacesAsync(cached);
            await other.ImportSubRacesAsync(cached);
            await other.ImportStandaloneFeaturesAsync(cached);
            await other.ImportConditionsAsync(cached);
            await other.ImportMagicAsync(cached);
            await other.ImportClassesAsync(cached, false);
            await other.ImportSubClassesAsync(cached, false);
            await other.ImportMonstersAsync(cached);
            cached.Config = null;
            var db = await dbFactory.GetDbManager(SourceService.Database);
            var exist = await db.GetRecordByIdAsync<string, SourceEntry>(SourceService.Storage, Name);
            if (exist != null) await db.DeleteRecordAsync(SourceService.Storage, Name);
            await db.AddRecordAsync(new StoreRecord<SourceEntry>()
            {
                StoreName = SourceService.Storage,
                Record = new SourceEntry() { Name = Name, Context = cached }

            });
        }

        private async Task<OGLContext?> GetCachedContext()
        {
            if (cached == null)
            {
                var db = await dbFactory.GetDbManager(SourceService.Database);
                SourceEntry? record = await db.GetRecordByIdAsync<string, SourceEntry>(SourceService.Storage, Name);
                if (record is not null) cached = record.Context;
            }
            return cached;
        }

        public async Task ImportBackgroundsAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null) 
            {
				foreach (var b in other.Backgrounds.Values) b.Register(context, b.FileName);
            }
        }

        public async Task ImportClassesAsync(OGLContext context, bool applyKeywords = false)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.Classes.Values) b.Register(context, b.FileName, applyKeywords);
            }
            
        }

        public async Task ImportConditionsAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.Conditions.Values) b.Register(context, b.FileName);
            }
        }

        public async Task ImportItemsAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.Items.Values)
                {
					
					if (b.Category != null && !Category.Categories.ContainsKey(b.Category.Path))
                    {
                        Category.Categories.Add(b.Category.Path, new Category(b.Category.Path, b.Category.CategoryPath, context));
                    }
                    b.Register(context, b.FileName);
                }
            }
        }

        public async Task ImportLanguagesAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.Languages.Values) b.Register(context, b.FileName);
            }
        }

        public async Task ImportMagicAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.MagicCategories.Values) if (!context.MagicCategories.ContainsKey(b.Name)) context.MagicCategories.Add(b.Name, new MagicCategory(b.Name, b.DisplayName, b.Indent));
                foreach (var mp in other.Magic.Values)
                {
                    string cat = mp.Category;
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
            }
        }

        public async Task ImportMonstersAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.Monsters.Values) b.Register(context, b.FileName);
            }
        }

        public async Task ImportRacesAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.Races.Values) b.Register(context, b.FileName);
            }
        }

        public async Task ImportSkillsAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.Skills.Values) b.Register(context, b.FileName);
            }
        }

        public async Task ImportSpellsAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.Spells.Values) b.Register(context, b.FileName);
            }
        }

        public async Task ImportStandaloneFeaturesAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.FeatureCategories)
                {
                    if (!context.FeatureCategories.ContainsKey(b.Key)) context.FeatureCategories.Add(b.Key, new List<Feature>(b.Value));
                    else context.FeatureCategories[b.Key].AddRange(b.Value);
                }
                foreach (var b in other.FeatureContainers)
                {
                    if (!context.FeatureContainers.ContainsKey(b.Key)) context.FeatureContainers.Add(b.Key, new List<FeatureContainer>(b.Value));
                    else context.FeatureContainers[b.Key].AddRange(b.Value);
                }
                foreach (var b in other.Boons.Values)
                {
                    if (context.BoonsSimple.ContainsKey(b.Name))
                    {
                        context.BoonsSimple[b.Name].ShowSource = true;
                        b.ShowSource = true;
                    }
                    else context.BoonsSimple.Add(b.Name, b);
                    if (context.Boons.ContainsKey(b.Name + " " + ConfigManager.SourceSeperator + " " + b.Source)) ConfigManager.LogError("Duplicate Boon: " + b.Name + " " + ConfigManager.SourceSeperator + " " + b.Source);
                    else context.Boons[b.Name + " " + ConfigManager.SourceSeperator + " " + b.Source] = b;
                }
                foreach (var b in other.Features) context.Features.Add(b);
            }
        }

        public async Task ImportSubClassesAsync(OGLContext context, bool applyKeywords = false)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.SubClasses.Values) b.Register(context, b.FileName, applyKeywords);
            }
        }

        public async Task ImportSubRacesAsync(OGLContext context)
        {
            var other = await GetCachedContext();
            if (other != null)
            {
				foreach (var b in other.SubRaces.Values) b.Register(context, b.FileName);
            }
        }
    }

    public class UrlSource: LocalSource
    {
        public string Url { get; private set; }
        public override string Type { get => $"Url ({Url})"; }
        public override string Status { get => _error ?? (_data == null ? "Unloaded" : "Loaded"); }
        private string? _error = null;
        public UrlSource(string name, string url) : base(name, null)
        {
            Url = url;
        }
        public override async Task<byte[]?> getData()
        {
            try
            {
                if (_data is null)
                {
                    _data = await new HttpClient().GetByteArrayAsync(Url);
                }
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }
            return _data;
        }
    }

    public class LocalSource : ISource
    {
        public string Name { get; private set; }
        public virtual string Type { get => "Imported File"; }
        public virtual string Status { get => "Imported"; }
        protected byte[]? _data;

        public LocalSource(string name, byte[]? data)
        {
            Name = name;
            _data = data;
        }

        public static Dictionary<string, ZipArchiveEntry> EnumerateZipFilesAsync(ZipArchive zf, string source, string type)
        {
            Dictionary<string, ZipArchiveEntry> result = new Dictionary<string, ZipArchiveEntry>();
            string t = type.TrimEnd('/', '\\').ToLowerInvariant() + "/";
            string tt = type.TrimEnd('/', '\\').ToLowerInvariant() + "\\";
            string f = source.ToLowerInvariant() + "/" + t;
            string ff = source.ToLowerInvariant() + "\\" + tt;
            foreach (ZipArchiveEntry entry in zf.Entries)
            {
                string name = entry.FullName.ToLowerInvariant();
                if ((name.StartsWith(f) || name.StartsWith(ff)) && name.EndsWith(".xml"))
                {
                    result.Add(name, entry);
                }
                else if ((name.StartsWith(t) || name.StartsWith(tt)) && name.EndsWith(".xml"))
                {
                    result.Add(name, entry);
                }
            }
            return result;
        }
        public Task ImportSkillsAsync(OGLContext context) => Import(context.Config.Skills_Directory, (stream, path) => OGLImport.ImportSkill(stream, "Base/"+path, Name, context));
        public Task ImportLanguagesAsync(OGLContext context) => Import(context.Config.Languages_Directory, (stream, path) => OGLImport.ImportLanguage(stream, "Base/" + path, Name, context));
        public Task ImportSpellsAsync(OGLContext context) => Import(context.Config.Spells_Directory, (stream, path) => OGLImport.ImportSpell(stream, "Base/" + path, Name, context));
        public Task ImportItemsAsync(OGLContext context) => Import(context.Config.Items_Directory, (stream, path) => OGLImport.ImportItem(stream, "Base/" + path, Name, context, GetPath("Base/" + path, "Base", Name)));
        public Task ImportMagicAsync(OGLContext context) => Import(context.Config.Magic_Directory, (stream, path) => OGLImport.ImportMagicItem(stream, "Base/" + path, Name, context, GetPath("Base/" + path, "Base", Name)));
        public Task ImportBackgroundsAsync(OGLContext context) => Import(context.Config.Backgrounds_Directory, (stream, path) => OGLImport.ImportBackground(stream, "Base/" + path, Name, context));
        public Task ImportRacesAsync(OGLContext context) => Import(context.Config.Races_Directory, (stream, path) => OGLImport.ImportRace(stream, "Base/" + path, Name, context));
        public Task ImportSubRacesAsync(OGLContext context) => Import(context.Config.SubRaces_Directory, (stream, path) => OGLImport.ImportSubRace(stream, "Base/" + path, Name, context));
        public Task ImportConditionsAsync(OGLContext context) => Import(context.Config.Conditions_Directory, (stream, path) => OGLImport.ImportCondition(stream, "Base/" + path, Name, context));
        public Task ImportStandaloneFeaturesAsync(OGLContext context) => Import(context.Config.Features_Directory, (stream, path) => OGLImport.ImportFeatureContainer(stream, "Base/" + path, Name, context, GetPath("Base/" + path, "Base", Name)));
        public Task ImportClassesAsync(OGLContext context, bool applyKeywords = false) => Import(context.Config.Classes_Directory, (stream, path) => OGLImport.ImportClass(stream, "Base/" + path, Name, context, applyKeywords));
        public Task ImportSubClassesAsync(OGLContext context, bool applyKeywords = false) => Import(context.Config.SubClasses_Directory, (stream, path) => OGLImport.ImportSubClass(stream, "Base/" + path, Name, context, applyKeywords));
        public Task ImportMonstersAsync(OGLContext context) => Import(context.Config.Monster_Directory, (stream, path) => OGLImport.ImportMonster(stream, "Base/" + path, Name, context));
        private async Task Import(string type, Action<Stream, string> import)
        {
            byte[]? data = await getData();
            if (data is null) return;
            using MemoryStream ms = new(data);
            ZipArchive archive = new ZipArchive(ms);
            var zfiles = EnumerateZipFilesAsync(archive, Name, type);
            foreach (var f in zfiles)
            {
                try
                {
                    using Stream reader = f.Value.Open();
                    import(reader, f.Key);
                }
                catch (Exception e)
                {
                    ConfigManager.LogError($"Error reading {f.Key} in {Name}", e);
                }
            }
        }
        public virtual Task<byte[]?> getData()
        {
            return Task.FromResult(_data);
        }

        public static IEnumerable<string> GetPath(string fullpath, string basepath, string source)
        {
            var full = fullpath.Split('/', '\\').ToList();
            var basep = string.IsNullOrEmpty(basepath) ? new List<string>() : basepath.Split('/', '\\').ToList();
            int i;
            for (i = 0; i < basep.Count; i++)
            {
                if (!StringComparer.OrdinalIgnoreCase.Equals(full[0], basep[0]))
                {
                    throw new Exception("Unmatched paths: " + fullpath + " vs. " + basepath);
                }
            }
            if (full.Count > i + 2 && StringComparer.OrdinalIgnoreCase.Equals(source, full[i])) i++;
            //else throw new Exception("Source: " + source + " not found in " + fullpath);
            return full.Skip(i + 1);
        }
    }
}
