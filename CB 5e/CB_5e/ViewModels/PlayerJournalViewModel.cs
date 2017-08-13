using CB_5e.Helpers;
using Character_Builder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class PlayerJournalViewModel : SubModel
    {
        public PlayerModel ParentView { get; set; }
        public PlayerJournalViewModel(PlayerModel parent) : base(parent, "Journal")
        {
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
            || Culture.CompareInfo.IndexOf(j.Title, journalSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(j.Time, journalSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(j.Session, journalSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(j.DM, journalSearch, CompareOptions.IgnoreCase) >= 0
            || Culture.CompareInfo.IndexOf(j.Text, journalSearch, CompareOptions.IgnoreCase) >= 0 orderby j.Added select new JournalViewModel(this, j));
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
                int magic = 0;
                foreach (JournalEntry je in Context.Player.ComplexJournal)
                {
                    down += je.Downtime;
                    renown += je.Renown;
                    magic += je.MagicItems;
                }
                return "Total Downtime: " + down + ", Renown : " + renown + ", magic items: " + magic;
            }
        }
    }
}
