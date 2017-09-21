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
	public partial class CustomTextEntrySuggestionsPage : ContentPage
	{
        public ICommand SaveCommand { get; set; }
        private string text;
        public string Text { get => text; set { text = value; OnPropertyChanged("Text"); } }
        public Keyboard Keyboard { get; set; } = Keyboard.Default;
        public IList<string> Suggestions { get; set; }
        public CustomTextEntrySuggestionsPage(string title, ICommand saveCommand, IList<String> suggestions)
		{
            SaveCommand = saveCommand;
            Title = title;
            BindingContext = this;
            Suggestions = suggestions;
            InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            SaveCommand.Execute(Text);
            await Navigation.PopAsync();
        }
    }
}