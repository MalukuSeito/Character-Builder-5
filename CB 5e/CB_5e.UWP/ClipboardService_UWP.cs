using CB_5e.UWP;
using Windows.ApplicationModel.DataTransfer;
using CB_5e.Views;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using CB_5e.Services;

[assembly: Dependency(typeof(ClipboardService_UWP))]
namespace CB_5e.UWP
{
    public class ClipboardService_UWP : IClipboardService
    {
        public byte[] GetImageData()
        {
            DataPackageView dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Bitmap))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    dataPackageView.GetBitmapAsync().GetResults().OpenReadAsync().GetResults().GetInputStreamAt(0).AsStreamForRead().CopyTo(ms);
                    return ms.ToArray();
                }
                
            }
            return null;
        }
        public string GetTextData()
        {
            DataPackageView dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                return dataPackageView.GetTextAsync().GetResults();
            }
            return null;
        }

        public void PutTextData(string text, string label = "Text")
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }
    }
}