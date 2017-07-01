using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGL.Common
{
    public interface ILicense
    {
        bool ShowLicense(string title, string[] lines);
    }
}
