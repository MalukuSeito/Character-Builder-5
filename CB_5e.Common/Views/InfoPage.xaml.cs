using CB_5e.Services;
using Character_Builder;
using OGL;
using OGL.Common;
using OGL.Features;
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
    public partial class InfoPage : ContentPage
    {
        private InfoPage(IXML obj)
        {
            InitializeComponent();
            BindingContext = this;
            Obj = obj;
            if (Obj is IOGLElement o) Title = o.Name;
            else if (Obj is Feature f) Title = f.Name;
            else if (Obj is DisplayPossession dp) Title = dp.Name;
            else Title = "No Info";
        }
        private HtmlWebViewSource src = new HtmlWebViewSource();
        public HtmlWebViewSource Info { get { return src; } set { src = value; OnPropertyChanged("Info"); } }

        public IXML Obj { get; private set; }

        public static InfoPage Show(IXML obj, Command cmd = null, string cmdText = "Do")
        {
            InfoPage Instance = new InfoPage(obj);
            if (cmd != null)
            {
                Instance.ToolbarItems.Add(new ToolbarItem()
                {
                    Command = cmd,
                    CommandParameter = obj,
                    Text = cmdText
                });
            }
            return Instance;
        }

        protected override void OnAppearing()
        {
            Task.Run(() =>
            {
                if (Obj != null)
                {
                    string loaded = DependencyService.Get<IHTMLService>().Convert(Obj);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        src.Html = loaded;
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        src.Html = "";
                    });
                }
            }).Forget();
        }
    }
}