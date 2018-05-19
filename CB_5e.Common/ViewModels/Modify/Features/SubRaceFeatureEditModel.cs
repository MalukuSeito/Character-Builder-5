using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class SubRaceFeatureEditModel : FeatureEditModel<SubRaceFeature>
    {
        public SubRaceFeatureEditModel(SubRaceFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }

        public List<string> ParentRaces { get => Feature.Races; }
    }
}
