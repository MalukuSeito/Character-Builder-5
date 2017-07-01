using OGL;
using OGL.Base;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class SkillForm : Form, IMainEditor
    {
        public LinkedList<Skill> UndoBuffer = new LinkedList<Skill>();
        public LinkedList<Skill> RedoBuffer = new LinkedList<Skill>();
        private string lastid = null;
        private int UnsavedChanges;
        private static int MaxBuffer = 100;
        private Skill skill;
        private bool doHistory = false;
        public SkillForm(Skill race)
        {
            InitializeComponent();
            this.skill = race;
            userControl11.Editor = this;
            refresh();
        }

        private void refresh()
        {
            bool oldHistory = doHistory;
            doHistory = false;
            name.DataBindings.Clear();
            name.DataBindings.Add("Text", skill, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            Ability.Items.Clear();
            foreach (Ability a in Enum.GetValues(typeof(Ability))) if (a != OGL.Base.Ability.None) Ability.Items.Add(a, skill.Base.HasFlag(a));
            source.DataBindings.Clear();
            source.DataBindings.Add("Text", skill, "Source", true, DataSourceUpdateMode.OnPropertyChanged);
            description.DataBindings.Clear();
            NewlineFormatter.Add(description.DataBindings, "Text", skill, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(skill.ToHTML());
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
            preview.Document.Write(skill.ToHTML());
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
                UndoBuffer.AddLast(skill.Clone());
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
                RedoBuffer.AddLast(skill);
                skill = UndoBuffer.Last.Value;
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
                UndoBuffer.AddLast(skill);
                skill = RedoBuffer.Last.Value;
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
        public bool Save()
        {
            if (name.Text == null || name.Text.Length == 0)
            {
                MessageBox.Show("Unable to save without a name");
                return false;
            }
            bool saved = skill.Save(false);
            if (!saved && MessageBox.Show("File exists! Overwrite?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes) saved = skill.Save(true);
            if (saved)
            {
                UnsavedChanges = 0;
                Saved?.Invoke(this, skill.Name + " " + ConfigManager.SourceSeperator + " " + skill.Source);
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

        private void Ability_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MakeHistory(null);
            if (e.NewValue == CheckState.Checked) skill.Base |= (Ability)Ability.Items[e.Index];
            else if (e.NewValue == CheckState.Unchecked) skill.Base &= ~(Ability)Ability.Items[e.Index];
        }
    }
}
