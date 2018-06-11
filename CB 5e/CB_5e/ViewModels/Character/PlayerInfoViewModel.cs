using CB_5e.Helpers;
using CB_5e.Views;
using Character_Builder;
using OGL.Base;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class PlayerInfoViewModel : SubModel
    {
        public ScoresModelViewModel Scores { get; private set; }

        public PlayerInfoViewModel(PlayerModel parent) : base(parent, "Overview")
        {
            Image = ImageSource.FromResource("CB_5e.images.overview.png");
            parent.PlayerChanged += Parent_PlayerChanged;
            Scores = new ScoresModelViewModel(Context);
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
            HitDice.ReplaceRange(from h in Context.Player.GetHitDie() orderby h select new HitDieViewModel(this, h));
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            HitDice.ReplaceRange(from h in Context.Player.GetHitDie() orderby h select new HitDieViewModel(this, h));
            Scores.Update(Context.Player);
            ShowClasses.ChangeCanExecute();
            ShowRace.ChangeCanExecute();
            ShowBackground.ChangeCanExecute();
        }

        public ImageSource Portrait
        {
            get { if (Context.Player != null && Context.Player.Portrait != null) return ImageSource.FromStream(() => new MemoryStream(Context.Player.Portrait)); return null; }
        }

        //public ImageSource FactionImage
        //{
        //    get { if (Context.Player != null && Context.Player.FactionImage != null) return ImageSource.FromStream(() => new MemoryStream(Context.Player.FactionImage)); return null; }
        //}

        public string HP
        {
            get
            {
                return (Context.Player.GetHitpointMax() + Context.Player.CurrentHPLoss).ToString();
            }
            set
            {
                if (HP == value) return;
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
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
                    int min = -Context.Player.GetHitpointMax();
                    if (parsedInt < min) parsedInt = min;
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
        public string Classes
        {
            get
            {
                int level = Context.Player.GetLevel();
                return String.Join(" | ", from pc in Context.Player.Classes select pc.ToString(Context, level));
                /**int level = Level;
                int total = 0;
                foreach (PlayerClass pc in Context.Player.Classes) total += pc.getClassLevelUpToLevel(level);
                if (total == level) return a;
                return a.Length > 0 ? a + ", Total Level " + Level : "Level " + Level;*/
            }
        }
        public Command ShowImage { get; private set; }
        public Command ShowClasses { get; private set; }
        public Command ShowRace { get; private set; }
        public Command ShowBackground { get; private set; }
        public string PlayerName { get { return Context.Player.Name; } }
        public int Level { get { return Context.Player.GetLevel(); } }
        public string LevelText { get { return "(Level " + Context.Player.GetLevel().ToString() + ")"; } }
        public string RaceAndLevel { get { return Race + " (" + Level + ")"; } }
        public string Background { get { return Context.Player?.Background?.Name; } }
        public string Alignment { get { return Context.Player?.Alignment; } }
        public int AC { get { return Context.Player.GetAC(); } }
        public string Initiative { get { return Context.Player.GetInitiative().ToString("+#;-#;0"); } }
        public string Proficiency { get { return Context.Player.GetProficiency().ToString("+#;-#;0"); } }
        public int Speed { get { return Context.Player.GetSpeed(); } }
        public bool Inspiration
        {
            get
            {
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
        public string Carried
        {
            get
            {
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

        public string HDTotal
        {
            get
            {
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
        public int DeathSaveSuccess
        {
            get
            {
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
        public int DeathSaveFail
        {
            get
            {
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

        public void DoMoneyChanged()
        {
            OnPropertyChanged("Money");
            OnPropertyChanged("EP");
            OnPropertyChanged("CP");
            OnPropertyChanged("SP");
            OnPropertyChanged("GP");
            OnPropertyChanged("PP");
        }
    }
}
