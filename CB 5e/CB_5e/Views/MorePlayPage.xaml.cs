using CB_5e.Services;
using CB_5e.ViewModels;
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
    public partial class MorePlayPage : ContentPage
    {
        public PlayerModel Model { get; private set; }
        public bool Editable { get; set; } = false;
        public bool Log { get; set; } = true;
        public bool Book { get; set; } = true;
        public bool IncludeResources { get; set; } = true;
        public string Exporter { get; set; }
        public List<string> Exporters { get => Model.Context.Config.PDF; }
        public MorePlayPage(PlayerModel model)
        {
            InitializeComponent();
            Model = model;
            Exporter = Model.Context.Config.PDF.FirstOrDefault();
            BindingContext = this;
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IPDFService>().ExportPDF(Exporter, Model.Context, Editable, IncludeResources, Log, Book).Forget();
        }
    }
}