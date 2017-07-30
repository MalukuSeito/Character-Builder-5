using Character_Builder;
using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.Services
{
    public interface IPDFService
    {
        Task ExportPDF(string Exporter, BuilderContext context, bool preserveEdit, bool includeResources, bool log, bool spell);
    }
}
