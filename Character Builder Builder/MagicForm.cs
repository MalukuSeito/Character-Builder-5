using Character_Builder_Forms;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class MagicForm : Form, IMainEditor, IImageEditor
    {
        public LinkedList<MagicProperty> UndoBuffer = new LinkedList<MagicProperty>();
        public LinkedList<MagicProperty> RedoBuffer = new LinkedList<MagicProperty>();
        private string lastid = null;
        private int UnsavedChanges;
        private static int MaxBuffer = 100;
        private MagicProperty cls;
        private bool doHistory = false;
        public MagicForm(MagicProperty cls)
        {
            InitializeComponent();
            this.cls = cls;
            userControl11.Editor = this;
            imageChooser1.Image = this;
            refresh();
            Attuned.HistoryManager = this;
            Equipped.HistoryManager = this;
            Carried.HistoryManager = this;
            OnUse.HistoryManager = this;
            AttunedEquipped.HistoryManager = this;
            AttunedOnUse.HistoryManager = this;
            imageChooser1.History = this;
            foreach (Slot s in Enum.GetValues(typeof(Slot))) Slot.Items.Add(s);
            foreach (Rarity s in Enum.GetValues(typeof(Rarity))) Rarity.Items.Add(s);
            ImportExtensions.ImportItems();

        }

        private void refresh()
        {
            bool oldHistory = doHistory;
            doHistory = false;
            name.DataBindings.Clear();
            name.DataBindings.Add("Text", cls, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            Requirement.DataBindings.Clear();
            Requirement.DataBindings.Add("Text", cls, "Requirement", true, DataSourceUpdateMode.OnPropertyChanged);
            Description.DataBindings.Clear();
            NewlineFormatter.Add(Description.DataBindings, "Text", cls, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            source.DataBindings.Clear();
            source.DataBindings.Add("Text", cls, "Source", true, DataSourceUpdateMode.OnPropertyChanged);
            Base.DataBindings.Clear();
            Base.DataBindings.Add("Text", cls, "Base", true, DataSourceUpdateMode.OnPropertyChanged);
            Slot.DataBindings.Clear();
            Slot.DataBindings.Add("SelectedItem", cls, "Slot", true, DataSourceUpdateMode.OnPropertyChanged);
            Rarity.DataBindings.Clear();
            Rarity.DataBindings.Add("SelectedItem", cls, "Rarity", true, DataSourceUpdateMode.OnPropertyChanged);
            PreName.DataBindings.Clear();
            PreName.DataBindings.Add("Text", cls, "PrependName", true, DataSourceUpdateMode.OnPropertyChanged);
            PostName.DataBindings.Clear();
            PostName.DataBindings.Add("Text", cls, "PostName", true, DataSourceUpdateMode.OnPropertyChanged);
            Attuned.features = cls.AttunementFeatures;
            Equipped.features = cls.EquipFeatures;
            Carried.features = cls.CarryFeatures;
            OnUse.features = cls.OnUseFeatures;
            AttunedOnUse.features = cls.AttunedOnUseFeatures;
            AttunedEquipped.features = cls.AttunedEquipFeatures;
            ImageChanged?.Invoke(this, cls.Image);
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(cls.ToHTML());
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
            preview.Document.Write(cls.ToHTML());
            preview.Refresh();
        }


        public void MakeHistory(string id)
        {
            if (!doHistory) return;
            if (id == null) id = "";
            if (id == "" || id != lastid)
            {
                UndoBuffer.AddLast(cls.Clone());
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
                RedoBuffer.AddLast(cls);
                cls = UndoBuffer.Last.Value;
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
                UndoBuffer.AddLast(cls);
                cls = RedoBuffer.Last.Value;
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
            bool saved = cls.Save(false);
            if (!saved && MessageBox.Show("File exists! Overwrite?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes) saved = cls.Save(true);
            if (saved)
            {
                UnsavedChanges = 0;
                Saved?.Invoke(this, cls.Name + " " + ConfigManager.SourceSeperator + " " + cls.Source);
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

        private void ParentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeHistory("Slot");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Requirement");
        }

        private void Rarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeHistory("Rarity");
        }

        private void PreName_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("PreName");
        }

        private void PostName_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("PostName");
        }

        private void Description_TextChanged_1(object sender, EventArgs e)
        {
            MakeHistory("Desc");
        }

        private void Base_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Condition");
            showPreviewMatching(sender, e);
        }

        private void showPreviewMatching(object sender, EventArgs e)
        {
            if (!doHistory) return;
            if (Base.Text == null || Base.Text == "")
            {
                showPreview(sender, e);
                return;
            }
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            try
            {
                Feature f = new Feature("Matching", "\n" + String.Join("\n", from i in Item.FilterPreview(Base.Text) select i.Name + " " + ConfigManager.SourceSeperator + " " + i.Source), 0, true);
                preview.Document.Write(f.ToHTML());
            }
            catch (Exception ex)
            {
                preview.Document.Write("<html><body><b>Error generating output:</b><br>" + ex.Message + "<br>" + ex.InnerException + "<br>" + ex.StackTrace + "</body></html>");
            }
            preview.Refresh();

        }

        public void SetImage(Bitmap Image)
        {
            cls.Image = Image;
            ImageChanged?.Invoke(this, Image);
            ShowPreview();
        }
    }
}
