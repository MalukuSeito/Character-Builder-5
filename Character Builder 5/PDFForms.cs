using Character_Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Character_Builder_5
{
    public class PDFForms : PDFBase
    {
        public Stream OutStream { get; set; }

        public override async Task<IPDFEditor> CreateEditor(string file)
        {
            using (var reader = File.Open(file, FileMode.Open))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await reader.CopyToAsync(ms);
                    return new PDFFormsEditor(ms.ToArray(), PreserveEdit);
                }
            }
        }

        public override IPDFSheet CreateSheet()
        {
            return new PDFFormsSheet(PreserveEdit, OutStream);
        }
    }
}
