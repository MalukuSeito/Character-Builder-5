using OGL.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class StringList : UserControl
    {
        private class Drag
        {
            public Drag(int i, string v)
            {
                index = i;
                curindex = index;
                value = v;
            }
            public int curindex;
            public int index;
            public string value;
        }
        private List<string> items;
        public List<string> Items { get{ return items; }  set { items = value; fill(); } }
        public List<string> suggestions = new List<string>();
        public IHistoryManager HistoryManager { get; set; }
        public IEnumerable<string> Suggestions {
            get { return suggestions.AsReadOnly(); }
            set
            {
                suggestions.Clear();
                suggestions.AddRange(value);
                input.Items.Clear();
                input.AutoCompleteCustomSource.Clear();
                if (suggestions.Count > 0)
                {
                    foreach (string s in suggestions) {
                        input.Items.Add(s);
                        input.AutoCompleteCustomSource.Add(s);
                    }
                    input.DropDownStyle = ComboBoxStyle.DropDown;
                    input.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    input.AutoCompleteSource = AutoCompleteSource.CustomSource;
                } else
                {
                    input.DropDownStyle = ComboBoxStyle.Simple;
                    input.AutoCompleteMode = AutoCompleteMode.None;
                    input.AutoCompleteSource = AutoCompleteSource.None;
                }
            }
        }
        private MouseEventArgs drag = null;
        public StringList()
        {
            InitializeComponent();
        }

        private void fill()
        {
            listBox1.Items.Clear();
            if (Items == null) return;
            foreach (object o in Items) listBox1.Items.Add(o);
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) drag = e;
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == null || this.listBox1.SelectedItem == null) return;
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            if ((e.X - drag.X) * (e.X - drag.X) + (e.Y - drag.Y) * (e.Y - drag.Y) < 6) return;
            this.listBox1.DoDragDrop(new Drag(listBox1.SelectedIndex, (string)this.listBox1.SelectedItem), DragDropEffects.Move);
            drag = null;
        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            if (point.Y < 0 || point.Y > listBox1.Size.Height) return;
            int index = this.listBox1.IndexFromPoint(point);
            if (index < 0) index = this.listBox1.Items.Count - 1;
            Drag data = (Drag)e.Data.GetData(typeof(Drag));
            this.listBox1.Items.RemoveAt(data.curindex);
            this.listBox1.Items.Insert(index, data.value);
            data.curindex = index;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            int index = this.listBox1.IndexFromPoint(point);
            if (index < 0) index = this.listBox1.Items.Count - 1;
            Drag data = (Drag)e.Data.GetData(typeof(Drag));
            if (data == null)
                return;
            HistoryManager?.MakeHistory(null);
            Items.RemoveAt(data.index);
            Items.Insert(index, data.value);
            fill();
        }

        private void listBox1_DragLeave(object sender, EventArgs e)
        {
            fill();
        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = null;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem == null) return;
            HistoryManager?.MakeHistory(null);
            Items.RemoveAt(listBox1.SelectedIndex);
            fill();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (input.Text.Length > 0)
            {
                HistoryManager?.MakeHistory(null);
                Items.Add(input.Text);
            }
            input.Text = "";
            fill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HistoryManager?.MakeHistory(null);
            foreach (string s in input.Text.Split(new char[] { ',', ';' }))
            {
                Items.Add(s.Trim());
            }
            input.Text = "";
            fill();
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                button2_Click(sender, e);
            }
        }

        private void input_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
