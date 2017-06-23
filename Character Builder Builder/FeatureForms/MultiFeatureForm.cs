using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class MultiFeatureForm : Form, IEditor<MultiFeature>
    {
        private MultiFeature bf;
        public MultiFeatureForm(MultiFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            features1.features = f.Features;
            Amount.DataBindings.Add("Text", f, "Condition", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public MultiFeature edit(IHistoryManager history)
        {
            features1.HistoryManager = history;
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
