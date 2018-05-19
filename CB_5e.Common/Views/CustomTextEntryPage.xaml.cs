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
	public partial class CustomTextEntryPage : ContentPage
	{
        public ICommand SaveCommand { get; set; }
        public string Text { get; set; }
        public Keyboard Keyboard { get; set; } = Keyboard.Default;
        public CustomTextEntryPage (string title, ICommand saveCommand, Keyboard keyboard = null, string initial = null)
		{
            if (keyboard != null) Keyboard = keyboard;
            SaveCommand = saveCommand;
            Title = title;
            Text = initial;
            BindingContext = this;
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            SaveCommand.Execute(Text);
            await Navigation.PopAsync();
        }
    }
}