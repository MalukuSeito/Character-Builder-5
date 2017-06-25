using OGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Character_Builder_Forms
{
    public partial class ErrorLog : Form
    {
        private bool shown = false;
        private LinkedList<LogEntry> entries = new LinkedList<LogEntry>();
        public ErrorLog()
        {
            InitializeComponent();
            ConfigManager.LogEvents += ConfigManager_LogEvents;
        }

        private void ConfigManager_LogEvents(object sender, string message, Exception e)
        {
            entries.AddFirst(new LogEntry(message, e));
            if (!shown)
            {
                Show();
            }
            else if (Visible)
            {
                UpdateLogs();
            }
        }

        private void UpdateLogs()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(entries.ToArray());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is LogEntry && (listBox1.SelectedItem as LogEntry).e != null)
            {
                textBox1.Text = (listBox1.SelectedItem as LogEntry).e.Message + "\r\n" + (listBox1.SelectedItem as LogEntry).e.StackTrace + "\r\n" + (listBox1.SelectedItem as LogEntry).e.InnerException;
            } else
            {
                textBox1.Text = "";
            }
        }

        private void ErrorLog_Shown(object sender, EventArgs e)
        {
            shown = true;
            UpdateLogs();
        }

        private void ErrorLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void ErrorLog_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                UpdateLogs();
            }
        }
    }
}
