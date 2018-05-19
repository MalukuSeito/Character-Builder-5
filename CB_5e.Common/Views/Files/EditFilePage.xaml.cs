using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Files
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditFilePage : ContentPage
    {
        public EditFilePage(FileInfo file)
        {
            //HTML.Html = "<html><body><pre>" + WebUtility.HtmlEncode(text) + "</pre></body></html>";
            HTML.Url = "file://" + file.FullName;
            Title = file.Name;
            InitializeComponent();
            BindingContext = this;
        }

        //public HtmlWebViewSource Content { get; set; } = new HtmlWebViewSource();
        public UrlWebViewSource HTML { get; set; } = new UrlWebViewSource();

    }
}