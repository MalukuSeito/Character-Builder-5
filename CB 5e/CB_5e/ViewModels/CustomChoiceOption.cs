using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class CustomChoiceOption: ChoiceOption
    {
        public CustomChoiceOption(string choice)
        {
            Choice = choice;
            Value = new Feature(choice, "Custom Value");
        }
        public string Choice { get; set; }
        public override string NameWithSource => Choice;
        public override Color SelectedColor => Color.DarkBlue;
    }
}
