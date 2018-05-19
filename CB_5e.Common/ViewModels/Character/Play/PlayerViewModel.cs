using CB_5e.Helpers;
using CB_5e.ViewModels.Character;
using CB_5e.Views;
using Character_Builder;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Items;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character.Play
{
    public class PlayerViewModel : PlayerModel
    {
        
        public QueuedLock Saving = new QueuedLock();
        public Mutex SaveLock = new Mutex();
        public PlayerModel Parent;
        public PlayerViewModel(BuilderContext context) : base(context)
        {
            
            PlayerChanged += PlayerViewModel_PlayerChanged;
            Context.HistoryButtonChange += Player_HistoryButtonChange;
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
            playerInfo = new PlayerInfoViewModel(this);
            playerSkills = new PlayerSkillViewModel(this);
            playerResources = new PlayerResourcesViewModel(this);
            playerFeatures = new PlayerFeaturesViewModel(this);
            playerProficiencies = new PlayerProficiencyViewModel(this);
            playerActions = new PlayerActionsViewModel(this);
            playerConditions = new PlayerConditionViewModel(this);
            playerInventory = new PlayerInventoryViewModel(this);
            playerShops = new PlayerShopViewModel(this);
            playerInventoryChoices = new PlayerInventoryChoicesViewModel(this);
            playerJournal = new PlayerJournalViewModel(this);
            playerForms = new PlayerFormsCompanionsViewModel(this);
            playerNotes = new PlayerNotesViewModel(this);
            playerPDF = new PlayerPDFViewModel(this);
            build = new SwitchToBuildModel(this);
            UpdateSpellcasting();
            UpdateForms();
        }

        public PlayerViewModel(PlayerModel parent) : base(parent.Context)
        {
            Parent = parent;
            ChildModel = true;
            PlayerChanged += PlayerViewModel_PlayerChanged;
            Undo = parent.Undo;
            Redo = parent.Redo;
            playerInfo = new PlayerInfoViewModel(this);
            playerSkills = new PlayerSkillViewModel(this);
            playerResources = new PlayerResourcesViewModel(this);
            playerFeatures = new PlayerFeaturesViewModel(this);
            playerProficiencies = new PlayerProficiencyViewModel(this);
            playerActions = new PlayerActionsViewModel(this);
            playerConditions = new PlayerConditionViewModel(this);
            playerInventory = new PlayerInventoryViewModel(this);
            playerShops = new PlayerShopViewModel(this);
            playerInventoryChoices = new PlayerInventoryChoicesViewModel(this);
            playerJournal = new PlayerJournalViewModel(this);
            playerForms = new PlayerFormsCompanionsViewModel(this);
            playerNotes = new PlayerNotesViewModel(this);
            playerPDF = new PlayerPDFViewModel(this);
            UpdateForms();
            UpdateSpellcasting();
        }



        private void Player_HistoryButtonChange(object sender, bool CanUndo, bool CanRedo)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Undo.ChangeCanExecute();
                Redo.ChangeCanExecute();
            });
        }


        public override void MakeHistory(string h = null)
        {
            if (Parent != null) Parent.MakeHistory(h);
            else Context.MakeHistory(h);
        }
        public override void Save()
        {
            if (Parent != null) Parent.Save();
            else if (App.AutoSaveDuringPlay && !ChildModel)
            {
                Task.Run(() => DoSave()).Forget();
            }
        }

        public override void DoSave()
        {
            if (Parent != null) Parent.DoSave();
            else if (Context.Player != null)
            {
                Modified = true;
                Player p = Context.Player;
                if (Saving.Enter())
                {
                    SaveLock.WaitOne();
                    try
                    {
                        if (p.FilePath is FileInfo file)
                        {
                            Context.UnsavedChanges = 0;
                            using (Stream s = new FileStream(file.FullName, FileMode.OpenOrCreate))
                            {
                                Player.Serializer.Serialize(s, p);
                                s.SetLength(s.Position);
                            }
                        }
                    } catch (Exception e)
                    {
                        ConfigManager.LogError("Error Saving", e);
                    }
                    SaveLock.ReleaseMutex();
                    Saving.Exit();
                }
            }

        }

        private void PlayerViewModel_PlayerChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(null);
            UpdateSpellcasting();
            UpdateForms();
        }
        
        public ObservableRangeCollection<SpellbookViewModel> Spellcasting { get; set; } = new ObservableRangeCollection<SpellbookViewModel>();
        public ObservableRangeCollection<FormsCompanionsViewModel> FormsCompanions { get; set; } = new ObservableRangeCollection<FormsCompanionsViewModel>();

        public void UpdateForms()
        {
            List<FormsCompanionsViewModel> views = new List<FormsCompanionsViewModel>();
            foreach (FormsCompanionInfo fci in Context.Player.GetFormsCompanionChoices().OrderBy(f => f.DisplayName))
            {
                foreach (FormsCompanionsViewModel fcvm in FormsCompanions)
                {
                    if (fci.ID == fcvm.ID)
                    {
                        fcvm.Refresh(fci);
                        views.Add(fcvm);
                        goto NEXT;
                    }
                }
                views.Add(new FormsCompanionsViewModel(this, fci));
                NEXT:;
            }
            if (!views.SequenceEqual(FormsCompanions))
            {
                FormsCompanions.ReplaceRange(views);
                UpdatePages();
            }
            if (views.Count == 0) UpdatePages();
        }

        public override void UpdateSpellcasting()
        {
            List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>(from f in Context.Player.GetFeatures() where f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID != "MULTICLASS" orderby Context.Player.GetClassLevel(((SpellcastingFeature)f).SpellcastingID) descending, ((SpellcastingFeature)f).DisplayName, ((SpellcastingFeature)f).SpellcastingID select f as SpellcastingFeature);
            //if (spellcasts.Count == 0) Spellcasting.ReplaceRange(new List<SpellbookViewModel>() { new SpellbookViewModel(this, null) });
            List<SpellbookViewModel> views = new List<SpellbookViewModel>();
            List<SpellbookViewModel> oldviews = new List<SpellbookViewModel>(Spellcasting);
            foreach (SpellcastingFeature sf in spellcasts)
            {
                SpellbookViewModel v = Spellcasting.FirstOrDefault(view => view is SpellbookSpellsViewModel && view.SpellcastingID == sf.SpellcastingID);
                if (v != null)
                {
                    v.Refresh(sf);
                    views.Add(v);
                }
                else
                {
                    views.Add(new SpellbookSpellsViewModel(this, sf));
                }
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

        public override void ChangedSelectedSpells(string id)
        {
            foreach (SpellbookViewModel svm in Spellcasting)
            {
                if (svm is SpellbookSpellsViewModel ssvm && ssvm.SpellcastingID == id) ssvm.Refresh(ssvm.SpellcastingFeature);
                if (svm is SpellPrepareViewModel spvm && spvm.SpellcastingID == id) spvm.Refresh(spvm.SpellcastingFeature);
            }
        }

        public void UpdateSlots(SpellbookSpellsViewModel me)
        {
            foreach (SpellbookViewModel svm in Spellcasting)
            {
                if (svm is SpellbookSpellsViewModel ssvm && svm != me) ssvm.UpdateSlots();
            }
        }

        public override void MoneyChanged()
        {
            if (Parent != null) Parent.MoneyChanged();
            playerInfo.DoMoneyChanged();
        }

        public ObservableRangeCollection<SubModel> SubPages { get; set; } = new ObservableRangeCollection<SubModel>();

        private void UpdatePages()
        {
            List<SubModel> pages = new List<SubModel>()
            {
                playerInfo, playerSkills, playerResources, playerFeatures, playerProficiencies, playerConditions
            };
            pages.AddRange(Spellcasting);
            pages.Add(playerInventory);
            pages.Add(playerActions);
            pages.Add(playerForms);
            pages.AddRange(FormsCompanions);
            pages.Add(playerShops);
            pages.Add(playerInventoryChoices);
            pages.Add(playerJournal);
            pages.Add(playerNotes);
            pages.Add(playerPDF);
            if (build != null) pages.Add(build);
            SubPages.ReplaceRange(pages);

        }

        public override Command RefreshInventory => playerInventory.RefreshItems;

        public override SubModel FirstPage => playerInfo;

        private PlayerInfoViewModel playerInfo;
        private PlayerSkillViewModel playerSkills;
        private PlayerResourcesViewModel playerResources;
        private PlayerFeaturesViewModel playerFeatures;
        private PlayerProficiencyViewModel playerProficiencies;
        private PlayerActionsViewModel playerActions;
        private PlayerConditionViewModel playerConditions;
        private PlayerInventoryViewModel playerInventory;
        private PlayerShopViewModel playerShops;
        private PlayerInventoryChoicesViewModel playerInventoryChoices;
        private PlayerJournalViewModel playerJournal;
        private PlayerNotesViewModel playerNotes;
        private PlayerPDFViewModel playerPDF;
        private PlayerFormsCompanionsViewModel playerForms;
        private SwitchToBuildModel build;
    }
    

}
