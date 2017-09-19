using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;

namespace CB_5e.ViewModels.Modify.Features
{
    public class AbilityFeatFeatureEditModel : FeatureEditModel<AbilityScoreFeatFeature>
    {
        public AbilityFeatFeatureEditModel(AbilityScoreFeatFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }

        public string UniqueID
        {
            get => Feature.UniqueID;
            set
            {
                if (value == null) return;
                if (value == UniqueID) return;
                MakeHistory("UniqueID");
                Feature.UniqueID = value;
                OnPropertyChanged("UniqueID");
            }
        }
        public static HashSet<string> UniqueIDs = new HashSet<string>();
        public List<String> Suggestions { get => UniqueIDs.Where(s => s != null && s != "").OrderBy(s => s).ToList(); }
    }
}
