using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ResistanceFeatureForm : Form, IEditor<ResistanceFeature>
    {
        private ResistanceFeature bf;
        public ResistanceFeatureForm(ResistanceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Immunities.Items = f.Immunities;
            Resistances.Items = f.Resistances;
            Vulnerabilities.Items = f.Vulnerabilities;
            SavingThrowAdvantages.Items = f.SavingThrowAdvantages;
        }

        public ResistanceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
