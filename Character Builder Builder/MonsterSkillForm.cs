using OGL.Monsters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OGL.Common;
using Character_Builder_Forms;

namespace Character_Builder_Builder
{
    public partial class MonsterSkillForm : Form, IEditor<MonsterSkillBonus>
    {
        private MonsterSkillBonus msb;
        public MonsterSkillForm(MonsterSkillBonus msb)
        {
            this.msb = msb;
            InitializeComponent();
            Program.Context.ImportSkills();
            foreach (string s in Program.Context.SkillsSimple.Keys)
            {
                Skill.Items.Add(s);
                Skill.AutoCompleteCustomSource.Add(s);
            }
            Skill.DataBindings.Clear();
            Skill.DataBindings.Add("Text", msb, "Skill", true, DataSourceUpdateMode.OnPropertyChanged);
            foreach (OGL.Base.Ability s in Enum.GetValues(typeof(OGL.Base.Ability))) Ability.Items.Add(s);
            Ability.DataBindings.Clear();
            Ability.DataBindings.Add("SelectedItem", msb, "Ability", true, DataSourceUpdateMode.OnPropertyChanged);

            text.DataBindings.Clear();
            text.DataBindings.Add("Text", msb, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            Bonus.DataBindings.Clear();
            Bonus.DataBindings.Add("Value", msb, "Bonus", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public MonsterSkillBonus edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return msb;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
