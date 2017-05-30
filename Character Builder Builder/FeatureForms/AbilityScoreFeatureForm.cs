using System;
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
    public partial class AbilityScoreFeatureForm : Form, IEditor<AbilityScoreFeature>
    {
        private AbilityScoreFeature af;
        public AbilityScoreFeatureForm(AbilityScoreFeature f)
        {
            InitializeComponent();
            basicFeature1.Feature = f;
            Str.DataBindings.Add("Value", f, "Strength", true, DataSourceUpdateMode.OnPropertyChanged);
            Con.DataBindings.Add("Value", f, "Constitution", true, DataSourceUpdateMode.OnPropertyChanged);
            Dex.DataBindings.Add("Value", f, "Dexterity", true, DataSourceUpdateMode.OnPropertyChanged);
            Int.DataBindings.Add("Value", f, "Intelligence", true, DataSourceUpdateMode.OnPropertyChanged);
            Wis.DataBindings.Add("Value", f, "Wisdom", true, DataSourceUpdateMode.OnPropertyChanged);
            Cha.DataBindings.Add("Value", f, "Charisma", true, DataSourceUpdateMode.OnPropertyChanged);
            af = f;
            Mode.SelectedIndex = (int)f.Modifier;
            
        }

        public AbilityScoreFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return af;
        }

        private void Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            af.Modifier = (AbilityScoreModifikation)Mode.SelectedIndex;
        }
    }
}
