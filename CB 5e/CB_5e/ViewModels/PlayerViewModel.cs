using CB_5e.Helpers;
using CB_5e.Views;
using Character_Builder;
using OGL;
using OGL.Base;
using OGL.Common;
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
        //private static int waiting;

        public static IntToStringConverter IntConverter { get; set; } = new IntToStringConverter();

        public static QueuedLock Saving = new QueuedLock();

        public event EventHandler PlayerChanged;

        public void FirePlayerChanged() => PlayerChanged?.Invoke(this, EventArgs.Empty);

        public ScoresModelViewModel Scores { get; private set; } 

        private static PlayerViewModel instance = null;
        public INavigation Navigation { get; set; }
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
            DeselectSkill = new Command(() =>
            {
                SkillBusy = true;
                SelectedSkill = null;
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
            Skills.ReplaceRange(Context.Player.GetSkills());
            HitDice.ReplaceRange(from h in Context.Player.GetHitDie() orderby h select new HitDieViewModel(this, h));

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
            if (App.AutoSaveDuringPlay)
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
                    if (p.FilePath is IFile file)
                    {
                        Context.UnsavedChanges = 0;
                        using (Stream s = file.OpenAsync(FileAccess.ReadAndWrite).Result)
                        {

                            Player.Serializer.Serialize(s, p);
                            s.SetLength(s.Position);
                        }
                    }
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
        public ObservableRangeCollection<SkillInfo> Skills { get; set; } = new ObservableRangeCollection<SkillInfo>();
        public String SkillValue {
            get {
                if (SelectedSkill != null) return ": " + Context.Player.GetSkill(SelectedSkill.Skill, _baseAbility).ToString("+#;-#;0");
                return ": 0";
            }
        }

        private SkillInfo _selectedSkill = null;
        public SkillInfo SelectedSkill { get { return _selectedSkill; } set { SetProperty(ref _selectedSkill, value); if (value != null) SkillBaseIndex = Abilities.FindIndex(a => value.Base.HasFlag(a)); } }

        public Command DeselectSkill { get; private set; }
        public bool SkillBusy { get; private set; }
        public Command ShowImage { get; private set; }
        public Command ShowClasses { get; private set; }
        public Command ShowRace { get; private set; }
        public Command ShowBackground { get; private set; }
        public BuilderContext Context { get; private set; }
    }

}
