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
    public class HitDieViewModel : HitDie, INotifyPropertyChanged
    {
        public HitDieViewModel(PlayerModel pvm, HitDie hd) : base(hd.Dice, hd.Count, hd.Used)
        {
            Reduce = new Command(() =>
            {
                if (Used < Count)
                {
                    pvm.MakeHistory("");
                    pvm.Context.Player.UseHitDie(Dice);
                    Used++;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Current"));
                    pvm.Save();
                }
            });
        }

        public string Current
        {
            get
            {
                return ToString() + " (" + Count + " Total)";
            }
        }
        public string TotalText
        {
            get
            {
                return Total() + " Total";
            }
        }
        public Command Reduce { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
