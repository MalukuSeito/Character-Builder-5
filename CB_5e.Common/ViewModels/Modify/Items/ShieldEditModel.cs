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
    public class ShieldEditModel : ItemEditModel<Shield>
    {
        public ShieldEditModel(Shield obj, OGLContext context): base(obj, context)
        {

        }
        public int ACBonus { get => Model.ACBonus; set { if (value == ACBonus) return; MakeHistory("ACBonus"); Model.ACBonus = value; OnPropertyChanged("ACBonus"); } }
    }
}
