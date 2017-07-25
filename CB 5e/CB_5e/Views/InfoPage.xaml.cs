using OGL;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views
{
    public interface IHTMLService
    {
        string Convert(IXML obj);
        void Reset(ConfigManager config);
    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        private InfoPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        private HtmlWebViewSource src = new HtmlWebViewSource();
        public HtmlWebViewSource Info { get { return src; } set { src = value; OnPropertyChanged("Info"); } }
        public static InfoPage Show(IXML obj)
        {
            InfoPage Instance = new InfoPage();
            if (obj != null)
            {
                Instance.src.Html = DependencyService.Get<IHTMLService>().Convert(obj);
                if (obj is IOGLElement o) Instance.Title = o.Name;
                else Instance.Title = obj.GetType().Name;
            }
            else
            {
                Instance.src.Html = "";
                Instance.Title = "No Info";
            }
            
            return Instance;
        }
    }
}