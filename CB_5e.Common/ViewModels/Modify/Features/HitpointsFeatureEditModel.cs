using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;

namespace CB_5e.ViewModels.Modify.Features
{
    public class HitpointsFeatureEditModel : FeatureEditModel<HitPointsFeature>
    {
        public HitpointsFeatureEditModel(HitPointsFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
            if (f.HitPointsPerLevel > 0)
            {
                f.HitPoints = (f.HitPoints == null || f.HitPoints.Trim() == "0" || f.HitPoints.Trim() == "" ? "" : f.HitPoints + " + ") + "PlayerLevel " + (f.HitPointsPerLevel > 1 ? " * " + f.HitPointsPerLevel : "");
                f.HitPointsPerLevel = 0;
            }
        }

        public string Expression
        {
            get => Feature.HitPoints;
            set
            {
                if (value == Expression) return;
                MakeHistory("Expression");
                Feature.HitPoints = value;
                OnPropertyChanged("Expression");
            }
        }
    }
}
