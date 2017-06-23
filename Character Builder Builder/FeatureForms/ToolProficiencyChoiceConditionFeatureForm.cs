using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ToolProficiencyChoiceConditionFeatureForm : Form, IEditor<ToolProficiencyChoiceConditionFeature>
    {
        private ToolProficiencyChoiceConditionFeature bf;
        public ToolProficiencyChoiceConditionFeatureForm(ToolProficiencyChoiceConditionFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
            Expression.DataBindings.Add("Text", f, "Condition", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public ToolProficiencyChoiceConditionFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
