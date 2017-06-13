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
            foreach (BackgroundOption s in Enum.GetValues(typeof(BackgroundOption))) if (s != BackgroundOption.None) BackgroundOptions.Items.Add(s, td.BackgroundOption.HasFlag(s));
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

        private void BackgroundOptions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) d.BackgroundOption |= (BackgroundOption)BackgroundOptions.Items[e.Index];
            else if (e.NewValue == CheckState.Unchecked) d.BackgroundOption &= ~(BackgroundOption)BackgroundOptions.Items[e.Index];
        }
    }
}
