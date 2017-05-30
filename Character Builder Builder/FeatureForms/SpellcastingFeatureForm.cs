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
    public partial class SpellcastingFeatureForm : Form, IEditor<SpellcastingFeature>
    {
        public static List<string> SPELLCASTING_FEATURES = new List<string>();
        private SpellcastingFeature bf;
        public SpellcastingFeatureForm(SpellcastingFeature f)
        {
            bf = f;

            if (f.PrepareCountAdditionalModifier != Ability.None)
            {
                f.PrepareCount = ResourceFeatureForm.convert(f.PrepareCountAdditionalModifier, f.PrepareCount);
                f.PrepareCountAdditionalModifier = Ability.None;
            }
            if (f.PrepareCountPerClassLevel != 0)
            {
                f.PrepareCount = (f.PrepareCount == null || f.PrepareCount.Trim() == "0" || f.PrepareCount.Trim() == "" ? "" : f.PrepareCount + " + ") + "ClassLevel" + (f.PrepareCountPerClassLevel > 1 ? " * " + f.PrepareCountPerClassLevel : "");
                f.PrepareCountPerClassLevel = 0;
            }
            if (f.PrepareCountAdditional != 0)
            {
                f.PrepareCount = (f.PrepareCount == null || f.PrepareCount.Trim() == "0" || f.PrepareCount.Trim() == "" ? "" : f.PrepareCount + " + ") + f.PrepareCountAdditional;
                f.PrepareCountAdditional = 0;
            }
            InitializeComponent();
            foreach (Ability s in Enum.GetValues(typeof(Ability))) if (s != Ability.None) SpellcastingAbility.Items.Add(s, f.SpellcastingAbility.HasFlag(s));
            foreach (PreparationMode s in Enum.GetValues(typeof(PreparationMode))) PrepareMode.Items.Add(s);
            PrepareMode.DataBindings.Add("SelectedItem", f, "Preparation", true, DataSourceUpdateMode.OnPropertyChanged);
            SpellcastingID.DataBindings.Add("Text", f, "SpellcastingID", true, DataSourceUpdateMode.OnPropertyChanged);
            Condition.DataBindings.Add("Text", f, "PrepareableSpells", true, DataSourceUpdateMode.OnPropertyChanged);
            checkBox1.DataBindings.Add("Checked", f, "IgnoreMulticlassing", true, DataSourceUpdateMode.OnPropertyChanged);
            DisplayName.DataBindings.Add("Text", f, "DisplayName", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Text", f, "PrepareCount", true, DataSourceUpdateMode.OnPropertyChanged);
            foreach (RechargeModifier s in Enum.GetValues(typeof(RechargeModifier))) Recharge.Items.Add(s);
            Recharge.DataBindings.Add("SelectedItem", f, "PreparationChange", true, DataSourceUpdateMode.OnPropertyChanged);
            basicFeature1.Feature = f;
        }
        private void SpellcastingModifier_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) bf.SpellcastingAbility |= (Ability)SpellcastingAbility.Items[e.Index];
            else if (e.NewValue == CheckState.Unchecked) bf.SpellcastingAbility &= ~(Ability)SpellcastingAbility.Items[e.Index];
        }
        private void SpellcastingFeatureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SpellcastingID.Text.Length > 0 && !SPELLCASTING_FEATURES.Contains(SpellcastingID.Text)) SPELLCASTING_FEATURES.Add(SpellcastingID.Text);
        }

        public SpellcastingFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
