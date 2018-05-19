using OGL.Base;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class MultiFeatureEditModel : FeatureEditModel<MultiFeature>
    {
        public MultiFeatureEditModel(MultiFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }
        public string Condition
        {
            get => Feature.Condition;
            set
            {
                if (value == Condition) return;
                MakeHistory("Condition");
                Feature.Condition = value;
                OnPropertyChanged("Condition");
            }
        }
        public List<Feature> Features { get => Feature.Features; }
    }
}
