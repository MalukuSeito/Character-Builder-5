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
    public class ItemChoiceEditModel : FeatureEditModel<ItemChoiceFeature>
    {
        public ItemChoiceEditModel(ItemChoiceFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }
        public List<string> Items { get => Feature.Items; }
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
        public string CP
        {
            get => Feature.CP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.CP == parsedInt) return;
                    MakeHistory("CP");
                    Feature.CP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("CP");
                }
                if (value != "" && value != "-") OnPropertyChanged("CP");
            }
        }

        public string SP
        {
            get => Feature.SP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.SP == parsedInt) return;
                    MakeHistory("SP");
                    Feature.SP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("SP");
                }
                if (value != "" && value != "-") OnPropertyChanged("SP");
            }
        }
        public string GP
        {
            get => Feature.GP.ToString();
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.GP == parsedInt) return;
                    MakeHistory("GP");
                    Feature.GP = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("GP");
                }
                if (value != "" && value != "-") OnPropertyChanged("GP");
            }
        }
    }
}
