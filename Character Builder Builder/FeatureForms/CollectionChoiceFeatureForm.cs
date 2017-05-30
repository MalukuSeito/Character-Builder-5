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
    public partial class CollectionChoiceFeatureForm : Form, IEditor<CollectionChoiceFeature>
    {
        private CollectionChoiceFeature bf;
        public CollectionChoiceFeatureForm(CollectionChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
            AllowSameChoice.DataBindings.Add("Checked", f, "AllowSameChoice", true, DataSourceUpdateMode.OnPropertyChanged);
            Expression.DataBindings.Add("Text", f, "Collection", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public CollectionChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
