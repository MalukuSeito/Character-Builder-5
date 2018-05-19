using CB_5e.Services;
using CB_5e.Views;
using CB_5e.Views.Character;
using CB_5e.Views.Files;
using CB_5e.Views.Modify;
using Character_Builder;
using OGL;
using PluginDMG;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CB_5e
{
    public partial class App : Application
    {
        public static DirectoryInfo Storage = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        private static bool? autosave;
        public static bool AutoSaveDuringPlay
        {
            get
            {
                if (autosave.HasValue) return autosave.Value;
                if (Current.Properties.ContainsKey("AutoSaveDuringPlay"))
                {
                    object o = Current.Properties["AutoSaveDuringPlay"];
                    autosave = o is bool b && b;
                    return autosave.Value;
                }
                autosave = true;
                return true;
            }
            set
            {
                autosave = value;
                Current.Properties["AutoSaveDuringPlay"] = value;
            }
        }
        public static TabbedPage MainTab { get; private set; }
        public App()
        {
            InitializeComponent();
            ConfigManager.LogEvents += DisplayLog;
            SetMainPage();
        }

        private void DisplayLog(object sender, string message, System.Exception e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Current.MainPage.DisplayAlert(message, e?.Message, "OK");
            });
        }

        public static void SetMainPage()
        {
            Current.MainPage = MainTab = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new CharactersPlayPage())
                    {
                        Title = "Play",
                        Icon = Device.OnPlatform("tab_feed.png",null,null)
                    },
                    new NavigationPage(new CharactersBuildPage())
                    {
                        Title = "Build",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                    new NavigationPage(new Compendium())
                    {
                        Title = "Edit",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                    new NavigationPage(new FileBrowser(Storage))
                    {
                        Title = "Files",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "Config",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                }
            };
        }
    }
}
