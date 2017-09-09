using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class FeatureViewModel: BaseViewModel
    {
        public Feature Feature { get; private set; }
        public FeatureViewModel(Feature f) => Feature = f;
        public void Refresh() => OnPropertyChanged("");
        public string Text { get => Feature.Name + (Feature.Level > 0 ? " (Level " + Feature.Level + ")" : ""); }
        public string Detail { get => Feature.Displayname(); }
        private bool moving = false;
        public bool Moving { get => moving; set => SetProperty(ref moving, value, "", () => OnPropertyChanged("TextColor")); }
        public Color TextColor { get => Moving ? Color.Orange : Color.Default; }
    }
}
