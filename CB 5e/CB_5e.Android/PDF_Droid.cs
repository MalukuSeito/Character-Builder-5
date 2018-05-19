
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
using CB_5e.Droid;
using iTextSharp.text;
using Android.Content;

[assembly: Dependency(typeof(PDF_Droid))]
namespace CB_5e.Droid
{
    public class PDF_Droid : PDFBase, IPDFService
    {
        private FileStream fs = null;
        public async Task ExportPDF(string Exporter, BuilderContext context)
        {
            using (fs = System.IO.File.OpenWrite(Path.Combine(Android.App.Application.Context.ExternalCacheDir.Path, context.Player.Name + ".pdf")))
            {
                PDF p = await Load(new FileInfo(Path.Combine(PCLSourceManager.Data.FullName, Exporter))).ConfigureAwait(false);
                await p.Export(context, this).ConfigureAwait(false);
            }
            var uri = Android.Net.Uri.Parse("file://" + Android.App.Application.Context.ExternalCacheDir.Path + "/" + context.Player.Name + ".pdf");
            var intent = new Intent(Intent.ActionSend);
            intent.PutExtra(Intent.ExtraStream, uri);
            intent.SetType("application/pdf");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Select App"));
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
            using (Stream ls = new FileStream(Path.Combine(PCLSourceManager.Data.FullName, file), FileMode.Open))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await ls.CopyToAsync(ms);
                    return new PDFDroidEditor(ms.ToArray(), PreserveEdit);
                }
            }
        }

        public override IPDFSheet CreateSheet()
        {
            return new PDFDroidSheet(PreserveEdit, fs);
        }
    }
}
