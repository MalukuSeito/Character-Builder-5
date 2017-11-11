using Character_Builder_Forms;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using System;
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
            Program.Context.ImportSpells();
            stringList1.Suggestions = Program.Context.SpellsSimple.Keys;
            foreach (string s in SpellcastingFeatureForm.SPELLCASTING_FEATURES)
            {
                SpellcastingID.AutoCompleteCustomSource.Add(s);
                SpellcastingID.Items.Add(s);
            }
            SpellcastingID.DataBindings.Add("Text", bf, "SpellcastingID", true, DataSourceUpdateMode.OnPropertyChanged);
            Condition.DataBindings.Add("Text", f, "Condition", true, DataSourceUpdateMode.OnPropertyChanged);
            foreach (PreparationMode s in Enum.GetValues(typeof(PreparationMode))) AddTo.Items.Add(s);
            AddTo.DataBindings.Add("SelectedItem", f, "AddTo", true, DataSourceUpdateMode.OnPropertyChanged);
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
