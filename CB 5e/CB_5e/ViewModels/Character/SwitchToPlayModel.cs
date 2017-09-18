using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Character
{
    public class SwitchToPlayModel : SubModel
    {
        public SwitchToPlayModel(PlayerModel parent) : base(parent, "Switch to Play")
        {
        }
    }
    public class SwitchToBuildModel : SubModel
    {
        public SwitchToBuildModel(PlayerModel parent) : base(parent, "Switch to Build")
        {
        }
    }
}
