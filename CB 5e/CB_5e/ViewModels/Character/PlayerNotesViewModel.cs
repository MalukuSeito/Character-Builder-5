using CB_5e.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class PlayerNotesViewModel : SubModel
    {
        public PlayerNotesViewModel(PlayerModel parent) : base(parent, "Notes")
        {
            Image = ImageSource.FromResource("CB_5e.images.notes.png");
            UpdateNotes();
            NewNote = new Command(() =>
            {
                if (Note != null)
                {
                    MakeHistory();
                    Context.Player.Journal.Add(Note);
                    RefreshNotes.Execute(null);
                    Save();
                }
            }, () => Note != null && Note != "");
            SaveNote = new Command(() =>
            {
                if (selectedNote >= 0)
                {
                    MakeHistory();
                    if (Note != null && Note != "")
                    {
                        Context.Player.Journal[selectedNote] = Note;
                    }
                    else
                    {
                        Context.Player.Journal.RemoveAt(selectedNote);
                    }
                    RefreshNotes.Execute(null);
                    Save();
                }
            }, () => selectedNote >= 0);
            RefreshNotes = new Command(() =>
            {
                NotesBusy = true;
                UpdateNotes();
                NotesBusy = false;
            });
            parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            UpdateNotes();
        }
        public ObservableRangeCollection<string> Notes { get; set; } = new ObservableRangeCollection<string>();
        private string notesSearch;
        public string NotesSearch
        {
            get => notesSearch;
            set
            {
                SetProperty(ref notesSearch, value);
                UpdateNotes();
            }
        }
        public void UpdateNotes() => Notes.ReplaceRange(from t in Context.Player.Journal where notesSearch == null || notesSearch == "" || Culture.CompareInfo.IndexOf(t ?? "", notesSearch, CompareOptions.IgnoreCase) >= 0 select t);

        private int selectedNote = 0;
        public String SelectedNote
        {
            get => selectedNote >= 0 && selectedNote < Notes.Count ? Notes[selectedNote] : null;
            set
            {
                SetProperty(ref selectedNote, Notes.IndexOf(value));
                SaveNote.ChangeCanExecute();
                if (selectedNote >= 0)
                {
                    note = value;
                }
                else
                {
                    note = null;
                }
                OnPropertyChanged("Note");
            }
        }
        private string note;
        public string Note
        {
            get => note;
            set
            {
                SetProperty(ref note, value);
                NewNote.ChangeCanExecute();
            }
        }

        public Command NewNote { get; set; }
        public Command SaveNote { get; set; }
        public Command RefreshNotes { get; set; }
        private bool notesBusy;
        public bool NotesBusy { get => notesBusy; set => SetProperty(ref notesBusy, value); }
    }
}
