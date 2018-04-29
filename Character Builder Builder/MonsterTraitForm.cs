using OGL.Common;
using OGL.Descriptions;
using OGL.Monsters;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class MonsterTraitForm : Form, IEditor<MonsterTrait>
    {
        MonsterTrait bd;
        public MonsterTraitForm(MonsterTrait d)
        {
            bd = d;
            InitializeComponent();
            name.DataBindings.Add("Text", d, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            //descText.DataBindings.Add("Text", d, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            Binding binding = new Binding("Text", d, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += NewlineFormatter.Binding_Format;
            descText.DataBindings.Add(binding);
        }

        public MonsterTrait edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bd;
        }
    }
}
