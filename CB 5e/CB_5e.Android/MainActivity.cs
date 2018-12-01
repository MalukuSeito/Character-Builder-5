using Android.Content;
using Android.App;
using Android.Content.PM;
using Android.OS;
using CB_5e.Services;
using System;
using System.Threading.Tasks;
using PCLStorage;
using CB_5e.Views;
using System.IO;
using Android.Runtime;
using CB_5e.Views.Files;

namespace CB_5e.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionSend, Intent.ActionSendMultiple }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"*/*")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public void CopyDirectory(Java.IO.File sourceLocation, Java.IO.File targetLocation)
        {

            if (sourceLocation.IsDirectory)
            {
                if (!targetLocation.Exists() && !targetLocation.Mkdirs()) throw new IOException("Cannot create dir " + targetLocation.AbsolutePath);

                String[] children = sourceLocation.List();
                for (int i = 0; i < children.Length; i++) CopyDirectory(new Java.IO.File(sourceLocation, children[i]), new Java.IO.File(targetLocation, children[i]));
            }
            else
            {
                // make sure the directory we plan to store the recording in exists
                Java.IO.File directory = targetLocation.ParentFile;
                if (directory != null && !directory.Exists() && !directory.Mkdirs())
                {
                    throw new IOException("Cannot create dir " + directory.AbsolutePath);
                }
                using (FileStream inn = new FileStream(sourceLocation.AbsolutePath, FileMode.Open))
                {
                    using (FileStream outn = new FileStream(targetLocation.AbsolutePath, FileMode.OpenOrCreate))
                    {
                        inn.CopyTo(outn);
                    }
                }
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //Task.Run(async () => {
            //    try
            //    {
            //        await PCLSourceManager.InitAsync();
            //    }
            //    catch (System.OperationCanceledException ex)
            //    {
            //        Console.WriteLine($"Text load cancelled: {ex.Message}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //});
            global::Xamarin.Forms.Forms.Init(this, bundle);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironmentOnUnhandledException;
            Java.IO.File ext = Application.Context.GetExternalFilesDir(null);

            //Java.IO.File ext = Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
            if (Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.GetExternalStorageState(ext)))
            {
                ext.SetExecutable(true);
                ext.SetReadable(true);
                ext.SetWritable(true);
                Java.IO.File data = new Java.IO.File(App.Storage.Path, "Data");
                Java.IO.File chars = new Java.IO.File(App.Storage.Path, "Characters");
                Java.IO.File ndata = new Java.IO.File(ext, "Data");
                Java.IO.File nchars = new Java.IO.File(ext, "Characters");
                if (!ndata.Exists() && data.Exists())
                    try
                    {
                        CopyDirectory(data, ndata);
                    }
                    catch (Exception) { }
                if (!nchars.Exists() && chars.Exists())
                    try
                    {
                        CopyDirectory(chars, nchars);
                    }
                    catch (Exception) { }
                App.Storage = FileSystem.Current.GetFolderFromPathAsync(ext.AbsolutePath).Result;

            }
            App a = new App();
            LoadApplication(a);
            if (Intent.Action == Intent.ActionSend || Intent.Action == Intent.ActionSendMultiple)
            {
                Task.Run(async () =>
                {
                    IFolder incoming = await App.Storage.CreateFolderAsync("Incoming", CreationCollisionOption.OpenIfExists);
                    // Get the info from ClipData 
                    for (int i = 0; i < Intent.ClipData.ItemCount; i++)
                    {
                        var pdf = Intent.ClipData.GetItemAt(i);

                        // Open a stream from the URI 
                        var pdfStream = ContentResolver.OpenInputStream(pdf.Uri);

                        // Save it over 
                        //var memOfPdf = new System.IO.MemoryStream();
                        //pdfStream.CopyTo(memOfPdf);
                        string file = System.IO.Path.GetFileName(pdf.Uri.Path);
                        if (file == null || file == "") file = "Incoming.file";
                        IFile f = await incoming.CreateFileAsync(file, CreationCollisionOption.GenerateUniqueName);
                        using (Stream s = await f.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                        {
                            await pdfStream.CopyToAsync(s);
                            pdfStream.Close();
                        }
                    }
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        App.MainTab.SelectedItem = App.MainTab.Children[3];
                        await App.MainTab.Children[3].Navigation.PopToRootAsync();
                        await App.MainTab.Children[3].Navigation.PushAsync(new FileBrowser(incoming));
                    });
                }).Forget();
            }
        }

        public TaskCompletionSource<byte[]> PickImageTaskCompletionSource { set; get; }
        public static readonly int PickImageId = 1000;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        PickImageTaskCompletionSource.SetResult(ms.ToArray());
                    }
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (sender != null) return;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (sender != null) return;
        }

        private void AndroidEnvironmentOnUnhandledException(object sender, RaiseThrowableEventArgs e)
        {
            if (sender != null) return;
        }
    }
}