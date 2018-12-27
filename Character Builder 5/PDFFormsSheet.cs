using Character_Builder;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    internal class PDFFormsSheet : IPDFSheet
    {
        private bool preserveEdit;
        private bool duplex;
        private bool duplexWhite;
        private Stream output;
        private PDFBase pdf;
        private PdfCopy copy;
        private Document document;
        int count = 0;
        private Rectangle lastSize;

        public PDFFormsSheet(bool preserveEdit, bool duplex, bool duplexWhite, Stream outStream, PDFBase pdf)
        {
            this.preserveEdit = preserveEdit;
            this.duplex = duplex;
            output = outStream;
            this.pdf = pdf;
            this.duplexWhite = duplexWhite;
        }

        public void Add(IPDFEditor sheet)
        {
            using (PdfReader x = new PdfReader(sheet.Result))
            {
                if (preserveEdit) x.RemoveUsageRights();
                if (copy == null)
                {
                    document = new Document(x.GetPageSizeWithRotation(1));
                    copy = new PdfCopy(document, output);
                    document.Open();
                }
                for (int i = 1; i <= x.NumberOfPages; i++)
                {
                    count++;
                    copy.SetPageSize(x.GetPageSize(i));
                    copy.AddPage(copy.GetImportedPage(x, i));
                    lastSize = x.GetPageSize(i);
                }
                
            }
        }

        public async Task AddBlankPages(string file)
        {
            if (duplex && count % 2 != 0)
            {
                if (!duplexWhite && file != null && file != "")
                {
                    Add(await pdf.CreateEditor(file));
                }
                else
                {
                    copy.SetPageSize(lastSize);
                    copy.AddPage(lastSize, 0);
                    count++;
                }
            }
        }

        public void Dispose()
        {
            if (document != null) document.Close();
            if (copy != null)
            {
                copy.CloseStream = false;
                copy.Close();
                copy.Dispose();
            }
        }
    }
}