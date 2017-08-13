using Android.Content;
using CB_5e.Droid;
using CB_5e.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PicturePicker_Droid))]
namespace CB_5e.Droid
{
    public class PicturePicker_Droid: IPhotoService
    {
        public Task<byte[]> GetImageDataAsync()
        {
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            // Get the MainActivity instance
            MainActivity activity = Forms.Context as MainActivity;

            // Start the picture-picker activity (resumes in MainActivity.cs)
            activity.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            activity.PickImageTaskCompletionSource = new TaskCompletionSource<byte[]>();

            // Return Task object
            return activity.PickImageTaskCompletionSource.Task;
        }
    }
}