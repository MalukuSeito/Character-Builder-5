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
    public class SubRaceChoice : ChoiceViewModel
    {
        public SubRaceChoice(PlayerModel model, List<string> races) : base(model, null, 1, null, model.Context.SubRaceFor(races).OrderBy(r=>r.Name).ToList(), false)
        {
            Name = "Subrace";
            OnSelect = new Command((par) =>
            {
                if (par is ChoiceOption co)
                {
                    Model.MakeHistory();
                    if (co.Selected)
                    {
                        Model.Context.Player.SubRaceName = null;
                    } 
                    else
                    {
                        Model.Context.Player.SubRaceName = co.NameWithSource;
                    }
                    Model.Save();
                    Model.FirePlayerChanged();
                } 
            });
        }

        public override List<string> GetMyTakenChoices()
        {
            return Model.Context.Player.SubRaceName == null ? new List<string>() : new List<string>()
            {
                Model.Context.Player.SubRaceName
            };
        }
        public override List<string> GetAllTakenChoices()
        {
            return Model.Context.Player.SubRaceName == null ? new List<string>() : new List<string>()
            {
                Model.Context.Player.SubRaceName
            };
        }

        public override int Taken => Model.Context.Player.SubRaceName == null ? 0 : 1;

        public override void Refresh(Feature feature)
        {
        }
    }
}
