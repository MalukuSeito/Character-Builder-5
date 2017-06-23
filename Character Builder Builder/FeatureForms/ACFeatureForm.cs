using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ACFeatureForm : Form, IEditor<ACFeature>
    {
        private ACFeature bf;
        public ACFeatureForm(ACFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Expr.DataBindings.Add("Text", f, "Expression", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public ACFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
