using Character_Builder;
using iTextSharp.text.pdf;
using System.IO;
using System;
using iTextSharp.text;
using OGL.Descriptions;
using OGL.Features;
using System.Collections.Generic;
using System.Linq;
using OGL.Common;

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

        //public void SetFeatures(string fieldName, IEnumerable<IInfoText> values)
        //{
        //    if (preserveEdit) SetField(fieldName, String.Join("\n", values.Select(f=>f.ToInfo(true))));
        //    else
        //    {
        //        int i = 0;
        //        foreach (AcroFields.FieldPosition pos in stamper.AcroFields.GetFieldPositions(fieldName))
        //        {
        //            var item = stamper.AcroFields.GetFieldItem(fieldName);
        //            var dic = item.GetMerged(i >= item.Size ? 0 : i);
        //            TextField t = new TextField(null, null, null);
        //            stamper.AcroFields.DecodeGenericDictionary(dic, t);
        //            i++;
        //            float size = 8;
                    
        //            while (size > 2) {
        //                ColumnText currentColumnText = new ColumnText(stamper.GetOverContent(pos.page));
        //                currentColumnText.SetSimpleColumn(pos.position);
        //                currentColumnText.SetLeading(0, 1);
        //                currentColumnText.Alignment = Element.ALIGN_JUSTIFIED;
        //                var bold = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.BOLD);
        //                var reg = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.NORMAL);
        //                foreach (IInfoText f in values)
        //                {
        //                    Paragraph p = new Paragraph(f.InfoTitle + ":", bold);
        //                    p.Font = reg;
        //                    p.SpacingAfter = size / 2;
        //                    p.SetLeading(0, 1);
        //                    p.Add(f.InfoText.Trim(new char[] { ' ', '\r', '\n', '\t' }));
        //                    currentColumnText.AddElement(p);
        //                }
        //                //currentColumnText.SetSimpleColumn(feats, pos.position.Left, pos.position.Bottom, pos.position.Right, pos.position.Top, 13, Element.ALIGN_LEFT);
        //                int res = currentColumnText.Go(true);
        //                if (res == ColumnText.NO_MORE_COLUMN)
        //                {
        //                    size -= 0.5f;
        //                }
        //                else break;
        //            }
        //            {
        //                ColumnText currentColumnText = new ColumnText(stamper.GetOverContent(pos.page));
        //                currentColumnText.SetSimpleColumn(pos.position);
        //                currentColumnText.SetLeading(0, 1);
        //                currentColumnText.Alignment = Element.ALIGN_JUSTIFIED;
        //                var bold = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.BOLD);
        //                var reg = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.NORMAL);
        //                foreach (IInfoText f in values)
        //                {
        //                    Paragraph p = new Paragraph(f.InfoTitle + ": ", bold);
        //                    p.Font = reg;
        //                    p.SpacingAfter = size / 2;
        //                    p.SetLeading(0, 1);
        //                    p.Add(f.InfoText.Trim(new char[] { ' ', '\r', '\n', '\t' }));
        //                    currentColumnText.AddElement(p);
        //                }
        //                currentColumnText.Go();
        //            }
        //        }
        //    }
        //}

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

        public void SetTextAndDescriptions(string fieldName, string text, IEnumerable<IInfoText> values, string textAfter = null)
        {
            if(preserveEdit) SetField(fieldName, (text != null ? text + "\n" : "" ) + String.Join("\n", values.Select(f => f.ToInfo(true))) + (textAfter != null ? "\n" + textAfter: ""));
            else
            {
                int i = 0;
                foreach (AcroFields.FieldPosition pos in stamper.AcroFields.GetFieldPositions(fieldName))
                {
                    var item = stamper.AcroFields.GetFieldItem(fieldName);
                    var dic = item.GetMerged(i >= item.Size ? 0 : i);
                    TextField t = new TextField(null, null, null);
                    stamper.AcroFields.DecodeGenericDictionary(dic, t);
                    i++;
                    float size = 8;

                    while (size > 2)
                    {
                        ColumnText currentColumnText = new ColumnText(stamper.GetOverContent(pos.page));
                        currentColumnText.SetSimpleColumn(pos.position);
                        currentColumnText.SetLeading(0, 1);
                        currentColumnText.Alignment = Element.ALIGN_JUSTIFIED;
                        var bold = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.BOLD);
                        var reg = FontFactory.GetFont(t.Font.FullFontName[0][3], size, Font.NORMAL);
                        if (text != null) currentColumnText.AddElement(new Paragraph(text, reg) { Leading = 0, MultipliedLeading = 1, SpacingAfter = size / 2});
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
                        ColumnText currentColumnText = new ColumnText(stamper.GetOverContent(pos.page));
                        currentColumnText.SetSimpleColumn(pos.position);
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