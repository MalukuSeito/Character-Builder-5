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
            SkillList.Suggestions = Skill.skills.Keys;
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

