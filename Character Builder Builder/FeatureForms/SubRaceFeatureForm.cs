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

namespace Character_Builder_Builder.FeatureForms
{
    public partial class SubRaceFeatureForm : Form, IEditor<SubRaceFeature>
    {
        private SubRaceFeature bf;
        public SubRaceFeatureForm(SubRaceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Race.ImportAll();
            stringList1.Items = f.Races;
            stringList1.Suggestions = Race.simple.Keys;
        }

        public SubRaceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
