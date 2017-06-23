using OGL.Keywords;
using System.Windows.Forms;

namespace Character_Builder_Builder.KeywordForms
{
    public partial class VersatileForm : Form
    {
        public VersatileForm(Versatile m)
        {
            InitializeComponent();
            textBox1.DataBindings.Add("Text", m, "Damage", true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
