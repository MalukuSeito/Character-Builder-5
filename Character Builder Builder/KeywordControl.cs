using Character_Builder_Forms;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class KeywordControl : UserControl
    {
        public enum KeywordGroup
        {
            NONE, FEAT, ITEM, SPELL, MONSTER
        }
        private List<Keyword> items = null;
        public List<Keyword> Keywords { get { return items;  } set { items = value; setItems(); } }
        private Dictionary<KeywordGroup, List<Keyword>> Groups = new Dictionary<KeywordGroup, List<Keyword>>();
        private KeywordGroup group = KeywordGroup.NONE;
        public KeywordGroup Group { get { return group; } set { group = value; setItems(); } }
        public bool handle = true;
        private static HashSet<String> userAdded = new HashSet<string>();
        public IHistoryManager HistoryManager { get; set; }
        public KeywordControl()
        {
            if (Groups.Count == 0)
            {
                List<Keyword> none = new List<Keyword>();
                Groups.Add(KeywordGroup.NONE, none);

                List<Keyword> feat = new List<Keyword>();
                Program.Context.ImportClasses(true, false);
                foreach (ClassDefinition c in Program.Context.ClassesSimple.Values) feat.Add(new Keyword(c.Name));
                Groups.Add(KeywordGroup.FEAT, feat);
                List<Keyword> item = new List<Keyword>();
                foreach (String s in "Unarmed, Simple, Martial, Finesse, Thrown, Loading, Reach, Melee, Ranged, Bludgeoning, Piercing, Slashing".Split(',')) item.Add(new Keyword(s.Trim()));
                item.Add(new Range(0, 0));
                item.Add(new Versatile("0"));
                foreach (String s in "Ammunition, Two-handed, Special, Light, Medium, Heavy, Instrument, Trinket, Game, Artisan, Focus, Arcane, Divine, Druidic".Split(',')) item.Add(new Keyword(s.Trim()));
                Groups.Add(KeywordGroup.ITEM, item);

                List<Keyword> spell = new List<Keyword>();
                foreach(String s in "Abjuration, Conjuration, Divination, Evocation, Enchantment, Illusion, Necromancy, Transmutation".Split(',')) spell.Add(new Keyword(s.Trim()));
                spell.Add(new Material(""));
                foreach (String s in "Somatic, Verbal, Attack".Split(',')) spell.Add(new Keyword(s.Trim()));
                spell.Add(new Save(Ability.None));
                foreach (String s in "Healing, Cantrip, Ritual, Ranged, Melee, Touch, Self, Cone, Cube, Cylinder, Line, Sphere, Wall, Instantaneous, Concentration".Split(',')) spell.Add(new Keyword(s.Trim()));
                foreach (String s in "Acid, Cold, Fire, Force, Lightning, Necrotic, Poison, Psychic, Radiant, Thunder".Split(',')) spell.Add(new Keyword(s.Trim()));
                foreach (ClassDefinition c in Program.Context.ClassesSimple.Values) spell.Add(new Keyword(c.Name));
                Groups.Add(KeywordGroup.SPELL, spell);

                List<Keyword> monster = new List<Keyword>();
                foreach (String s in "Aberration, Beast, Celestial, Construct, Dragon, Elemental, Fey, Fiend, Giant, Humanoid, Monstrosity, Ooze, Plant, Undead, Shapechanger, Devil, Demon, Titan".Split(',')) monster.Add(new Keyword(s.Trim()));
                Groups.Add(KeywordGroup.MONSTER, monster);
            }
            InitializeComponent();
            foreach (string s in userAdded)
            {
                newKeyword.Items.Add(s);
                newKeyword.AutoCompleteCustomSource.Add(s);
            }
        }

        private void setItems()
        {
            handle = false;
            keywords.Items.Clear();
            if (items != null) foreach (Keyword k in items) keywords.Items.Add(k, true);
            if (Groups.ContainsKey(group)) foreach (Keyword k in Groups[group]) if (items == null || !items.Contains(k)) keywords.Items.Add(k, false);
            handle = true;
        }

        private void keywords_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!handle) return;
            HistoryManager?.MakeHistory(null);
            if (e.NewValue == CheckState.Checked && e.CurrentValue == CheckState.Unchecked)
            {
                Keyword kw = keywords.Items[e.Index] as Keyword;
                DialogResult d = DialogResult.OK;
                if (kw is Material)
                {
                    kw = new Material();
                    d = new KeywordForms.MaterialForm(kw as Material).ShowDialog();
                }
                if (kw is Range)
                {
                    kw = new Range(10, 20);
                    d = new KeywordForms.RangeForm(kw as Range).ShowDialog();
                }
                if (kw is Versatile)
                {
                    kw = new Versatile("1d8");
                    d = new KeywordForms.VersatileForm(kw as Versatile).ShowDialog();
                }
                if (kw is Save)
                {
                    kw = new Save(Ability.None);
                    d = new KeywordForms.SaveForm(kw as Save).ShowDialog();
                }
                if (d != DialogResult.Abort && d != DialogResult.Cancel) items.Add(kw);
            } else if (e.NewValue == CheckState.Unchecked && e.CurrentValue == CheckState.Checked) items.Remove(keywords.Items[e.Index] as Keyword);
            this.BeginInvoke((MethodInvoker)(
                () => setItems()));
        }

        private void Add_Click(object sender, EventArgs e)
        {
            HistoryManager?.MakeHistory(null);
            string t = newKeyword.Text.Trim();
            t = Regex.Replace(t, "[^A-Za-z]", "_");
            if (t.Length > 0)
            {
                newKeyword.Items.Add(t);
                newKeyword.AutoCompleteCustomSource.Add(t);
                userAdded.Add(t);
                Keyword kw = new Keyword(t);
                items.Add(kw);
                setItems();
            }
            newKeyword.Text = "";
        }

        private void newKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                Add_Click(sender, e);
            }
        }

        public void Add(Keyword kw)
        {
            if (items.Contains(kw)) return;
            items.Add(kw);
            setItems();
        }

        public void Remove(Keyword kw)
        {
            items.RemoveAll(k => k.Equals(kw));
            setItems();
        }
    }
}
