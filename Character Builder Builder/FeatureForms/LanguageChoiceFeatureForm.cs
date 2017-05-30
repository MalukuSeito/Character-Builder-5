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
    public partial class LanguageChoiceFeatureForm : Form, IEditor<LanguageChoiceFeature>
    {
        private LanguageChoiceFeature bf;
        public LanguageChoiceFeatureForm(LanguageChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public LanguageChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
