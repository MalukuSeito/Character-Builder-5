using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character.ChoiceOptions
{
    public class WrongChoiceOption: ChoiceOption
    {
        public WrongChoiceOption(string choice)
        {
            Choice = choice;
            Value = new Feature(choice, "Does not exist or is not valid");
        }
        public string Choice { get; set; }
        public override string NameWithSource => Choice;
        public override Color SelectedColor => Color.DarkRed;
    }
}
