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
using System.Text.RegularExpressions;
using System.IO;

namespace Character_Builder_Builder
{
    public partial class SpellForm : Form, IMainEditor
    {
        public LinkedList<Spell> UndoBuffer = new LinkedList<Spell>();
        public LinkedList<Spell> RedoBuffer = new LinkedList<Spell>();
        private static List<string> castingtime;
        private static List<string> range;
        private static List<string> duration;
        private string lastid = null;
        private int UnsavedChanges;
        private static int MaxBuffer = 100;
        private Spell spell;
        private bool doHistory = false;
        public SpellForm(Spell spell)
        {
            InitializeComponent();
            this.spell = spell;
            userControl11.Editor = this;
            decriptions1.HistoryManager = this;
            keywordControl1.HistoryManager = this;
            if (castingtime == null)
            {
                castingtime = new List<string>();
                castingtime.Add("1 action");
                castingtime.Add("1 bonus action");
                castingtime.Add("1 minute");
                castingtime.Add("10 minutes");
                castingtime.Add("1 hour");
            }
            if (range == null)
            {
                range = new List<string>();
                range.Add("Self");
                range.Add("Touch");
                range.Add("30 feet");
                range.Add("60 feet");
                range.Add("90 feet");
                range.Add("120 feet");
            }
            if (duration == null)
            {
                duration = new List<string>();
                duration.Add("Instantaneous");
                duration.Add("1 round");
                duration.Add("1 minute");
                duration.Add("10 minutes");
                duration.Add("1 hour");
                duration.Add("8 hours");
                duration.Add("24 hours");
                duration.Add("Concentration, up to 1 minute");
                duration.Add("Concentration, up to 10 minutes");
                duration.Add("Concentration, up to 1 hour");
                duration.Add("Concentration, up to 8 hours");
            }
            foreach (String s in duration)
            {
                Duration.Items.Add(s);
                Duration.AutoCompleteCustomSource.Add(s);
            }
            foreach (String s in range)
            {
                Range.Items.Add(s);
                Range.AutoCompleteCustomSource.Add(s);
            }
            foreach (String s in castingtime)
            {
                CastingTime.Items.Add(s);
                CastingTime.AutoCompleteCustomSource.Add(s);
            }
            refresh();
        }

