using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using CB_5e.Droid;
using CB_5e.Views;
using OGL;
using System.IO;
using CB_5e.Services;

[assembly: Dependency(typeof(DocumentViewer_Droid))]
namespace CB_5e.Droid
{
    public class DocumentViewer_Droid : IDocumentViewer
    {
        public void ShowDocumentFile(string name, Stream file, string mimeType)
        {
            try
            {
                using (FileStream f = File.OpenWrite(Path.Combine(Android.App.Application.Context.ExternalCacheDir.Path, name)))
                {
                    file.CopyTo(f);
                }
                var uri = Android.Net.Uri.Parse("file://" + Android.App.Application.Context.ExternalCacheDir.Path + "/" + name);
                var intent = new Intent(Intent.ActionSend);
                intent.PutExtra(Intent.ExtraStream, uri);
                intent.SetType(mimeType);
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                Forms.Context.StartActivity(Intent.CreateChooser(intent, "Select App"));
            }
            catch (Exception ex)
            {
                ConfigManager.LogError(ex);
            }
        }
    }
    
}