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
    public partial class DescriptionForm : Form, IEditor<Description>
    {
        Description bd;
        public DescriptionForm(Description d)
        {
            bd = d;
            InitializeComponent();
            name.DataBindings.Add("Text", d, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            //descText.DataBindings.Add("Text", d, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            Binding binding = new Binding("Text", d, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += NewlineFormatter.Binding_Format;
            descText.DataBindings.Add(binding);
        }

        public static Description dispatch(Description d, IHistoryManager h)
        {
            if (d is ListDescription) return new ListDescriptionForm(d as ListDescription).edit(h);
            if (d is TableDescription) return new TableDescriptionForm(d as TableDescription).edit(h);
            return new DescriptionForm(d).edit(h);
        }

        public Description edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bd;
        }
    }
}
