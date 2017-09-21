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
    public class ToolProficiencyFeatureEditModel : FeatureEditModel<ToolProficiencyFeature>
    {
        public ToolProficiencyFeatureEditModel(ToolProficiencyFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }

        public List<string> Tools { get => Feature.Tools; }
    }
}
