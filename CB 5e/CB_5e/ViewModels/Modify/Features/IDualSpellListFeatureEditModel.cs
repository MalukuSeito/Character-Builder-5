using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public interface IDualSpellListFeatureEditModel : IFeatureEditModel
    {
        string Condition { get; set; }
        List<String> Spells { get; }
    }
}
