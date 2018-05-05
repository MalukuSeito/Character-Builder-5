using OGL.Base;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class FormsCompanionsFeatureEditModel : FeatureEditModel<FormsCompanionsFeature>
    {
        public FormsCompanionsFeatureEditModel(FormsCompanionsFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }

        public string UniqueID
        {
            get => Feature.UniqueID;
            set
            {
                if (value == UniqueID) return;
                MakeHistory("UniqueID");
                Feature.UniqueID = value;
                OnPropertyChanged("UniqueID");
            }
        }

        public int FormsCompanionsCount
        {
            get => Feature.FormsCompanionsCount;
            set
            {
                if (value == FormsCompanionsCount) return;
                MakeHistory("FormsCompanionsCount");
                Feature.FormsCompanionsCount = value;
                OnPropertyChanged("FormsCompanionsCount");
            }
        }

        public int FormsCompanionsCountValue { get => FormsCompanionsCount + 1; set => FormsCompanionsCount = value - 1; }

        public string FormsCompanionsOptions
        {
            get => Feature.FormsCompanionsOptions;
            set
            {
                if (value == FormsCompanionsOptions) return;
                MakeHistory("FormsCompanionsOptions");
                Feature.FormsCompanionsOptions = value;
                OnPropertyChanged("FormsCompanionsOptions");
            }
        }

        public string DisplayName
        {
            get => Feature.DisplayName;
            set
            {
                if (value == DisplayName) return;
                MakeHistory("DisplayName");
                Feature.DisplayName = value;
                OnPropertyChanged("DisplayName");
            }
        }
    }
}
