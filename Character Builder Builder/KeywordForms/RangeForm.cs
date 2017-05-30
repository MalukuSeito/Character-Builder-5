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
    public partial class RangeForm : Form
    {
        public RangeForm(Range m)
        {
            InitializeComponent();
            ShortRange.DataBindings.Add("Value", m, "Short", true, DataSourceUpdateMode.OnPropertyChanged);
            LongRange.DataBindings.Add("Value", m, "Long", true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
