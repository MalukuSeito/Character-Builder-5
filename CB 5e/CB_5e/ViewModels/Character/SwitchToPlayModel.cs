using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class SwitchToPlayModel : SubModel
    {
        public SwitchToPlayModel(PlayerModel parent) : base(parent, "Switch to Play")
        {
            Image = ImageSource.FromResource("CB_5e.images.play.png");
        }
    }
    public class SwitchToBuildModel : SubModel
    {
        public SwitchToBuildModel(PlayerModel parent) : base(parent, "Switch to Build")
        {
            Image = ImageSource.FromResource("CB_5e.images.build.png");
        }
    }
}
