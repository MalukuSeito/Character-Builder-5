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
    public class NamesViewModel : BaseViewModel
    {
        public Names Entry { get; private set; }
        public NamesViewModel(Names te) => Entry = te;
        public void Refresh() => OnPropertyChanged("");
        public string Text { get => Entry.Title; }
        public string Detail { get => String.Join(", ", Entry.ListOfNames); }
        private bool moving = false;
        public bool Moving { get => moving; set => SetProperty(ref moving, value, "", () => OnPropertyChanged("TextColor")); }
        public Color TextColor { get => Moving ? Color.Orange : Color.Default; }

        public string Save()
        {
            return new ListDescription("", "", new List<Names>() { Entry }).Save();
        }
    }
}
