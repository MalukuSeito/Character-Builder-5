using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify.Features
{
    public class SpellSlotsFeatureEditModel : FeatureEditModel<SpellSlotsFeature>
    {
        public SpellSlotsFeatureEditModel(SpellSlotsFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
            Generate = new Command(() => {
                Text = Feature.tostring();
            });
        }

        public List<int> Slots { get => Feature.Slots; }
        public Command Generate { get; set; }
        public string SpellcastingID
        {
            get => Feature.SpellcastingID;
            set
            {
                if (value == null) return;
                if (value == SpellcastingID) return;
                MakeHistory("SpellcastingID");
                Feature.SpellcastingID = value;
                OnPropertyChanged("SpellcastingID");
            }
        }
        public List<String> Suggestions { get => SpellcastingFeatureEditModel.SpellcastingIDs.Where(s => s != null && s != "").OrderBy(s => s).ToList(); }

    }
}
