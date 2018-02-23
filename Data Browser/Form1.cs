using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using OGL;
using OGL.Common;
using System.Collections;
using OGL.Features;
using OGL.Items;

namespace Data_Browser
{
    public partial class Drowser : Form
    {
        private bool layouting;
        private int sortColumn = 0;
        public Display display;
        public static List<ITable> tables = new List<ITable>() {
            new Table<Background>(),
            new Table<ClassDefinition>("Class").Add(e=>e.AvailableAtFirstLevel).Add(e=>e.HitDie).Add(e=>e.MulticlassingCondition),
            new Table<SubClass>().Add(e=>e.ClassName),
            new Table<Race>().Add(e=>e.Size),
            new Table<SubRace>().Add(e=>e.RaceName),
            new Table<Spell>().Add(e=>e.Level).Add(e=>e.Keywords).Add(e=>e.CastingTime).Add(e=>e.Range).Add(e=>e.Duration),
            new Table<Item>().Add(e=>e.Type).Add(e=>e.Category).Add(e=>e.Keywords).Add<Weapon, string>(e=>e.Damage).Add<Weapon, string>(e=>e.DamageType).Add<Armor, int>(e=>e.BaseAC).Add(e=>e.Price).Add(e=>e.Weight).Add(e=>e.StackSize),
            new Table<MagicProperty>("Magic").Add(e=>e.Base, "Base Item").Add(e=>e.Rarity).Add(e=>e.Requirement),
            new Table<Feature>().Add(e=>e.Level).Add(e=>e.Category).Add(e=>e.Prerequisite).Add(e=>e.Keywords),
            new Table<Skill>().Add(e=>e.Base),
            new Table<Condition>(),
            new Table<Language>().Add(e=>e.Skript).Add(e=>e.TypicalSpeakers), 
        };
        private List<IXML> results;
        public Drowser()
        {
            tables.Sort();
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(tables.ToArray());

            foreach (ITable t in tables)
            {
                t.GetDistinct();
            }
            comboBox1.SelectedIndex = 9;
            //display = new Display();
        }
        /*private ListViewItem makeItem(var r)
        {
            ListViewItem result = new ListViewItem(r.Name);
            result.Name = r.Id.ToString();
            result.SubItems.Add(r.Type);
            result.SubItems.Add(r.Source);
            return result;
        }*/

        private ColumnHeader makeHeader(string p)
        {
            ColumnHeader t = new ColumnHeader();
            t.Text=p;
            return t;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && comboBox1.SelectedItem != null && comboBox1.SelectedItem is ITable)
            {
                String id = listView1.SelectedItems[0].Name.ToString();
                new Display(results).Show(results.First(s => id.Equals(s.Name + " " + ConfigManager.SourceSeperator + " " + s.Source)));
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.ForeColor == SystemColors.GrayText)
            {
                textBox1.Text = "";
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.ForeColor = SystemColors.GrayText;
                textBox1.Text = "Search";
            }
        }

        private void UpdateSearch(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (ITable s in tables) {
                s.Results = s.GetValues(textBox1.ForeColor == SystemColors.WindowText ? textBox1.Text : null, checkBox1.Checked, checkBox2.Checked).Count();
            }
            for (int i = 0; i < comboBox1.Items.Count;i++ )
            {
                layouting = true;
                comboBox1.Items[i] = comboBox1.Items[i];
                layouting = false;
            }
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem is ITable t)
            {
                listView1.SuspendLayout();
                listView1.Items.Clear();

                results = t.GetValues(textBox1.ForeColor == SystemColors.WindowText ? textBox1.Text : null, checkBox1.Checked, checkBox2.Checked).ToList();
                foreach (IXML o in results)
                {
                    ListViewItem result = new ListViewItem(o.Name);
                    result.Name = o.ToString();
                    for (int i = 1; i < t.Columns.Count; i++) if (t.Columns[i].Value != "") result.SubItems.Add(t.GetValue(t.Columns[i].Key, o));
                    listView1.Items.Add(result);
                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.ResumeLayout();
            }
            //switch ()

        }
        private void RefineSearch(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem is ITable t)
            {
                listView1.SuspendLayout();
                listView1.Items.Clear();
                results = t.Refine(t.GetValues(textBox1.ForeColor == SystemColors.WindowText ? textBox1.Text : null, checkBox1.Checked, checkBox2.Checked)).ToList();
                foreach (IXML o in results)
                {
                    ListViewItem result = new ListViewItem(o.Name);
                    result.Name = o.ToString();
                    for (int i = 1; i < t.Columns.Count; i++) if (t.Columns[i].Value != "") result.SubItems.Add(t.GetValue(t.Columns[i].Key, o));
                    listView1.Items.Add(result);
                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.ResumeLayout();
                button3.Enabled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!layouting && comboBox1.SelectedItem != null && comboBox1.SelectedItem is ITable t)
            {
                listView1.SuspendLayout();
                listView1.Items.Clear();
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                listView1.Columns.Clear();
                flowLayoutPanel1.Controls.Clear();
                t.AddControls(flowLayoutPanel1);
                button2.Enabled = true;
                foreach (string s in from kv in t.Columns select kv.Value) if (s != "") listView1.Columns.Add(s, 0);
                t.ResetRefinements();
                sortColumn = 0;
                UpdateSearch(sender, e);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem is ITable t)
            {
                t.ResetRefinements();
                UpdateSearch(sender, e);
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //if (e.Column != sortColumn)
            //{
            //    sortColumn = e.Column;
            //    reverse = false;
            //    RefineSearch(sender, e);
            //}
            //else
            //{
            //    reverse = !reverse;
            //    RefineSearch(sender, e);
            //}
           
            // Determine whether the column is the same as the last column clicked.
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                listView1.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (listView1.Sorting == SortOrder.Ascending) listView1.Sorting = SortOrder.Descending;
                else listView1.Sorting = SortOrder.Ascending;
            }
            this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, listView1.Sorting);
            // Call the sort method to manually sort.
            listView1.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            
        }
    }
    class ListViewItemComparer : IComparer
    {
        private int col;
        private SortOrder sorting;

        public ListViewItemComparer()
        {
            col = 0;
            sorting = SortOrder.Ascending;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
            sorting = SortOrder.Ascending;
        }

        public ListViewItemComparer(int column, SortOrder sorting) : this(column)
        {
            this.sorting = sorting;
        }

        public int Compare(object x, object y)
        {
            return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text) * (sorting == SortOrder.Descending ? -1 : 1);
        }
    }
}
