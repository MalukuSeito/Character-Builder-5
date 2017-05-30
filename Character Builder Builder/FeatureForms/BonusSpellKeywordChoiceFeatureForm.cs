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
    public partial class BonusSpellKeywordChoiceFeatureForm : Form, IEditor<BonusSpellKeywordChoiceFeature>
    {
        private BonusSpellKeywordChoiceFeature bf;
        public BonusSpellKeywordChoiceFeatureForm(BonusSpellKeywordChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            foreach (Ability s in Enum.GetValues(typeof(Ability))) if (s != Ability.None) SpellcastingModifier.Items.Add(s, f.SpellCastingAbility.HasFlag(s));
            basicFeature1.Feature = f;
            foreach (RechargeModifier s in Enum.GetValues(typeof(RechargeModifier))) Recharge.Items.Add(s);
            Recharge.DataBindings.Add("SelectedItem", bf, "SpellCastModifier", true, DataSourceUpdateMode.OnPropertyChanged);
            Condition.DataBindings.Add("Text", bf, "Condition", true, DataSourceUpdateMode.OnPropertyChanged);
            UniqueID.DataBindings.Add("Text", bf, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", bf, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
            keywordControl1.Keywords = bf.KeywordsToAdd;
        }

        public BonusSpellKeywordChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }

        private void SpellcastingModifier_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) bf.SpellCastingAbility |= (Ability)SpellcastingModifier.Items[e.Index];
            else if (e.NewValue == CheckState.Unchecked) bf.SpellCastingAbility &= ~(Ability)SpellcastingModifier.Items[e.Index];
        }
    }
}
