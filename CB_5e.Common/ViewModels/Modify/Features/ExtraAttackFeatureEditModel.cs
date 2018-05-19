using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;

namespace CB_5e.ViewModels.Modify.Features
{
    public class ExtraAttackFeatureEditModel : FeatureEditModel<ExtraAttackFeature>
    {
        public ExtraAttackFeatureEditModel(ExtraAttackFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }

        public int Value
        {
            get => Feature.ExtraAttacks;
            set
            {
                if (value == Value) return;
                MakeHistory("Value");
                Feature.ExtraAttacks = value;
                OnPropertyChanged("Value");
            }
        }
    }
}
