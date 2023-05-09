using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class ChoiceList: System.Windows.Forms.ListBox
    {
        public IChoiceProvider ChoiceProvider { get; set; }
    }
}
