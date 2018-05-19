using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify.Features
{
    public class SpellModifyFeatureEditModel : FeatureEditModel<SpellModifyFeature>
    {
        public SpellModifyFeatureEditModel(SpellModifyFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }

        public string Spells
        {
            get => Feature.Spells;
            set
            {
                if (value == null) return;
                if (value == Spells) return;
                MakeHistory("Spells");
                Feature.Spells = value;
                OnPropertyChanged("Spells");
            }
        }
    }
}
