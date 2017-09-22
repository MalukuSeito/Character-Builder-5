using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;
using Xamarin.Forms;
using OGL.Base;
using OGL.Keywords;

namespace CB_5e.ViewModels.Modify.Features
{
    public class AddSpellsFeatureEditModel : FeatureEditModel<BonusSpellPrepareFeature>, IDualSpellListFeatureEditModel
    {
        public AddSpellsFeatureEditModel(BonusSpellPrepareFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }
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
        public List<String> Spells { get => Feature.Spells; }
        public string Condition
        {
            get => Feature.Condition;
            set
            {
                if (value == Condition) return;
                MakeHistory("Condition");
                Feature.Condition = value;
                OnPropertyChanged("Condition");
            }
        }
        public string AddTo
        {
            get => Feature.AddTo.ToString();
            set
            {
                PreparationMode parsed = PreparationMode.LearnSpells;
                if (Enum.TryParse(value, out parsed))
                {
                    if (Feature.AddTo == parsed) return;
                    MakeHistory("AddTo");
                    Feature.AddTo = parsed;
                    OnPropertyChanged("AddTo");
                }
                OnPropertyChanged("AddTo");
            }
        }
        public List<string> PreparationTypes
        {
            get => Enum.GetNames(typeof(PreparationMode)).ToList();
        }
        public List<Keyword> AdditionalKeywords { get => Feature.KeywordsToAdd; }
    }
}
