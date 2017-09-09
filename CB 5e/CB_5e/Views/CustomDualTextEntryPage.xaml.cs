using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomDualTextEntryPage : ContentPage
	{
        public ICommand SaveCommand { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public Keyboard Keyboard1 { get; set; } = Keyboard.Default;
        public Keyboard Keyboard2 { get; set; } = Keyboard.Default;
        public CustomDualTextEntryPage(string title, ICommand saveCommand, string title1, string title2, Keyboard keyboard1 = null, Keyboard keyboard2 = null, string initial1 = null, string initial2 = null)
		{
            if (keyboard1 != null) Keyboard1 = keyboard1;
            if (keyboard2 != null) Keyboard2 = keyboard2;
            SaveCommand = saveCommand;
            Title = title;
            Title1 = title1;
            Title2 = title2;
            Text1 = initial1;
            Text2 = initial1;
            BindingContext = this;
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            SaveCommand.Execute(new string[2] { Text1, Text2 });
            await Navigation.PopAsync();
        }
    }
}