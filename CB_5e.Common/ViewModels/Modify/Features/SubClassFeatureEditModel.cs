using CB_5e.Helpers;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class SubClassFeatureEditModel: FeatureEditModel<SubClassFeature>
    {
        public SubClassFeatureEditModel(SubClassFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }

        public string ParentClass
        {
            get => Feature.ParentClass;
            set
            {
                if (value == null) return;
                if (value == ParentClass) return;
                MakeHistory("ParentClass");
                Feature.ParentClass = value;
                OnPropertyChanged("ParentClass");
            }
        }

        public ObservableRangeCollection<string> Suggestions { get; set; } = new ObservableRangeCollection<string>();
    }
}
