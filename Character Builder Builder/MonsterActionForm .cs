using OGL.Common;
using OGL.Descriptions;
using OGL.Monsters;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    public partial class MonsterActionForm : Form, IEditor<MonsterAction>
    {
        MonsterAction bd;
        public MonsterActionForm(MonsterAction d)
        {
            bd = d;
            InitializeComponent();
            name.DataBindings.Add("Text", d, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            Damage.DataBindings.Add("Text", d, "Damage", true, DataSourceUpdateMode.OnPropertyChanged);
            Attack.DataBindings.Add("Value", d, "AttackBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            //descText.DataBindings.Add("Text", d, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            Binding binding = new Binding("Text", d, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += NewlineFormatter.Binding_Format;
            descText.DataBindings.Add(binding);
        }

        public MonsterAction edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bd;
        }
    }
}
