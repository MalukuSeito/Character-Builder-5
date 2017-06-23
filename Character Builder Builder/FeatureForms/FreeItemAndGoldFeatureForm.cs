using OGL;
using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class FreeItemAndGoldFeatureForm : Form, IEditor<FreeItemAndGoldFeature>
    {
        private FreeItemAndGoldFeature bf;
        public FreeItemAndGoldFeatureForm(FreeItemAndGoldFeature f)
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
        }

        public FreeItemAndGoldFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}

