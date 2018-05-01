using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public abstract class PDFBase
    {
        public bool PreserveEdit { get; set; }
        public bool IncludeResources { get; set; }
        public bool IncludeLog { get; set; }
        public bool IncludeSpellbook { get; set; }
        public bool IncludeActions { get; set; }
        public abstract Task<IPDFEditor> CreateEditor(string file);
        public abstract IPDFSheet CreateSheet();
    }
}
