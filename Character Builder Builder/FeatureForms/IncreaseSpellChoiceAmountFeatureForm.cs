using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class IncreaseSpellChoiceAmountFeatureForm : Form, IEditor<IncreaseSpellChoiceAmountFeature>
    {
        private IncreaseSpellChoiceAmountFeature bf;
        public IncreaseSpellChoiceAmountFeatureForm(IncreaseSpellChoiceAmountFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            foreach (string s in SpellChoiceFeatureForm.SPELLCHOICE_FEATURES)
            {
                UniqueID.AutoCompleteCustomSource.Add(s);
                UniqueID.Items.Add(s);
            }
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public IncreaseSpellChoiceAmountFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
