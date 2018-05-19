using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.Services
{
    public interface IHTMLService
    {
        string Convert(IXML obj);
        void Reset(ConfigManager config);
    }
}
