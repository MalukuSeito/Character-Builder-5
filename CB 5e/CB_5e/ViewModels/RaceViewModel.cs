using CB_5e.Helpers;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels
{
    public class RaceViewModel : SubModel
    {
        public RaceViewModel(PlayerModel parent) : base(parent, "Race")
        {
            UpdateRaceChoices();
            Parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            UpdateRaceChoices();
        }

        public ObservableRangeCollection<ChoiceViewModel> RaceChoices { get; set; } = new ObservableRangeCollection<ChoiceViewModel>();
        public void UpdateRaceChoices()
        {
            List<ChoiceViewModel> choices = new List<ChoiceViewModel>
            {
                new RaceChoice(this)
            };
            List<String> parentraces = new List<string>();
            foreach (Feature f in Context.Player.GetFeatures().Where(f => f is SubRaceFeature)) parentraces.AddRange(((SubRaceFeature)f).Races);
            if (parentraces.Count > 0) choices.Add(new SubRaceChoice(this, parentraces));
            foreach (Feature f in Context.Player.GetRaceFeatures())
            {
                ChoiceViewModel c = ChoiceViewModel<Feature>.GetChoice(this, f);
                if (c != null) choices.Add(c);
            }
            if (Context.Player.Race != null) foreach (Description d in Context.Player.Race.Descriptions) if (d is TableDescription td && td.Amount > 0) choices.Add(new DescriptionChoice(this, td, Navigation));
            if (Context.Player.SubRace != null) foreach (Description d in Context.Player.SubRace.Descriptions) if (d is TableDescription td && td.Amount > 0) choices.Add(new DescriptionChoice(this, td, Navigation));
            RaceChoices.ReplaceRange(choices);

        }
    }
}
