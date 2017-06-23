using OGL;
using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class SkillProficiencyFeatureForm : Form, IEditor<SkillProficiencyFeature>
    {
        private SkillProficiencyFeature bf;
        public SkillProficiencyFeatureForm(SkillProficiencyFeature f)
        {
            bf = f;
            InitializeComponent();
            ProfMultiplier.DataBindings.Add("Value", f, "ProficiencyMultiplier", true, DataSourceUpdateMode.OnPropertyChanged);
            SkillList.Items = f.Skills;
            Skill.ImportAll();
            SkillList.Suggestions = Skill.simple.Keys;
            basicFeature1.Feature = f;
        }

        public SkillProficiencyFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}

