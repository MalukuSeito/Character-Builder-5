using CB_5e.Helpers;
using CB_5e.Views;
using Character_Builder;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Spells;
using PCLStorage;
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

namespace CB_5e.ViewModels
{

    public class HitDieViewModel: HitDie, INotifyPropertyChanged
    {
        public HitDieViewModel(PlayerViewModel pvm, HitDie hd) : base(hd.Dice, hd.Count, hd.Used)
        {
            Reduce = new Command(() =>
            {
                if (Used < Count)
                {
                    pvm.MakeHistory("");
                    pvm.Context.Player.UseHitDie(Dice);
                    Used++;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Current"));
                    pvm.Save();
                }
            });
        }

        public string Current {
            get {
                return ToString() + " (" + Count + " Total)";  
            }
        }
        public string TotalText
        {
            get
            {
                return Total() + " Total";
            }
        }
        public Command Reduce { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ScoresModelViewModel : BaseViewModel
    {
        public AbilityScoreArray Current;
        public AbilityScoreArray Max;
        public Dictionary<Ability, int> Saves;
        public ScoresModelViewModel(BuilderContext Context)
        {
            Update(Context.Player);
        }
        public void Update(Player p)
        {
            Saves = p.GetSaves();
            Current = p.GetFinalAbilityScores(out Max);
            OnPropertyChanged(null);
        }

        public int StrengthValue { get { return Current.Strength; } }
        public int StrengthMax { get { return Max.Strength; } }
        public string StrengthMod { get { return Current.StrMod.ToString("+#;-#;0"); } }
        public string StrengthSave { get { return Saves[Ability.Strength].ToString("+#;-#;0"); } }

        public int DexterityValue { get { return Current.Dexterity; } }
        public int DexterityMax { get { return Max.Dexterity; } }
        public string DexterityMod { get { return Current.DexMod.ToString("+#;-#;0"); } }
        public string DexteritySave { get { return Saves[Ability.Dexterity].ToString("+#;-#;0"); } }

        public int ConstitutionValue { get { return Current.Constitution; } }
        public int ConstitutionMax { get { return Max.Constitution; } }
        public string ConstitutionMod { get { return Current.ConMod.ToString("+#;-#;0"); } }
        public string ConstitutionSave { get { return Saves[Ability.Constitution].ToString("+#;-#;0"); } }

        public int IntelligenceValue { get { return Current.Intelligence; } }
        public int IntelligenceMax { get { return Max.Intelligence; } }
        public string IntelligenceMod { get { return Current.IntMod.ToString("+#;-#;0"); } }
        public string IntelligenceSave { get { return Saves[Ability.Intelligence].ToString("+#;-#;0"); } }

        public int WisdomValue { get { return Current.Wisdom; } }
        public int WisdomMax { get { return Max.Wisdom; } }
        public string WisdomMod { get { return Current.WisMod.ToString("+#;-#;0"); } }
        public string WisdomSave { get { return Saves[Ability.Wisdom].ToString("+#;-#;0"); } }

        public int CharismaValue { get { return Current.Charisma; } }
        public int CharismaMax { get { return Max.Charisma; } }
        public string CharismaMod { get { return Current.ChaMod.ToString("+#;-#;0"); } }
        public string CharismaSave { get { return Saves[Ability.Charisma].ToString("+#;-#;0"); } }
    }


    public sealed class QueuedLock
    {
        private object innerLock;
        private volatile int ticketsCount = 0;
        private volatile int ticketToRide = 1;
        private int MAX_WAITING = 3;

        public QueuedLock()
        {
            innerLock = new Object();
        }

        public bool Enter()
        {
            int myTicket = Interlocked.Increment(ref ticketsCount);
            Monitor.Enter(innerLock);
            while (true)
            {

                if (myTicket == ticketToRide)
                {
                    return true;
                } 
                else if (myTicket + MAX_WAITING < ticketsCount)
                {
                    Exit();
                    return false;
                }
                else
                {
                    Monitor.Wait(innerLock);
                }
            }
        }

        public void WaitForAll()
        {

            int myTicket = Interlocked.Increment(ref ticketsCount);
            Monitor.Enter(innerLock);
            while (true)
            {

                if (myTicket == ticketToRide)
                {
                    Exit();
                    return;
                }
                else
                {
                    Monitor.Wait(innerLock);
                }
            }
        }

        public void Exit()
        {
            Interlocked.Increment(ref ticketToRide);
            Monitor.PulseAll(innerLock);
            Monitor.Exit(innerLock);
        }
    }

    public class PlayerViewModel : BaseViewModel
    {
        private static CultureInfo culture = CultureInfo.InvariantCulture;

        public static IntToStringConverter IntConverter { get; set; } = new IntToStringConverter();

        public QueuedLock Saving = new QueuedLock();
        public Mutex SaveLock = new Mutex();

        public event EventHandler PlayerChanged;

        public void FirePlayerChanged() => PlayerChanged?.Invoke(this, EventArgs.Empty);

        public ScoresModelViewModel Scores { get; private set; } 

        public INavigation Navigation { get; set; }
        public INavigation SpellNavigation { get; set; }
        public bool ChildModel { get; set; } = false;
        public PlayerViewModel(BuilderContext context)
        {
            Context = context;
            Scores = new ScoresModelViewModel(Context);
            PlayerChanged += PlayerViewModel_PlayerChanged;
            ResetHitDie = new Command(() => {
                HDBusy = true;
                MakeHistory();
                Context.Player.UsedHitDice = new List<int>();
                HitDice.ReplaceRange(from h in Context.Player.GetHitDie() orderby h select new HitDieViewModel(this, h));
                Save();
                HDBusy = false;
            });
            ResetHD = new Command(() => {
                MakeHistory();
                Context.Player.FailedDeathSaves = 0;
                Context.Player.SuccessDeathSaves = 0;
                OnPropertyChanged("DeathSaveFail");
                OnPropertyChanged("DeathSaveSuccess");
                Save();
            });
            DeselectSkill = new Command(async () =>
            {
                SkillBusy = true;
                SkillSearch = null;
                SelectedSkill = null;
                await Task.Delay(500); //Stupid Refereshindicator
                SkillBusy = false;

            });
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
            ShowImage = new Command(async (par) =>
            {
                await Navigation.PushAsync(new ImageViewer(par as ImageSource, Context.Player?.Portrait, Context.Player?.Name));
            });
            ShowClasses = new Command(async () =>
            {
                List<IOGLElement> classes = new List<IOGLElement>();
                foreach (PlayerClass pc in Context.Player.Classes)
                {
                    classes.Add(pc.GetClass(Context));
                    classes.Add(pc.GetSubClass(Context));
                }
                await Navigation.PushAsync(InfoSelectPage.Show(classes));
            }, () => 
                Context.Player?.Classes?.Count > 0);
            ShowRace = new Command(async () => {
                await Navigation.PushAsync(InfoSelectPage.Show(Context.Player.Race, Context.Player.SubRace));
            }, () => Context.Player?.RaceName != null && Context.Player?.RaceName != "");
            ShowBackground = new Command(async () => {
                await Navigation.PushAsync(InfoSelectPage.Show(Context.Player.Background));
            }, () => Context.Player?.BackgroundName != null && Context.Player?.BackgroundName != "");
            skills = Context.Player.GetSkills();
            Skills.ReplaceRange(skills);
            skillSearch = null;
            HitDice.ReplaceRange(from h in Context.Player.GetHitDie() orderby h select new HitDieViewModel(this, h));
            UpdateAllConditions();
            UpdateCondtions();

            resources = new List<ResourceViewModel>();
            resources.AddRange(from r in Context.Player.GetResourceInfo(true).Values select new ResourceViewModel(r, this));
            resources.AddRange(from r in Context.Player.GetBonusSpells(false) select new ResourceViewModel(r, this));
            UpdateResources();

            features = (from f in Context.Player.GetFeatures() where f.Name != "" && !f.Hidden select f).ToList();
            UpdateFeatures();

            proficiencies = new List<IXML>();
            proficiencies.AddRange(Context.Player.GetLanguages());
            proficiencies.AddRange(Context.Player.GetToolProficiencies());
            proficiencies.AddRange(from p in Context.Player.GetToolKWProficiencies() select new Feature(p, "Proficiency"));
            proficiencies.AddRange(from p in Context.Player.GetOtherProficiencies() select new Feature(p, "Proficiency"));
            UpdateProficiencies();

            UpdateSpellcasting();

            AddCustomCondition = new Command(() =>
            {
                if (CustomCondition != null && CustomCondition != "")
                {
                    MakeHistory();
                    Context.Player.Conditions.Add(CustomCondition);
                    UpdateCondtions();
                    Save();
                }
            }, () => CustomCondition != null && CustomCondition != "");
            AddCondition = new Command((par) =>
            {
                if (par is OGL.Condition p)
                {
                    MakeHistory();
                    Context.Player.Conditions.Add(p.Name + " " + ConfigManager.SourceSeperator + " " + p.Source);
                    UpdateCondtions();
                    Save();
                }
            });
            RemoveCondition = new Command((par) =>
            {
                if (par is OGL.Condition p)
                {
                    MakeHistory();
                    Context.Player.Conditions.RemoveAll(s => ConfigManager.SourceInvariantComparer.Equals(s, p.Name + " " + ConfigManager.SourceSeperator + " " + p.Source));
                    UpdateCondtions();
                    Save();
                }
            });
            ResetConditions = new Command((par) =>
            {
                ConditionsBusy = true;
                MakeHistory();
                Context.Player.Conditions.Clear();
                UpdateCondtions();
                Save();
                ConditionsBusy = false;
            });
            DeselectResource = new Command(async () =>
            {
                ResourceBusy = true;
                SelectedResource = null;
                Resources.ReplaceRange(new List<ResourceViewModel> ());
                resources.Clear();
                resources.AddRange(from r in Context.Player.GetResourceInfo(true).Values select new ResourceViewModel(r, this));
                resources.AddRange(from r in Context.Player.GetBonusSpells(false) select new ResourceViewModel(r, this));
                UpdateResources();
                await Task.Delay(500); //Stupid Refereshindicator
                ResourceBusy = false;

            });
            LongRest = new Command(() =>
            {
                MakeHistory("LongRest");
                foreach (ResourceInfo r in Context.Player.GetResourceInfo(true).Values)
                {
                    if (r.Recharge >= RechargeModifier.LongRest) Context.Player.SetUsedResources(r.ResourceID, 0);
                }
                foreach (ModifiedSpell ms in Context.Player.GetBonusSpells())
                {
                    if (ms.RechargeModifier >= RechargeModifier.LongRest) Context.Player.SetUsedResources(ms.getResourceID(), 0);
                }
                DeselectResource.Execute(null);
            });
            ShortRest = new Command(() =>
            {
                MakeHistory("ShortRest");
                foreach (ResourceInfo r in Context.Player.GetResourceInfo(true).Values)
                {
                    if (r.Recharge >= RechargeModifier.ShortRest) Context.Player.SetUsedResources(r.ResourceID, 0);
                }
                foreach (ModifiedSpell ms in Context.Player.GetBonusSpells())
                {
                    if (ms.RechargeModifier >= RechargeModifier.ShortRest) Context.Player.SetUsedResources(ms.getResourceID(), 0);
                }
                DeselectResource.Execute(null);
            });
            EditItem = new Command(async (par) =>
            {
                if (par is InventoryViewModel ivm)
                {
                    if (ivm.Item != null) await Navigation.PushAsync(new AboutPage() { Title = par.ToString() });
                    else await Navigation.PushAsync(InfoPage.Show(ivm.Boon));
                }
            });
            ShowItemInfo = new Command(async (par) =>
            {
                if (par is InventoryViewModel ivm)
                {
                    if (ivm.Item is Possession p)
                    {
                        await Navigation.PushAsync(InfoPage.Show(new DisplayPossession(ivm.Item, Context.Player)));
                    } else
                    {
                        await Navigation.PushAsync(InfoPage.Show(ivm.Boon));
                    }
                }
            });
            DeleteItem = new Command((par) =>
            {
                if (par is InventoryViewModel ivm)
                {
                    if (ivm.Item is Possession p)
                    {
                        Context.MakeHistory("");
                        Context.Player.RemovePossessionAndItems(p);
                        Save();
                    }
                    else if (ivm.Boon is Feature f)
                    {
                        Context.MakeHistory("");
                        Context.Player.RemoveBoon(f);
                        Save();
                    }
                    RefreshItems.Execute(null);
                }
            });
            RefreshItems = new Command(() =>
            {
                ItemsBusy = true;
                UpdateItems();
                ItemsBusy = false;
            });
            UpdateItems();
        }

        public Color Accent { get { return Color.Accent; } }

        private void Player_HistoryButtonChange(object sender, bool CanUndo, bool CanRedo)
        {
            Undo.ChangeCanExecute();
            Redo.ChangeCanExecute();
        }

        public Command Undo { get; set; }
        public Command Redo { get; set; }

        public void MakeHistory(string h = null)
        {
            Context.MakeHistory(h);
        }
        public virtual void Save()
        {
            if (App.AutoSaveDuringPlay && !ChildModel)
            {
                Task.Run(() => DoSave()).Forget();
            }
        }

        public void DoSave()
        {
            
            if (Context.Player != null)
            {
                Player p = Context.Player;
                if (Saving.Enter())
                {
                    SaveLock.WaitOne();
                    try
                    {
                        if (p.FilePath is IFile file)
                        {
                            Context.UnsavedChanges = 0;
                            using (Stream s = file.OpenAsync(FileAccess.ReadAndWrite).Result)
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
            HitDice.ReplaceRange(from h in Context.Player.GetHitDie() orderby h select new HitDieViewModel(this, h));
            OnPropertyChanged(null);
            Skills.ReplaceRange(Context.Player.GetSkills());
            Scores.Update(Context.Player);
            ShowClasses.ChangeCanExecute();
            ShowRace.ChangeCanExecute();
            ShowBackground.ChangeCanExecute();
            UpdateAllConditions();
            UpdateCondtions();

            resources.Clear();
            resources.AddRange(from r in Context.Player.GetResourceInfo(true).Values select new ResourceViewModel(r, this));
            resources.AddRange(from r in Context.Player.GetBonusSpells(false) select new ResourceViewModel(r, this));
            UpdateResources();

            features = (from f in Context.Player.GetFeatures() where f.Name != "" && !f.Hidden select f).ToList();
            UpdateFeatures();

            proficiencies = new List<IXML>();
            proficiencies.AddRange(Context.Player.GetLanguages());
            proficiencies.AddRange(Context.Player.GetToolProficiencies());
            proficiencies.AddRange(from p in Context.Player.GetToolKWProficiencies() select new Feature(p, "Proficiency"));
            proficiencies.AddRange(from p in Context.Player.GetOtherProficiencies() select new Feature(p, "Proficiency"));
            UpdateProficiencies();
            UpdateItems();

            UpdateSpellcasting();

        }

        public void UpdateItems()
        {
            inventory.Clear();
            foreach (Possession p in Context.Player.GetItemsAndPossessions())
            {
                inventory.Add(new InventoryViewModel
                {
                    Item = p,
                    ShowInfo = ShowItemInfo,
                    Edit = EditItem,
                    Delete = DeleteItem
                });
            }
            foreach (Feature f in Context.Player.GetBoons())
            {
                inventory.Add(new InventoryViewModel
                {
                    Boon = f,
                    ShowInfo = ShowItemInfo,
                    Edit = EditItem,
                    Delete = DeleteItem
                });
            }
            UpdateInventory();
        }

        public ImageSource Portrait
        {
            get { if (Context.Player != null && Context.Player.Portrait != null) return ImageSource.FromStream(() => new MemoryStream(Context.Player.Portrait)); return null; }
        }

        public ImageSource FactionImage
        {
            get { if (Context.Player != null && Context.Player.FactionImage != null) return ImageSource.FromStream(() => new MemoryStream(Context.Player.FactionImage)); return null; }
        }

        public string HP {
            get {
                return (Context.Player.GetHitpointMax() + Context.Player.CurrentHPLoss).ToString();
            }
            set
            {
                if (HP == value) return;
                int parsedInt = 0;
                if (value == "" || value == "-"|| int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    MakeHistory("CurHP");
                    int loss = parsedInt - Context.Player.GetHitpointMax();
                    if (loss > 0) loss = 0;
                    Context.Player.CurrentHPLoss = loss;
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("HP");
            }
        }

        public string HPMax
        {
            get
            {
                return "Current: (Max " + Context.Player.GetHitpointMax() + ")";
            }
        }
        public string BonusHP
        {
            get
            {
                return Context.Player.BonusMaxHP.ToString();
            }
            set
            {
                if (BonusHP == value) return;
                int parsedInt = 0;
                if (value == "" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (parsedInt < 0) parsedInt = 0;
                    MakeHistory("CurHP");
                    Context.Player.BonusMaxHP = parsedInt;
                    OnPropertyChanged("HP");
                    OnPropertyChanged("HPMax");
                    Save();
                }
                if (value != "") OnPropertyChanged("BonusHP");
            }
        }

        public string TempHP
        {
            get
            {
                return Context.Player.TempHP.ToString();
            }
            set
            {
                if (TempHP == value) return;
                int parsedInt = 0;
                if (value == "" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (parsedInt < 0) parsedInt = 0;
                    MakeHistory("TempHP");
                    Context.Player.TempHP = parsedInt;
                    Save();
                }
                if (value != "") OnPropertyChanged("TempHP");
            }
        }

        public string Race { get { return Context.Player.GetRaceSubName(); } }
        public string Classes {
            get {
                int level = Context.Player.GetLevel();
                return String.Join(" | ", from pc in Context.Player.Classes select pc.ToString(Context, level));
                /**int level = Level;
                int total = 0;
                foreach (PlayerClass pc in Context.Player.Classes) total += pc.getClassLevelUpToLevel(level);
                if (total == level) return a;
                return a.Length > 0 ? a + ", Total Level " + Level : "Level " + Level;*/
            }
        }
        public string Name { get { return Context.Player.Name; } }
        public int Level { get { return Context.Player.GetLevel(); } }
        public string LevelText { get { return "(Level " + Context.Player.GetLevel().ToString() + ")"; } }
        public string RaceAndLevel { get { return Race + " (" + Level + ")"; } }
        public string Background { get { return Context.Player?.Background?.Name; } }
        public string Alignment { get { return Context.Player?.Alignment; } }
        public int AC { get { return Context.Player.GetAC(); } }
        public string Initiative { get { return Context.Player.GetInitiative().ToString("+#;-#;0"); } }
        public string Proficiency { get { return Context.Player.GetProficiency().ToString("+#;-#;0"); } }
        public int Speed { get { return Context.Player.GetSpeed(); } }
        public bool Inspiration {
            get {
                return Context.Player.Inspiration;
            }
            set
            {
                if (Inspiration == value) return;
                Context.Player.Inspiration = value;
                MakeHistory("Inspiration");
                OnPropertyChanged("Inspiration");
                Save();
            }
        }
        public int XP { get { return Context.Player.GetXP(); } }
        public string Carried {
            get {
                double carry = Context.Player.GetCarryCapacity();
                double weight = Context.Player.GetWeight();
                return weight.ToString("N") + " / " + carry.ToString("N0") + " lb";
            }
        }

        public string Money
        {
            get
            {
                return Context.Player.GetMoney().ToString();
            }
        }

        public string HDTotal {
            get {
                List<HitDie> hd = Context.Player.GetHitDie();
                hd.Sort();
                return "Total: " + String.Join(", ", from h in hd select h.Total());
            }
        }

        public Command ResetHitDie { get; set; }

        public ObservableRangeCollection<HitDieViewModel> HitDice { get; set; } = new ObservableRangeCollection<HitDieViewModel>();
        private bool _hdBusy = false;
        public bool HDBusy { get { return _hdBusy; } set { SetProperty(ref _hdBusy, value); } }
        public Command ResetHD { get; set; }
        public int DeathSaveSuccess {
            get {
                return Context.Player.SuccessDeathSaves >= 0 && Context.Player.SuccessDeathSaves <= 3 ? Context.Player.SuccessDeathSaves : 0;
            }
            set
            {
                if (DeathSaveSuccess == value) return;
                MakeHistory("SuccessDeathSaves");
                Context.Player.SuccessDeathSaves = value;
                Save();
            }
        }
        public int DeathSaveFail {
            get {
                return Context.Player.FailedDeathSaves >= 0 && Context.Player.FailedDeathSaves <= 3 ? Context.Player.FailedDeathSaves : 0;
            }
            set
            {
                if (DeathSaveFail == value) return;
                MakeHistory("FailedDeathSaves");
                Context.Player.FailedDeathSaves = value;
                Save();
            }
        }
        public string PP
        {
            get { return Context.Player.GetMoney().pp.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    Price p = Context.Player.GetMoney();
                    if (p.pp == parsedInt) return;
                    MakeHistory("MoneyPP");
                    Context.Player.SetMoney(p.cp, p.sp, p.ep, p.gp, parsedInt);
                    OnPropertyChanged("Money");
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("PP");
            }
        }
        public string GP
        {
            get { return Context.Player.GetMoney().gp.ToString(); }
            set
            {
                Price p = Context.Player.GetMoney();
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (p.gp == parsedInt) return;
                    MakeHistory("MoneyGP");
                    Context.Player.SetMoney(p.cp, p.sp, p.ep, parsedInt, p.pp);
                    OnPropertyChanged("Money");
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("GP");
            }
        }
        public string EP
        {
            get { return Context.Player.GetMoney().ep.ToString(); }
            set
            {
                Price p = Context.Player.GetMoney();
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (p.ep == parsedInt) return;
                    MakeHistory("MoneyEP");
                    Context.Player.SetMoney(p.cp, p.sp, parsedInt, p.gp, p.pp);
                    OnPropertyChanged("Money");
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("EP");
            }
        }
        public string SP
        {
            get { return Context.Player.GetMoney().sp.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    Price p = Context.Player.GetMoney();
                    if (p.sp == parsedInt) return;
                    MakeHistory("MoneySP");
                    Context.Player.SetMoney(p.cp, parsedInt, p.ep, p.gp, p.pp);
                    OnPropertyChanged("Money");
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("SP");
            }
        }
        public string CP
        {
            get { return Context.Player.GetMoney().cp.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    Price p = Context.Player.GetMoney();
                    if (p.cp == parsedInt) return;
                    MakeHistory("MoneyCP");
                    Context.Player.SetMoney(parsedInt, p.sp, p.ep, p.gp, p.pp);
                    OnPropertyChanged("Money");
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("CP");
            }
        }

        public List<Ability> Abilities { get; set; } = new List<Ability>() { Ability.Strength, Ability.Constitution, Ability.Dexterity, Ability.Intelligence, Ability.Wisdom, Ability.Charisma };

        private Ability _baseAbility;
        public int SkillBaseIndex { get { return Abilities.IndexOf(_baseAbility); } set { SetProperty(ref _baseAbility, Abilities[value]); OnPropertyChanged("SkillValue"); } }
        private List<SkillInfo> skills;
        private string skillSearch;
        public string SkillSearch { get { return skillSearch; } set { SetProperty(ref skillSearch, value); UpdateSkills(); } }
        public ObservableRangeCollection<SkillInfo> Skills { get; set; } = new ObservableRangeCollection<SkillInfo>();
        public String SkillValue {
            get {
                if (SelectedSkill != null) return ": " + Context.Player.GetSkill(SelectedSkill.Skill, _baseAbility).ToString("+#;-#;0");
                return ": 0";
            }
        }

        private void UpdateSkills()
        {

            if (skillSearch == null || skillSearch == "") Skills.ReplaceRange(skills);
            else
            {
                Skills.ReplaceRange(from c in skills where culture.CompareInfo.IndexOf(c.Desc, skillSearch, CompareOptions.IgnoreCase) >= 0 || culture.CompareInfo.IndexOf(c.Skill.Description, skillSearch, CompareOptions.IgnoreCase) >= 0 select c);
            }
        }

        private SkillInfo _selectedSkill = null;
        public SkillInfo SelectedSkill { get { return _selectedSkill; } set { SetProperty(ref _selectedSkill, value); if (value != null) SkillBaseIndex = Abilities.FindIndex(a => value.Base.HasFlag(a)); } }

        public Command DeselectSkill { get; private set; }
        private bool skillbusy = false;
        public bool SkillBusy { get { return skillbusy; } set { SetProperty(ref skillbusy, value); } }
        public Command ShowImage { get; private set; }
        public Command ShowClasses { get; private set; }
        public Command ShowRace { get; private set; }
        public Command ShowBackground { get; private set; }
        public BuilderContext Context { get; private set; }

        private string customCondition;
        public string CustomCondition { get => customCondition; set
            {
                SetProperty(ref customCondition, value);
                AddCustomCondition.ChangeCanExecute();
            }
        }

        public object Nothing { get { return null; }  set { OnPropertyChanged("Nothing"); } }

        private string condtionSearch;
        public ObservableRangeCollection<OGL.Condition> ActiveConditions { get; set; } = new ObservableRangeCollection<OGL.Condition>();
        public ObservableRangeCollection<OGL.Condition> AllConditions { get; set; } = new ObservableRangeCollection<OGL.Condition>();
        public string ConditionSearch
        {
            get => condtionSearch;
            set {
                SetProperty(ref condtionSearch, value);
                UpdateAllConditions();
            }
        }

        public Command AddCustomCondition { get; private set; }
        public Command AddCondition { get; private set; }
        public Command RemoveCondition { get; private set; }
        public Command ResetConditions { get; private set; }
        private bool cbusy;
        public bool ConditionsBusy { get => cbusy; private set => SetProperty(ref cbusy, value); }

        public void UpdateAllConditions() => AllConditions.ReplaceRange(from c in Context.Conditions.Values where condtionSearch == null || condtionSearch == "" || culture.CompareInfo.IndexOf(c.Description, condtionSearch, CompareOptions.IgnoreCase) >= 0 || culture.CompareInfo.IndexOf(c.Name, condtionSearch, CompareOptions.IgnoreCase) >= 0 orderby c.Name select c);
        public void UpdateCondtions() => ActiveConditions.ReplaceRange(from c in Context.Player.Conditions orderby c select Context.GetCondition(c, null));


        private string resourceSearch;
        public string ResourceSearch
        {
            get => resourceSearch;
            set
            {
                SetProperty(ref resourceSearch, value);
                UpdateResources();
            }
        }
        private List<ResourceViewModel> resources;
        public ObservableRangeCollection<ResourceViewModel> Resources { get; set; } = new ObservableRangeCollection<ResourceViewModel>();

        public void UpdateResources() => Resources.ReplaceRange(from r in resources where resourceSearch == null || resourceSearch == "" 
             || culture.CompareInfo.IndexOf(r.Name, resourceSearch, CompareOptions.IgnoreCase) >= 0 orderby r.ToString() select r);

        private ResourceViewModel selectedResource;
        public ResourceViewModel SelectedResource { get => selectedResource; set
            {
                SetProperty(ref selectedResource, value);
                if (value != null && value.IsChangeable)
                {
                    currentResourceValue = value.Used;
                    OnPropertyChanged("CurrentResourceValue");
                }
                else {
                    ResourceViewModel.last = null;
                    currentResourceValue = 0;
                    OnPropertyChanged("CurrentResourceValue");
                }
            }
        }        
        private int currentResourceValue;
        public int CurrentResourceValue { get => currentResourceValue; set {
                if (value >= 0 && value != currentResourceValue && selectedResource != null && selectedResource.IsChangeable)
                {
                    ResourceViewModel r = selectedResource;
                    MakeHistory("Resource" + r.ResourceID);
                    if (r.Max > 0 && value > r.Max) value = r.Max;
                    r.Used = value;
                    Context.Player.SetUsedResources(r.ResourceID, value);
                    Save();
                }
                SetProperty(ref currentResourceValue, value);
            } }

        public void UpdateUsed()
        {
            currentResourceValue = selectedResource.Used;
            OnPropertyChanged("CurrentResourceValue");
        }
        public bool ResourceBusy { get => resourceBusy; set => SetProperty(ref resourceBusy, value); }
        public Command DeselectResource { get; private set; }
        public Command LongRest { get; private set; }
        public Command ShortRest { get; private set; }

        bool resourceBusy = false;

        private string featureSearch;
        public string FeatureSearch
        {
            get => featureSearch;
            set
            {
                SetProperty(ref featureSearch, value);
                UpdateFeatures();
            }
        }
        private List<Feature> features;
        public ObservableRangeCollection<Feature> Features { get; set; } = new ObservableRangeCollection<Feature>();

        public void UpdateFeatures() => Features.ReplaceRange(from f in features where featureSearch == null || featureSearch == "" 
            || culture.CompareInfo.IndexOf(f.Name, featureSearch, CompareOptions.IgnoreCase) >= 0
            || culture.CompareInfo.IndexOf(f.Text, featureSearch, CompareOptions.IgnoreCase) >= 0 orderby f.Name select f);

        private string proficiencySearch;
        public string ProficiencySearch
        {
            get => proficiencySearch;
            set
            {
                SetProperty(ref proficiencySearch, value);
                UpdateProficiencies();
            }
        }
        private List<IXML> proficiencies;
        public ObservableRangeCollection<IXML> Proficiencies { get; set; } = new ObservableRangeCollection<IXML>();

        public void UpdateProficiencies() => Proficiencies.ReplaceRange(from f in proficiencies where proficiencySearch == null || proficiencySearch == ""
            || culture.CompareInfo.IndexOf(f.ToString(), proficiencySearch, CompareOptions.IgnoreCase) >= 0 orderby f.ToString() select f);

        public ObservableRangeCollection<SpellbookViewModel> Spellcasting { get; set; } = new ObservableRangeCollection<SpellbookViewModel>();
        public void UpdateSpellcasting()
        {
            List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>(from f in Context.Player.GetFeatures() where f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID != "MULTICLASS" orderby Context.Player.GetClassLevel(((SpellcastingFeature)f).SpellcastingID) descending, ((SpellcastingFeature)f).DisplayName, ((SpellcastingFeature)f).SpellcastingID select f as SpellcastingFeature);
            if (spellcasts.Count == 0) Spellcasting.ReplaceRange(new List<SpellbookViewModel>() { new SpellbookViewModel(this, null) });
            List<SpellbookViewModel> views = new List<SpellbookViewModel>();
            List<SpellbookViewModel> oldviews = new List<SpellbookViewModel>(Spellcasting);
            foreach (SpellcastingFeature sf in spellcasts)
            {
                SpellbookViewModel v = Spellcasting.FirstOrDefault(view => view is SpellbookSpellsViewModel && view.SpellcastingID == sf.SpellcastingID);
                if (v != null)
                {
                    v.Refresh();
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
                        v2.Refresh();
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
            }
        }
        public void ChangedPreparedSpells(string id)
        {
            foreach (SpellbookViewModel svm in Spellcasting)
            {
                if (svm is SpellbookSpellsViewModel ssvm && ssvm.SpellcastingID == id) ssvm.Refresh();
            }
        }

        public void UpdateSlots(SpellbookSpellsViewModel me)
        {
            foreach (SpellbookViewModel svm in Spellcasting)
            {
                if (svm is SpellbookSpellsViewModel ssvm && svm != me) ssvm.UpdateSlots();
            }
        }
        private List<InventoryViewModel> inventory = new List<InventoryViewModel>();
        public ObservableRangeCollection<InventoryViewModel> Inventory { get; set; } = new ObservableRangeCollection<InventoryViewModel>();

        public void UpdateInventory() => Inventory.ReplaceRange(from f in inventory where inventorysearch == null || inventorysearch == "" 
            || culture.CompareInfo.IndexOf(f.Name, featureSearch, CompareOptions.IgnoreCase) >= 0
            || culture.CompareInfo.IndexOf(f.Detail, featureSearch, CompareOptions.IgnoreCase) >= 0
            || culture.CompareInfo.IndexOf(f.Description, featureSearch, CompareOptions.IgnoreCase) >= 0 orderby f.Name select f);

        private string inventorysearch;
        public string InventorySearch
        {
            get => inventorysearch;
            set
            {
                SetProperty(ref inventorysearch, value);
                UpdateInventory();
            }
        }
        private bool itemsBusy;
        public bool ItemsBusy { get => itemsBusy; set => SetProperty(ref itemsBusy, value); }
        public Command EditItem { get; private set; }
        public Command ShowItemInfo { get; private set; }
        public Command DeleteItem { get; private set; }
        public Command RefreshItems { get; private set; }
    }
    
    public class InventoryViewModel: ObservableObject
    {
        public Possession Item { get; set; }
        public Feature Boon { get; set; }
        public string Name { get => Item != null ? Item.FullName + (Item.Amount != null ? " (" + Item.Amount + ")" : "") : Boon.ToString(); }
        public string Description { get => Item != null ? Item.Description : Boon.Text; }
        public string Detail { get => Item != null ? Item.Stats : "Boon"; }
        public Command ShowInfo { get; set; }
        public Command Edit { get; set; }
        public Command Delete { get; set; }
    }

    public class ResourceViewModel: ObservableObject
    {
        public static string last;

        public Object Value;

        public ResourceViewModel(object value, PlayerViewModel model)
        {
            Value = value;
            Reduce = new Command(() =>
            {
                if (IsChangeable && last != null && last.Equals(ResourceID))
                {
                    Used++;
                    model.MakeHistory("Resource" + ResourceID);
                    if (Max > 0 && Used > Max) Used = Max;
                    model.Context.Player.SetUsedResources(ResourceID, Used);
                    model.UpdateUsed();

                    model.Save();
                }
                last = ResourceID;
            });
        }

        public int Max
        {
            get
            {
                if (Value is ResourceInfo r) return r.Max;
                else if (Value is ModifiedSpell ms) return ms.count;
                return 0;
            }
        }

        public int Used
        {
            get
            {
                if (Value is ResourceInfo r) return r.Used;
                else if (Value is ModifiedSpell ms) return ms.used;
                return 0;
            }
            set
            {
                if (Value is ResourceInfo r) r.Used = value;
                else if (Value is ModifiedSpell ms) ms.used = value;
                OnPropertyChanged("Desc");
            }
        }

        public String ResourceID
        {
            get
            {
                if (Value is ResourceInfo r) return r.ResourceID;
                else if (Value is ModifiedSpell ms) return ms.getResourceID();
                return null;
            }
        }

        public string Name
        {
            get
            {
                if (Value is ResourceInfo r) return r.Name;
                else if (Value is ModifiedSpell ms) return ms.Name;
                return null;
            }
        }

        public string Desc
        {
            get
            {
                if (Value is ResourceInfo r) return r.Desc;
                else if (Value is ModifiedSpell ms) return ms.Desc;
                return null;
            }
        }
        public string Text
        {
            get
            {
                if (Value is ResourceInfo r) return r.Text;
                else if (Value is ModifiedSpell ms) return ms.Text;
                return null;
            }
        }

        public Command Reduce { get; private set; }
        public bool IsChangeable { get
            {
                return Value is ResourceInfo || Value is ModifiedSpell ms && ((ms.Level > 0 && ms.RechargeModifier < RechargeModifier.AtWill) || (ms.Level == 0 && ms.RechargeModifier != RechargeModifier.Unmodified && ms.RechargeModifier < RechargeModifier.AtWill));
            }
        }
    }

}
