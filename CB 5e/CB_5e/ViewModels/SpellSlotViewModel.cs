using Character_Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class SpellSlotViewModel : SpellSlotInfo, INotifyPropertyChanged
    {
        public SpellSlotViewModel(SpellSlotInfo s) : base(s.SpellcastingID, s.Level, s.Slots, s.Used)
        {
        }

        public Command Reduce { get; set; }
        public Command Reset { get; set; }

        public SpellSlotViewModel UpdateUsed()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Used"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text {
            get => ToString();
        }
    }
}
