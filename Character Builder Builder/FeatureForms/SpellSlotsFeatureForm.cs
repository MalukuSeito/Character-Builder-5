using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Character_Builder_5;

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
