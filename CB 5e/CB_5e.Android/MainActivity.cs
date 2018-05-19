﻿using Android.Content;
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
            App a = new App();
            LoadApplication(a);

            if (Intent.Action == Intent.ActionSend || Intent.Action == Intent.ActionSendMultiple)
            {
                Task.Run(async () =>
                {
                    DirectoryInfo incoming = new DirectoryInfo(Path.Combine(App.Storage.FullName, "Incoming"));
                    if (!incoming.Exists) incoming.Create();
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
                        FileInfo f = new FileInfo(Path.Combine(incoming.FullName, file));
                        int j = 0;
                        while (f.Exists) f = new FileInfo(Path.Combine(incoming.FullName, file + "(" + ++j + ")"));
                        using (Stream s = new FileStream(f.FullName, FileMode.Create))
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