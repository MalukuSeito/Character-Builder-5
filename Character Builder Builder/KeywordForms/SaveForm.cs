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

namespace Character_Builder_Builder.KeywordForms
{
    public partial class SaveForm : Form
    {
        private Save ss;
        public SaveForm(Save s)
        {
            ss = s;
            InitializeComponent();
            foreach (Ability a in Enum.GetValues(typeof(Ability))) if (a != Ability.None) checkedListBox1.Items.Add(a, s.Throw.HasFlag(a));
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) ss.Throw |= (Ability)checkedListBox1.Items[e.Index];
            else if (e.NewValue == CheckState.Unchecked) ss.Throw &= ~(Ability)checkedListBox1.Items[e.Index];
        }
    }
}
