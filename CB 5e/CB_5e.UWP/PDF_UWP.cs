
using CB_5e.Services;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;
using Character_Builder;
using System;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using OGL.Keywords;
using System.Text;
using OGL.Features;
using System.Linq;
using OGL.Descriptions;
using OGL;
using System.IO;
using PCLStorage;
using OGL.Spells;
using OGL.Base;
using OGL.Items;
using CB_5e.UWP;
using iTextSharp.text;
using Windows.Storage.Pickers;
using Windows.Storage;

[assembly: Dependency(typeof(PDF_UWP))]
namespace CB_5e.UWP
{
    public class PDF_UWP : PDFBase, IPDFService
    {
        private Stream fs = null;
        public async Task ExportPDF(string Exporter, BuilderContext context)
        {
            FileSavePicker fileSave = new FileSavePicker()
            {
                SuggestedFileName = context.Player.Name + ".pdf"
            };
            fileSave.FileTypeChoices.Add("PDF", new List<string>() { ".pdf" });
  
            StorageFile file = await fileSave.PickSaveFileAsync();
            if (file != null) {
                fs = await file.OpenStreamForWriteAsync();
                PDF p = await Load(new FileInfo(Path.Combine(PCLSourceManager.Data.Path, Exporter))).ConfigureAwait(false);
                await p.Export(context, this).ConfigureAwait(false);
                fs.Close();
            }
        }
        public async static Task<PDF> Load(FileInfo file)
        {
            using (Stream s = new FileStream(file.FullName, FileMode.Open))
            {
                PDF p = await Task.Run(() => (PDF)PDF.Serializer.Deserialize(s)).ConfigureAwait(false);
                p.File = PCLImport.MakeRelativeFile(p.File);
                p.SpellFile = PCLImport.MakeRelativeFile(p.SpellFile);
                p.LogFile = PCLImport.MakeRelativeFile(p.LogFile);
                p.SpellbookFile = PCLImport.MakeRelativeFile(p.SpellbookFile);
                p.ActionsFile = PCLImport.MakeRelativeFile(p.ActionsFile);
                p.ActionsFile2 = PCLImport.MakeRelativeFile(p.ActionsFile2);
                return p;
            }
        }

        public override async Task<IPDFEditor> CreateEditor(string file)
        {
            using (Stream ls = new FileStream(Path.Combine(PCLSourceManager.Data.Path, file), FileMode.Open))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await ls.CopyToAsync(ms);
                    return new PDFUWPEditor(ms.ToArray(), PreserveEdit);
                }
            }
        }

        public override IPDFSheet CreateSheet()
        {
            return new PDFUWPSheet(PreserveEdit, fs);
        }
    }
}
