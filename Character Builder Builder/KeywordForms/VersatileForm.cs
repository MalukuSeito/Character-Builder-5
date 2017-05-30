using Character_Builder_5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Character_Builder_Builder.KeywordForms
{
    public partial class VersatileForm : Form
    {
        public VersatileForm(Versatile m)
        {
            InitializeComponent();
            textBox1.DataBindings.Add("Text", m, "Damage", true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
