
using Foundation;
using PCLStorage;
using System.Threading.Tasks;
using UIKit;
using CB_5e.Views;
using System.IO;
using CB_5e.Views.Files;

namespace CB_5e.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
            App.Storage = PCLStorage.FileSystem.Current.LocalStorage.CreateFolderAsync("CB5", PCLStorage.CreationCollisionOption.OpenIfExists).Result;
			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            Task.Run(async () =>
            {
                IFolder incoming = await App.Storage.CreateFolderAsync("Incoming", CreationCollisionOption.OpenIfExists);
                IFile target = await incoming.CreateFileAsync(url.LastPathComponent, CreationCollisionOption.GenerateUniqueName);
                using (Stream f = File.OpenRead(url.Path))
                {
                    using (Stream o = await target.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                    {
                        await f.CopyToAsync(o);
                    }
                }
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    App.MainTab.SelectedItem = App.MainTab.Children[3];
                    await App.MainTab.Children[3].Navigation.PopToRootAsync();
                    await App.MainTab.Children[3].Navigation.PushAsync(new FileBrowser(incoming));
                });
            }).Forget();
            return true;
        }
    }
}
