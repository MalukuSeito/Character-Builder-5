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
    public partial class BonusSpellPrepareFeatureForm : Form, IEditor<BonusSpellPrepareFeature>
    {
        private BonusSpellPrepareFeature bf;
        public BonusSpellPrepareFeatureForm(BonusSpellPrepareFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Character_Builder_5.Spell.ImportAll();
            stringList1.Suggestions = Spell.simple.Keys;
            foreach (string s in SpellcastingFeatureForm.SPELLCASTING_FEATURES)
            {
                SpellcastingID.AutoCompleteCustomSource.Add(s);
                SpellcastingID.Items.Add(s);
            }
            SpellcastingID.DataBindings.Add("Text", bf, "SpellcastingID", true, DataSourceUpdateMode.OnPropertyChanged);
            keywordControl1.Keywords = bf.KeywordsToAdd;
            stringList1.Items = bf.Spells;
        }

        public BonusSpellPrepareFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
