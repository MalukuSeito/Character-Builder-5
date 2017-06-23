using OGL.Common;
using OGL.Features;
using System;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class SpellSlotsFeatureForm : Form, IEditor<SpellSlotsFeature>
    {
        private SpellSlotsFeature bf;
        public SpellSlotsFeatureForm(SpellSlotsFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            intList1.Items = f.Slots;
            SpellcastingID.DataBindings.Add("Text", f, "SpellcastingID", true, DataSourceUpdateMode.OnPropertyChanged);
            foreach (string s in SpellcastingFeatureForm.SPELLCASTING_FEATURES)
            {
                SpellcastingID.AutoCompleteCustomSource.Add(s);
                SpellcastingID.Items.Add(s);
            }
        }

        public SpellSlotsFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bf.Text = bf.tostring();
            basicFeature1.Feature = bf;
        }
    }
}
