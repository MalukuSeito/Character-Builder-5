using Character_Builder_5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ItemChoiceFeatureForm : Form, IEditor<ItemChoiceFeature>
    {
        private ItemChoiceFeature bf;
        public ItemChoiceFeatureForm(ItemChoiceFeature f)
        {
            bf = f;
            InitializeComponent();
            CP.DataBindings.Add("Value", f, "CP", true, DataSourceUpdateMode.OnPropertyChanged);
            SP.DataBindings.Add("Value", f, "SP", true, DataSourceUpdateMode.OnPropertyChanged);
            GP.DataBindings.Add("Value", f, "GP", true, DataSourceUpdateMode.OnPropertyChanged);
            basicFeature1.Feature = f;
            stringList1.Items = f.Items;
            Item.ImportAll();
            stringList1.Suggestions = Item.simple.Keys;
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            Amount.DataBindings.Add("Value", f, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public ItemChoiceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}