        private void refresh()
        {
            bool oldHistory = doHistory;
            doHistory = false;
            name.DataBindings.Clear();
            name.DataBindings.Add("Text", spell, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            source.DataBindings.Clear();
            source.DataBindings.Add("Text", spell, "Source", true, DataSourceUpdateMode.OnPropertyChanged);
            description.DataBindings.Clear();
            NewlineFormatter.Add(description.DataBindings, "Text", spell, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            Level.DataBindings.Clear();
            Level.DataBindings.Add("Value", spell, "Level", true, DataSourceUpdateMode.OnPropertyChanged);
            CastingTime.DataBindings.Clear();
            CastingTime.DataBindings.Add("Text", spell, "CastingTime", true, DataSourceUpdateMode.OnPropertyChanged);
            Range.DataBindings.Clear();
            Range.DataBindings.Add("Text", spell, "Range", true, DataSourceUpdateMode.OnPropertyChanged);
            Duration.DataBindings.Clear();
            Duration.DataBindings.Add("Text", spell, "Duration", true, DataSourceUpdateMode.OnPropertyChanged);
            dataGridView1.DataSource = new BindingList<CantripDamage>(spell.CantripDamage);
            keywordControl1.Keywords = spell.Keywords;
            decriptions1.descriptions = spell.Descriptions;
            preview.Navigate("about:blank");
            preview.Document.OpenNew(true);
            preview.Document.Write(spell.toHTML());
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
            preview.Document.Write(spell.toHTML());
            preview.Refresh();
        }


        public void MakeHistory(string id)
        {
            if (!doHistory) return;
            if (id == null) id = "";
            if (id == "" || id != lastid)
            {
                UndoBuffer.AddLast(spell.clone());
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
                RedoBuffer.AddLast(spell);
                spell = UndoBuffer.Last.Value;
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
                UndoBuffer.AddLast(spell);
                spell = RedoBuffer.Last.Value;
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
            bool saved = spell.save(false);
            if (!saved && MessageBox.Show("File exists! Overwrite?", "File exists", MessageBoxButtons.YesNo) == DialogResult.Yes) saved = spell.save(true);
            if (saved)
            {
                UnsavedChanges = 0;
                Saved?.Invoke(this, spell.Name + " " + ConfigManager.SourceSeperator + " " + spell.Source);
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
            else
            {
                if (Duration.Text != null && Duration.Text.Length > 0 && !duration.Contains(Duration.Text)) duration.Add(Duration.Text);
                if (Range.Text != null && Range.Text.Length > 0 && !range.Contains(Range.Text)) range.Add(Range.Text);
                if (CastingTime.Text != null && CastingTime.Text.Length > 0 && !castingtime.Contains(CastingTime.Text)) castingtime.Add(CastingTime.Text);
            }
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

        private void CastingTime_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("CastingTime");
        }

        private void Range_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Range");
            if (!doHistory) return;
            if (Range.Text != null && Range.Text.ToLowerInvariant().Contains("self")) keywordControl1.Add(new Keyword("Self"));
            else keywordControl1.Remove(new Keyword("Self"));
            if (Range.Text != null && Range.Text.ToLowerInvariant().Contains("touch")) keywordControl1.Add(new Keyword("Touch"));
            else keywordControl1.Remove(new Keyword("Touch"));
            if (Range.Text != null && (Range.Text.ToLowerInvariant().Contains("touch") || Range.Text.ToLowerInvariant().Contains("self"))) keywordControl1.Add(new Keyword("Melee"));
            else keywordControl1.Remove(new Keyword("Melee"));
            if (Range.Text != null && Range.Text.ToLowerInvariant().Contains("feet") && !Range.Text.ToLowerInvariant().Contains("self")) keywordControl1.Add(new Keyword("Ranged"));
            else keywordControl1.Remove(new Keyword("Ranged"));
        }

        private void Duration_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Duration");
            if (!doHistory) return;
            if (Duration.Text != null && Duration.Text.ToLowerInvariant().Contains("instantaneous")) keywordControl1.Add(new Keyword("Instantaneous"));
            else keywordControl1.Remove(new Keyword("Instantaneous"));
            if (Duration.Text != null && Duration.Text.ToLowerInvariant().Contains("concentration")) keywordControl1.Add(new Keyword("Concentration"));
            else keywordControl1.Remove(new Keyword("Concentration"));
        }

        private void Level_ValueChanged(object sender, EventArgs e)
        {
            MakeHistory("Level");
            if (!doHistory) return;
            if (Level.Value == 0) keywordControl1.Add(new Keyword("Cantrip"));
            else keywordControl1.Remove(new Keyword("Cantrip"));
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            MakeHistory(null);
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            MakeHistory(null);
        }

        private void description_TextChanged(object sender, EventArgs e)
        {
            MakeHistory("Description");
            if (!doHistory) return;
            string desc = description.Text;
            if (desc != null) desc = desc.ToLowerInvariant();
            // if (desc != null && desc.Contains("range"))
            // {
            //    keywordControl1.Add(new Keyword("Ranged"));
            // } else keywordControl1.Remove(new Keyword("Ranged"));
            //if (desc != null && desc.Contains("make a melee spell attack"))
            //{
            //    keywordControl1.Add(new Keyword("Melee"));
            //} else
            //{
            //    keywordControl1.Remove(new Keyword("Melee"));
            //}
            if (desc != null && (desc.Contains("make a melee spell attack") || desc.Contains("make a ranged spell attack"))) {
                keywordControl1.Add(new Keyword("Attack"));
            }
            else keywordControl1.Remove(new Keyword("Attack"));
            if (desc != null && (desc.Contains("your spell save dc")))
            {
                keywordControl1.Add(new Save(Ability.None));
            }
            else keywordControl1.Remove(new Save(Ability.None));
            if (desc != null && (desc.Contains("must make a strength saving throw") || desc.Contains("must succeed on a strength saving throw") || desc.Contains("must make strength saving throws")))
            {
                keywordControl1.Add(new Save(Ability.Strength));
            }
            else keywordControl1.Remove(new Save(Ability.Strength));
            if (desc != null && (desc.Contains("must make a dexterity saving throw") || desc.Contains("must succeed on a dexterity saving throw") || desc.Contains("must make dexterity saving throws")))
            {
                keywordControl1.Add(new Save(Ability.Dexterity));
            }
            else keywordControl1.Remove(new Save(Ability.Dexterity));
            if (desc != null && (desc.Contains("must make a constitution saving throw") || desc.Contains("must succeed on a constitution saving throw") || desc.Contains("must make constitution saving throws")))
            {
                keywordControl1.Add(new Save(Ability.Constitution));
            }
            else keywordControl1.Remove(new Save(Ability.Constitution));
            if (desc != null && (desc.Contains("must make an intelligence saving throw") || desc.Contains("must succeed on an intelligence saving throw") || desc.Contains("must make intelligence saving throws")))
            {
                keywordControl1.Add(new Save(Ability.Intelligence));
            }
            else keywordControl1.Remove(new Save(Ability.Intelligence));
            if (desc != null && (desc.Contains("must make a wisdom saving throw") || desc.Contains("must succeed on a wisdom saving throw") || desc.Contains("must make wisdom saving throws")))
            {
                keywordControl1.Add(new Save(Ability.Wisdom));
            }
            else keywordControl1.Remove(new Save(Ability.Wisdom));
            if (desc != null && (desc.Contains("must make a charisma saving throw") || desc.Contains("must succeed on a charisma saving throw") || desc.Contains("must make charisma saving throws")))
            {
                keywordControl1.Add(new Save(Ability.Charisma));
            }
            else keywordControl1.Remove(new Save(Ability.Charisma));
            //Acid, Cold, Fire, Force, Lightning, Necrotic, Poison, Psychic, Radiant, Thunder
            if (desc != null && desc.Contains("acid damage")) keywordControl1.Add(new Keyword("acid"));
            else keywordControl1.Remove(new Keyword("acid"));
            if (desc != null && desc.Contains("cold")) keywordControl1.Add(new Keyword("cold"));
            else keywordControl1.Remove(new Keyword("cold damage"));
            if (desc != null && desc.Contains("fire damage")) keywordControl1.Add(new Keyword("fire"));
            else keywordControl1.Remove(new Keyword("fire"));
            if (desc != null && desc.Contains("force damage")) keywordControl1.Add(new Keyword("force"));
            else keywordControl1.Remove(new Keyword("force"));
            if (desc != null && desc.Contains("lightning damage")) keywordControl1.Add(new Keyword("lightning"));
            else keywordControl1.Remove(new Keyword("lightning"));
            if (desc != null && desc.Contains("necrotic damage")) keywordControl1.Add(new Keyword("necrotic"));
            else keywordControl1.Remove(new Keyword("necrotic"));
            if (desc != null && desc.Contains("poison damage")) keywordControl1.Add(new Keyword("poison"));
            else keywordControl1.Remove(new Keyword("poison"));
            if (desc != null && desc.Contains("psychic damage")) keywordControl1.Add(new Keyword("psychic"));
            else keywordControl1.Remove(new Keyword("psychic"));
            if (desc != null && desc.Contains("radiant damage")) keywordControl1.Add(new Keyword("radiant"));
            else keywordControl1.Remove(new Keyword("radiant"));
            if (desc != null && desc.Contains("thunder damage")) keywordControl1.Add(new Keyword("thunder"));
            else keywordControl1.Remove(new Keyword("thunder"));
            //Cone, Cube, Cylinder, Line, Sphere, Wall
            if(desc != null && desc.Contains("cone")) keywordControl1.Add(new Keyword("cone"));
            else keywordControl1.Remove(new Keyword("cone"));
            if (desc != null && desc.Contains("cube")) keywordControl1.Add(new Keyword("cube"));
            else keywordControl1.Remove(new Keyword("cube"));
            if (desc != null && desc.Contains("cylinder")) keywordControl1.Add(new Keyword("cylinder"));
            else keywordControl1.Remove(new Keyword("cylinder"));
            if (desc != null && desc.Contains("line")) keywordControl1.Add(new Keyword("line"));
            else keywordControl1.Remove(new Keyword("line"));
            if (desc != null && desc.Contains("sphere")) keywordControl1.Add(new Keyword("sphere"));
            else keywordControl1.Remove(new Keyword("sphere"));
            if (desc != null && desc.Contains("wall")) keywordControl1.Add(new Keyword("wall"));
            else keywordControl1.Remove(new Keyword("wall"));
            if (desc != null && desc.Contains("hit points")) keywordControl1.Add(new Keyword("healing"));
            else keywordControl1.Remove(new Keyword("healing"));
        }

        private void SpellForm_Shown(object sender, EventArgs e)
        {
            doHistory = true;
        }
    }
}


