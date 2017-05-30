using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;

namespace Character_Builder_5
{
    public class NoScrollPanel: Panel
    {
        protected override Point ScrollToControl(Control activeControl)
        {
            Point p = DisplayRectangle.Location;
            p.Y -= Padding.Top;
            return p;
        }
    }
}
