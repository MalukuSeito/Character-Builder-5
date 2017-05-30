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
    public partial class SpellChoiceFeatureForm : Form, IEditor<SpellChoiceFeature>
    {
        public static List<string> SPELLCHOICE_FEATURES = new List<string>();
        SpellChoiceFeature bf;
        public SpellChoiceFeatureForm(SpellChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            keywordControl1.Keywords = f.KeywordsToAdd;
            Condition.DataBindings.Add("Text", f, "AvailableSpellChoices", true, DataSourceUpdateMode.OnPropertyChanged);
            foreach (string s in SpellcastingFeatureForm.SPELLCASTING_FEATURES)
            {
                SpellcastingID.AutoCompleteCustomSource.Add(s);
                SpellcastingID.Items.Add(s);
            }
            SpellcastingID.DataBindings.Add("Text", f, "SpellcastingID", true, DataSourceUpdateMode.OnPropertyChanged);
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            foreach (PreparationMode s in Enum.GetValues(typeof(PreparationMode))) AddTo.Items.Add(s);
            AddTo.DataBindings.Add("SelectedItem", f, "AddTo", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public SpellChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }

        private void SpellChoiceFeatureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UniqueID.Text.Length > 0 && !SPELLCHOICE_FEATURES.Contains(UniqueID.Text)) SPELLCHOICE_FEATURES.Add(UniqueID.Text);
        }
    }
}
