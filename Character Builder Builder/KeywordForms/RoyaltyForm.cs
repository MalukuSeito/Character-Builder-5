using OGL.Keywords;
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
    public partial class RoyaltyForm : Form
    {
        public RoyaltyForm(Royalty r)
        {
            InitializeComponent();
            textBox1.DataBindings.Add("Text", r, "Price", true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
