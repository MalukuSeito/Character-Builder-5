using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPage : ContentPage
    {
        public SelectPage(IEnumerable<SelectOption> options, Command command)
        {
            InitializeComponent();
            Command = command;
            Options = options;
            BindingContext = this;
        }

        public Command Command { get; private set; }
        public IEnumerable<SelectOption> Options { get; private set; }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PopAsync(false);
            Command.Execute(e.SelectedItem);
        }
    }
}