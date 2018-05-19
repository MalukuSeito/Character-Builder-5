using CB_5e.Helpers;
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

    public class InfoObj
    {

        public InfoObj(IOGLElement obj, Command showInfo)
        {
            Value = obj;
            ShowInfo = showInfo;
        }

        public IOGLElement Value { get; set; }
        public String Name { get { return Value.Name; } }
        public String Desc { get { return Value.GetType().Name.Replace("Definition", "").Replace("Sub", "Sub "); } }
        public Command ShowInfo { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoSelectPage : ContentPage
    {
        public InfoSelectPage()
        {
            BindingContext = this;
            InitializeComponent();
            ShowInfo = new Command(async (par) => {
                if (par is IXML o)
                {
                    await Navigation.PushAsync(InfoPage.Show(o));
                }
            });
        }

        public ObservableRangeCollection<InfoObj> Values { get; private set; } = new ObservableRangeCollection<InfoObj>();

        public Command ShowInfo { get; private set; }

        public static ContentPage Show(params IOGLElement[] vals)
        {
            return Show(vals.AsEnumerable());
        }

        public static ContentPage Show(IEnumerable<IOGLElement> vals) 
        {
            
            List<IOGLElement> values = (from v in vals where v != null select v).ToList();
            if (values.Count == 0) return InfoPage.Show(null);
            else if (values.Count == 1) return InfoPage.Show(values[0]);
            InfoSelectPage Instance = new InfoSelectPage();
            Instance.Values.ReplaceRange(from s in values select new InfoObj(s, Instance.ShowInfo));
            return Instance;
        }
    }
}