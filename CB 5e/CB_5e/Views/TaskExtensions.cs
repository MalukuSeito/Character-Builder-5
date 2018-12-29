using OGL;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace CB_5e.Views
{
    public static class TaskExtensions
    {
        public static void Forget(this Task task)
        {
            task.ContinueWith(
                t => {
                    ConfigManager.LogError(t.Exception);
                },
                TaskContinuationOptions.OnlyOnFaulted);
        }

        public static ImageSource ToSource(this byte[] data) => data != null ? ImageSource.FromStream(() => new MemoryStream(data)) : null;
    }
}