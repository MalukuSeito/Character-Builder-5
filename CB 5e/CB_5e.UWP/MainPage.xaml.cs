using System.IO;

namespace CB_5e.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            CB_5e.App.Storage = new DirectoryInfo(Windows.Storage.ApplicationData.Current.RoamingFolder.Path);
            LoadApplication(new CB_5e.App()); 
        }
    }
}