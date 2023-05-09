using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OGL;

namespace CB5e.Services
{
    public class ConfigService
    {
        private IWebAssemblyHostEnvironment hostEnvironment;
        private ContextService contextService;
        public ConfigManager? Config { get; private set; }
        public Level? Level { get; private set; }
		public AbilityScores? Scores { get; private set; }

		public ConfigService(IWebAssemblyHostEnvironment hostEnvironment, ContextService contextService)
        {
            this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            this.contextService = contextService ?? throw new ArgumentNullException(nameof(contextService));
            ConfigManager.LogEvents += ConfigManager_LogEvents;
            LoadConfig().Forget();
        }

        private void ConfigManager_LogEvents(object sender, string message, Exception e)
        {
            Console.WriteLine($"{message}: {e?.Message} {e?.StackTrace}");
        }

        private static string MakeRelative(string dir)
        {
            if (dir.Contains(Path.DirectorySeparatorChar) || dir.Contains(Path.AltDirectorySeparatorChar))
            {
                return Path.GetDirectoryName(dir);
            }
            return dir;
        }

        public async Task LoadConfig()
        {
            var http = new HttpClient { BaseAddress = new Uri(hostEnvironment.BaseAddress) };
            using (var con = await http.GetStreamAsync("Config.xml"))
            {
                Config = ConfigManager.Serializer.Deserialize(con) as ConfigManager;
                Config.Items_Directory = MakeRelative(Config.Items_Directory);
                Config.Skills_Directory = MakeRelative(Config.Skills_Directory);
                Config.Languages_Directory = MakeRelative(Config.Languages_Directory);
                Config.Features_Directory = MakeRelative(Config.Features_Directory);
                Config.Backgrounds_Directory = MakeRelative(Config.Backgrounds_Directory);
                Config.Classes_Directory = MakeRelative(Config.Classes_Directory);
                Config.SubClasses_Directory = MakeRelative(Config.SubClasses_Directory);
                Config.Races_Directory = MakeRelative(Config.Races_Directory);
                Config.SubRaces_Directory = MakeRelative(Config.SubRaces_Directory);
                Config.Spells_Directory = MakeRelative(Config.Spells_Directory);
                Config.Magic_Directory = MakeRelative(Config.Magic_Directory);
                Config.Conditions_Directory = MakeRelative(Config.Conditions_Directory);
                Config.Plugins_Directory = MakeRelative(Config.Plugins_Directory);
                Config.Monster_Directory = MakeRelative(Config.Monster_Directory);
                contextService.Context.Config = Config;
            }
            using (var con = await http.GetStreamAsync("Levels.xml"))
            {
                Level = Level.Serializer.Deserialize(con) as Level;
                contextService.Context.Levels = Level;
            }
			using (var con = await http.GetStreamAsync("AbilityScores.xml"))
			{
				Scores = AbilityScores.Serializer.Deserialize(con) as AbilityScores;
				contextService.Context.Scores = Scores;
			}
		}
    }
}
