using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ChoiceFeatureForm : Form, IEditor<ChoiceFeature>
    {
        private ChoiceFeature bf;

        public ChoiceFeatureForm(ChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
            AllowSameChoice.DataBindings.Add("Checked", f, "AllowSameChoice", true, DataSourceUpdateMode.OnPropertyChanged);
            features1.features = f.Choices;
        }

        public ChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            features1.HistoryManager = history;
            ShowDialog();
            return bf;
        }
    }
}
