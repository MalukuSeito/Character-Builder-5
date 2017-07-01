using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class SubRaceFeatureForm : Form, IEditor<SubRaceFeature>
    {
        private SubRaceFeature bf;
        public SubRaceFeatureForm(SubRaceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            ImportExtensions.ImportRaces();
            stringList1.Items = f.Races;
            stringList1.Suggestions = Race.simple.Keys;
        }

        public SubRaceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
