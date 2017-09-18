using OGL.Descriptions;
using OGL.Features;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify.Collections
{
    public class KeywordViewModel : BaseViewModel
    {
        public Keyword Value { get; set; }
        public KeywordViewModel(Keyword te) => Value = te;
        public string Text { get => Value.ToString(); }
        public void Refresh() => OnPropertyChanged("");
        private bool selected = false;
        public bool Selected { get => selected; set => SetProperty(ref selected, value, "", () => OnPropertyChanged("TextColor")); }
        public Color TextColor { get => Selected ? Color.DarkBlue : Color.Default; }
        public Color Accent { get => Color.Accent; }
    }
}
