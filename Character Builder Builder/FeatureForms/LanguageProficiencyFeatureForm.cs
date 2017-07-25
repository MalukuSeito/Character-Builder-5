using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class LanguageProficiencyFeatureForm : Form, IEditor<LanguageProficiencyFeature>
    {
        private LanguageProficiencyFeature bf;
        public LanguageProficiencyFeatureForm(LanguageProficiencyFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;

            stringList1.Items = f.Languages;
            Program.Context.ImportLanguages();
            stringList1.Suggestions = Program.Context.LanguagesSimple.Keys;
        }

        public LanguageProficiencyFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}

