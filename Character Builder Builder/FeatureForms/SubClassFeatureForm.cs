using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

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
            Program.Context.ImportClasses(true, false);
            foreach (string s in Program.Context.ClassesSimple.Keys)
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
