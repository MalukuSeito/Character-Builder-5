using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CB_5e.iOS;
using Xamarin.Forms;
using CB_5e.Services;

[assembly: Dependency(typeof(ClipboardService_IOS))]
namespace CB_5e.iOS
{
    public class ClipboardService_IOS : IClipboardService
    {
        public byte[] GetImageData()
        {
            if (UIPasteboard.General.HasImages)
            {
                using (NSData imageData = UIPasteboard.General.Image.AsPNG())
                {
                    Byte[] myByteArray = new Byte[imageData.Length];
                    System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                    return myByteArray;
                }
            }
            return null;
        }

        public string GetTextData()
        {
            if (UIPasteboard.General.HasStrings)
            {
                return UIPasteboard.General.String;
            }
            return null;
        }

        public void PutTextData(string text, string label = "Text")
        {
            UIPasteboard.General.String = text;
        }
    }
}