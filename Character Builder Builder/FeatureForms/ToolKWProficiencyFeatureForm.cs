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
    public partial class ToolKWProficiencyFeatureForm : Form, IEditor<ToolKWProficiencyFeature>
    {
        private ToolKWProficiencyFeature bf;
        public ToolKWProficiencyFeatureForm(ToolKWProficiencyFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Description.DataBindings.Add("Text", f, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            Expression.DataBindings.Add("Text", f, "Condition", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public ToolKWProficiencyFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
