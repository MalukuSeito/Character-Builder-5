using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;
using Xamarin.Forms;
using OGL.Keywords;

namespace CB_5e.ViewModels.Modify.Features
{
    public class ModifySpellchoiceEditModel : FeatureEditModel<ModifySpellChoiceFeature>, IDualSpellListFeatureEditModel
    {
        public ModifySpellchoiceEditModel(ModifySpellChoiceFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }
        public string Condition
        {
            get => Feature.AdditionalSpellChoices;
            set
            {
                if (value == Condition) return;
                MakeHistory("AdditionalSpellChoices");
                Feature.AdditionalSpellChoices = value;
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
        public List<String> Suggestions { get => SpellchoiceFeatureEditModel.UniqueIDs.Where(s => s != null && s != "").OrderBy(s => s).ToList(); }

        public List<string> Spells => Feature.AdditionalSpells;
        public List<Keyword> AdditionalKeywords => Feature.KeywordsToAdd;
    }
}
