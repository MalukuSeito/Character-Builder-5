using Character_Builder;
using iTextSharp.text.pdf;
using System.IO;
using System;
using iTextSharp.text;

namespace Character_Builder_5
{
    internal class PDFFormsEditor : IPDFEditor
    {
        private bool preserveEdit;
        private MemoryStream ms;
        private PdfReader sheet;
        private PdfStamper stamper;
        private bool closed;

        public PDFFormsEditor(byte[] file, bool preserveEdit)
        {
            this.preserveEdit = preserveEdit;
            ms = new MemoryStream();
            sheet = new PdfReader(file);
            if (preserveEdit) sheet.RemoveUsageRights();
            stamper = new PdfStamper(sheet, ms);
        }


        public byte[] Result
        {
            get
            {
                if (!closed)
                {
                    stamper.FormFlattening = !preserveEdit;
                    stamper.Writer.CloseStream = false;
                    stamper.Dispose();
                    sheet.Dispose();
                    closed = true;
                }
                return ms.ToArray();
            }
        }
        public void Dispose()
        {
            if (!closed)
            {
                stamper.FormFlattening = !preserveEdit;
                stamper.Writer.CloseStream = false;
                stamper.Dispose();
                sheet.Dispose();
            }
            ms.Dispose();
        }

        public void SetField(string fieldName, string value)
        {
            if (closed) throw new InvalidOperationException("PDF already closed");
            stamper.AcroFields.SetField(fieldName, value);
        }

        public void SetImage(string fieldName, byte[] image)
        {
            if (image == null) return;
            using (MemoryStream ms = new MemoryStream(image)) {
                foreach (AcroFields.FieldPosition pos in stamper.AcroFields.GetFieldPositions(fieldName))
                {
                    Image img = Image.GetInstance(new System.Drawing.Bitmap(ms), BaseColor.WHITE);
                    img.ScaleToFit(pos.position.Width, pos.position.Height);
                    img.SetAbsolutePosition(pos.position.Left + pos.position.Width / 2 - img.ScaledWidth / 2, pos.position.Bottom + pos.position.Height / 2 - img.ScaledHeight / 2);
                    stamper.GetOverContent(pos.page).AddImage(img);
                }
            }
        }
    }
}