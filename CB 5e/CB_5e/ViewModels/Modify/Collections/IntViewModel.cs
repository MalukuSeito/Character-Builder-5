using OGL;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify.Collections
{
    public class IntViewModel : BaseViewModel
    {
        public int Value { get; set; }
        public IntViewModel(int level, int val, string prepend = "Level ", string format = "0")
        {
            Value = val;
            Prepend = prepend;
            Count = level;
            Format = format;
        }
        public void Refresh() => OnPropertyChanged("");
        public string Text { get => Prepend + Count + ": " + Value.ToString(Format); }
        private bool moving = false;
        public bool Moving { get => moving; set => SetProperty(ref moving, value, "", () => OnPropertyChanged("TextColor")); }
        public Color TextColor { get => Moving ? Color.Orange : Color.Default; }
        public Color Accent { get => Color.Accent; }
        public string Prepend { get; private set; }
        public int Count { get; private set; }
        public string Format { get; private set; }
    }
}
