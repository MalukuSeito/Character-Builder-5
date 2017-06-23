using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

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
