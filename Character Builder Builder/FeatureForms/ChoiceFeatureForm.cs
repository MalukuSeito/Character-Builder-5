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
    public partial class ChoiceFeatureForm : Form, IEditor<ChoiceFeature>
    {
        private ChoiceFeature bf;

        public ChoiceFeatureForm(ChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
            AllowSameChoice.DataBindings.Add("Checked", f, "AllowSameChoice", true, DataSourceUpdateMode.OnPropertyChanged);
            features1.features = f.Choices;
        }

        public ChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            features1.HistoryManager = history;
            ShowDialog();
            return bf;
        }
    }
}
