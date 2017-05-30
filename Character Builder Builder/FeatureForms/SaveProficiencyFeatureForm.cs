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
    public partial class SaveProficiencyFeatureForm : Form, IEditor<SaveProficiencyFeature>
    {
        private SaveProficiencyFeature bf;
        public SaveProficiencyFeatureForm(SaveProficiencyFeature f)
        {
            bf = f;
            InitializeComponent();
            foreach (Ability s in Enum.GetValues(typeof(Ability))) if (s != Ability.None) Abilities.Items.Add(s, f.Ability.HasFlag(s));
            basicFeature1.Feature = f;
        }

        public SaveProficiencyFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }

        private void SpellcastingModifier_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) bf.Ability |= (Ability)Abilities.Items[e.Index];
            else if (e.NewValue == CheckState.Unchecked) bf.Ability &= ~(Ability)Abilities.Items[e.Index];
        }
    }
}
