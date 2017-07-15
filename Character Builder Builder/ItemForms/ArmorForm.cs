using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class ArmorForm : Form, IMainEditor
    {
        public static LinkedList<Armor> UndoBuffer = new LinkedList<Armor>();
        public static LinkedList<Armor> RedoBuffer = new LinkedList<Armor>();
        private string lastid = null;
        private int UnsavedChanges;
        private static int MaxBuffer = 100;
        private Armor Armor;
        private bool doHistory = false;
        public ArmorForm(Armor container)
        {
            InitializeComponent();
            this.Armor = container;
            userControl11.Editor = this;
            basicItem1.HistoryManager = this;
            refresh();
        }

        private void refresh()
        {
            bool oldHistory = doHistory;
            doHistory = false;
            basicItem1.Item = Armor;
            BaseAC.DataBindings.Clear();
            BaseAC.DataBindings.Add("Value", Armor, "BaseAC", true, DataSourceUpdateMode.OnPropertyChanged);
            StrengthRequired.DataBindings.Clear();
            StrengthRequired.DataBindings.Add("Value", Armor, "StrengthRequired", true, DataSourceUpdateMode.OnPropertyChanged);
            DisStealth.DataBindings.Clear();
            DisStealth.DataBindings.Add("Checked", Armor, "StealthDisadvantage", true, DataSourceUpdateMode.OnPropertyChanged);
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(Armor.ToHTML());
            preview.Refresh();
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
            preview.Document.Write(Armor.ToHTML());
            preview.Refresh();
        }


        public void MakeHistory(string id)
        {
            if (!doHistory) return;
            if (id == null) id = "";
            if (id == "" || id != lastid)
            {
                UndoBuffer.AddLast((Armor)Armor.Clone());
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
                RedoBuffer.AddLast(Armor);
                Armor = UndoBuffer.Last.Value;
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
                UndoBuffer.AddLast(Armor);
                Armor = RedoBuffer.Last.Value;
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
            if (Armor.Name == null || Armor.Name.Length == 0)
            {
                MessageBox.Show("Unable to save without a name");
                return false;
            }
            try
            {
                bool saved = Armor.Save(false);
                if (!saved && MessageBox.Show("File exists! Overwrite?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes) saved = Armor.Save(true);
                if (saved)
                {
                    UnsavedChanges = 0;
                    Saved?.Invoke(this, Armor.Name + " " + ConfigManager.SourceSeperator + " " + Armor.Source);
                }
                return saved;
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
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

        private void DisStealth_CheckedChanged(object sender, EventArgs e)
        {
            MakeHistory(null);
        }

        private void BaseAC_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("BaseAC");
        }

        private void StrengthRequired_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("StrengthRequired");
        }
    }
}
