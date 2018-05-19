using CB_5e.Helpers;
using CB_5e.ViewModels.Character.Choices;
using OGL;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Character.Build
{
    public class BackgroundViewModel : SubModel
    {
        public BackgroundViewModel(PlayerModel parent) : base(parent, "Background")
        {
            UpdateRaceChoices();
            Parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            UpdateRaceChoices();
        }

        public ObservableRangeCollection<ChoiceViewModel> BackgroundChoices { get; set; } = new ObservableRangeCollection<ChoiceViewModel>();
        public void UpdateRaceChoices()
        {
            List<ChoiceViewModel> choices = new List<ChoiceViewModel>
            {
                new BackgroundChoice(this)
            };
            foreach (Feature f in Context.Player.GetOnlyBackgroundFeatures())
            {
                ChoiceViewModel c = ChoiceViewModel<Feature>.GetChoice(this, f);
                if (c != null) choices.Add(c);
            }
            Background back = Context.Player.Background;
            List<TableDescription> tables = Context.Player.CollectTables();

            List<TableEntry> traits = new List<TableEntry>();
            if (back != null) traits.AddRange(back.PersonalityTrait.ToArray<TableEntry>());
            foreach (TableDescription td in tables) if (td.BackgroundOption.HasFlag(BackgroundOption.Trait)) traits.AddRange(td.Entries.ToArray<TableEntry>());
            choices.Add(new PersonalityTraitChoice(this, traits));

            List<TableEntry> ideals = new List<TableEntry>();
            if (back != null) ideals.AddRange(back.Ideal.ToArray<TableEntry>());
            foreach (TableDescription td in tables) if (td.BackgroundOption.HasFlag(BackgroundOption.Ideal)) ideals.AddRange(td.Entries.ToArray<TableEntry>());
            choices.Add(new IdealChoice(this, ideals));

            List<TableEntry> bonds = new List<TableEntry>();
            if (back != null) bonds.AddRange(back.Bond.ToArray<TableEntry>());
            foreach (TableDescription td in tables) if (td.BackgroundOption.HasFlag(BackgroundOption.Bond)) bonds.AddRange(td.Entries.ToArray<TableEntry>());
            choices.Add(new BondChoice(this, bonds));

            List<TableEntry> flaws = new List<TableEntry>();
            if (back != null) flaws.AddRange(back.Flaw.ToArray<TableEntry>());
            foreach (TableDescription td in tables) if (td.BackgroundOption.HasFlag(BackgroundOption.Flaw)) flaws.AddRange(td.Entries.ToArray<TableEntry>());
            choices.Add(new FlawChoice(this, flaws));

            foreach (TableDescription td in tables) if (td.Amount > 0) choices.Add(new DescriptionChoice(this, td, Navigation));
            BackgroundChoices.ReplaceRange(choices);

        }
    }
}
