using Character_Builder_5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }

        public ModifySpellChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
