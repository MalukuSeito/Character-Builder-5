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
		public CustomTextEntryPage (string title, ICommand saveCommand)
		{
            SaveCommand = saveCommand;
            Title = title;
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