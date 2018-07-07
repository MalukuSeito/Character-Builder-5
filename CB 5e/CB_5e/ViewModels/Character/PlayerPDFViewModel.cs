using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class PlayerPDFViewModel : SubModel
    {
        public PlayerPDFViewModel(PlayerModel parent) : base(parent, "PDF Export")
        {
            Image = ImageSource.FromResource("CB_5e.images.export.png");
        }
    }
}
