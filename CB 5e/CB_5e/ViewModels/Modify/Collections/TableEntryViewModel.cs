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
    public class TableEntryViewModel : BaseViewModel
    {
        public TableEntry Entry { get; private set; }
        public TableEntryViewModel(TableEntry te) => Entry = te;
        public void Refresh() => OnPropertyChanged("");
        public string Text { get => Entry.MinRoll + "-" + Entry.MaxRoll + ":" + (Entry.Title ?? ""); }
        public string Detail { get => Entry.Text; }
        private bool moving = false;
        public bool Moving { get => moving; set => SetProperty(ref moving, value, "", () => OnPropertyChanged("TextColor")); }
        public Color TextColor { get => Moving ? Color.Orange : Color.Default; }
    }
}
