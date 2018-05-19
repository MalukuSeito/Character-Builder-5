using OGL.Base;
using OGL.Features;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class SpellchoiceFeatureEditModel : FeatureEditModel<SpellChoiceFeature>
    {
        public SpellchoiceFeatureEditModel(SpellChoiceFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }

        public static HashSet<string> UniqueIDs = new HashSet<string>();

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

        public List<String> Suggestions { get => SpellcastingFeatureEditModel.SpellcastingIDs.Where(s => s != null && s != "").OrderBy(s => s).ToList(); }

        public string AvailableSpellChoices
        {
            get => Feature.AvailableSpellChoices;
            set
            {
                if (value == AvailableSpellChoices) return;
                MakeHistory("AvailableSpellChoices");
                Feature.AvailableSpellChoices = value;
                OnPropertyChanged("AvailableSpellChoices");
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
