using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class IntList : UserControl
    {
        private class Slots
        {
            public int level;
            public int slots;
            public string format;

            public Slots(int level, int slots, string format)
            {
                this.level = level;
                this.slots = slots;
                this.format = format;
            }

            public override string ToString()
            {
                return String.Format(format, level, slots);
            }
        }
        private class Drag
        {
            public Drag(int i, int v)
            {
                index = i;
                curindex = index;
                value = v;
            }
            public int curindex;
            public int index;
            public int value;
        }

        public IHistoryManager HistoryManager { get; set; }

        private List<int> items;
        public List<int> Items { get{ return items; }  set { items = value; fill(); } }
        private MouseEventArgs drag = null;
        public string Format { get; set; }
        private int start;
        public int Start { get { return start; } set { start = value; fill(); } }
        public IntList()
        {
            InitializeComponent();
            Format = "Level {0}: {1}";
            Start = 1;
        }

        private void fill()
        {
            listBox1.Items.Clear();
            if (Items == null) return;
            int level = Start;
            foreach (int o in Items) listBox1.Items.Add(new Slots(level++, o, Format));
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
            this.listBox1.DoDragDrop(new Drag(listBox1.SelectedIndex, ((Slots)this.listBox1.SelectedItem).slots), DragDropEffects.Move);
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
            int v = 0;
            if (Int32.TryParse(input.Text, out v))
            {
                HistoryManager?.MakeHistory(null);
                Items.Add(v);
            }
            input.Text = "";
            fill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HistoryManager?.MakeHistory(null);
            foreach (string s in input.Text.Split(new char[] { ',', ';' }))
            {
                int v = 0;
                if (Int32.TryParse(s.Trim(), out v))
                {
                    Items.Add(v);
                }
            }
            input.Text = "";
            fill();
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                button1_Click(sender, e);
            }
        }
    }
}
