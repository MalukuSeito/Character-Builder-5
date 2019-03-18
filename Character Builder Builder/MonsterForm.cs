using Character_Builder_Forms;
using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

        private void MonsterForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void ParseCilipboard()
        {
            if (Clipboard.ContainsText())
            {
                MakeHistory(null);
                string[] elms = Clipboard.GetText().Split('\n');
                Section section = Section.NAME;
                int passive = 10;
                StringBuilder text = new StringBuilder();
                Dictionary<string, string> rolls = new Dictionary<string, string>();
                List<string> saves = new List<string>();
                List<string> skills = new List<string>();
                OGL.Descriptions.Description desc = null;
                for (int i = 0; i < elms.Length; i++)
                {
                    string s = elms[i];
                    string trim = s.Trim(new char[] { '\n', '\t', ' ', '\r' });
                    if (StringComparer.OrdinalIgnoreCase.Equals("Traits", trim))
                    {
                        section = Section.TRAITS;
                        monster.Traits.Clear();
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Actions", trim))
                    {
                        section = Section.ACTIONS;
                        monster.Actions.Clear();
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Legendary Actions", trim))
                    {
                        section = Section.LEGENDARY;
                        monster.LegendaryActions.Clear();
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Lair Actions", trim)) section = Section.LAIR;
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Reactions", trim))
                    {
                        monster.LegendaryActions.Clear();
                        section = Section.REACTIONS;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Attributes", trim)) section = Section.ATTRIBUTES;
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Show Formatted Attributes", trim)) ;
                    else if ((StringComparer.OrdinalIgnoreCase.Equals("AC", trim) || StringComparer.OrdinalIgnoreCase.Equals("Armor Class", trim)) && i < elms.Length - 1)
                    {
                        string val = elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' });
                        string[] ab = val.Split(new char[] { ' ' }, 2);
                        if (int.TryParse(ab[0], out int ac)) monster.AC = ac;
                        if (ab.Length > 1) monster.ACText = ab[1].Replace("(", "").Replace(")", "").Trim(new char[] { '\n', '\t', ' ', '\r' });
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Alignment", trim) && i < elms.Length - 1)
                    {
                        monster.Alignment = elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' }).ToLowerInvariant();
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("CHA", trim) && i < elms.Length - 1)
                    {
                        if (int.TryParse(elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Charisma = v;
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("CON", trim) && i < elms.Length - 1)
                    {
                        if (int.TryParse(elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Constitution = v;
                        section = Section.ATTRIBUTES;
                    }
                    else if ((StringComparer.OrdinalIgnoreCase.Equals("Challenge Rating", trim) || StringComparer.OrdinalIgnoreCase.Equals("CR", trim)) && i < elms.Length - 1)
                    {
                        if (decimal.TryParse(elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' }), out decimal v)) monster.CR = v;
                        monster.XP = ToXP(monster.CR);
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Condition Immunities", trim) && i < elms.Length - 1)
                    {
                        ReplaceRange(monster.ConditionImmunities, elms[++i].Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })));
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("DEX", trim) && i < elms.Length - 1)
                    {
                        if (int.TryParse(elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Dexterity = v;
                        section = Section.ATTRIBUTES;
                    }
                    else if ((StringComparer.OrdinalIgnoreCase.Equals("HP", trim) || StringComparer.OrdinalIgnoreCase.Equals("Hit Points", trim)) && i < elms.Length - 1)
                    {
                        string val = elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' });
                        string[] ab = val.Split(new char[] { ' ' }, 2);
                        if (int.TryParse(ab[0], out int hp)) monster.HP = hp;
                        if (ab.Length > 1) monster.HPRoll = ab[1].Trim(new char[] { '(', ')' });
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("INT", trim) && i < elms.Length - 1)
                    {
                        if (int.TryParse(elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Intelligence = v;
                        section = Section.ATTRIBUTES;
                    }
                    else if ((StringComparer.OrdinalIgnoreCase.Equals("Immunities", trim) || StringComparer.OrdinalIgnoreCase.Equals("Damage Immunities", trim)) && i < elms.Length - 1)
                    {
                        monster.Immunities.Clear();
                        foreach (String entry in elms[++i].Split(';'))
                        {
                            if (entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).ToLowerInvariant().StartsWith("bludgeoning, piercing, and slashing from"))
                            {
                                string from = entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).Substring("bludgeoning, piercing, and slashing from".Length);
                                monster.Immunities.Add("bludgeoning from" + from);
                                monster.Immunities.Add("piercing from" + from);
                                monster.Immunities.Add("slashing from" + from);
                            }
                            else monster.Immunities.AddRange(entry.Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })).ToList());
                        }
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Languages", trim) && i < elms.Length - 1)
                    {
                        ReplaceRange(monster.Languages, elms[++i].Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })));
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Passive Perception", trim) && i < elms.Length - 1)
                    {
                        int.TryParse(elms[++i], out passive);
                        section = Section.ATTRIBUTES;
                    }
                    else if ((StringComparer.OrdinalIgnoreCase.Equals("Resistances", trim) || StringComparer.OrdinalIgnoreCase.Equals("Damage Resistances", trim)) && i < elms.Length - 1)
                    {
                        monster.Resistances.Clear();
                        foreach (string entry in elms[++i].Split(';'))
                        {
                            if (entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).ToLowerInvariant().StartsWith("bludgeoning, piercing, and slashing from"))
                            {
                                string from = entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).Substring("bludgeoning, piercing, and slashing from".Length);
                                monster.Resistances.Add("bludgeoning from" + from);
                                monster.Resistances.Add("piercing from" + from);
                                monster.Resistances.Add("slashing from" + from);
                            }
                            else monster.Resistances.AddRange(entry.Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })).ToList());
                        }
                        section = Section.ATTRIBUTES;
                    }
                    else if (Regex.IsMatch(trim, "Roll \\d+", RegexOptions.IgnoreCase) && i < elms.Length - 1)
                    {
                        //string val = elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' });
                        //string[] ab = val.Split(new char[] { ' ' }, 2);
                        //if (ab.Length > 1) rolls.Add(ab[0], ab[1]);
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("STR", trim) && i < elms.Length - 1)
                    {
                        if (int.TryParse(elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Strength = v;
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Saving Throws", trim) && i < elms.Length - 1)
                    {
                        string val = elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' });
                        foreach (string ss in val.Split(new char[] { ',' }))
                        {
                            //string[] ab = val.Split(new char[] { '+', '-' }, 2);
                            //if (ab.Length > 1 && int.TryParse(ab[1], out int v)) saves.Add(ab[0].Trim(new char[] { '\n', '\t', ' ', '\r' }), val.Contains('-') ? -v: v);
                            //else saves.Add(ab[0].Trim(new char[] { '\n', '\t', ' ', '\r' }), 0);
                            saves.Add(ss);

                        }
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Senses", trim) && i < elms.Length - 1)
                    {
                        ReplaceRange(monster.Senses, elms[++i].Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })));
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Size", trim) && i < elms.Length - 1)
                    {
                        if (Enum.TryParse(elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' }), true, out OGL.Base.Size v)) monster.Size = v;
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Skills", trim) && i < elms.Length - 1)
                    {
                        string val = elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' });
                        foreach (string ss in val.Split(new char[] { ',' }))
                        {
                            //string[] ab = val.Split(new char[] { '+', '-' }, 2);
                            //if (ab.Length > 1 && int.TryParse(ab[1], out int v)) skills.Add(ab[0].Trim(new char[] { '\n', '\t', ' ', '\r' }), val.Contains('-') ? -v : v);
                            //else skills.Add(ab[0].Trim(new char[] { '\n', '\t', ' ', '\r' }), 0);
                            skills.Add(ss);

                        }
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Speed", trim) && i < elms.Length - 1)
                    {
                        ReplaceRange(monster.Speeds, elms[++i].Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })));
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Type", trim) && i < elms.Length - 1)
                    {
                        string val = elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' });
                        monster.Keywords.Clear();
                        foreach (string ss in val.Split(new char[] { '(', ')', ',', ';' }))
                        {
                            if (ss.Trim().Length > 0) monster.Keywords.Add(new OGL.Keywords.Keyword(ss.Trim().ToLowerInvariant()));
                        }
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("Vulnerabilities", trim) && i < elms.Length - 1)
                    {
                        monster.Vulnerablities.Clear();
                        foreach (string entry in elms[++i].Split(';'))
                        {
                            if (entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).ToLowerInvariant().StartsWith("bludgeoning, piercing, and slashing from"))
                            {
                                string from = entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).Substring("bludgeoning, piercing, and slashing from".Length);
                                monster.Vulnerablities.Add("bludgeoning from" + from);
                                monster.Vulnerablities.Add("piercing from" + from);
                                monster.Vulnerablities.Add("slashing from" + from);
                            }
                            else monster.Vulnerablities.AddRange(entry.Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })).ToList());
                        }
                        section = Section.ATTRIBUTES;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Equals("WIS", trim) && i < elms.Length - 1)
                    {
                        if (int.TryParse(elms[++i].Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Wisdom = v;
                        section = Section.ATTRIBUTES;
                    }
                    else
                    {
                        if (trim.Length == 0) continue;
                        if (section == Section.NAME)
                        {
                            monster.Name = trim;
                            section = Section.DESCRIPTION;
                        }
                        else if (section == Section.DESCRIPTION || section == Section.ATTRIBUTES)
                        {
                            string t = trim.ToLowerInvariant();
                            if (t.StartsWith("ac "))
                            {
                                string val = trim.Substring("ac ".Length);
                                string[] ab = val.Split(new char[] { ' ' }, 2);
                                if (int.TryParse(ab[0], out int ac)) monster.AC = ac;
                                if (ab.Length > 1) monster.ACText = ab[1].Replace("(", "").Replace(")", "").Trim(new char[] { '\n', '\t', ' ', '\r' });
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("armor class "))
                            {
                                string val = trim.Substring("armor class ".Length);
                                string[] ab = val.Split(new char[] { ' ' }, 2);
                                if (int.TryParse(ab[0], out int ac)) monster.AC = ac;
                                if (ab.Length > 1) monster.ACText = ab[1].Replace("(", "").Replace(")", "").Trim(new char[] { '\n', '\t', ' ', '\r' });
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("alignment "))
                            {
                                monster.Alignment = trim.Substring("alignment ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' });
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("cha "))
                            {
                                if (int.TryParse(trim.Substring("cha ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Charisma = v;
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("con "))
                            {
                                if (int.TryParse(trim.Substring("con ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Constitution = v;
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("challenge rating "))
                            {
                                if (decimal.TryParse(trim.Substring("challenge rating ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' }), out decimal v)) monster.CR = v;
                                monster.XP = ToXP(monster.CR);
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("cr "))
                            {
                                if (decimal.TryParse(trim.Substring("cr ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' }), out decimal v)) monster.CR = v;
                                monster.XP = ToXP(monster.CR);
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("condition immunities "))
                            {
                                ReplaceRange(monster.ConditionImmunities, trim.Substring("condition immunities ".Length).Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })));
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("dex"))
                            {
                                if (int.TryParse(trim.Substring("dex ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Dexterity = v;
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("hp "))
                            {
                                string val = trim.Substring("hp ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' });
                                string[] ab = val.Split(new char[] { ' ' }, 2);
                                if (int.TryParse(ab[0], out int hp)) monster.HP = hp;
                                if (ab.Length > 1) monster.HPRoll = ab[1].Trim(new char[] { '(', ')' });
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("hit points "))
                            {
                                string val = trim.Substring("hit points ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' });
                                string[] ab = val.Split(new char[] { ' ' }, 2);
                                if (int.TryParse(ab[0], out int hp)) monster.HP = hp;
                                if (ab.Length > 1) monster.HPRoll = ab[1].Trim(new char[] { '(', ')' });
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("int "))
                            {
                                if (int.TryParse(trim.Substring("int ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Intelligence = v;
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("immunities "))
                            {
                                monster.Immunities.Clear();
                                foreach (String entry in trim.Substring("immunities ".Length).Split(';'))
                                {
                                    if (entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).ToLowerInvariant().StartsWith("bludgeoning, piercing, and slashing from"))
                                    {
                                        string from = entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).Substring("bludgeoning, piercing, and slashing from".Length);
                                        monster.Immunities.Add("bludgeoning from" + from);
                                        monster.Immunities.Add("piercing from" + from);
                                        monster.Immunities.Add("slashing from" + from);
                                    }
                                    else monster.Immunities.AddRange(entry.Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })).ToList());
                                }
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("damage immunities "))
                            {
                                monster.Immunities.Clear();
                                foreach (String entry in trim.Substring("damage immunities ".Length).Split(';'))
                                {
                                    if (entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).ToLowerInvariant().StartsWith("bludgeoning, piercing, and slashing from"))
                                    {
                                        string from = entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).Substring("bludgeoning, piercing, and slashing from".Length);
                                        monster.Immunities.Add("bludgeoning from" + from);
                                        monster.Immunities.Add("piercing from" + from);
                                        monster.Immunities.Add("slashing from" + from);
                                    }
                                    else monster.Immunities.AddRange(entry.Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })).ToList());
                                }
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("languages "))
                            {
                                ReplaceRange(monster.Languages, trim.Substring("languages ".Length).Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })));
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("resistances "))
                            {
                                monster.Resistances.Clear();
                                foreach (string entry in trim.Substring("resistances ".Length).Split(';'))
                                {
                                    if (entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).ToLowerInvariant().StartsWith("bludgeoning, piercing, and slashing from"))
                                    {
                                        string from = entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).Substring("bludgeoning, piercing, and slashing from".Length);
                                        monster.Resistances.Add("bludgeoning from" + from);
                                        monster.Resistances.Add("piercing from" + from);
                                        monster.Resistances.Add("slashing from" + from);
                                    }
                                    else monster.Resistances.AddRange(entry.Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })).ToList());
                                }
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("damage resistances "))
                            {
                                monster.Resistances.Clear();
                                foreach (string entry in trim.Substring("damage resistances ".Length).Split(';'))
                                {
                                    if (entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).ToLowerInvariant().StartsWith("bludgeoning, piercing, and slashing from"))
                                    {
                                        string from = entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).Substring("bludgeoning, piercing, and slashing from".Length);
                                        monster.Resistances.Add("bludgeoning from" + from);
                                        monster.Resistances.Add("piercing from" + from);
                                        monster.Resistances.Add("slashing from" + from);
                                    }
                                    else monster.Resistances.AddRange(entry.Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })).ToList());
                                }
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("str "))
                            {
                                if (int.TryParse(trim.Substring("str ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Strength = v;
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("saving throws "))
                            {
                                string val = trim.Substring("saving throws ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' });
                                foreach (string ss in val.Split(new char[] { ',' }))
                                {
                                    //string[] ab = val.Split(new char[] { '+', '-' }, 2);
                                    //if (ab.Length > 1 && int.TryParse(ab[1], out int v)) saves.Add(ab[0].Trim(new char[] { '\n', '\t', ' ', '\r' }), val.Contains('-') ? -v: v);
                                    //else saves.Add(ab[0].Trim(new char[] { '\n', '\t', ' ', '\r' }), 0);
                                    saves.Add(ss);

                                }
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("senses "))
                            {
                                var list = trim.Substring("senses ".Length).Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' }));
                                monster.Senses.Clear();
                                foreach (string sss in list)
                                {
                                    if (sss.ToLowerInvariant().StartsWith("passive perception ")) int.TryParse(sss.Substring("passive perception ".Length), out passive);
                                    else monster.Senses.Add(sss);
                                }
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("size "))
                            {
                                if (Enum.TryParse(trim.Substring("size ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' }), true, out OGL.Base.Size v)) monster.Size = v;
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("skills "))
                            {
                                string val = trim.Substring("skills ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' });
                                foreach (string ss in val.Split(new char[] { ',' }))
                                {
                                    //string[] ab = val.Split(new char[] { '+', '-' }, 2);
                                    //if (ab.Length > 1 && int.TryParse(ab[1], out int v)) skills.Add(ab[0].Trim(new char[] { '\n', '\t', ' ', '\r' }), val.Contains('-') ? -v : v);
                                    //else skills.Add(ab[0].Trim(new char[] { '\n', '\t', ' ', '\r' }), 0);
                                    skills.Add(ss);

                                }
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("speed "))
                            {
                                ReplaceRange(monster.Speeds, trim.Substring("speed ".Length).Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })));
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("type "))
                            {
                                string val = trim.Substring("type ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' });
                                monster.Keywords.Clear();
                                foreach (string ss in val.Split(new char[] { '(', ')', ',', ';' }))
                                {
                                    if (ss.Trim().Length > 0) monster.Keywords.Add(new OGL.Keywords.Keyword(ss.Trim().ToLowerInvariant()));
                                }
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("vulnerabilities "))
                            {
                                monster.Vulnerablities.Clear();
                                foreach (string entry in trim.Substring("vulnerabilities ".Length).Split(';'))
                                {
                                    if (entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).ToLowerInvariant().StartsWith("bludgeoning, piercing, and slashing from"))
                                    {
                                        string from = entry.Trim(new char[] { '\n', '\t', ' ', '\r' }).Substring("bludgeoning, piercing, and slashing from".Length);
                                        monster.Vulnerablities.Add("bludgeoning from" + from);
                                        monster.Vulnerablities.Add("piercing from" + from);
                                        monster.Vulnerablities.Add("slashing from" + from);
                                    }
                                    else monster.Vulnerablities.AddRange(entry.Split(',').Select(ss => ss.Trim(new char[] { '\n', '\t', ' ', '\r' })).ToList());
                                }
                                section = Section.ATTRIBUTES;
                            }
                            else if (t.StartsWith("wis "))
                            {
                                if (int.TryParse(trim.Substring("wis ".Length).Trim(new char[] { '\n', '\t', ' ', '\r' }), out int v)) monster.Wisdom = v;
                                section = Section.ATTRIBUTES;
                            }
                            else if (section == Section.DESCRIPTION) {
                                monster.Description += (monster.Description?.Length > 0 ? "\n" : "") + trim;
                            }
                        }
                        else if (section == Section.TRAITS)
                        {
                            string[] ab = trim.Split(new char[] { '.' }, 2);
                            if (ab.Length > 1) monster.Traits.Add(new OGL.Monsters.MonsterTrait(ab[0], ab[1].Trim()));
                            else if (monster.Traits.Count > 0) monster.Traits.Last().Text += "\n" + trim;
                            else monster.Traits.Add(new OGL.Monsters.MonsterTrait("", trim));
                        }
                        else if (section == Section.ACTIONS)
                        {
                            string[] ab = trim.Split(new char[] { '.' }, 2);
                            if (ab.Length > 1)
                            {
                                Regex attck = new Regex("Attack:\\s+\\+(\\d+)\\s+to\\s+hit", RegexOptions.IgnoreCase);
                                Regex damage2 = new Regex("\\(([^)]+)\\)\\s+(\\w+)\\s*damage\\s+plus\\s+[^(]+\\s+\\(([^)]+)\\)\\s+(\\w+)\\s*damage");
                                Regex damage = new Regex("\\(([^)]+)\\)\\s+(\\w+)\\s*damage");
                                Match m = attck.Match(ab[1]);
                                if (m.Success && int.TryParse(m.Groups[1].Value, out int bonus))
                                {
                                    Match md = damage.Match(ab[1]);
                                    Match m2 = damage2.Match(ab[1]);
                                    if (m2.Success && m2.Groups[2].Value == m2.Groups[4].Value) monster.Actions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, m2.Groups[1].Value + " + " + m2.Groups[3].Value + " " + m2.Groups[2].Value));
                                    else if (m2.Success) monster.Actions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, m2.Groups[1].Value + " " + m2.Groups[2].Value + " + " + m2.Groups[3].Value + " " + m2.Groups[4].Value));
                                    else if (md.Success) monster.Actions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, md.Groups[1].Value + "  " + md.Groups[2].Value));
                                    else monster.Actions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, ""));
                                }
                                else
                                {
                                    monster.Actions.Add(new OGL.Monsters.MonsterTrait(ab[0], ab[1].Trim()));
                                }
                            }
                            else if (monster.Actions.Count > 0) monster.Actions.Last().Text += "\n" + trim;
                            else monster.Actions.Add(new OGL.Monsters.MonsterTrait("", trim));
                        }
                        else if (section == Section.REACTIONS)
                        {
                            string[] ab = trim.Split(new char[] { '.' }, 2);
                            if (ab.Length > 1)
                            {
                                Regex attck = new Regex("Attack:\\s+\\+(\\d+)\\s+to\\s+hit", RegexOptions.IgnoreCase);
                                Regex damage2 = new Regex("\\(([^)]+)\\)\\s+(\\w+)\\s*damage\\s+plus\\s+[^(]+\\s+\\(([^)]+)\\)\\s+(\\w+)\\s*damage");
                                Regex damage = new Regex("\\(([^)]+)\\)\\s+(\\w+)\\s*damage");
                                Match m = attck.Match(ab[1]);
                                if (m.Success && int.TryParse(m.Groups[1].Value, out int bonus))
                                {
                                    Match md = damage.Match(ab[1]);
                                    Match m2 = damage2.Match(ab[1]);
                                    if (m2.Success && m2.Groups[2].Value == m2.Groups[4].Value) monster.Reactions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, m2.Groups[1].Value + " + " + m2.Groups[3].Value + " " + m2.Groups[2].Value));
                                    else if (m2.Success) monster.Reactions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, m2.Groups[1].Value + " " + m2.Groups[2].Value + " + " + m2.Groups[3].Value + " " + m2.Groups[4].Value));
                                    else if (md.Success) monster.Reactions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, md.Groups[1].Value + "  " + md.Groups[2].Value));
                                    else monster.Reactions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, ""));
                                }
                                else
                                {
                                    monster.Reactions.Add(new OGL.Monsters.MonsterTrait(ab[0], ab[1].Trim()));
                                }
                            }
                            else if (monster.Reactions.Count > 0) monster.Reactions.Last().Text += "\n" + trim;
                            else monster.Reactions.Add(new OGL.Monsters.MonsterTrait("", trim));
                        }
                        else if (section == Section.LEGENDARY)
                        {
                            if (monster.LegendaryActions.Count == 0)
                            {
                                monster.LegendaryActions.Add(new OGL.Monsters.MonsterTrait("", trim));
                            }
                            else
                            {
                                string[] ab = trim.Split(new char[] { '.' }, 2);
                                if (ab.Length > 1)
                                {
                                    Regex attck = new Regex("Attack:\\s+\\+(\\d+)\\s+to\\s+hit", RegexOptions.IgnoreCase);
                                    Regex damage2 = new Regex("\\(([^)]+)\\)\\s+(\\w+)\\s*damage\\s+plus\\s+[^(]+\\s+\\(([^)]+)\\)\\s+(\\w+)\\s*damage");
                                    Regex damage = new Regex("\\(([^)]+)\\)\\s+(\\w+)\\s*damage");
                                    Match m = attck.Match(ab[1]);
                                    if (m.Success && int.TryParse(m.Groups[1].Value, out int bonus))
                                    {
                                        Match md = damage.Match(ab[1]);
                                        Match m2 = damage2.Match(ab[1]);
                                        if (m2.Success && m2.Groups[2].Value == m2.Groups[4].Value) monster.LegendaryActions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, m2.Groups[1].Value + " + " + m2.Groups[3].Value + " " + m2.Groups[2].Value));
                                        else if (m2.Success) monster.LegendaryActions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, m2.Groups[1].Value + " " + m2.Groups[2].Value + " + " + m2.Groups[3].Value + " " + m2.Groups[4].Value));
                                        else if (md.Success) monster.LegendaryActions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, md.Groups[1].Value + "  " + md.Groups[2].Value));
                                        else monster.LegendaryActions.Add(new OGL.Monsters.MonsterAction(ab[0], ab[1].Trim(), bonus, ""));
                                    }
                                    else
                                    {
                                        monster.LegendaryActions.Add(new OGL.Monsters.MonsterTrait(ab[0], ab[1]));
                                    }
                                }
                                else if (monster.LegendaryActions.Count > 0) monster.LegendaryActions.Last().Text += "\n" + trim;
                                else monster.LegendaryActions.Add(new OGL.Monsters.MonsterTrait("", trim));
                            }
                        }
                        else if (section == Section.LAIR)
                        {
                            if (desc == null)
                            {
                                desc = new OGL.Descriptions.Description("Lair Actions", trim);
                                monster.Descriptions.Add(desc);
                            }
                            else desc.Text += "\n" + trim;
                        }
                    }
                }
                monster.PassivePerception = passive - AbilityScores.GetMod(monster.Wisdom) - 10;
                if (skills.Count > 0)
                {
                    Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();
                    if (Program.Context.SkillsSimple.Count == 0) Program.Context.ImportSkills();
                    foreach (OGL.Skill s in Program.Context.SkillsSimple.Values)
                    {
                        Skills.Add(s.Name.ToLowerInvariant(), s);
                    }
                    monster.SkillBonus.Clear();
                    foreach (string skill in skills)
                    {
                        foreach (Match match in Regex.Matches(skill, @"(\D+?)\s+\+?(\-?\s*\d+)(?:\s*\(([^\)]+)\))?", RegexOptions.IgnoreCase))
                        {
                            string name = match.Groups[1].Value.Trim();
                            if (int.TryParse(match.Groups[2].Value, out int bonus))
                            {
                                if ("perception".Equals(name, StringComparison.OrdinalIgnoreCase))
                                {
                                    monster.PassivePerception -= bonus - (monster.Wisdom / 2 - 5);
                                }
                                if (Skills.ContainsKey(name.ToLowerInvariant())) monster.SkillBonus.Add(new OGL.Monsters.MonsterSkillBonus()
                                {
                                    Skill = Skills[name.ToLowerInvariant()].Name,
                                    Bonus = bonus - monster.getAbility(Skills[name.ToLowerInvariant()].Base) / 2 + 5,
                                    Text = match.Groups.Count > 2 ? match.Groups[3].Value : null

                                });
                                else monster.SkillBonus.Add(new OGL.Monsters.MonsterSkillBonus()
                                {
                                    Skill = name.Substring(0, 1).ToUpperInvariant() + name.Substring(1).ToLowerInvariant(),
                                    Bonus = bonus,
                                    Text = match.Captures.Count > 2 ? match.Captures[3].Value : null
                                });
                            }
                        }
                    }
                }
                if (saves.Count > 0)
                {
                    monster.SaveBonus.Clear();
                    Dictionary<string, OGL.Base.Ability> abilities = new Dictionary<string, OGL.Base.Ability>();
                    abilities.Add("str", OGL.Base.Ability.Strength);
                    abilities.Add("strength", OGL.Base.Ability.Strength);
                    abilities.Add("dex", OGL.Base.Ability.Dexterity);
                    abilities.Add("dexterity", OGL.Base.Ability.Dexterity);
                    abilities.Add("con", OGL.Base.Ability.Constitution);
                    abilities.Add("constitution", OGL.Base.Ability.Constitution);
                    abilities.Add("int", OGL.Base.Ability.Intelligence);
                    abilities.Add("intelligence", OGL.Base.Ability.Intelligence);
                    abilities.Add("wis", OGL.Base.Ability.Wisdom);
                    abilities.Add("wisdom", OGL.Base.Ability.Wisdom);
                    abilities.Add("cha", OGL.Base.Ability.Charisma);
                    abilities.Add("charisma", OGL.Base.Ability.Charisma);
                    foreach (string save in saves)
                    {
                        foreach (Match match in Regex.Matches(save, @"(\D+?)\s+\+?(\-?\s*\d+)(?:\s*\(([^\)]+)\))?", RegexOptions.IgnoreCase))
                        {
                            string name = match.Groups[1].Value.Trim();
                            if (int.TryParse(match.Groups[2].Value, out int bonus))
                            {
                                if (abilities.ContainsKey(name.ToLowerInvariant())) monster.SaveBonus.Add(new OGL.Monsters.MonsterSaveBonus()
                                {
                                    Ability = abilities[name.ToLowerInvariant()],
                                    Bonus = bonus - monster.getAbility(abilities[name.ToLowerInvariant()]) / 2 + 5,
                                    Text = match.Captures.Count > 2 ? match.Groups[3].Value : null

                                });
                            }
                        }
                    }
                }
                refresh();
            }
        }

        private void ReplaceRange(List<string> list, IEnumerable<string> enumerable)
        {
            list.Clear();
            list.AddRange(enumerable);
        }

        private int ToXP(decimal cR)
        {
            if (cR >= 30) return 155000;
            if (cR >= 29) return 135000;
            if (cR >= 28) return 120000;
            if (cR >= 27) return 105000;
            if (cR >= 26) return 90000;
            if (cR >= 25) return 75000;
            if (cR >= 24) return 62000;
            if (cR >= 23) return 50000;
            if (cR >= 22) return 41000;
            if (cR >= 21) return 33000;
            if (cR >= 20) return 25000;
            if (cR >= 19) return 22000;
            if (cR >= 18) return 20000;
            if (cR >= 17) return 18000;
            if (cR >= 16) return 15000;
            if (cR >= 15) return 13000;
            if (cR >= 14) return 11000;
            if (cR >= 13) return 10000;
            if (cR >= 12) return 8400;
            if (cR >= 11) return 7200;
            if (cR >= 10) return 5900;
            if (cR >= 9) return 5000;
            if (cR >= 8) return 3900;
            if (cR >= 7) return 2900;
            if (cR >= 6) return 2300;
            if (cR >= 5) return 1800;
            if (cR >= 4) return 1100;
            if (cR >= 3) return 700;
            if (cR >= 2) return 450;
            if (cR >= 1) return 200;
            if (cR >= 0.5m) return 100;
            if (cR >= 0.25m) return 50;
            if (cR >= 0.125m) return 25;
            if (cR >= 0) return 10;
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParseCilipboard();
        }
    }
    public enum Section
    {
        NAME, DESCRIPTION, ATTRIBUTES, TRAITS, ACTIONS, REACTIONS, LEGENDARY, LAIR
    }
}
