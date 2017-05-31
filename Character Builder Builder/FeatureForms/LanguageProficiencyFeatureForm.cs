using Character_Builder_5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Language.ImportAll();
            stringList1.Suggestions = Language.simple.Keys;
        }

        public LanguageProficiencyFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}

