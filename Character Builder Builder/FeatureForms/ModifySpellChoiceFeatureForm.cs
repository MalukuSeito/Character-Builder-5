using OGL.Common;
using OGL.Features;
using Character_Builder_Forms;
using System.Windows.Forms;
using OGL;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ModifySpellChoiceFeatureForm : Form, IEditor<ModifySpellChoiceFeature>
    {
        private ModifySpellChoiceFeature bf;
        public ModifySpellChoiceFeatureForm(ModifySpellChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            foreach (string s in SpellChoiceFeatureForm.SPELLCHOICE_FEATURES)
            {
                UniqueID.AutoCompleteCustomSource.Add(s);
                UniqueID.Items.Add(s);
            }
            Condition.DataBindings.Add("Text", f, "AdditionalSpellChoices", true, DataSourceUpdateMode.OnPropertyChanged);
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            keywordControl1.Keywords = f.KeywordsToAdd;
            AdditinalSpells.Items = f.AdditionalSpells;
            Program.Context.ImportSpells();
            AdditinalSpells.Suggestions = Program.Context.SpellsSimple.Keys;
        }

        public ModifySpellChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
