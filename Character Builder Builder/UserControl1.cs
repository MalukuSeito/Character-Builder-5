using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class UserControl1 : UserControl
    {
        public IMainEditor editor = null;
        public IMainEditor Editor
        {
            get { return editor; }
            set
            {
                editor = value;
                if (editor != null)
                {
                    editor.ButtonChange += Editor_ButtonChange;
                }
            }
        }

        private void Editor_ButtonChange(object sender, bool CanUndo, bool CanRedo)
        {
            undoToolStripMenuItem.Enabled = CanUndo;
            redoToolStripMenuItem.Enabled = CanRedo;
        }

        public UserControl1()
        {
            InitializeComponent();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Save();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.Redo();
        }
    }
}
