using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ToolKWProficiencyFeatureForm : Form, IEditor<ToolKWProficiencyFeature>
    {
        private ToolKWProficiencyFeature bf;
        public ToolKWProficiencyFeatureForm(ToolKWProficiencyFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Description.DataBindings.Add("Text", f, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            Expression.DataBindings.Add("Text", f, "Condition", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public ToolKWProficiencyFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
