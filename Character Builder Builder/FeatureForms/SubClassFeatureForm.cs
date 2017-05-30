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
    public partial class SubClassFeatureForm : Form, IEditor<SubClassFeature>
    {
        private SubClassFeature bf;
        public SubClassFeatureForm(SubClassFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Parentclass.DataBindings.Add("Text", f, "ParentClass", true, DataSourceUpdateMode.OnPropertyChanged);
            ClassDefinition.ImportAll();
            foreach (string s in ClassDefinition.simple.Keys)
            {
                Parentclass.AutoCompleteCustomSource.Add(s);
                Parentclass.Items.Add(s);
            }
        }

        public SubClassFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
