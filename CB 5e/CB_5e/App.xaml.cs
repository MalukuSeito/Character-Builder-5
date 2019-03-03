using CB_5e.Services;
using CB_5e.Views;
using CB_5e.Views.Character;
using CB_5e.Views.Files;
using CB_5e.Views.Modify;
using Character_Builder;
using OGL;
using PCLStorage;
using PluginDMG;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CB_5e
{
    public partial class App : Application
    {
        public static IFolder Storage = PCLStorage.FileSystem.Current.LocalStorage;
        private static bool? autosave;
        private static bool? hideLost;
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

        public static bool HideLostItems
        {
            get
            {
                if (hideLost.HasValue) return hideLost.Value;
                if (Current.Properties.ContainsKey("HideLostItems"))
                {
                    object o = Current.Properties["HideLostItems"];
                    hideLost = o is bool b && b;
                    return hideLost.Value;
                }
                hideLost = true;
                return true;
            }
            set
            {
                hideLost = value;
                Current.Properties["HideLostItems"] = value;
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
                        Icon = Device.RuntimePlatform == Device.iOS ? "people.png" : null
                    },
                    new NavigationPage(new CharactersBuildPage())
                    {
                        Title = "Build",
                        Icon = Device.RuntimePlatform == Device.iOS ? "pencil.png" : null
                    },
                    new NavigationPage(new Compendium())
                    {
                        Title = "Edit",
                        Icon = Device.RuntimePlatform == Device.iOS ? "services.png" : null
                    },
                    new NavigationPage(new FileBrowser(Storage))
                    {
                        Title = "Files",
                        Icon = Device.RuntimePlatform == Device.iOS ? "open.png" : null
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "Config",
                        Icon = Device.RuntimePlatform == Device.iOS ? "support.png" : null
                    },
                }
            };
        }
    }
}
