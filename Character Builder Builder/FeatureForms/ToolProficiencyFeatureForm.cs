using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ToolProficiencyFeatureForm : Form, IEditor<ToolProficiencyFeature>
    {
        private ToolProficiencyFeature bf;
        public ToolProficiencyFeatureForm(ToolProficiencyFeature f)
        {
            bf = f;
            InitializeComponent();

            basicFeature1.Feature = f;
            Program.Context.ImportItems();
            stringList1.Items = f.Tools;
            stringList1.Suggestions = Program.Context.ItemsSimple.Keys;
        }

        public ToolProficiencyFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
