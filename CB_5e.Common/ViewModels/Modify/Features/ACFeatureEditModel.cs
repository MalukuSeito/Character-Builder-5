using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;

namespace CB_5e.ViewModels.Modify.Features
{
    public class ACFeatureEditModel : FeatureEditModel<ACFeature>
    {
        public ACFeatureEditModel(ACFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }

        public string Expression
        {
            get => Feature.Expression;
            set
            {
                if (value == Expression) return;
                MakeHistory("Expression");
                Feature.Expression = value;
                OnPropertyChanged("Expression");
            }
        }
    }
}
