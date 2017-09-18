using Android.Content;
using CB_5e.Droid;
using CB_5e.Views;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;
using Android.Database;
using CB_5e.Services;

[assembly: Dependency(typeof(ClipboardService_Droid))]
namespace CB_5e.Droid
{
    public class ClipboardService_Droid : IClipboardService
    {
        public byte[] GetImageData()
        {
            ClipboardManager clipboard = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
            ContentResolver cr = Forms.Context.ContentResolver;
            ClipData clip = clipboard.PrimaryClip;
            byte[] p = null;
            if (clip != null)
            {

                // Gets the first item from the clipboard data
                ClipData.Item item = clip.GetItemAt(0);

                // Tries to get the item's contents as a URI
                Android.Net.Uri pasteUri = item.Uri;
                // If the clipboard contains a URI reference
                if (pasteUri != null)
                {

                    // Is this a content URI?
                    String uriMimeType = cr.GetType(pasteUri);

                    // If the return value is not null, the Uri is a content Uri
                    if (uriMimeType != null)
                    {

                        // Does the content provider offer a MIME type that the current application can use?
                        if (uriMimeType.StartsWith("image/"))
                        {

                            // Get the data from the content provider.
                            ICursor pasteCursor = cr.Query(pasteUri, null, null, null, null);

                            // If the Cursor contains data, move to the first record
                            if (pasteCursor != null)
                            {
                                if (pasteCursor.MoveToFirst())
                                {
                                    p = pasteCursor.GetBlob(0);
                                }
                            }

                            // close the Cursor
                            pasteCursor.Close();
                        }
                    }
                }
            }
            return p;
        }
        public string GetTextData()
        {
            ClipboardManager clipboard = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
            return clipboard.Text;
        }

        public void PutTextData(string text, string label = "Text")
        {
            ClipboardManager clipboard = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
            clipboard.PrimaryClip = ClipData.NewPlainText(label, text);
        }
    }
}