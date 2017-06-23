using OGL.Keywords;
using System.Windows.Forms;

namespace Character_Builder_Builder.KeywordForms
{
    public partial class RangeForm : Form
    {
        public RangeForm(Range m)
        {
            InitializeComponent();
            ShortRange.DataBindings.Add("Value", m, "Short", true, DataSourceUpdateMode.OnPropertyChanged);
            LongRange.DataBindings.Add("Value", m, "Long", true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
