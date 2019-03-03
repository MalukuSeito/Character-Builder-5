using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
        }
        public bool AutoSave { get => App.AutoSaveDuringPlay; set => App.AutoSaveDuringPlay = value; }
        public bool HideLost { get => App.HideLostItems; set => App.HideLostItems = value; }
    }
}
