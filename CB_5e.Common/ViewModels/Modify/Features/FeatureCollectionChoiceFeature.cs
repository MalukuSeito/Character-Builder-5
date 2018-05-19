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
    public class CollectionChoiceFeatureEditModel : FeatureEditModel<CollectionChoiceFeature>
    {
        public CollectionChoiceFeatureEditModel(CollectionChoiceFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }
        public string Condition
        {
            get => Feature.Collection;
            set
            {
                if (value == Condition) return;
                MakeHistory("Collection");
                Feature.Collection = value.Replace('\n', ' ');
                OnPropertyChanged("Condition");
            }
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
        public static HashSet<string> UniqueIDs = new HashSet<string>();
        public List<String> Suggestions { get => UniqueIDs.Where(s => s != null && s != "").OrderBy(s => s).ToList(); }
    }
}
