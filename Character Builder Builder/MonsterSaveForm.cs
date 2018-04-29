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

namespace Character_Builder_Builder
{
    public partial class MonsterSaveForm : Form, IEditor<MonsterSaveBonus>
    {
        private MonsterSaveBonus msb;
        public MonsterSaveForm(MonsterSaveBonus msb)
        {
            this.msb = msb;
            InitializeComponent();
            foreach (OGL.Base.Ability s in Enum.GetValues(typeof(OGL.Base.Ability))) Ability.Items.Add(s);
            Ability.DataBindings.Clear();
            Ability.DataBindings.Add("SelectedItem", msb, "Ability", true, DataSourceUpdateMode.OnPropertyChanged);
            text.DataBindings.Clear();
            text.DataBindings.Add("Text", msb, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            Bonus.DataBindings.Clear();
            Bonus.DataBindings.Add("Value", msb, "Bonus", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public MonsterSaveBonus edit(IHistoryManager history)
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
