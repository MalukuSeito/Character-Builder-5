using Character_Builder_Forms;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using System;
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
            foreach (ProficiencyBonus s in Enum.GetValues(typeof(ProficiencyBonus))) BonusType.Items.Add(s);
            BonusType.DataBindings.Add("SelectedItem", bf, "BonusType", true, DataSourceUpdateMode.OnPropertyChanged);
            SkillList.Items = f.Skills;
            ImportExtensions.ImportSkills();
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

