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

namespace Character_Builder_Builder.DescriptionForms
{
    public partial class TableDescriptionForm : Form, IEditor<TableDescription>
    {
        private TableDescription d;
        public TableDescriptionForm(TableDescription td)
        {
            d = td;
            InitializeComponent();
            name.DataBindings.Add("Text", td, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            descText.DataBindings.Add("Text", td, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            amount.DataBindings.Add("Value", td, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
            UniqueID.DataBindings.Add("Text", td, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            dataGridView1.DataSource = new BindingList<TableEntry>(td.Entries);
        }

        public TableDescription edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return d;
        }
    }
}
