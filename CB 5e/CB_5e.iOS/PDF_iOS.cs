
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
using iTextSharp.text;
using CB_5e.iOS;
using QuickLook;
using UIKit;

[assembly: Dependency(typeof(PDF_iOS))]
namespace CB_5e.iOS
{
    public class PDF_iOS : PDFBase, IPDFService
    {
        private FileStream fs = null;
        public async Task ExportPDF(string Exporter, BuilderContext context)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var tmp = Path.Combine(documents, "..", "tmp");
            using (fs = System.IO.File.OpenWrite(Path.Combine(tmp, context.Player.Name + ".pdf")))
            {
                PDF p = await Load(await PCLSourceManager.Data.GetFileAsync(Exporter).ConfigureAwait(false)).ConfigureAwait(false);
                await p.Export(context, this).ConfigureAwait(false);
            }
            var fileinfo = new FileInfo(Path.Combine(tmp, context.Player.Name + ".pdf"));
            Device.BeginInvokeOnMainThread(() =>
            {
                var previewController = new QLPreviewController
                {
                    DataSource = new PreviewControllerDataSource(fileinfo.FullName, fileinfo.Name)
                };

                UINavigationController controller = FindNavigationController();

                if (controller != null)
                {
                    controller.PresentViewController((UIViewController)previewController, true, (Action)null);
                }
            });
        }

        private UINavigationController FindNavigationController()
        {
            foreach (var window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController.NavigationController != null)
                {
                    return window.RootViewController.NavigationController;
                }
                else
                {
                    UINavigationController value = CheckSubs(window.RootViewController.ChildViewControllers);
                    if (value != null)
                    {
                        return value;
                    }
                }
            }
            return null;
        }

        private UINavigationController CheckSubs(UIViewController[] controllers)
        {
            foreach (var controller in controllers)
            {
                if (controller.NavigationController != null)
                {
                    return controller.NavigationController;
                }
                else
                {
                    UINavigationController value = CheckSubs(controller.ChildViewControllers);
                    if (value != null)
                    {
                        return value;
                    }
                }
            }
            return null;
        }

        public async static Task<PDF> Load(IFile file)
        {
            using (Stream s = await file.OpenAsync(PCLStorage.FileAccess.Read))
            {
                PDF p = (PDF)PDF.Serializer.Deserialize(s);
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
            IFile f = await PCLSourceManager.Data.GetFileAsync(file).ConfigureAwait(false);
            using (Stream ls = await f.OpenAsync(PCLStorage.FileAccess.Read).ConfigureAwait(false))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await ls.CopyToAsync(ms);
                    return new PDFiOSEditor(ms.ToArray(), PreserveEdit);
                }
            }
        }

        public override IPDFSheet CreateSheet()
        {
            return new PDFiOSSheet(PreserveEdit, fs);
        }
    }
}
