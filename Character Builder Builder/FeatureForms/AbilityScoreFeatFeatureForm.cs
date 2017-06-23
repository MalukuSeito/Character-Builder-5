using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class AbilityScoreFeatFeatureForm : Form, IEditor<AbilityScoreFeatFeature>
    {
        private AbilityScoreFeatFeature bf;
        public AbilityScoreFeatFeatureForm(AbilityScoreFeatFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public AbilityScoreFeatFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
