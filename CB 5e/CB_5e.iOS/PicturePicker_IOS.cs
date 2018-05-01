using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CB_5e.Services;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using CB_5e.iOS;

[assembly: Dependency(typeof(PicturePicker_IOS))]
namespace CB_5e.iOS
{
    public class PicturePicker_IOS: IPhotoService
    {
        TaskCompletionSource<Stream> taskCompletionSource;
        UIImagePickerController imagePicker;

        public Task<Stream> GetImageStreamAsync()
        {
            // Create and define UIImagePickerController
            imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
            };

            // Set event handlers
            imagePicker.FinishedPickingMedia += OnImagePickerFinishedPickingMedia;
            imagePicker.Canceled += OnImagePickerCancelled;

            // Present UIImagePickerController;
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            //var viewController = window.RootViewController;
            var viewController = TopViewController(UIApplication.SharedApplication.KeyWindow.RootViewController);
            viewController.PresentModalViewController(imagePicker, true);

            // Return Task object
            taskCompletionSource = new TaskCompletionSource<Stream>();
            return taskCompletionSource.Task;
        }

        void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs args)
        {
            UIImage image = args.EditedImage ?? args.OriginalImage;

            if (image != null)
            {
                // Convert UIImage to .NET Stream object
                NSData data = image.AsJPEG(1);
                Stream stream = data.AsStream();

                // Set the Stream as the completion of the Task
                taskCompletionSource.SetResult(stream);
            }
            else
            {
                taskCompletionSource.SetResult(null);
            }
            imagePicker.DismissModalViewController(true);
        }

        void OnImagePickerCancelled(object sender, EventArgs args)
        {
            taskCompletionSource.SetResult(null);
            imagePicker.DismissModalViewController(true);
        }

        public async Task<byte[]> GetImageDataAsync()
        {
            using (MemoryStream ms = new MemoryStream()) {
                Stream s = await GetImageStreamAsync();
                if (s != null)
                {
                    await s.CopyToAsync(ms);
                    return ms.ToArray();
                }
                return null;
            }
        }

        public UIViewController TopViewController(UIViewController b) {
            if (b is UINavigationController nav) {
                return TopViewController(nav.VisibleViewController);
            }
            if (b is UITabBarController tab) {
                if (tab.SelectedViewController is UIViewController selected) {
                    return TopViewController(selected);
                }
            }
            if (b.PresentedViewController is UIViewController presented) {
                return TopViewController(presented);
            }
            return b;
        }
    }
}