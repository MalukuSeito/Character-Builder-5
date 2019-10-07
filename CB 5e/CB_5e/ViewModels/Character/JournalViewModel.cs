using CB_5e.Helpers;
using Character_Builder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class JournalViewModel: ObservableObject
    {

        public JournalViewModel(PlayerJournalViewModel model)
        {
            IsNew = true;
            Context = model;
            Journal = new JournalEntry();
        }

        public JournalViewModel(PlayerJournalViewModel model, JournalEntry entry)
        {
            Context = model;
            Journal = entry;
        }

        public PlayerJournalViewModel Context { get; set; }
        
        public bool IsNew { get; set; }
        public bool XPChanged { get; set; }
        public bool MoneyChanged { get; set; }
        public bool StatsChanged { get; set; }
        private bool changed = false;
        public bool IsChanged
        {
            get => changed;
            set
            {
                if (changed != value)
                {
                    changed = value;
                    if (changed)
                    {
                        Context.MakeHistory();
                    }
                }
            }
        }

        public JournalEntry Journal { get; private set; }

        public String Title {
            get => Journal.Title;
            set {
                if (value != Title)
                {
                    IsChanged = true;
                    Journal.Title = value;
                    OnPropertyChanged("Name");
                }
                OnPropertyChanged("Title");
            }
        }
        public String Text {
            get => Journal.Text;
            set {
                if (value != Text)
                {
                    IsChanged = true;
                    Journal.Text = value;
                }
                OnPropertyChanged("Text");
            }
        }
        public String Time
        {
            get => Journal.Time;
            set
            {
                if (value != Time)
                {
                    IsChanged = true;
                    Journal.Time = value;
                }
                OnPropertyChanged("Time");
            }
        }
        public String Session
        {
            get => Journal.Session;
            set
            {
                if (value != Session)
                {
                    IsChanged = true;
                    Journal.Session = value;
                }
                OnPropertyChanged("Session");
            }
        }
        public String DM
        {
            get => Journal.DM;
            set
            {
                if (value != DM)
                {
                    IsChanged = true;
                    Journal.DM = value;
                }
                OnPropertyChanged("DM");
            }
        }
        public String XP
        {
            get => Journal.XP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.XP == parsedInt) return;
                    IsChanged = true;
                    XPChanged = true;
                    Journal.XP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("XP");
                }
                if (value != "" && value != "-") OnPropertyChanged("XP");
            }
        }
        public String PP
        {
            get => Journal.PP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.PP == parsedInt) return;
                    IsChanged = true;
                    MoneyChanged = true;
                    Journal.PP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("PP");
                }
                if (value != "" && value != "-") OnPropertyChanged("PP");
            }
        }
        public String GP
        {
            get => Journal.GP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.GP == parsedInt) return;
                    IsChanged = true;
                    MoneyChanged = true;
                    Journal.GP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("GP");
                }
                if (value != "" && value != "-") OnPropertyChanged("GP");
            }
        }
        public String SP
        {
            get => Journal.SP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.SP == parsedInt) return;
                    IsChanged = true;
                    MoneyChanged = true;
                    Journal.SP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("SP");
                }
                if (value != "" && value != "-") OnPropertyChanged("SP");
            }
        }
        public String EP
        {
            get => Journal.EP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.EP == parsedInt) return;
                    IsChanged = true;
                    MoneyChanged = true;
                    Journal.EP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("EP");
                }
                if (value != "" && value != "-") OnPropertyChanged("EP");
            }
        }
        public String CP
        {
            get => Journal.CP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.CP == parsedInt) return;
                    IsChanged = true;
                    MoneyChanged = true;
                    Journal.CP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("CP");
                }
                if (value != "" && value != "-") OnPropertyChanged("CP");
            }
        }
        public String Downtime
        {
            get => Journal.Downtime.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.Downtime == parsedInt) return;
                    IsChanged = true;
                    StatsChanged = true;
                    Journal.Downtime = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("Downtime");
                }
                if (value != "" && value != "-") OnPropertyChanged("Downtime");
            }
        }
        public String MagicItems
        {
            get => Journal.MagicItems.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.MagicItems == parsedInt) return;
                    IsChanged = true;
                    StatsChanged = true;
                    Journal.MagicItems = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("MagicItems");
                }
                if (value != "" && value != "-") OnPropertyChanged("MagicItems");
            }
        }

        public String Renown
        {
            get => Journal.Renown.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.Renown == parsedInt) return;
                    IsChanged = true;
                    StatsChanged = true;
                    Journal.Renown = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("Renown");
                }
                if (value != "" && value != "-") OnPropertyChanged("Renown");
            }
        }
        public String AP
        {
            get => Journal.AP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.AP == parsedInt) return;
                    IsChanged = true;
                    StatsChanged = true;
                    Journal.AP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("AP");
                }
                if (value != "" && value != "-") OnPropertyChanged("AP");
            }
        }
        public String T1TP
        {
            get => Journal.T1TP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.T1TP == parsedInt) return;
                    IsChanged = true;
                    StatsChanged = true;
                    Journal.T1TP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("T1TP");
                }
                if (value != "" && value != "-") OnPropertyChanged("T1TP");
            }
        }
        public String T2TP
        {
            get => Journal.T2TP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.T2TP == parsedInt) return;
                    IsChanged = true;
                    StatsChanged = true;
                    Journal.T2TP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("T2TP");
                }
                if (value != "" && value != "-") OnPropertyChanged("T2TP");
            }
        }
        public String T3TP
        {
            get => Journal.T3TP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.T3TP == parsedInt) return;
                    IsChanged = true;
                    StatsChanged = true;
                    Journal.T3TP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("T3TP");
                }
                if (value != "" && value != "-") OnPropertyChanged("T3TP");
            }
        }
        public String T4TP
        {
            get => Journal.T4TP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Journal.T4TP == parsedInt) return;
                    IsChanged = true;
                    StatsChanged = true;
                    Journal.T4TP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("T4TP");
                }
                if (value != "" && value != "-") OnPropertyChanged("T4TP");
            }
        }
        public bool InSheet
        {
            get => Journal.InSheet;
            set
            {
                if (value != InSheet)
                {
                    IsChanged = true;
                    Journal.InSheet = value;
                }
                OnPropertyChanged("InSheet");
            }
        }

        public bool Milestone
        {
            get => Journal.Milestone;
            set
            {
                if (value != Milestone)
                {
                    IsChanged = true;
                    Journal.Milestone = value;
                }
                OnPropertyChanged("Milestone");
            }
        }

        public DateTime Date
        {
            get => Journal.Added.Date;
            set
            {
                DateTime v = value.Date;
                TimeSpan t = AddedTime;
                v = v + t;
                if (v != Journal.Added)
                {
                    IsChanged = true;
                    Journal.Added = v;
                }
                OnPropertyChanged("AddedTime");
                OnPropertyChanged("Date");
                OnPropertyChanged("Added");
            }
        }

        public TimeSpan AddedTime
        {
            get => Journal.Added - Journal.Added.Date;
            set
            {
                if (value != Journal.Added - Journal.Added.Date)
                {
                    IsChanged = true;
                    Journal.Added = Journal.Added.Date + value;
                }
                OnPropertyChanged("Date");
                OnPropertyChanged("AddedTime");
                OnPropertyChanged("Added");
            }
        }

        public string Added { get => Journal.Added.ToString("d"); }

        public string Desc { get => Journal.GetChanges(); }
        public string Name { get => Added + ": " + Title; }
    }
}
