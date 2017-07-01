using Character_Builder_Forms;
using OGL;
using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class SkillProficiencyChoiceFeatureForm : Form, IEditor<SkillProficiencyChoiceFeature> {
        private SkillProficiencyChoiceFeature bf;
        public SkillProficiencyChoiceFeatureForm(SkillProficiencyChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
            ProfMultiplier.DataBindings.Add("Value", f, "ProficiencyMultiplier", true, DataSourceUpdateMode.OnPropertyChanged);
            Restrict.DataBindings.Add("Checked", f, "OnlyAlreadyKnownSkills", true, DataSourceUpdateMode.OnPropertyChanged);
            SkillList.Items = f.Skills;
            ImportExtensions.ImportSkills();
            SkillList.Suggestions = Skill.simple.Keys;
            basicFeature1.Feature = f;
        }

        public SkillProficiencyChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}

