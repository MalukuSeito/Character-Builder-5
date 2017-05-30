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
    public partial class ACFeatureForm : Form, IEditor<ACFeature>
    {
        private ACFeature bf;
        public ACFeatureForm(ACFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Expr.DataBindings.Add("Text", f, "Expression", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public ACFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
