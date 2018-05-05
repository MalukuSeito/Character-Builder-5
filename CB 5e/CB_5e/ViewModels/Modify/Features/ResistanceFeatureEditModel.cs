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
    public class ResistanceFeatureEditModel : FeatureEditModel<ResistanceFeature>
    {
        public ResistanceFeatureEditModel(ResistanceFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
        }
        public List<string> Resistances { get => Feature.Resistances; }
        public List<string> Vulnerabilities { get => Feature.Vulnerabilities; }
        public List<string> Immunities { get => Feature.Immunities; }
        public List<string> SavingThrowAdvantages { get => Feature.SavingThrowAdvantages; }
    }
}
