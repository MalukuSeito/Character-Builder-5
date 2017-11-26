using OGL.Descriptions;
using OGL.Features;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify.Collections
{
    public class CantripDamageViewModel : BaseViewModel
    {
        public CantripDamage Entry { get; private set; }
        public CantripDamageViewModel(CantripDamage te) => Entry = te;
        public void Refresh() => OnPropertyChanged("");
        public string Text { get => Entry.Level + ": " + (Entry.Damage ?? ""); }
        private bool moving = false;
        public bool Moving { get => moving; set => SetProperty(ref moving, value, "", () => OnPropertyChanged("TextColor")); }
        public Color TextColor { get => Moving ? Color.Orange : Color.Default; }
    }
}
