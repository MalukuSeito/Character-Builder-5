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
    public partial class SpellModifyFeatureForm : Form, IEditor<SpellModifyFeature>
    {
        private SpellModifyFeature bf;
        public SpellModifyFeatureForm(SpellModifyFeature f)
        {
            bf = f;
            InitializeComponent();
            Condition.DataBindings.Add("Text", f, "Spells", true, DataSourceUpdateMode.OnPropertyChanged);
            basicFeature1.Feature = f;
        }

        public SpellModifyFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
