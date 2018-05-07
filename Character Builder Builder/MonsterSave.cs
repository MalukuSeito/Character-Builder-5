using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Features;
using OGL.Monsters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Character_Builder_Builder
{
    public partial class MonsterSave : UserControl
    {
        private List<MonsterSaveBonus> list;
        private MouseEventArgs drag = null;
        public IHistoryManager HistoryManager { get; set; }

        public static XmlSerializer Serializer = new XmlSerializer(typeof(MonsterSaveBonus));
        public List<MonsterSaveBonus> Items
        {
            get
            {
                return list;
            }
            set
            {
                list = value;
                fill();
            }
        }
        public MonsterSave()
        {
            InitializeComponent();
        }

        private void fill()
        {
            listBox1.Items.Clear();
            if (list == null) return;
            foreach (object o in list) listBox1.Items.Add(o);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                HistoryManager?.MakeHistory(null);
                list.Remove((MonsterSaveBonus)listBox1.SelectedItem);
            }
            fill();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void editFeature(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                list[listBox1.SelectedIndex] = new MonsterSaveForm((MonsterSaveBonus)listBox1.SelectedItem).edit(HistoryManager);
            fill();
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) drag = e;
        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = null;
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == null || this.listBox1.SelectedItem == null) return;
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            if ((e.X - drag.X) * (e.X - drag.X) + (e.Y - drag.Y) * (e.Y - drag.Y) < 6) return;
            this.listBox1.DoDragDrop(this.listBox1.SelectedItem, DragDropEffects.Move);
            drag = null;
        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            if (point.Y < 0 || point.Y > listBox1.Size.Height) return;
            int index = this.listBox1.IndexFromPoint(point);
            if (index < 0) index = this.listBox1.Items.Count - 1;
            object data = e.Data.GetData(e.Data.GetFormats()[0]);
            if (data == null) return;
            this.listBox1.Items.Remove(data);
            this.listBox1.Items.Insert(index, data);
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            Point point = listBox1.PointToClient(new Point(e.X, e.Y));
            int index = this.listBox1.IndexFromPoint(point);
            if (index < 0) index = this.listBox1.Items.Count - 1;
            object data = e.Data.GetData(e.Data.GetFormats()[0]);
            if (data == null)
                return;
            if (data is MonsterSaveBonus)
            {
                HistoryManager?.MakeHistory(null);
                list.Remove(data as MonsterSaveBonus);
                list.Insert(index, data as MonsterSaveBonus);
                fill();
            }
        }

        private void listBox1_DragLeave(object sender, EventArgs e)
        {
            fill();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MonsterSaveBonus msb = new MonsterSaveBonus();
            list.Add(new MonsterSaveForm(msb).edit(HistoryManager));
            fill();
        }       

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                using (StringWriter sw = new StringWriter()) {
                    Serializer.Serialize(sw, (MonsterSaveBonus)listBox1.SelectedItem);
                    Clipboard.SetText(sw.ToString(), TextDataFormat.UnicodeText);
                }
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    Serializer.Serialize(sw, (MonsterSaveBonus)listBox1.SelectedItem);
                    Clipboard.SetText(sw.ToString(), TextDataFormat.UnicodeText);
                }
                HistoryManager?.MakeHistory(null);
                list.Remove((MonsterSaveBonus)listBox1.SelectedItem);
                fill();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    HistoryManager?.MakeHistory(null);
                    using (StringReader sr = new StringReader(Clipboard.GetText())) {
                        list.Add((MonsterSaveBonus)Serializer.Deserialize(sr));
                        fill();
                    }
                }
            } catch (Exception) { }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C) copyToolStripMenuItem_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.V) pasteToolStripMenuItem_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.X) cutToolStripMenuItem_Click(sender, e);
            else if (e.KeyCode == Keys.Delete) deleteToolStripMenuItem_Click(sender, e);
        }
    }
}
