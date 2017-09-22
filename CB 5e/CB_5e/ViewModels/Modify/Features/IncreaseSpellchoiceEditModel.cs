using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify.Features
{
    public class IncreaseSpellchoiceEditModel : FeatureEditModel<IncreaseSpellChoiceAmountFeature>
    {
        public IncreaseSpellchoiceEditModel(IncreaseSpellChoiceAmountFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
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
        public List<String> Suggestions { get => SpellchoiceFeatureEditModel.UniqueIDs.Where(s => s != null && s != "").OrderBy(s => s).ToList(); }

    }
}
