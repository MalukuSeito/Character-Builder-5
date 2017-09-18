using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify.Descriptions
{
    public class DescriptionViewModel : BaseViewModel
    {
        public Description Description { get; private set; }
        public DescriptionViewModel(Description d) => Description = d;
        public void Refresh() => OnPropertyChanged("");
        public string Text { get => Description.Name; }
        public string Detail { get => Description.GetType().Name; }
        private bool moving = false;
        public bool Moving { get => moving; set => SetProperty(ref moving, value, "", () => OnPropertyChanged("TextColor")); }
        public Color TextColor { get => Moving ? Color.Orange : Color.Default; }
    }
}
