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
    public class LanguageProficiencyChoiceFeatureEditModel : FeatureEditModel<LanguageChoiceFeature>
    {
        public LanguageProficiencyChoiceFeatureEditModel(LanguageChoiceFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
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

        public int Amount
        {
            get => Feature.Amount;
            set
            {
                if (value == Amount) return;
                MakeHistory("Amount");
                Feature.Amount = value;
                OnPropertyChanged("Amount");
            }
        }
    }
}
