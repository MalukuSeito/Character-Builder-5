using CB_5e.Services;
using CB_5e.ViewModels;
using CB_5e.ViewModels.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Character
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MorePlayPage : ContentPage
    {
        public PlayerPDFViewModel Model { get; private set; }
        public bool Editable { get; set; } = false;
        public bool Log { get; set; } = true;
        public bool Book { get; set; } = true;
        public bool Actions { get; set; } = true;
        public bool Monsters { get; set; } = true;
        public bool IncludeResources { get; set; } = true;
        public string Exporter { get; set; }
        public List<string> Exporters { get => Model.Context.Config.PDF; }
        public MorePlayPage(PlayerPDFViewModel model)
        {
            InitializeComponent();
            Model = model;
            Exporter = Model.Context.Config.PDF.FirstOrDefault();
            BindingContext = Model;
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            IPDFService s = DependencyService.Get<IPDFService>();
            Model.Set(s);
            s.ExportPDF(Model.Exporter, Model.Context).Forget();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Model.Load().ConfigureAwait(false);
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Model.SaveCustom();
        }
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await Model.SaveConfig();
        }
        protected override bool OnBackButtonPressed()
        {
            //Model.SaveConfig().Wait();
            return base.OnBackButtonPressed();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}