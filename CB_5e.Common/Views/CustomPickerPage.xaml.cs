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
	public partial class CustomPickerPage : ContentPage
	{
        public ICommand SaveCommand { get; set; }
        public string Text { get; set; }
        public List<string> Items { get; set; }
        public CustomPickerPage(string title, ICommand saveCommand, List<string> items, string initial = null)
		{
            Items = items;
            SaveCommand = saveCommand;
            Title = title;
            Text = initial;
            BindingContext = this;
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (Text != null)
            {
                SaveCommand.Execute(Text);
                await Navigation.PopAsync();
            }
        }
    }
}