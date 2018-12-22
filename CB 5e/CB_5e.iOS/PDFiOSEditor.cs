using System;
using System.Collections.Generic;
using System.Linq;
using Character_Builder;
using System.IO;
using iTextSharp.text.pdf;
using OGL.Common;
using iTextSharp.text;

namespace CB_5e.iOS
{
    public class PDFiOSEditor: IPDFEditor
    {
        private bool preserveEdit;
        private MemoryStream ms;
        private PdfReader sheet;
        private PdfStamper stamper;
        private bool closed;

        public PDFiOSEditor(byte[] file, bool preserveEdit)
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
                img.ScaleToFit(Math.Abs(pos[i + 1] - pos[i + 3]), Math.Abs(pos[i + 2] - pos[i + 4]));
                img.SetAbsolutePosition(pos[i + 1] + Math.Abs(pos[i + 1] - pos[i + 3]) / 2 - img.ScaledWidth / 2, pos[i + 2] + Math.Abs(pos[i + 2] - pos[i + 4]) / 2 - img.ScaledHeight / 2);
                stamper.GetOverContent((int)pos[i]).AddImage(img);
            }
        }
        public void SetTextAndDescriptions(string fieldName, string text, IEnumerable<IInfoText> values, string textAfter = null)
        {
            if (preserveEdit) SetField(fieldName, (text != null ? text + "\n" : "") + String.Join("\n", values.Select(f => f.ToInfo(true))) + (textAfter != null ? "\n" + textAfter : ""));
            else
            {
                int j = 0;
                float[] pos = stamper.AcroFields.GetFieldPositions(fieldName);
                for (int i = 0; i < pos.Length - 4; i += 5)
                {
                    var item = stamper.AcroFields.GetFieldItem(fieldName);
                    var dic = item.GetMerged(j >= item.Size ? 0 : j);
                    TextField t = new TextField(null, null, null);
                    stamper.AcroFields.DecodeGenericDictionary(dic, t);
                    j++;
                    float size = 8;

                    while (size > 2)
                    {
                        ColumnText currentColumnText = new ColumnText(stamper.GetOverContent((int)pos[i]));
                        currentColumnText.SetSimpleColumn(pos[i + 1], pos[i + 2], pos[i + 3], pos[i + 4]);
                        currentColumnText.SetLeading(0, 1);
                        currentColumnText.Alignment = Element.ALIGN_JUSTIFIED;
                        var bold = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.BOLD);
                        var reg = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.NORMAL);
                        if (text != null) currentColumnText.AddElement(new Paragraph(text, reg) { Leading = 0, MultipliedLeading = 1, SpacingAfter = size / 2 });
                        foreach (IInfoText f in values)
                        {
                            Paragraph p = new Paragraph(f.InfoTitle + ":", bold);
                            p.Font = reg;
                            p.SpacingAfter = size / 2;
                            p.SetLeading(0, 1);
                            p.Add(f.InfoText.Trim(new char[] { ' ', '\r', '\n', '\t' }));
                            currentColumnText.AddElement(p);
                        }
                        if (textAfter != null) currentColumnText.AddElement(new Paragraph(textAfter, reg) { Leading = 0, MultipliedLeading = 1 });
                        int res = currentColumnText.Go(true);
                        if (res == ColumnText.NO_MORE_COLUMN)
                        {
                            size -= 0.5f;
                        }
                        else break;
                    }
                    {
                        ColumnText currentColumnText = new ColumnText(stamper.GetOverContent((int)pos[i]));
                        currentColumnText.SetSimpleColumn(pos[i + 1], pos[i + 2], pos[i + 3], pos[i + 4]);
                        currentColumnText.SetLeading(0, 1);
                        currentColumnText.Alignment = Element.ALIGN_JUSTIFIED;
                        var bold = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.BOLD);
                        var reg = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.NORMAL);
                        if (text != null) currentColumnText.AddElement(new Paragraph(text, reg) { Leading = 0, MultipliedLeading = 1, SpacingAfter = size / 2 });
                        foreach (IInfoText f in values)
                        {
                            Paragraph p = new Paragraph(f.InfoTitle + ": ", bold);
                            p.Font = reg;
                            p.SpacingAfter = size / 2;
                            p.SetLeading(0, 1);
                            p.Add(f.InfoText.Trim(new char[] { ' ', '\r', '\n', '\t' }));
                            currentColumnText.AddElement(p);
                        }
                        if (textAfter != null) currentColumnText.AddElement(new Paragraph(textAfter, reg) { Leading = 0, MultipliedLeading = 1 });
                        currentColumnText.Go();
                    }
                }
            }
        }
    }
}