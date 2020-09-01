using CB_5e.Helpers;
using CB_5e.ViewModels.Character;
using CB_5e.ViewModels.Character.Build;
using CB_5e.ViewModels.Character.Play;
using CB_5e.Views;
using CB_5e.Views.Character;
using CB_5e.Views.Character.Build;
using CB_5e.Views.Character.Play;
using Character_Builder;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public abstract class PlayerModel : BaseViewModel
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        public event EventHandler PlayerChanged;
        public virtual void FirePlayerChanged() => PlayerChanged?.Invoke(this, EventArgs.Empty);
        public BuilderContext Context { get; private set; }
        public INavigation Navigation { get; set; }
        public bool ChildModel { get; set; } = false;

        public PlayerModel(BuilderContext context)
        {
            Context = context;
        }

        public virtual void FirePropertyChanged(string prop)
        {
            OnPropertyChanged(prop);
        }

        public abstract void UpdateSpellcasting();
        public abstract void UpdateForms();

        public Command Undo { get; set; }
        public Command Redo { get; set; }
        public abstract Command RefreshInventory { get; }

        public abstract void MakeHistory(string h = null);
        public abstract void Save();
        public abstract void DoSave();
        public abstract SubModel FirstPage { get; }
        public string Name { get { return Context.Player.Name; } }
        public abstract void MoneyChanged();
        public static ContentPage GetPage(SubModel item)
        {
            if (item is PlayerInfoViewModel pivm) return new PlayerInfoPage(pivm);
            if (item is PlayerSkillViewModel psvm) return new PlayerSkillPage(psvm);
            if (item is PlayerResourcesViewModel prvm) return new PlayerResourcesPage(prvm);
            if (item is PlayerFeaturesViewModel pfvm) return new PlayerFeaturesPage(pfvm);
            if (item is PlayerProficiencyViewModel ppvm) return new PlayerProficiencyPage(ppvm);
            if (item is PlayerActionsViewModel pavm) return new PlayerActionsPage(pavm);
            if (item is PlayerConditionViewModel pcvm) return new PlayerConditionPage(pcvm);
            if (item is PlayerInventoryViewModel ivm) return new PlayerInventoryPage(ivm);
            if (item is PlayerShopViewModel svm) return new ShopPage(svm);
            if (item is PlayerInventoryChoicesViewModel icvm) return new PlayerInventoryOptionsPage(icvm);
            if (item is PlayerJournalViewModel jvm) return new JournalPage(jvm);
            if (item is PlayerNotesViewModel nvm) return new PlayerNotesPage(nvm);
            if (item is PlayerPDFViewModel pvm) return new MorePlayPage(pvm);
            if (item is SpellbookSpellsViewModel ssvm) return new PlayerSpellsPage(ssvm);
            if (item is SpellPrepareViewModel spvm) return new PlayerPreparePage(spvm);
            if (item is SpellChoiceViewModel scvm) return new PlayerPreparePage(scvm);
            if (item is RaceViewModel rvm) return new RacePage(rvm);
            if (item is ClassesViewModel cvm) return new ClassesPage(cvm);
            if (item is BackgroundViewModel bvm) return new BackgroundPage(bvm);
            if (item is PlayerScoresViewModel psm) return new PlayerScoresPage(psm);
            if (item is PlayerPersonalViewModel ppm) return new PlayerPersonalPage(ppm);
            if (item is SourcesViewModel sm) return new SourcesPage(sm);
            if (item is SwitchToBuildModel sbm) return new FlexPage(new PlayerBuildModel(sbm.Parent));
            if (item is SwitchToPlayModel spm) return new FlexPage(new PlayerViewModel(spm.Parent));
            if (item is FormsCompanionsViewModel fcvm) return new PlayerFormsCompanionsPage(fcvm);
            if (item is PlayerFormsCompanionsViewModel pfcvm) return new PlayerFormsCompanionsOverviewPage(pfcvm);
            return new AboutPage();
        }

        public abstract void ChangedSelectedSpells(string spellcastingID);

        public bool Modified { get; set; }
    }
}
