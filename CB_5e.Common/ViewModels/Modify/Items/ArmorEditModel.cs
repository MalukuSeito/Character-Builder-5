using OGL;
using OGL.Common;
using OGL.Items;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Items
{
    public class ArmorEditModel : ItemEditModel<Armor>
    {
        public ArmorEditModel(Armor obj, OGLContext context): base(obj, context)
        {

        }
        public int StrengthRequired { get => Model.StrengthRequired; set { if (value == StrengthRequired) return; MakeHistory("StrengthRequired"); Model.StrengthRequired = value; OnPropertyChanged("StrengthRequired"); } }
        public bool StealthDisadvantage { get => Model.StealthDisadvantage; set { if (value == StealthDisadvantage) return; MakeHistory("StealthDisadvantage"); Model.StealthDisadvantage = value; OnPropertyChanged("StealthDisadvantage"); } }
        public int BaseAC { get => Model.BaseAC; set { if (value == BaseAC) return; MakeHistory("BaseAC"); Model.BaseAC = value; OnPropertyChanged("BaseAC"); } }
    }
}
