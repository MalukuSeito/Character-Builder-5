
using Foundation;
using PCLStorage;
using System.Threading.Tasks;
using UIKit;
using CB_5e.Views;
using System.IO;
using CB_5e.Views.Files;
using Character_Builder;
using PluginDMG;
using System.Collections.Generic;
using CB_5e.ViewModels.Character;
using CB_5e.Views.Character;
using CB_5e.ViewModels.Character.Build;
using Xamarin.Forms;
using System;
using OGL;

namespace CB_5e.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
            App.Storage = FileSystem.Current.GetFolderFromPathAsync(Path.Combine(Path.GetDirectoryName(FileSystem.Current.LocalStorage.Path), "Documents")).Result;
			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            Task.Run(async () =>
            {
                if ("Inbox".Equals(Path.GetDirectoryName(url.Path)))
                {
                    IFolder incoming = await App.Storage.CreateFolderAsync("Inbox", CreationCollisionOption.OpenIfExists);
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        App.MainTab.SelectedItem = App.MainTab.Children[3];
                        await App.MainTab.Children[3].Navigation.PopToRootAsync();
                        await App.MainTab.Children[3].Navigation.PushAsync(new FileBrowser(incoming));
                    });
                }
                else if (url.Path?.EndsWith(".cb5") ?? false)
                {
                    try {
                        using (Stream f = File.OpenRead(url.Path)) {
                            Player player = Player.Serializer.Deserialize(f) as Player;
                            BuilderContext Context = new BuilderContext(player);
                            PluginManager manager = new PluginManager();
                            manager.Add(new SpellPoints());
                            manager.Add(new SingleLanguage());
                            manager.Add(new CustomBackground());
                            manager.Add(new NoFreeEquipment());
                            Context.Plugins = manager;


                            Context.UndoBuffer = new LinkedList<Player>();
                            Context.RedoBuffer = new LinkedList<Player>();
                            Context.UnsavedChanges = 0;

                            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                            {
                                LoadingProgress loader = new LoadingProgress(Context);
                                LoadingPage l = new LoadingPage(loader, false);
                                App.MainTab.SelectedItem = App.MainTab.Children[2];
                                await App.MainTab.Children[2].Navigation.PushModalAsync(l);
                                var t = l.Cancel.Token;
                                try
                                {
                                    await loader.Load(t).ConfigureAwait(false);
                                    t.ThrowIfCancellationRequested();
                                    PlayerBuildModel model = new PlayerBuildModel(Context);
                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await App.MainTab.Children[2].Navigation.PopModalAsync(false);
                                        await App.MainTab.Children[2].Navigation.PushModalAsync(new NavigationPage(new FlowPage(model)));
                                    });
                                } catch (Exception e) {
                                    ConfigManager.LogError(e);
                                }
                            });
                        }
                    } 
                    catch (Exception e)
                    {
                        ConfigManager.LogError(e);
                    }
                } else {
                    
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        App.MainTab.SelectedItem = App.MainTab.Children[3];
                        await App.MainTab.Children[3].Navigation.PopToRootAsync();
                        if (url.Path.StartsWith(App.Storage.Path, System.StringComparison.Ordinal)) {
                            await App.MainTab.Children[3].Navigation.PushAsync(new FileBrowser(await FileSystem.Current.GetFolderFromPathAsync(Path.GetDirectoryName(url.Path))));
                        }

                    });
                }
            }).Forget();
            return true;
        }
    }
}
