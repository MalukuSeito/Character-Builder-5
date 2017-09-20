using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Features;
using System.Globalization;

namespace CB_5e.ViewModels.Modify.Features
{
    public class AbilityFeatureEditModel : FeatureEditModel<AbilityScoreFeature>
    {
        public AbilityFeatureEditModel(AbilityScoreFeature feature, IEditModel parent, FeatureViewModel fvm) : base(feature, parent, fvm)
        {
        }
        public string Strength
        {
            get { return Feature.Strength.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.Strength == parsedInt) return;
                    MakeHistory("Strength");
                    Feature.Strength = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("Strength");
                }
                if (value != "" && value != "-") OnPropertyChanged("Strength");
            }
        }
        public string Dexterity
        {
            get { return Feature.Dexterity.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.Dexterity == parsedInt) return;
                    MakeHistory("Dexterity");
                    Feature.Dexterity = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("Dexterity");
                }
                if (value != "" && value != "-") OnPropertyChanged("Dexterity");
            }
        }
        public string Constitution
        {
            get { return Feature.Constitution.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.Constitution == parsedInt) return;
                    MakeHistory("Constitution");
                    Feature.Constitution = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("Constitution");
                }
                if (value != "" && value != "-") OnPropertyChanged("Constitution");
            }
        }
        public string Intelligence
        {
            get { return Feature.Intelligence.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.Intelligence == parsedInt) return;
                    MakeHistory("Intelligence");
                    Feature.Intelligence = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("Intelligence");
                }
                if (value != "" && value != "-") OnPropertyChanged("Intelligence");
            }
        }
        public string Wisdom
        {
            get { return Feature.Wisdom.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.Wisdom == parsedInt) return;
                    MakeHistory("Wisdom");
                    Feature.Wisdom = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("Wisdom");
                }
                if (value != "" && value != "-") OnPropertyChanged("Wisdom");
            }
        }
        public string Charisma
        {
            get { return Feature.Charisma.ToString(); }
            set
            {
                int parsedInt = 0;
                if (value == "" || value == "-" || int.TryParse(value, NumberStyles.AllowLeadingSign, null, out parsedInt))
                {
                    if (Feature.Charisma == parsedInt) return;
                    MakeHistory("Charisma");
                    Feature.Charisma = parsedInt;
                    if (value != "" && value != "-") OnPropertyChanged("Charisma");
                }
                if (value != "" && value != "-") OnPropertyChanged("Charisma");
            }
        }
        public String Modifier
        {
            get => values[(int)Feature.Modifier];
            set
            {
                if (value == Modifier) return;
                if (values.Contains(value))
                {
                    MakeHistory("Modifier");
                    Feature.Modifier = (AbilityScoreModifikation)values.IndexOf(value);
                    if (value != "" && value != "-") OnPropertyChanged("Modifier");
                }
            }
        }
        private List<String> values = new List<string>()
        {
            "Add to Scores",
            "Set Scores",
            "Add to Maximum",
            "Set Maximum",
            "Bonus to Scores (Add)",
            "Bonus to Scores (Set)",
            "Bonus to Maximum (Add)",
            "Bonus to Maximum (Set)"
        };
        public List<String> ModifierValues { get => values; }
    }
}
