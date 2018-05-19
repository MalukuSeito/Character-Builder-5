using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CB_5e.UWP;
using CB_5e.Services;
using System.IO;
using Windows.Storage.Pickers;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(PicturePicker_UWP))]
namespace CB_5e.UWP
{
    public class PicturePicker_UWP : IPhotoService
    {
        public async Task<byte[]> GetImageDataAsync()
        {
            // Create and initialize the FileOpenPicker
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };

            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            // Get a file and return a Stream
            StorageFile storageFile = await openPicker.PickSingleFileAsync();

            if (storageFile == null)
            {
                return null;
            }

            var raStream = await storageFile.OpenReadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                await raStream.AsStreamForRead().CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
