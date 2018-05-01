using Character_Builder;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Character_Builder_5
{
    internal class PDFFormsSheet : IPDFSheet
    {
        private bool preserveEdit;
        private Stream output;
        private PdfCopy copy;
        private Document document;

        public PDFFormsSheet(bool preserveEdit, Stream outStream)
        {
            this.preserveEdit = preserveEdit;
            output = outStream;
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
                    copy.SetPageSize(x.GetPageSize(i));
                    copy.AddPage(copy.GetImportedPage(x, i));
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