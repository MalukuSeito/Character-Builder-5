using OGL.Base;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class ChoiceFeatureEditModel : FeatureEditModel<ChoiceFeature>
    {
        public ChoiceFeatureEditModel(ChoiceFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
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
        public bool AllowSameChoice
        {
            get => Feature.AllowSameChoice;
            set
            {
                if (value == AllowSameChoice) return;
                MakeHistory("AllowSameChoice");
                Feature.AllowSameChoice = value;
                OnPropertyChanged("AllowSameChoice");
            }
        }
        public List<Feature> Choices { get => Feature.Choices; }
    }
}
