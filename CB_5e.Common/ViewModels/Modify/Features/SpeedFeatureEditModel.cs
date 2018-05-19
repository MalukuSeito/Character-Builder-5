using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class SpeedFeatureEditModel : FeatureEditModel<SpeedFeature>
    {
        public SpeedFeatureEditModel(SpeedFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }


        public bool IgnoreArmor
        {
            get => Feature.IgnoreArmor;
            set
            {
                if (value == IgnoreArmor) return;
                MakeHistory("IgnoreArmor");
                Feature.IgnoreArmor = value;
                OnPropertyChanged("IgnoreArmor");
            }
        }

        public int BaseSpeed
        {
            get => Feature.BaseSpeed;
            set
            {
                if (value == BaseSpeed) return;
                MakeHistory("BaseSpeed");
                Feature.BaseSpeed = value;
                OnPropertyChanged("BaseSpeed");
            }
        }

        public string ExtraSpeed
        {
            get => Feature.ExtraSpeed;
            set
            {
                if (value == ExtraSpeed) return;
                MakeHistory("ExtraSpeed");
                Feature.ExtraSpeed = value;
                OnPropertyChanged("ExtraSpeed");
            }
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
    }
}
