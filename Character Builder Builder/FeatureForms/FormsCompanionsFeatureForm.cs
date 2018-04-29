using OGL.Common;
using OGL.Features;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class FormsCompanionsFeatureForm : Form, IEditor<FormsCompanionsFeature>
    {
        public static List<string> MONSTER_FEATURES = new List<string>();
        private FormsCompanionsFeature bf;
        public FormsCompanionsFeatureForm(FormsCompanionsFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            FormCompCount.DataBindings.Add("Value", f, "FormsCompanionsCount", true, DataSourceUpdateMode.OnPropertyChanged);
            FormsCompanionOptions.DataBindings.Add("Text", f, "FormsCompanionsOptions", true, DataSourceUpdateMode.OnPropertyChanged);
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            DisplayName.DataBindings.Add("Text", f, "DisplayName", true, DataSourceUpdateMode.OnPropertyChanged);

        }

        public FormsCompanionsFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }

        private void FormsCompanionsFeatureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UniqueID.Text.Length > 0 && !MONSTER_FEATURES.Contains(UniqueID.Text)) MONSTER_FEATURES.Add(UniqueID.Text);
        }
    }
}
