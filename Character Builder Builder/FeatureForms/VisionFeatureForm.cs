using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Character_Builder_5;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class VisionFeatureForm : Form, IEditor<VisionFeature>
    {
        private VisionFeature bf;
        public VisionFeatureForm(VisionFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            numericUpDown1.DataBindings.Add("Value", f, "Range", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public VisionFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
