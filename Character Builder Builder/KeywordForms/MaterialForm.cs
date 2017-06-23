using OGL.Keywords;
using System.Windows.Forms;

namespace Character_Builder_Builder.KeywordForms
{
    public partial class MaterialForm : Form
    {
        public MaterialForm(Material m)
        {
            InitializeComponent();
            textBox1.DataBindings.Add("Text", m, "Components", true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
