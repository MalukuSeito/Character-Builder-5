using CB5e.Pages;
using Character_Builder;
using OGL;
using OGL.Items;
using PluginDMG;

namespace CB5e.Services
{
	public class ContextService
    {
        private readonly SourceService sourceService;
        public BuilderContext Context { get; private set; } = new BuilderContext() { Player = null };
        public int Loaded { get; private set; } = 0;
        public int Steps { get; private set; } = 1;
        public string? Loading = null;
        public int Level { get; set; } = 0;

        public event Func<ChangeType, Task>? PlayerChange;

		public event Func<JournalEntry?, Possession?, Task>? ShopEvent;

		public event Action? LoadEvent;

		private bool SourcesChanged = false;

		public async Task NotifyPlayerChange(ChangeType type)
        {
			if (SourcesChanged)
			{
				Console.WriteLine("Sources Changed, Reloading");
				ClearContext();
				await Load(Context);
				SourcesChanged = false;
			}
			if (type.HasAnyFlag(ChangeType.Full, ChangeType.Journal))
			{
				Level = Context.Player.GetLevel();
			}
			if (PlayerChange != null)
            {
                await PlayerChange.Invoke(type);
            }

        }

        private void ClearContext()
        {
			Context.Backgrounds.Clear();
			Context.BackgroundsSimple.Clear();
			Context.Classes.Clear();
			Context.ClassesSimple.Clear();
			Context.Conditions.Clear();
			Context.ConditionsSimple.Clear();
			Context.FeatureCollections.Clear();
			Context.FeatureContainers.Clear();
			Context.FeatureCategories.Clear();
			Context.Boons.Clear();
			Context.Features.Clear();
			Context.BoonsSimple.Clear();
			Context.Items.Clear();
			Context.ItemLists.Clear();
			Context.ItemsSimple.Clear();
			Context.Languages.Clear();
			Context.LanguagesSimple.Clear();
			Context.Magic.Clear();
			Context.MagicCategories.Clear();
			Context.MagicCategories.Add("Magic", new MagicCategory("Magic", "Magic", 0));
			Context.MagicSimple.Clear();
			Context.Monsters.Clear();
			Context.MonstersSimple.Clear();
			Context.Races.Clear();
			Context.RacesSimple.Clear();
			Context.Skills.Clear();
			Context.SkillsSimple.Clear();
			Context.Spells.Clear();
			Context.SpellLists.Clear();
			Context.SpellsSimple.Clear();
			Context.SubClasses.Clear();
			Context.SubClassesSimple.Clear();
			Context.SubRaces.Clear();
			Context.SubRacesSimple.Clear();
		}

		public async Task OpenShop(JournalEntry? Entry, Possession? Possession)
		{
			if (ShopEvent != null)
			{
				await ShopEvent.Invoke(Entry, Possession);
			}
		}

        private async Task Load(BuilderContext context, CancellationToken token = default)
        {
			Loaded = 0;
			Loading = "Config";
			var sources = sourceService.Sources.AsReadOnly().Reverse().DistinctBy(s => s.Name).ToList();
			Steps = sources.Count;
			foreach (var source in sources)
			{
                if (context.ExcludedSources.Contains(source.Name, StringComparer.OrdinalIgnoreCase)) continue;
				Loading = source.Name;
				token.ThrowIfCancellationRequested();
				await Load(source, context, token);
				await Task.Delay(1);
				if (LoadEvent is not null) LoadEvent.Invoke();
				Loaded++;
			}
			await Task.Delay(1);
            Loading = null;
		}

		public ContextService(SourceService sourceService) { this.sourceService = sourceService; }

		public async Task<BuilderContext> CreateContext(ConfigService config, IEnumerable<string> excludedSources, CancellationToken token = default)
        {
			BuilderContext context = new()
			{
				Config = config.Config,
				Levels = config.Level,
                Scores = config.Scores
			};
			context.ExcludedSources.UnionWith(excludedSources);
            await Load(context, token);
			context.SourcesChangedEvent += Context_SourcesChangedEvent;
            foreach (ClassDefinition c in Context.Classes.Values) c.ApplyKeywords(Context);
            foreach (SubClass c in Context.SubClasses.Values) c.ApplyKeywords(Context);
            PluginManager manager = new();
            manager.Add(new OptionalClassFeatures());
            manager.Add(new SpellPoints());
            manager.Add(new SingleLanguage());
            manager.Add(new PlaneTouchedWings());
            manager.Add(new CustomBackground());
            manager.Add(new BackgroundFeat());
            manager.Add(new NoFreeEquipment());
            manager.Add(new RacialAbilityShift());
            manager.Add(new LanguageChoice());
            manager.Add(new SkillChoice());
            manager.Add(new ToolChoice());
            context.Plugins = manager;
            context.UndoBuffer = new LinkedList<Player>();
            context.RedoBuffer = new LinkedList<Player>();
            context.UnsavedChanges = 0;
            Context = context;
            return context;
        }

		private void Context_SourcesChangedEvent(object? sender, EventArgs e)
		{
			SourcesChanged = true;
		}

		private static async Task Load(ISource source, BuilderContext context, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await source.ImportSkillsAsync(context);

            token.ThrowIfCancellationRequested();
            await source.ImportLanguagesAsync(context);

            token.ThrowIfCancellationRequested();
            await source.ImportSpellsAsync(context);

            token.ThrowIfCancellationRequested();
            await source.ImportItemsAsync(context);

            token.ThrowIfCancellationRequested();
            await source.ImportBackgroundsAsync(context);

            token.ThrowIfCancellationRequested();
            await source.ImportRacesAsync(context);

            token.ThrowIfCancellationRequested();
            await source.ImportSubRacesAsync(context);

            token.ThrowIfCancellationRequested();
            await source.ImportStandaloneFeaturesAsync(context);
            
            token.ThrowIfCancellationRequested();
            await source.ImportConditionsAsync(context);
            
            token.ThrowIfCancellationRequested();
            await source.ImportMagicAsync(context);
            
            token.ThrowIfCancellationRequested();
            await source.ImportClassesAsync(context, false);
            
            token.ThrowIfCancellationRequested();
            await source.ImportSubClassesAsync(context, false);
            
            token.ThrowIfCancellationRequested();
            await source.ImportMonstersAsync(context);

            foreach (var cls in context.Classes.Values) cls.ApplyKeywords(context);
			foreach (var cls in context.SubClasses.Values) cls.ApplyKeywords(context);
		}
    }
}
