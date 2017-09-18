using OGL;
using System.Threading.Tasks;

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
    }
}