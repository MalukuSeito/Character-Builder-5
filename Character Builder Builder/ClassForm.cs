using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Character_Builder_5;
using System.IO;

namespace Character_Builder_Builder
{
    public partial class ClassForm : Form, IMainEditor, IImageEditor
    {
        public LinkedList<ClassDefinition> UndoBuffer = new LinkedList<ClassDefinition>();
        public LinkedList<ClassDefinition> RedoBuffer = new LinkedList<ClassDefinition>();
        private string lastid = null;
        private int UnsavedChanges;
        private static int MaxBuffer = 100;
        private ClassDefinition cls;
        private bool doHistory = false;
        public ClassForm(ClassDefinition cls)
        {
            InitializeComponent();
            this.cls = cls;
            if (cls.MulticlassingCondition == null || cls.MulticlassingCondition.Length == 0)
            {
                List<string> cond = new List<string>();
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Strength)) cond.Add("Strength >= " + ConfigManager.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Dexterity)) cond.Add("Dexterity >= " + ConfigManager.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Constitution)) cond.Add("Constitution >= " + ConfigManager.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Intelligence)) cond.Add("Intelligence >= " + ConfigManager.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Wisdom)) cond.Add("Wisdom >= " + ConfigManager.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Charisma)) cond.Add("Charisma >= " + ConfigManager.MultiClassTarget);
                if (cond.Count > 0) cls.MulticlassingCondition = String.Join(" and ", cond);
                else cls.MulticlassingCondition = "true";
            }
            userControl11.Editor = this;
            imageChooser1.Image = this;
            refresh();
            imageChooser1.History = this;
            features1.HistoryManager = this;
            decriptions1.HistoryManager = this;
            MulticlassSpellLevels.HistoryManager = this;
            classFeats.HistoryManager = this;
            classSpells.HistoryManager = this;
            featuresFirstClass.HistoryManager = this;
            featuresMultiClass.HistoryManager = this;
            FeatureCollection.ImportAll();
            HashSet<string> feats = new HashSet<string>();
            foreach (Feature f in FeatureCollection.Features) feats.Add(f.Name);
            classFeats.Suggestions = feats;
            MulticlassSpellLevels.Items = cls.MulticlassingSpellLevels;
            Spell.ImportAll();
            classSpells.Suggestions = Spell.simple.Keys;
            
        }

        private void refresh()
        {
            bool oldHistory = doHistory;
            doHistory = false;
            name.DataBindings.Clear();
            name.DataBindings.Add("Text", cls, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            flavour.DataBindings.Clear();
            NewlineFormatter.Add(flavour.DataBindings, "Text", cls, "Flavour", true, DataSourceUpdateMode.OnPropertyChanged);
            source.DataBindings.Clear();
            source.DataBindings.Add("Text", cls, "Source", true, DataSourceUpdateMode.OnPropertyChanged);
            description.DataBindings.Clear();
            NewlineFormatter.Add(description.DataBindings, "Text", cls, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            HD.DataBindings.Clear();
            HD.DataBindings.Add("Value", cls, "HitDie", true, DataSourceUpdateMode.OnPropertyChanged);
            HDAverage.DataBindings.Clear();
            HDAverage.DataBindings.Add("Value", cls, "AverageHPPerLevel", true, DataSourceUpdateMode.OnPropertyChanged);
            HDFirst.DataBindings.Clear();
            HDFirst.DataBindings.Add("Value", cls, "HPFirstLevel", true, DataSourceUpdateMode.OnPropertyChanged);
            MulticlassCond.DataBindings.Clear();
            MulticlassCond.DataBindings.Add("Text", cls, "MulticlassingCondition", true, DataSourceUpdateMode.OnPropertyChanged);
            features1.features = cls.Features;
            featuresFirstClass.features = cls.FirstClassFeatures;
            featuresMultiClass.features = cls.MulticlassingFeatures;
            classFeats.Items = cls.FeaturesToAddClassKeywordTo;
            classSpells.Items = cls.SpellsToAddClassKeywordTo;
            ImageChanged?.Invoke(this, cls.Image);
            decriptions1.descriptions = cls.Descriptions;
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(cls.toHTML());
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
            preview.Document.Write(cls.toHTML());
            preview.Refresh();
        }


        public void MakeHistory(string id)
        {
            if (!doHistory) return;
            if (id == null) id = "";
            if (id == "" || id != lastid)
            {
                UndoBuffer.AddLast(cls.clone());
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
            bool saved = cls.save(false);
            if (!saved && MessageBox.Show("File exists! Overwrite?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes) saved = cls.save(true);
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

        private void flavour_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("flavour");
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

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("HD");
        }

        private void HDFirst_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("HPFirst");
        }

        private void HD_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("HPAverage");
        }

        public void SetImage(Bitmap Image)
        {
            cls.Image = Image;
            ImageChanged?.Invoke(this, Image);
            ShowPreview();
        }
    }
}
