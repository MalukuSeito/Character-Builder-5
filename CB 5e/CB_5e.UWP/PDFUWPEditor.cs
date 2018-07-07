using System;
using System.Collections.Generic;
using System.Linq;
using Character_Builder;
using System.IO;
using iTextSharp.text.pdf;

namespace CB_5e.UWP
{
    public class PDFUWPEditor : IPDFEditor
    {
        private bool preserveEdit;
        private MemoryStream ms;
        private PdfReader sheet;
        private PdfStamper stamper;
        private bool closed;

        public PDFUWPEditor(byte[] file, bool preserveEdit)
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
                    stamper.Close();
                    sheet.Close();
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
                stamper.Close();
                sheet.Close();
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
            float[] pos = stamper.AcroFields.GetFieldPositions(fieldName);
            for (int i = 0; i < pos.Length - 4; i += 5)
            {
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(image);
                img.ScaleToFit(Math.Abs(pos[i + 1] - pos[3]), Math.Abs(pos[i + 2] - pos[i + 4]));
                img.SetAbsolutePosition(pos[i + 1] + Math.Abs(pos[i + 1] - pos[i + 3]) / 2 - img.ScaledWidth / 2, pos[i + 2] + Math.Abs(pos[i + 2] - pos[i + 4]) / 2 - img.ScaledHeight / 2);
                stamper.GetOverContent((int)pos[i]).AddImage(img);
            }
        }
    }
}