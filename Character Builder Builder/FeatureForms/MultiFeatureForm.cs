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
    public partial class MultiFeatureForm : Form, IEditor<MultiFeature>
    {
        private MultiFeature bf;
        public MultiFeatureForm(MultiFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            features1.features = f.Features;
            Amount.DataBindings.Add("Text", f, "Condition", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public MultiFeature edit(IHistoryManager history)
        {
            features1.HistoryManager = history;
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
