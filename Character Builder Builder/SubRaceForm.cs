using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class SubRaceForm : Form, IMainEditor, IImageEditor
    {
        public LinkedList<SubRace> UndoBuffer = new LinkedList<SubRace>();
        public LinkedList<SubRace> RedoBuffer = new LinkedList<SubRace>();
        private string lastid = null;
        private int UnsavedChanges;
        private static int MaxBuffer = 100;
        private SubRace race;
        private bool doHistory = false;
        public SubRaceForm(SubRace race)
        {
            InitializeComponent();
            this.race = race;
            userControl11.Editor = this;
            imageChooser1.Image = this;
            refresh();
            features1.HistoryManager = this;
            decriptions1.HistoryManager = this;
            imageChooser1.History = this;
            Race.ImportAll();
            foreach (string s in Race.simple.Keys)
            {
                ParentRace.AutoCompleteCustomSource.Add(s);
                ParentRace.Items.Add(s);
            }
        }

        private void refresh()
        {
            bool oldHistory = doHistory;
            doHistory = false;
            name.DataBindings.Clear();
            name.DataBindings.Add("Text", race, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            flavour.DataBindings.Clear();
            NewlineFormatter.Add(flavour.DataBindings, "Text", race, "Flavour", true, DataSourceUpdateMode.OnPropertyChanged);
            source.DataBindings.Clear();
            source.DataBindings.Add("Text", race, "Source", true, DataSourceUpdateMode.OnPropertyChanged);
            description.DataBindings.Clear();
            NewlineFormatter.Add(description.DataBindings, "Text", race, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            features1.features = race.Features;
            decriptions1.descriptions = race.Descriptions;
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(race.toHTML());
            ParentRace.DataBindings.Clear();
            ParentRace.DataBindings.Add("Text", race, "RaceName", true, DataSourceUpdateMode.OnPropertyChanged);
            ImageChanged?.Invoke(this, race.Image);
            preview.Refresh();
            source.AutoCompleteCustomSource.Clear();
            source.AutoCompleteCustomSource.AddRange(SourceManager.Sources.ToArray());
            onChange();
            doHistory = oldHistory;
        }



        private void showPreview(object sender, EventArgs e)
        {
            ShowPreview();
        }

        public void ShowPreview()
        {
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(race.toHTML());
            preview.Refresh();
        }


        public void MakeHistory(string id)
        {
            if (!doHistory) return;
            if (id == null) id = "";
            if (id == "" || id != lastid)
            {
                UndoBuffer.AddLast(race.clone());
                RedoBuffer.Clear();
                onChange();
                if (UndoBuffer.Count > MaxBuffer) UndoBuffer.RemoveFirst();
                UnsavedChanges++;
            }
            lastid = id;
        }
        public bool Undo()
        {
            if (UndoBuffer.Count > 0)
            {
                lastid = "";
                RedoBuffer.AddLast(race);
                race = UndoBuffer.Last.Value;
                UndoBuffer.RemoveLast();
                if (UnsavedChanges > 0) UnsavedChanges--;
                refresh();
                return true;
            }
            return false;
        }
        public bool Redo()
        {
            if (RedoBuffer.Count > 0)
            {
                lastid = "";
                UndoBuffer.AddLast(race);
                race = RedoBuffer.Last.Value;
                RedoBuffer.RemoveLast();
                UnsavedChanges++;
                refresh();
                return true;
            }
            return false;
        }
        public bool CanUndo()
        {
            return UndoBuffer.Count > 0;
        }
        public bool CanRedo()
        {
            return RedoBuffer.Count > 0;
        }

        public event HistoryButtonChangeEvent ButtonChange;

        private void onChange()
        {
            ButtonChange?.Invoke(this, CanUndo(), CanRedo());
        }

        public event SavedEvent Saved;
        public event ImageChanged ImageChanged;

        public bool Save()
        {
            if (name.Text == null || name.Text.Length == 0)
            {
                MessageBox.Show("Unable to save without a name");
                return false;
            }
            bool saved = race.save(false);
            if (!saved && MessageBox.Show("File exists! Overwrite?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes) saved = race.save(true);
            if (saved)
            {
                UnsavedChanges = 0;
                Saved?.Invoke(this, race.Name + " " + ConfigManager.SourceSeperator + " " + race.Source);
            }
            return saved;
        }
        public void Exit()
        {
            this.Close();    
        }

        private bool checkClose()
        {
            if (UnsavedChanges > 0 && MessageBox.Show("There are " + UnsavedChanges + " unsaved changes? Do you want to discard them?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return false;
            }
            return true;
        }

        private void RaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!checkClose()) e.Cancel = true;
        }

        private void name_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("name");
        }

        private void flavour_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("flavour");
        }

        private void size_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Size");
        }

        private void source_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Source");
        }

        private void description_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Desc");
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (Save()) Exit();
        }

        private void abort_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void RaceForm_Shown(object sender, EventArgs e)
        {
            doHistory = true;
        }

        private void ParentRace_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Parent");
        }

        public void SetImage(Bitmap Image)
        {
            race.Image = Image;
            ImageChanged?.Invoke(this, Image);
            ShowPreview();
        }
    }
}
