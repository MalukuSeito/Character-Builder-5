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
    public partial class SpeedFeatureForm : Form, IEditor<SpeedFeature>
    {
        private SpeedFeature bf;
        public SpeedFeatureForm(SpeedFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Condition.DataBindings.Add("Text", f, "Condition", true, DataSourceUpdateMode.OnPropertyChanged);
            ExtraSpeed.DataBindings.Add("Text", f, "ExtraSpeed", true, DataSourceUpdateMode.OnPropertyChanged);
            BaseSpeed.DataBindings.Add("Value", f, "BaseSpeed", true, DataSourceUpdateMode.OnPropertyChanged);
            IgnoreArmor.DataBindings.Add("Checked", f, "IgnoreArmor", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public SpeedFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
