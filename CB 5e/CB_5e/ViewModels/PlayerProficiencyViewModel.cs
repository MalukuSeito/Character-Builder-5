using CB_5e.Helpers;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels
{
    public class PlayerProficiencyViewModel : SubModel
    {
        public PlayerProficiencyViewModel(PlayerModel parent) : base(parent, "Proficiencies")
        {
            proficiencies = new List<IXML>();
            proficiencies.AddRange(Context.Player.GetLanguages());
            proficiencies.AddRange(Context.Player.GetToolProficiencies());
            proficiencies.AddRange(from p in Context.Player.GetToolKWProficiencies() select new Feature(p, "Proficiency"));
            proficiencies.AddRange(from p in Context.Player.GetOtherProficiencies() select new Feature(p, "Proficiency"));
            UpdateProficiencies();
            parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            proficiencies = new List<IXML>();
            proficiencies.AddRange(Context.Player.GetLanguages());
            proficiencies.AddRange(Context.Player.GetToolProficiencies());
            proficiencies.AddRange(from p in Context.Player.GetToolKWProficiencies() select new Feature(p, "Proficiency"));
            proficiencies.AddRange(from p in Context.Player.GetOtherProficiencies() select new Feature(p, "Proficiency"));
            UpdateProficiencies();
        }

        private string proficiencySearch;
        public string ProficiencySearch
        {
            get => proficiencySearch;
            set
            {
                SetProperty(ref proficiencySearch, value);
                UpdateProficiencies();
            }
        }
        private List<IXML> proficiencies;
        public ObservableRangeCollection<IXML> Proficiencies { get; set; } = new ObservableRangeCollection<IXML>();

        public void UpdateProficiencies() => Proficiencies.ReplaceRange(from f in proficiencies where proficiencySearch == null || proficiencySearch == ""
            || Culture.CompareInfo.IndexOf(f.ToString() ?? "", proficiencySearch, CompareOptions.IgnoreCase) >= 0 orderby f.ToString() select f);

    }
}
