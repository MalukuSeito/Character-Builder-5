using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Character_Builder_5;

namespace Character_Builder_Builder.ItemForms
{
    public partial class BasicItem : UserControl
    {
        private Item item = null;
        public Item Item {
            get
            {
                return item;
            }
            set
            {
                item = value;
                ItemName.DataBindings.Clear();
                Source.DataBindings.Clear();
                Description.DataBindings.Clear();
                Unit.DataBindings.Clear();
                SingleUnit.DataBindings.Clear();
                Weight.DataBindings.Clear();
                StackSize.DataBindings.Clear();
                PP.DataBindings.Clear();
                GP.DataBindings.Clear();
                EP.DataBindings.Clear();
                SP.DataBindings.Clear();
                CP.DataBindings.Clear();
                if (item != null)
                {
                    KeywordControl.Keywords = item.Keywords;
                    KeywordControl.HistoryManager = HistoryManager;
                    ItemName.DataBindings.Add("Text", item, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
                    Source.DataBindings.Add("Text", item, "Source", true, DataSourceUpdateMode.OnPropertyChanged);
                    //Description.DataBindings.Add("Text", item, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
                    //Binding binding = new Binding("Text", item, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
                    //binding.Format += NewlineFormatter.Binding_Format;
                    //Description.DataBindings.Add(binding);
                    NewlineFormatter.Add(Description.DataBindings, "Text", item, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
                    Unit.DataBindings.Add("Text", item, "Unit", true, DataSourceUpdateMode.OnPropertyChanged);
                    SingleUnit.DataBindings.Add("Text", item, "SingleUnit", true, DataSourceUpdateMode.OnPropertyChanged);
                    Weight.DataBindings.Add("Value", item, "Weight", true, DataSourceUpdateMode.OnPropertyChanged);
                    StackSize.DataBindings.Add("Value", item, "StackSize", true, DataSourceUpdateMode.OnPropertyChanged);
                    PP.DataBindings.Add("Value", item.Price, "pp", true, DataSourceUpdateMode.OnPropertyChanged);
                    GP.DataBindings.Add("Value", item.Price, "gp", true, DataSourceUpdateMode.OnPropertyChanged);
                    EP.DataBindings.Add("Value", item.Price, "ep", true, DataSourceUpdateMode.OnPropertyChanged);
                    SP.DataBindings.Add("Value", item.Price, "sp", true, DataSourceUpdateMode.OnPropertyChanged);
                    CP.DataBindings.Add("Value", item.Price, "cp", true, DataSourceUpdateMode.OnPropertyChanged);
                }
                else
                {
                    KeywordControl.Keywords = null;
                }
            }
        }
        private IMainEditor history;
        public IMainEditor HistoryManager
        {
            get { return history; }
            set { history = value; KeywordControl.HistoryManager = history; }
        }
        public BasicItem()
        {
            InitializeComponent();
        }

        private void showPreview(object sender, EventArgs e)
        {
            history?.ShowPreview();
        }

        private void ItemName_TextChanged(object sender, EventArgs e)
        {
            history?.MakeHistory(sender.ToString());
        }
    }
}
