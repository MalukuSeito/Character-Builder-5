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
    public class PlayerJournalViewModel : SubModel
    {
        public PlayerModel ParentView { get; set; }
        public PlayerJournalViewModel(PlayerModel parent) : base(parent, "Journal")
        {
            Image = ImageSource.FromResource("CB_5e.images.journal.png");
            ParentView = parent;
            UpdateJournal();

            RefreshJournal = new Command(() =>
            {
                JournalBusy = true;
                UpdateJournal();
                JournalBusy = false;
            });
            parent.PlayerChanged += Parent_PlayerChanged;

        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            UpdateJournal();
        }

        public ObservableRangeCollection<JournalViewModel> JournalEntries { get; set; } = new ObservableRangeCollection<JournalViewModel>();
        
        private string journalSearch;
        public string JournalSearch
        {
            get => journalSearch;
            set
            {
                SetProperty(ref journalSearch, value);
                UpdateJournal();
            }
        }

        

        public void UpdateJournal() {
            JournalEntries.ReplaceRange(from j in Context.Player.ComplexJournal where journalSearch == null || journalSearch == ""
            || Culture.CompareInfo.IndexOf(j.Title ?? "", journalSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(j.Time ?? "", journalSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(j.Session ?? "", journalSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(j.DM ?? "", journalSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(j.Text ?? "", journalSearch, CompareOptions.IgnoreCase) >= 0 orderby j.Added select new JournalViewModel(this, j));
        }
        
        public Command RefreshJournal { get; set; }
        private bool journalBusy;
        public bool JournalBusy { get => journalBusy; set => SetProperty(ref journalBusy, value); }

        public void StatsChanged()
        {
            OnPropertyChanged("JournalStats");
        }

        public string JournalStats
        {
            get
            {
                int down = 0;
                int renown = 0;
                int t1tp = 0;
                int t2tp = 0;
                int t3tp = 0;
                int t4tp = 0;
                int magic = 0;
                foreach (JournalEntry je in Context.Player.ComplexJournal)
                {
                    down += je.Downtime;
                    renown += je.Renown;
                    magic += je.MagicItems;
                    t1tp += je.T1TP;
                    t2tp += je.T2TP;
                    t3tp += je.T3TP;
                    t4tp += je.T4TP;
                }
                List<string> c = new List<string>();
                if (down != 0) c.Add(down + " Downtime");
                if (renown != 0) c.Add(renown + " Renown");
                if (magic != 0) c.Add(magic + " Magic Items");
                if (t1tp != 0) c.Add(t1tp + " Tier 1 TP");
                if (t2tp != 0) c.Add(t2tp + " Tier 2 TP");
                if (t3tp != 0) c.Add(t3tp + " Tier 3 TP");
                if (t4tp != 0) c.Add(t4tp + " Tier 4 TP");
                return "Total: " + String.Join(", ", c);
            }
        }
    }
}
