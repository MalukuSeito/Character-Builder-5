using CB_5e.Models;
using CB_5e.Services;
using Character_Builder;
using OGL;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class LoadingProgress : BaseDataObject
    {
        public LoadingProgress(BuilderContext context) {
            Context = context;
        }
        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }
        private double percent;
        public double Percentage
        {
            get { return percent; }
            set { SetProperty(ref percent, value); }
        }

        public BuilderContext Context { get; private set; }

        public async Task Load(CancellationToken token = default(CancellationToken))
        {
            double count = 18;
            double cur = 0;
            Text = "Sources";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLSourceManager.InitAsync().ConfigureAwait(false);
            

            Text = "Config";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            ConfigManager config = await Context.LoadConfigAsync(await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false)).ConfigureAwait(false);
            DependencyService.Get<IHTMLService>().Reset(config);

            Text = "Levels";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.LoadLevelAsync(await PCLSourceManager.Data.GetFileAsync(config.Levels).ConfigureAwait(false)).ConfigureAwait(false);

            Text = "AbilityScores";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.LoadAbilityScoresAsync(await PCLSourceManager.Data.GetFileAsync(config.AbilityScores).ConfigureAwait(false)).ConfigureAwait(false);

            Text = "Zip Modules";

            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportZips(false).ConfigureAwait(true);

            Text = "Skills";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportSkillsAsync(false).ConfigureAwait(true);

            Text = "Languages";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportLanguagesAsync(false).ConfigureAwait(true);

            Text = "Spells";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportSpellsAsync(false).ConfigureAwait(true);

            Text = "Items";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportItemsAsync(false).ConfigureAwait(true);

            Text = "Backgrounds";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportBackgroundsAsync(false).ConfigureAwait(true);

            Text = "Races";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportRacesAsync(false).ConfigureAwait(true);

            Text = "Subraces";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportSubRacesAsync(false).ConfigureAwait(true);

            Text = "Feats and Features";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportStandaloneFeaturesAsync(false).ConfigureAwait(true);

            Text = "Conditions";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportConditionsAsync(false).ConfigureAwait(true);

            Text = "Magic Properties";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportMagicAsync(false).ConfigureAwait(true);

            Text = "Classes";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            foreach (ClassDefinition c in Context.Classes.Values) c.ApplyKeywords(Context);
            await Context.ImportClassesAsync(false, true).ConfigureAwait(true);

            Text = "Subclasses";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            foreach (SubClass c in Context.SubClasses.Values) c.ApplyKeywords(Context);
            await Context.ImportSubClassesAsync(false, true).ConfigureAwait(true);

            Text = "Monsters";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await Context.ImportMonstersAsync(false).ConfigureAwait(true);

            Text = "UI";
            Percentage = 1;
        }
    }
}
