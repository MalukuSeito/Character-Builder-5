using System;
using OGL.Descriptions;
using System.Windows.Forms;

namespace Character_Builder_Builder.DescriptionForms
{
    public partial class NamesForm : Form
    {
        public NamesForm(Names names)
        {
            InitializeComponent();
            title.DataBindings.Add("Text", names, "Title", true, DataSourceUpdateMode.OnPropertyChanged);
            stringList1.Items = names.ListOfNames;
        }

        private void NamesForm_Load(object sender, EventArgs e)
        {

        }

        private void stringList1_Load(object sender, EventArgs e)
        {

        }
    }
}
