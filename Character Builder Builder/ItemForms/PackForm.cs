using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class PackForm : Form, IMainEditor
    {
        public static LinkedList<Pack> UndoBuffer = new LinkedList<Pack>();
        public static LinkedList<Pack> RedoBuffer = new LinkedList<Pack>();
        private string lastid = null;
        private int UnsavedChanges;
        private static int MaxBuffer = 100;
        private Pack Pack;
        private bool doHistory = false;
        public PackForm(Pack container)
        {
            InitializeComponent();
            this.Pack = container;
            userControl11.Editor = this;
            basicItem1.HistoryManager = this;
            stringList1.HistoryManager = this;
            ImportExtensions.ImportItems();
            stringList1.Suggestions = Item.simple.Keys;
            refresh();
        }

        private void refresh()
        {
            bool oldHistory = doHistory;
            doHistory = false;
            basicItem1.Item = Pack;
            stringList1.Items = Pack.Contents;
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(Pack.ToHTML());
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
            preview.Document.Write(Pack.ToHTML());
            preview.Refresh();
        }


        public void MakeHistory(string id)
        {
            if (!doHistory) return;
            if (id == null) id = "";
            if (id == "" || id != lastid)
            {
                UndoBuffer.AddLast((Pack)Pack.Clone());
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
                RedoBuffer.AddLast(Pack);
                Pack = UndoBuffer.Last.Value;
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
                UndoBuffer.AddLast(Pack);
                Pack = RedoBuffer.Last.Value;
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
            if (Pack.Name == null || Pack.Name.Length == 0)
            {
                MessageBox.Show("Unable to save without a name");
                return false;
            }
            Pack.Weight = 0;
            foreach (string i in Pack.Contents)
            {
                Pack.Weight += Item.Get(i, Pack.Source).Weight;
            }
            try
            {
                bool saved = Pack.Save(false);
                if (!saved && MessageBox.Show("File exists! Overwrite?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes) saved = Pack.Save(true);
                if (saved)
                {
                    UnsavedChanges = 0;
                    Saved?.Invoke(this, Pack.Name + " " + ConfigManager.SourceSeperator + " " + Pack.Source);
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
    }
}
