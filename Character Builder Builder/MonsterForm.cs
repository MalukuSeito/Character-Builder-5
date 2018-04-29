using Character_Builder_Forms;
using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class MonsterForm : Form, IMainEditor, IImageEditor
    {
        public LinkedList<Monster> UndoBuffer = new LinkedList<Monster>();
        public LinkedList<Monster> RedoBuffer = new LinkedList<Monster>();
        private string lastid = null;
        private int UnsavedChanges;
        private static int MaxBuffer = 100;
        private Monster monster;
        private bool doHistory = false;
        public MonsterForm(Monster monster)
        {
            InitializeComponent();
            Program.Context.ImportSpells();
            Spells.Suggestions = Program.Context.SpellsSimple.Keys;
            Program.Context.ImportLanguages();
            Languages.Suggestions = Program.Context.LanguagesSimple.Keys;
            this.monster = monster;
            userControl11.Editor = this;
            imageChooser1.Image = this;
            refresh();
            decriptions1.HistoryManager = this;
            imageChooser1.History = this;
            Resistances.HistoryManager = this;
            Vulnerablities.HistoryManager = this;
            Immunities.HistoryManager = this;
            ConditionImmunities.HistoryManager = this;
            Keywords.HistoryManager = this;
            Spells.HistoryManager = this;
            Slots.HistoryManager = this;
            Speeds.HistoryManager = this;
            Senses.HistoryManager = this;
            Languages.HistoryManager = this;
            Traits.HistoryManager = this;
            Actions.HistoryManager = this;
            Reactions.HistoryManager = this;
            LegendaryActions.HistoryManager = this;
            Keywords.Group = KeywordControl.KeywordGroup.MONSTER;
            foreach (OGL.Base.Size s in Enum.GetValues(typeof(OGL.Base.Size))) size.Items.Add(s);

            monsterSkill1.HistoryManager = this;
            monsterSave1.HistoryManager = this;
        }

        private void refresh()
        {
            bool oldHistory = doHistory;
            doHistory = false;
            name.DataBindings.Clear();
            name.DataBindings.Add("Text", monster, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            flavour.DataBindings.Clear();
            NewlineFormatter.Add(flavour.DataBindings, "Text", monster, "Flavour", true, DataSourceUpdateMode.OnPropertyChanged);
            
            size.DataBindings.Clear();
            size.DataBindings.Add("SelectedItem", monster, "Size", true, DataSourceUpdateMode.OnPropertyChanged);
            source.DataBindings.Clear();
            source.DataBindings.Add("Text", monster, "Source", true, DataSourceUpdateMode.OnPropertyChanged);
            description.DataBindings.Clear();
            NewlineFormatter.Add(description.DataBindings, "Text", monster, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            decriptions1.descriptions = monster.Descriptions;
            ImageChanged?.Invoke(this, monster.GetImage());

            Alignment.DataBindings.Clear();
            Alignment.DataBindings.Add("Text", monster, "Alignment", true, DataSourceUpdateMode.OnPropertyChanged);
            ACText.DataBindings.Clear();
            ACText.DataBindings.Add("Text", monster, "ACText", true, DataSourceUpdateMode.OnPropertyChanged);
            AC.DataBindings.Clear();
            AC.DataBindings.Add("Value", monster, "AC", true, DataSourceUpdateMode.OnPropertyChanged);
            HPRoll.DataBindings.Clear();
            HPRoll.DataBindings.Add("Text", monster, "HPRoll", true, DataSourceUpdateMode.OnPropertyChanged);
            HP.DataBindings.Clear();
            HP.DataBindings.Add("Value", monster, "HP", true, DataSourceUpdateMode.OnPropertyChanged);

            Strength.DataBindings.Clear();
            Strength.DataBindings.Add("Value", monster, "Strength", true, DataSourceUpdateMode.OnPropertyChanged);
            Dexterity.DataBindings.Clear();
            Dexterity.DataBindings.Add("Value", monster, "Dexterity", true, DataSourceUpdateMode.OnPropertyChanged);
            Constitution.DataBindings.Clear();
            Constitution.DataBindings.Add("Value", monster, "Constitution", true, DataSourceUpdateMode.OnPropertyChanged);
            Intelligence.DataBindings.Clear();
            Intelligence.DataBindings.Add("Value", monster, "Intelligence", true, DataSourceUpdateMode.OnPropertyChanged);
            Wisdom.DataBindings.Clear();
            Wisdom.DataBindings.Add("Value", monster, "Wisdom", true, DataSourceUpdateMode.OnPropertyChanged);
            Charisma.DataBindings.Clear();
            Charisma.DataBindings.Add("Value", monster, "Charisma", true, DataSourceUpdateMode.OnPropertyChanged);

            PassivePerception.DataBindings.Clear();
            PassivePerception.DataBindings.Add("Value", monster, "PassivePerception", true, DataSourceUpdateMode.OnPropertyChanged);

            CR.DataBindings.Clear();
            CR.DataBindings.Add("Value", monster, "CR", true, DataSourceUpdateMode.OnPropertyChanged);
            XP.DataBindings.Clear();
            XP.DataBindings.Add("Value", monster, "XP", true, DataSourceUpdateMode.OnPropertyChanged);

            Resistances.Items = monster.Resistances;
            Vulnerablities.Items = monster.Vulnerablities;
            Immunities.Items = monster.Immunities;
            ConditionImmunities.Items = monster.ConditionImmunities;
            Keywords.Keywords = monster.Keywords;
            Speeds.Items = monster.Speeds;
            Senses.Items = monster.Senses;
            Languages.Items = monster.Languages;
            Spells.Items = monster.Spells;
            Slots.Items = monster.Slots;

            monsterSkill1.Items = monster.SkillBonus;
            monsterSave1.Items = monster.SaveBonus;

            Traits.Traits = monster.Traits;
            Actions.Traits = monster.Actions;
            Reactions.Traits = monster.Reactions;
            LegendaryActions.Traits = monster.LegendaryActions;

            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(monster.ToHTML());
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
            preview.Document.Write(monster.ToHTML());
            preview.Refresh();
        }


        public void MakeHistory(string id)
        {
            if (!doHistory) return;
            if (id == null) id = "";
            if (id == "" || id != lastid)
            {
                UndoBuffer.AddLast(monster.Clone());
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
                RedoBuffer.AddLast(monster);
                monster = UndoBuffer.Last.Value;
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
                UndoBuffer.AddLast(monster);
                monster = RedoBuffer.Last.Value;
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
            bool saved = monster.Save(false);
            if (!saved && MessageBox.Show("File exists! Overwrite?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes) saved = monster.Save(true);
            if (saved)
            {
                UnsavedChanges = 0;
                Saved?.Invoke(this, monster.Name + " " + ConfigManager.SourceSeperator + " " + monster.Source);
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

        public void SetImage(Bitmap Image)
        {
            monster.SetImage(Image);
            ImageChanged?.Invoke(this, Image);
            ShowPreview();
        }

        private void Alignment_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Alignment");
        }

        private void ACText_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("ACText");
        }

        private void AC_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("AC");
        }

        private void Strength_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("Strength");
        }

        private void HP_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("HP");
        }

        private void HPRoll_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("HPRoll");
        }

        private void Dexterity_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("Dexterity");
        }

        private void Constitution_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("Constitution");
        }

        private void Intelligence_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("Intelligence");
        }

        private void Wisdom_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("Wisdom");
        }

        private void Charisma_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("Charisma");
        }

        private void PassivePerception_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("PassivePerception");
        }

        private void CR_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("CR");
        }

        private void XP_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("XP");
        }

        private void splitContainer17_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
