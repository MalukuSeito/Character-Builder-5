using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

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
