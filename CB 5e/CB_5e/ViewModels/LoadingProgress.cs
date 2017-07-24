using CB_5e.Services;
using Character_Builder;
using OGL;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.Models
{
    public class LoadingProgress : BaseDataObject
    {
        private static LoadingProgress instance = null;
        public static LoadingProgress Instance
        {
            get
            {
                if (instance == null) instance = new LoadingProgress();
                return instance;
            }
        }
        private LoadingProgress() { }
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

        public async Task Load(CancellationToken token = default(CancellationToken))
        {
            double count = 16;
            double cur = 0;
            Text = "Sources";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLSourceManager.InitAsync().ConfigureAwait(false);
            DependencyService.Get<Views.IHTMLService>().Reset();

            Text = "Config";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            ConfigManager config = await PCLImport.LoadConfigAsync(await PCLSourceManager.Data.GetFileAsync("Config.xml").ConfigureAwait(false)).ConfigureAwait(false);

            Text = "Levels";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.LoadLevelAsync(await PCLSourceManager.Data.GetFileAsync(config.Levels).ConfigureAwait(false)).ConfigureAwait(false);

            Text = "AbilityScores";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.LoadAbilityScoresAsync(await PCLSourceManager.Data.GetFileAsync(config.AbilityScores).ConfigureAwait(false)).ConfigureAwait(false);

            Text = "Skills";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportSkillsAsync().ConfigureAwait(true);

            Text = "Languages";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportLanguagesAsync().ConfigureAwait(true);

            Text = "Spells";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportSpellsAsync().ConfigureAwait(true);

            Text = "Items";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportItemsAsync().ConfigureAwait(true);

            Text = "Backgrounds";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportBackgroundsAsync().ConfigureAwait(true);

            Text = "Races";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportRacesAsync().ConfigureAwait(true);

            Text = "Subraces";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportSubRacesAsync().ConfigureAwait(true);

            Text = "Feats and Features";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportStandaloneFeaturesAsync().ConfigureAwait(true);

            Text = "Conditions";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportConditionsAsync().ConfigureAwait(true);

            Text = "Magic Properties";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportMagicAsync().ConfigureAwait(true);

            Text = "Classes";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportClassesAsync(true).ConfigureAwait(true);

            Text = "Subclasses";
            Percentage = cur++ / count;
            token.ThrowIfCancellationRequested();
            await PCLImport.ImportSubClassesAsync(true).ConfigureAwait(true);

            Text = "UI";
            Percentage = 1;
        }
    }
}
