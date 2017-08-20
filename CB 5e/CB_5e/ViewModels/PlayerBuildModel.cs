using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Character_Builder;
using Xamarin.Forms;
using System.Threading;
using CB_5e.Helpers;
using OGL.Features;
using OGL;
using PCLStorage;
using System.IO;
using OGL.Descriptions;
using OGL.Base;
using CB_5e.Models;
using CB_5e.Views;
using OGL.Spells;

namespace CB_5e.ViewModels
{
    public class PlayerBuildModel : PlayerModel
    {
        public Mutex SaveLock = new Mutex();
        public PlayerModel Parent;
        public PlayerBuildModel(BuilderContext context) : base(context)
        {
            PlayerChanged += PlayerBuildModel_PlayerChanged;
            Context.HistoryButtonChange += Player_HistoryButtonChange;
            Context.SourcesChangedEvent += Context_SourcesChangedEvent;
            Undo = new Command(() =>
            {
                Context.Undo();
                FirePlayerChanged();
                Save();
            }, () =>
            {
                return Context.CanUndo();
            });
            Redo = new Command(() =>
            {
                Context.Redo();
                FirePlayerChanged();
                Save();
            }, () =>
            {
                return Context.CanRedo();
            });
            race = new RaceViewModel(this);
            classes = new ClassesViewModel(this);
            background = new BackgroundViewModel(this);
            playerInventory = new PlayerInventoryViewModel(this);
            playerShops = new PlayerShopViewModel(this);
            playerInventoryChoices = new PlayerInventoryChoicesViewModel(this);
            playerScores = new PlayerScoresViewModel(this);
            playerPersonal = new PlayerPersonalViewModel(this);
            playerJournal = new PlayerJournalViewModel(this);
            playerFeatures = new PlayerFeaturesViewModel(this);
            playerSources = new SourcesViewModel(this);
            playerInfo = new PlayerInfoViewModel(this);
            play = new SwitchToPlayModel(this);
            UpdateSpellcasting();
        }

        public PlayerBuildModel(PlayerModel parent) : base(parent.Context)
        {
            Parent = parent;
            ChildModel = true;
            PlayerChanged += PlayerBuildModel_PlayerChanged;
            Undo = Parent.Undo;
            Redo = Parent.Redo;
            race = new RaceViewModel(this);
            classes = new ClassesViewModel(this);
            background = new BackgroundViewModel(this);
            playerInventory = new PlayerInventoryViewModel(this);
            playerShops = new PlayerShopViewModel(this);
            playerInventoryChoices = new PlayerInventoryChoicesViewModel(this);
            playerScores = new PlayerScoresViewModel(this);
            playerPersonal = new PlayerPersonalViewModel(this);
            playerJournal = new PlayerJournalViewModel(this);
            playerFeatures = new PlayerFeaturesViewModel(this);
            playerSources = new SourcesViewModel(this);
            playerInfo = new PlayerInfoViewModel(this);

            UpdateSpellcasting();
        }

        private async void Context_SourcesChangedEvent(object sender, EventArgs e)
        {
            IsBusy = true;
            LoadingProgress loader = new LoadingProgress(Context);
            LoadingPage l = new LoadingPage(loader, false);
            await Navigation.PushModalAsync(l);
            var t = l.Cancel.Token;
            try
            {
                await loader.Load(t).ConfigureAwait(false);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync(false);
                });
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void PlayerBuildModel_PlayerChanged(object sender, EventArgs e)
        {
            UpdateSpellcasting();
        }

        public override void DoSave()
        {
            if (Parent != null) Parent.DoSave();
            else if (Context.Player != null)
            {
                Modified = true;
                Player p = Context.Player;
                SaveLock.WaitOne();
                try
                {
                    if (p.FilePath == null) p.FilePath = App.Storage.CreateFolderAsync("Characters", CreationCollisionOption.OpenIfExists).Result.CreateFileAsync(p.Name + ".cb5", CreationCollisionOption.GenerateUniqueName).Result;
                        if (p.FilePath is IFile file)
                        {
                            Context.UnsavedChanges = 0;
                            using (Stream s = file.OpenAsync(FileAccess.ReadAndWrite).Result)
                            {
                                Player.Serializer.Serialize(s, p);
                                s.SetLength(s.Position);
                            }
                        }
                }
                catch (Exception e)
                {
                    ConfigManager.LogError("Error Saving", e);
                }
                SaveLock.ReleaseMutex();
            }
        }

        public override void MakeHistory(string h = null)
        {
            if (Parent != null) Parent.MakeHistory(h);
            else Context.MakeHistory(h);
        }

        public override void Save()
        {
            if (Parent != null) Parent.Save();
        }

