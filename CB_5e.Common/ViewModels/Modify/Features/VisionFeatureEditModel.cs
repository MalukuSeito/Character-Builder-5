using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;

namespace CB_5e.ViewModels.Modify.Features
{
    public class VisionFeatureEditModel : FeatureEditModel<VisionFeature>
    {
        public VisionFeatureEditModel(VisionFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }

        public int Value
        {
            get => Feature.Range;
            set
            {
                if (value == Value) return;
                MakeHistory("Value");
                Feature.Range = value;
                OnPropertyChanged("Value");
            }
        }
    }
}
