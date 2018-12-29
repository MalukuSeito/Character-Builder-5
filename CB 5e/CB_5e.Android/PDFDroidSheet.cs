using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Character_Builder;
using System.Threading.Tasks;

namespace CB_5e.Droid
{
    public class PDFDroidSheet : IPDFSheet
    {
        private bool preserveEdit;
        private bool duplex;
        private bool duplexWhite;
        private Stream output;
        private PdfCopy copy;
        private PDFBase pdf;
        private Document document;
        private Rectangle lastSize;
        private int count = 0;

        public PDFDroidSheet(bool preserveEdit, Stream outStream, bool duplex, bool duplexWhite, PDFBase pdf)
        {
            this.preserveEdit = preserveEdit;
            this.duplex = duplex;
            output = outStream;
            this.pdf = pdf;
            this.duplexWhite = duplexWhite;
        }

        public void Add(IPDFEditor sheet)
        {
            PdfReader x = new PdfReader(sheet.Result);
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
            x.Close();
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
            }
        }
    }
}