using System;
using System.Drawing;
using System.Windows.Forms;
using OGL.Descriptions;
using OGL.Common;

namespace Character_Builder_Builder.DescriptionForms
{
    public partial class ListDescriptionForm : Form, IEditor<ListDescription>
    {
        private ListDescription ld;
        private MouseEventArgs drag = null;
        public ListDescriptionForm(ListDescription ld)
        {
            InitializeComponent();
            
            name.DataBindings.Add("Text", ld, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            descText.DataBindings.Add("Text", ld, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            this.ld = ld;
            fill();
        }

        private void fill()
        {
            names.Items.Clear();
            if (ld.Names == null) return;
            foreach (object o in ld.Names) names.Items.Add(o);
        }

        private void names_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) drag = e;
        }

        private void names_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point point = names.PointToClient(new Point(e.X, e.Y));
            if (point.Y < 0 || point.Y > names.Size.Height) return;
            int index = this.names.IndexFromPoint(point);
            if (index < 0) index = this.names.Items.Count - 1;
            object data = e.Data.GetData(typeof(Names));
            this.names.Items.Remove(data);
            this.names.Items.Insert(index, data);
        }

        private void names_DragDrop(object sender, DragEventArgs e)
        {
            Point point = names.PointToClient(new Point(e.X, e.Y));
            int index = this.names.IndexFromPoint(point);
            if (index < 0) index = this.names.Items.Count - 1;
            Names data = (Names)e.Data.GetData(typeof(Names));
            if (data == null)
                return;
            ld.Names.Remove(data);
            ld.Names.Insert(index, data);
            fill();

        }

        private void names_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == null  || this.names.SelectedItem == null) return;
            Point point = names.PointToClient(new Point(e.X, e.Y));
            if ((e.X - drag.X) * (e.X - drag.X) + (e.Y - drag.Y) * (e.Y - drag.Y) < 6) return;
            this.names.DoDragDrop(this.names.SelectedItem, DragDropEffects.Move);
            drag = null;
        }

        private void names_DragLeave(object sender, EventArgs e)
        {
            fill();
        }

        private void names_MouseUp(object sender, MouseEventArgs e)
        {
            drag = null;
        }

        private void editNames(object sender, EventArgs e)
        {
            if (names.SelectedItem != null) new NamesForm((Names)names.SelectedItem).ShowDialog();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (names.SelectedItem != null) ld.Names.RemoveAt(names.SelectedIndex);
            fill();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Names n = new Names();
            new NamesForm(n).ShowDialog();
            ld.Names.Add(n);
            fill();
        }

        public ListDescription edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return ld;
        }
    }
}
