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
        public HitDieViewModel(HitDie hd) : base(hd.Dice, hd.Count, hd.Used)
        {
            Reduce = new Command(() =>
            {
                if (Used < Count)
                {
                    PlayerViewModel.MakeHistory("");
                    Player.Current.UseHitDie(Dice);
                    Used++;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Current"));
                    PlayerViewModel.Instance.Save();
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
        public ScoresModelViewModel()
        {
            Update(Player.Current);
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

        public ScoresModelViewModel Scores { get; } = new ScoresModelViewModel();

        private static PlayerViewModel instance = null;
        public static PlayerViewModel Instance
        {
            get
            {
                if (instance == null) instance = new PlayerViewModel();
                return instance;
            }
        }
        public INavigation Navigation { get; set; }
        private PlayerViewModel()
        {
            
            PlayerChanged += PlayerViewModel_PlayerChanged;
            ResetHitDie = new Command(() => {
                HDBusy = true;
                MakeHistory();
                Player.Current.UsedHitDice = new List<int>();
                HitDice.ReplaceRange(from h in Player.Current.GetHitDie() orderby h select new HitDieViewModel(h));
                Save();
                HDBusy = false;
            });
            ResetHD = new Command(() => {
                MakeHistory();
                Player.Current.FailedDeathSaves = 0;
                Player.Current.SuccessDeathSaves = 0;
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
            Player.HistoryButtonChange += Player_HistoryButtonChange;
            Undo = new Command(() =>
            {
                Player.Undo();
                FirePlayerChanged();
                Save();
            }, () =>
            {
                return Player.CanUndo();
            });
            Redo = new Command(() =>
            {
                Player.Redo();
                FirePlayerChanged();
                Save();
            }, () =>
            {
                return Player.CanRedo();
            });
            ShowImage = new Command(async (par) =>
            {
                await Navigation.PushAsync(new ImageViewer(par as ImageSource, Player.Current?.Portrait, Player.Current?.Name));
            });
            ShowClasses = new Command(async () =>
            {
                List<IOGLElement> classes = new List<IOGLElement>();
                foreach (PlayerClass pc in Player.Current.Classes)
                {
                    classes.Add(pc.Class);
                    classes.Add(pc.SubClass);
                }
                await Navigation.PushAsync(InfoSelectPage.Show(classes));
            }, () => 
                Player.Current?.Classes?.Count > 0);
            ShowRace = new Command(async () => {
                await Navigation.PushAsync(InfoSelectPage.Show(Player.Current.Race, Player.Current.SubRace));
            }, () => Player.Current?.RaceName != null && Player.Current?.RaceName != "");
            ShowBackground = new Command(async () => {
                await Navigation.PushAsync(InfoSelectPage.Show(Player.Current.Background));
            }, () => Player.Current?.BackgroundName != null && Player.Current?.BackgroundName != "");
            Skills.ReplaceRange(Player.Current.GetSkills());
            
        }

        public Color Accent { get { return Color.Accent; } }

        private void Player_HistoryButtonChange(object sender, bool CanUndo, bool CanRedo)
        {
            Instance.Undo.ChangeCanExecute();
            Instance.Redo.ChangeCanExecute();
        }

        public Command Undo { get; set; }
        public Command Redo { get; set; }

        public static void MakeHistory(string h = null)
        {
            Player.MakeHistory(h);
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
            
            if (Player.Current != null)
            {
                Player p = Player.Current;
                if (Saving.Enter())
                {
                    if (p.FilePath is IFile file)
                    {
                        Player.UnsavedChanges = 0;
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
            HitDice.ReplaceRange(from h in Player.Current.GetHitDie() orderby h select new HitDieViewModel(h));
            OnPropertyChanged(null);
            Skills.ReplaceRange(Player.Current.GetSkills());
            Scores.Update(Player.Current);
            ShowClasses.ChangeCanExecute();
            ShowRace.ChangeCanExecute();
            ShowBackground.ChangeCanExecute();
        }
        public ImageSource Portrait
        {
            get { if (Player.Current != null && Player.Current.Portrait != null) return ImageSource.FromStream(() => new MemoryStream(Player.Current.Portrait)); return null; }
        }

        public ImageSource FactionImage
        {
            get { if (Player.Current != null && Player.Current.FactionImage != null) return ImageSource.FromStream(() => new MemoryStream(Player.Current.FactionImage)); return null; }
        }

        public string HP {
            get {
                return (Player.Current.GetHitpointMax() + Player.Current.CurrentHPLoss).ToString();
            }
            set
            {
                if (HP == value) return;
                int parsedInt = 0;
                if (value == "" || value == "-"|| int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    MakeHistory("CurHP");
                    int loss = parsedInt - Player.Current.GetHitpointMax();
                    if (loss > 0) loss = 0;
                    Player.Current.CurrentHPLoss = loss;
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("HP");
            }
        }

        public string HPMax
        {
            get
            {
                return "Current: (Max " + Player.Current.GetHitpointMax() + ")";
            }
        }
        public string BonusHP
        {
            get
            {
                return Player.Current.BonusMaxHP.ToString();
            }
            set
            {
                if (BonusHP == value) return;
                int parsedInt = 0;
                if (value == "" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (parsedInt < 0) parsedInt = 0;
                    MakeHistory("CurHP");
                    Player.Current.BonusMaxHP = parsedInt;
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
                return Player.Current.TempHP.ToString();
            }
            set
            {
                if (TempHP == value) return;
                int parsedInt = 0;
                if (value == "" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (parsedInt < 0) parsedInt = 0;
                    MakeHistory("TempHP");
                    Player.Current.TempHP = parsedInt;
                    Save();
                }
                if (value != "") OnPropertyChanged("TempHP");
            }
        }

        public string Race { get { return Player.Current.GetRaceSubName(); } }
        public string Classes {
            get {
                return String.Join(" | ", Player.Current.Classes);
                /**int level = Level;
                int total = 0;
                foreach (PlayerClass pc in Player.Current.Classes) total += pc.getClassLevelUpToLevel(level);
                if (total == level) return a;
                return a.Length > 0 ? a + ", Total Level " + Level : "Level " + Level;*/
            }
        }
        public string Name { get { return Player.Current.Name; } }
        public int Level { get { return Player.Current.GetLevel(); } }
        public string LevelText { get { return "(Level " + Player.Current.GetLevel().ToString() + ")"; } }
        public string RaceAndLevel { get { return Race + " (" + Level + ")"; } }
        public string Background { get { return Player.Current?.Background?.Name; } }
        public string Alignment { get { return Player.Current?.Alignment; } }
        public int AC { get { return Player.Current.GetAC(); } }
        public string Initiative { get { return Player.Current.GetInitiative().ToString("+#;-#;0"); } }
        public string Proficiency { get { return Player.Current.GetProficiency().ToString("+#;-#;0"); } }
        public int Speed { get { return Player.Current.GetSpeed(); } }
        public bool Inspiration {
            get {
                return Player.Current.Inspiration;
            }
            set
            {
                if (Inspiration == value) return;
                Player.Current.Inspiration = value;
                MakeHistory("Inspiration");
                OnPropertyChanged("Inspiration");
                Save();
            }
        }
        public int XP { get { return Player.Current.GetXP(); } }
        public string Carried {
            get {
                double carry = Player.Current.GetCarryCapacity();
                double weight = Player.Current.GetWeight();
                return weight.ToString("N") + " / " + carry.ToString("N0") + " lb";
            }
        }

        public string Money
        {
            get
            {
                return Player.Current.GetMoney().ToString();
            }
        }

        public string HDTotal {
            get {
                List<HitDie> hd = Player.Current.GetHitDie();
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
                return Player.Current.SuccessDeathSaves >= 0 && Player.Current.SuccessDeathSaves <= 3 ? Player.Current.SuccessDeathSaves : 0;
            }
            set
            {
                if (DeathSaveSuccess == value) return;
                MakeHistory("SuccessDeathSaves");
                Player.Current.SuccessDeathSaves = value;
                Save();
            }
        }
        public int DeathSaveFail {
            get {
                return Player.Current.FailedDeathSaves >= 0 && Player.Current.FailedDeathSaves <= 3 ? Player.Current.FailedDeathSaves : 0;
            }
            set
            {
                if (DeathSaveFail == value) return;
                MakeHistory("FailedDeathSaves");
                Player.Current.FailedDeathSaves = value;
                Save();
            }
        }
        public string PP
        {
            get { return Player.Current.GetMoney().pp.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    Price p = Player.Current.GetMoney();
                    if (p.pp == parsedInt) return;
                    MakeHistory("MoneyPP");
                    Player.Current.SetMoney(p.cp, p.sp, p.ep, p.gp, parsedInt);
                    OnPropertyChanged("Money");
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("PP");
            }
        }
        public string GP
        {
            get { return Player.Current.GetMoney().gp.ToString(); }
            set
            {
                Price p = Player.Current.GetMoney();
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (p.gp == parsedInt) return;
                    MakeHistory("MoneyGP");
                    Player.Current.SetMoney(p.cp, p.sp, p.ep, parsedInt, p.pp);
                    OnPropertyChanged("Money");
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("GP");
            }
        }
        public string EP
        {
            get { return Player.Current.GetMoney().ep.ToString(); }
            set
            {
                Price p = Player.Current.GetMoney();
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (p.ep == parsedInt) return;
                    MakeHistory("MoneyEP");
                    Player.Current.SetMoney(p.cp, p.sp, parsedInt, p.gp, p.pp);
                    OnPropertyChanged("Money");
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("EP");
            }
        }
        public string SP
        {
            get { return Player.Current.GetMoney().sp.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    Price p = Player.Current.GetMoney();
                    if (p.sp == parsedInt) return;
                    MakeHistory("MoneySP");
                    Player.Current.SetMoney(p.cp, parsedInt, p.ep, p.gp, p.pp);
                    OnPropertyChanged("Money");
                    Save();
                }
                if (value != "" && value != "-") OnPropertyChanged("SP");
            }
        }
        public string CP
        {
            get { return Player.Current.GetMoney().cp.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    Price p = Player.Current.GetMoney();
                    if (p.cp == parsedInt) return;
                    MakeHistory("MoneyCP");
                    Player.Current.SetMoney(parsedInt, p.sp, p.ep, p.gp, p.pp);
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
                if (SelectedSkill != null) return ": " + Player.Current.GetSkill(SelectedSkill.Skill, _baseAbility).ToString("+#;-#;0");
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
    }

}
