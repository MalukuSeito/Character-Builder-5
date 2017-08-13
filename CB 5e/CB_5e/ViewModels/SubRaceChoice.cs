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
    public class SubRaceChoice : ChoiceViewModel<Feature>
    {
        public SubRaceChoice(PlayerModel model, List<string> races) : base(model, null, 1, null, false)
        {
            Races = races;
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

            UpdateOptions();
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

        public List<string> Races { get; private set; }

        public override void Refresh(Feature feature)
        {
        }

        public void Refresh(List<string> races)
        {
            Races = races;
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return Model.Context.SubRaceFor(Races).OrderBy(r => r.Name).ToList();
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetSubRace(nameWithSource, null);
        }
        protected override void UpdateOptionsImmediately()
        {
        }
    }
}