        private void Player_HistoryButtonChange(object sender, bool CanUndo, bool CanRedo)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Undo.ChangeCanExecute();
                Redo.ChangeCanExecute();
            });
        }

        public ObservableRangeCollection<SpellbookViewModel> Spellcasting { get; set; } = new ObservableRangeCollection<SpellbookViewModel>();
        public override void UpdateSpellcasting()
        {
            List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>(from f in Context.Player.GetFeatures() where f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID != "MULTICLASS" orderby Context.Player.GetClassLevel(((SpellcastingFeature)f).SpellcastingID) descending, ((SpellcastingFeature)f).DisplayName, ((SpellcastingFeature)f).SpellcastingID select f as SpellcastingFeature);
            //if (spellcasts.Count == 0) Spellcasting.ReplaceRange(new List<SpellbookViewModel>() { new SpellbookViewModel(this, null) });
            List<SpellbookViewModel> views = new List<SpellbookViewModel>();
            List<SpellbookViewModel> oldviews = new List<SpellbookViewModel>(Spellcasting);
            foreach (SpellcastingFeature sf in spellcasts)
            {
                List<SpellChoiceFeature> spellfeatures = new List<SpellChoiceFeature>(from f in Context.Player.GetFeatures() where f is SpellChoiceFeature select f as SpellChoiceFeature);
                foreach (var f in spellfeatures)
                {
                    if (f.SpellcastingID == sf.SpellcastingID)
                    {
                        if (Spellcasting.FirstOrDefault(view => view is SpellChoiceViewModel scv  && view.SpellcastingID == sf.SpellcastingID && scv.UniqueID == f.UniqueID) is SpellChoiceViewModel v)
                        {
                            v.Refresh(sf, f);
                            views.Add(v);
                        }
                        else
                        {
                            views.Add(new SpellChoiceViewModel(this, sf, f));
                        }
                    }
                }
                List<ModifiedSpell> bonusprepared = null;
                if (sf.Preparation != PreparationMode.LearnSpells)
                {
                    SpellbookViewModel v2 = Spellcasting.FirstOrDefault(view => view is SpellPrepareViewModel && view.SpellcastingID == sf.SpellcastingID);
                    if (v2 != null)
                    {
                        v2.Refresh(sf);
                        if (v2 is SpellPrepareViewModel spvm && spvm.Able > 0) views.Add(spvm);
                    }
                    else
                    {
                        v2 = new SpellPrepareViewModel(this, sf);
                        if (v2 is SpellPrepareViewModel spvm && spvm.Able > 0) views.Add(spvm);
                    }
                }
                else
                {
                    bonusprepared = Context.Player.GetSpellcasting(sf.SpellcastingID).GetPrepared(Context.Player, Context).ToList();
                    if (bonusprepared.Count > 0 && Spellcasting.FirstOrDefault(view => view is SpellChoiceViewModel scv && view.SpellcastingID == sf.SpellcastingID && scv.Choice == null) is SpellChoiceViewModel v)
                    {
                        v.Refresh(sf, null);
                        v.SetSpells(bonusprepared);
                        views.Add(v);
                    }
                    else if (bonusprepared.Count > 0)
                    {
                        v = new SpellChoiceViewModel(this, sf, null);
                        v.SetSpells(bonusprepared);
                        views.Add(v);
                    }
                }
            }
            if (!views.SequenceEqual(Spellcasting))
            {
                Spellcasting.ReplaceRange(views);
                UpdatePages();
            }
            if (views.Count == 0) UpdatePages();
        }
        public void ChangedPreparedSpells(string id)
        {
            foreach (SpellbookViewModel svm in Spellcasting)
            {
                if (svm is SpellbookSpellsViewModel ssvm && ssvm.SpellcastingID == id) ssvm.Refresh(ssvm.SpellcastingFeature);
            }
        }
        public ObservableRangeCollection<SubModel> SubPages { get; set; } = new ObservableRangeCollection<SubModel>();

        private void UpdatePages()
        {
            List<SubModel> pages = new List<SubModel>()
            {
                race, classes, background, playerScores, playerPersonal
            };
            pages.AddRange(Spellcasting);
            pages.Add(playerInventory);
            pages.Add(playerShops);
            pages.Add(playerInventoryChoices);
            pages.Add(playerJournal);
            pages.Add(playerInfo);
            pages.Add(playerFeatures);
            pages.Add(playerSources);
            if (play != null) pages.Add(play);
            SubPages.ReplaceRange(pages);

        }

        public override void MoneyChanged()
        {
            if (Parent != null) Parent.MoneyChanged();
            playerInfo.DoMoneyChanged();
        }

        public override void ChangedSelectedSpells(string id)
        {
            foreach (SpellbookViewModel svm in Spellcasting)
            {
                if (svm is SpellbookSpellsViewModel ssvm && ssvm.SpellcastingID == id) ssvm.Refresh(ssvm.SpellcastingFeature);
                if (svm is SpellPrepareViewModel spvm && spvm.SpellcastingID == id) spvm.Refresh(spvm.SpellcastingFeature);
            }
        }

        public override Command RefreshInventory => playerInventory.RefreshItems;

        public override SubModel FirstPage => race;

        private RaceViewModel race;
        private ClassesViewModel classes;
        private BackgroundViewModel background;
        private PlayerInventoryViewModel playerInventory;
        private PlayerShopViewModel playerShops;
        private PlayerInventoryChoicesViewModel playerInventoryChoices;
        private PlayerScoresViewModel playerScores;
        private PlayerPersonalViewModel playerPersonal;
        private PlayerJournalViewModel playerJournal;
        private PlayerFeaturesViewModel playerFeatures;
        private SourcesViewModel playerSources;
        private PlayerInfoViewModel playerInfo;
        private SwitchToPlayModel play;

    }
}
