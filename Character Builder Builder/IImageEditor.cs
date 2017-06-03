using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_Builder
{
    public delegate void ImageChanged(object sender, Bitmap Image);
    public interface IImageEditor
    {
        event ImageChanged ImageChanged;
        void SetImage(Bitmap Image);
    }
}
