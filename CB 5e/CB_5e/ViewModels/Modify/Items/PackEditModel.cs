using OGL;
using OGL.Common;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Items
{
    public class PackEditModel : ItemEditModel<Pack>
    {
        public PackEditModel(Pack obj, OGLContext context): base(obj, context)
        {

        }
        public List<string> Contents { get => Model.Contents; }
    }
}
