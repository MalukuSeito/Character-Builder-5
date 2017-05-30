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
    public partial class HitPointsFeatureForm : Form, IEditor<HitPointsFeature>
    {
        private HitPointsFeature bf;
        public HitPointsFeatureForm(HitPointsFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            if (f.HitPointsPerLevel > 0)
            {
                f.HitPoints = (f.HitPoints == null || f.HitPoints.Trim() == "0" || f.HitPoints.Trim() == "" ? "" : f.HitPoints + " + ") + "PlayerLevel " + (f.HitPointsPerLevel > 1 ? " * " + f.HitPointsPerLevel : "");
                f.HitPointsPerLevel = 0;
            }
            Amount.DataBindings.Add("Text", f, "HitPoints", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public HitPointsFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
