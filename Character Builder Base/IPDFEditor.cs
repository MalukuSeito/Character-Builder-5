using OGL.Common;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public interface IPDFEditor : IDisposable
    {
        byte[] Result { get; }

        void SetField(string fieldName, string value);
        void SetImage(string fieldName, byte[] image);
        void SetTextAndDescriptions(string fieldName, string text, IEnumerable<IInfoText> values, string textAfter = null);
    }
}
