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
        bool PreserveEdit { get; set; }
        bool IncludeResources { get; set; }
        bool IncludeLog { get; set; }
        bool IncludeSpellbook { get; set; }
        bool IncludeActions { get; set; }
        Task ExportPDF(string Exporter, BuilderContext context);
    }
}
