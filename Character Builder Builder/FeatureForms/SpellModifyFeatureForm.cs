using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class SpellModifyFeatureForm : Form, IEditor<SpellModifyFeature>
    {
        private SpellModifyFeature bf;
        public SpellModifyFeatureForm(SpellModifyFeature f)
        {
            bf = f;
            InitializeComponent();
            Condition.DataBindings.Add("Text", f, "Spells", true, DataSourceUpdateMode.OnPropertyChanged);
            basicFeature1.Feature = f;
        }

        public SpellModifyFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
