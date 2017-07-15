using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Descriptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class Decriptions : UserControl
    {
        private List<Description> list;
        private MouseEventArgs drag = null;
        public IHistoryManager HistoryManager { get; set; }
        public List<Description> descriptions
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
        public WebBrowser preview { get; set; }
        public Decriptions()
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
                HistoryManager.MakeHistory(null);
                list.Remove((Description)listBox1.SelectedItem);
            }
            fill();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                preview.Navigate("about:blank");
                preview.Document.OpenNew(true);
                preview.Document.Write(new DescriptionContainer((Description)listBox1.SelectedItem).ToHTML());
                preview.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(button1, new Point(0, button1.Size.Height));
        }

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Description d = new Description();
            new DescriptionForms.DescriptionForm(d).ShowDialog();
            list.Add(d);
            fill();
        }

        private void editDescription(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null) 
                list[listBox1.SelectedIndex] = DescriptionForms.DescriptionForm.dispatch((Description)listBox1.SelectedItem, HistoryManager);
            fill();
        }

        private void listOfNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListDescription d = new ListDescription();
            new DescriptionForms.ListDescriptionForm(d).ShowDialog();
            list.Add(d);
            fill();
        }

        private void tableOfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableDescription d = new TableDescription();
            new DescriptionForms.TableDescriptionForm(d).ShowDialog();
            list.Add(d);
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
            if (data is Description)
            {
                HistoryManager.MakeHistory(null);
                list.Remove(data as Description);
                list.Insert(index, data as Description);
                fill();
            }
        }

        private void listBox1_DragLeave(object sender, EventArgs e)
        {
            fill();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string s = ((Description)listBox1.SelectedItem).Save();
                Clipboard.SetText(s, TextDataFormat.UnicodeText);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string s = ((Description)listBox1.SelectedItem).Save();
                Clipboard.SetText(s);
                HistoryManager.MakeHistory(null);
                list.Remove((Description)listBox1.SelectedItem);
                fill();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    foreach (Description f in DescriptionContainer.LoadString(Clipboard.GetText()).Descriptions) list.Add(f);
                    fill();
                }
            }
            catch (Exception) { }
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
