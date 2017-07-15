using Character_Builder_Forms;
using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class ConditionForm : Form, IMainEditor, IImageEditor
    {
        public LinkedList<Condition> UndoBuffer = new LinkedList<Condition>();
        public LinkedList<Condition> RedoBuffer = new LinkedList<Condition>();
        private string lastid = null;
        private int UnsavedChanges;
        private static int MaxBuffer = 100;
        private Condition lang;
        private bool doHistory = false;
        public ConditionForm(Condition race)
        {
            InitializeComponent();
            this.lang = race;
            userControl11.Editor = this;
            imageChooser1.Image = this;
            refresh();
            imageChooser1.History = this;
        }

        private void refresh()
        {
            bool oldHistory = doHistory;
            doHistory = false;
            name.DataBindings.Clear();
            name.DataBindings.Add("Text", lang, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            source.DataBindings.Clear();
            source.DataBindings.Add("Text", lang, "Source", true, DataSourceUpdateMode.OnPropertyChanged);
            description.DataBindings.Clear();
            NewlineFormatter.Add(description.DataBindings, "Text", lang, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            ImageChanged?.Invoke(this, lang.Image);
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(lang.ToHTML());
            preview.Refresh();
            source.AutoCompleteCustomSource.Clear();
            source.AutoCompleteCustomSource.AddRange(SourceManager.Sources.ToArray());
            onChange();
            doHistory = oldHistory;
        }

        public void ShowPreview()
        {
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(lang.ToHTML());
            preview.Refresh();
        }

        private void showPreview(object sender, EventArgs e)
        {
            ShowPreview();
        }


        public void MakeHistory(string id)
        {
            if (!doHistory) return;
            if (id == null) id = "";
            if (id == "" || id != lastid)
            {
                UndoBuffer.AddLast(lang.Clone());
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
                RedoBuffer.AddLast(lang);
                lang = UndoBuffer.Last.Value;
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
                UndoBuffer.AddLast(lang);
                lang = RedoBuffer.Last.Value;
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
            bool saved = lang.Save(false);
            if (!saved && MessageBox.Show("File exists! Overwrite?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes) saved = lang.Save(true);
            if (saved)
            {
                UnsavedChanges = 0;
                Saved?.Invoke(this, lang.Name + " " + ConfigManager.SourceSeperator + " " + lang.Source);
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

        public void SetImage(Bitmap Image)
        {
            lang.Image = Image;
            ImageChanged?.Invoke(this, Image);
            ShowPreview();
        }
    }
}
