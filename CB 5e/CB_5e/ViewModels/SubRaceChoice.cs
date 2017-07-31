using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class RaceChoice : ChoiceViewModel
    {
        public RaceChoice(PlayerModel model) : base(model, null, 1, null, model.Context.Races.Values.OrderBy(r=>r.Name).ToList(), false)
        {
            Name = "Select a Race (Tap to choose, " + Device.OnPlatform("swipe", "hold", "hold") + " for info)";
            OnSelect = new Command((par) =>
            {
                if (par is ChoiceOption co)
                {
                    Model.MakeHistory();
                    if (co.Selected)
                    {
                        Model.Context.Player.RaceName = null;
                    } 
                    else
                    {
                        Model.Context.Player.RaceName = co.NameWithSource;
                    }
                    Model.Save();
                    Model.FirePlayerChanged();
                } 
            });
        }

        public override List<string> GetMyTakenChoices()
        {
            return Model.Context.Player.RaceName == null ? new List<string>() : new List<string>()
            {
                Model.Context.Player.RaceName
            };
        }
        public override List<string> GetAllTakenChoices()
        {
            return Model.Context.Player.RaceName == null ? new List<string>() : new List<string>()
            {
                Model.Context.Player.RaceName
            };
        }

        public override int Taken => Model.Context.Player.RaceName == null ? 0 : 1;

        public override void Refresh(Feature feature)
        {
        }
    }
}
