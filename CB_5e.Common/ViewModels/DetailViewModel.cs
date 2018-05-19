using CB_5e.ViewModels.Character.Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character.Play
{
    public class InPlayDetailViewModel : BaseViewModel
    {
        public PlayerViewModel Model { get; private set; }
        public INavigation Navigation { get; set; }

        public InPlayDetailViewModel(PlayerViewModel model)
        {
            Model = model;
        }

        public virtual void Refresh()
        {
            OnPropertyChanged(null);
        }
    }
}
