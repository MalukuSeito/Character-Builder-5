using OGL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Character_Builder_Forms;

namespace Data_Browser
{
    public partial class Display : Form
    {
        List<IXML> result;
        int current;
        public Display(List<IXML> values)
        {
            InitializeComponent();
            this.Hide();
            result = values;
            current = 0;
        }
        public void Show(IXML id)
        {
            current = result.FindIndex(s => s == id);
            if (current < 0)
            {
                Show(0);
            }
            Show(current);
        }
        public void Show(int c)
        {
            current = c;
            ShowHTML(result[c]);
        }
        private void ShowHTML(IXML o) {
            webBrowser1.Navigate("about:blank");
            webBrowser1.Document.OpenNew(true);
            webBrowser1.Document.Write(o.ToHTML());
            webBrowser1.Refresh();
            counter.Text = (current + 1) + "/" + result.Count;
            this.Show();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            prev();
        }
        private void prev()
        {
            if (current > 0 && result.Count > 0)
            {
                current--;
                Show(result[current]);
            }
            prevButton.Focus();
        }
        private void nextButton_Click(object sender, EventArgs e)
        {
            next();
        }
        private void next()
        {
            if (current < result.Count - 1)
            {
                current++;
                Show(result[current]);
            }
            nextButton.Focus();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.PageUp)
            {
                prev();
                return true;
            }
            else if (keyData == Keys.Right || keyData == Keys.Down || keyData == Keys.PageDown)
            {
                next();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                this.Hide();
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
