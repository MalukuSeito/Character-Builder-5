using CB_5e.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPage : ContentPage
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;
        public SelectPage(IEnumerable<SelectOption> options, Command command)
        {
            InitializeComponent();
            Command = command;
            BaseOptions = options;
            UpdateEntries();
            BindingContext = this;
        }

        public Command Command { get; private set; }
        public IEnumerable<SelectOption> BaseOptions { get; private set; }
        public ObservableRangeCollection<SelectOption> Options { get; set; } = new ObservableRangeCollection<SelectOption>();

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PopAsync(false);
            Command.Execute(e.SelectedItem);
        }

        private string search;

        public string Search
        {
            get => search;
            set
            {
                search = value;
                OnPropertyChanged("Search");
                if (!IsBusy) UpdateEntries();
            }
        }


        public void UpdateEntries()
        {
            Options.ReplaceRange(new List<SelectOption>());
            Options.ReplaceRange(from r in BaseOptions
                                 where search == null
                                 || search == ""
                                 || Culture.CompareInfo.IndexOf(r.Text ?? "", search, CompareOptions.IgnoreCase) >= 0
                                 || Culture.CompareInfo.IndexOf(r.Detail ?? "", search, CompareOptions.IgnoreCase) >= 0
                                 select r);
        }
    }
}