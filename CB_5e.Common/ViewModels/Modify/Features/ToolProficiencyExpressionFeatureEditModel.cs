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
    public class ToolProficiencyExpressionFeatureEditModel : FeatureEditModel<ToolKWProficiencyFeature>
    {
        public ToolProficiencyExpressionFeatureEditModel(ToolKWProficiencyFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }
        public string Condition
        {
            get => Feature.Condition;
            set
            {
                if (value == Condition) return;
                MakeHistory("Condition");
                Feature.Condition = value.Replace('\n', ' ');
                OnPropertyChanged("Condition");
            }
        }
        public string Description
        {
            get => Feature.Description;
            set
            {
                if (value == Description) return;
                MakeHistory("Description");
                Feature.Description = value;
                OnPropertyChanged("Description");
            }
        }


    }
}
