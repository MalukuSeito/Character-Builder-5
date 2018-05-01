using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Character_Builder;

namespace CB_5e.iOS
{
    public class PDFiOSSheet : IPDFSheet
    {
        private bool preserveEdit;
        private Stream output;
        private PdfCopy copy;
        private Document document;

        public PDFiOSSheet(bool preserveEdit, Stream outStream)
        {
            this.preserveEdit = preserveEdit;
            output = outStream;
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
                copy.SetPageSize(x.GetPageSize(i));
                copy.AddPage(copy.GetImportedPage(x, i));
            }
            x.Close();
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