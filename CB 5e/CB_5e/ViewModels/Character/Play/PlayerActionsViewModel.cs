using CB_5e.Helpers;
using Character_Builder;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Character.Play
{
    public class PlayerActionsViewModel : SubModel
    {
        public PlayerActionsViewModel(PlayerModel parent) : base(parent, "Actions")
        {
            actions = new List<ActionInfo>();
            actions.AddRange(Context.Player.GetActions());
            UpdateActions();
            parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            actions = new List<ActionInfo>();
            actions.AddRange(Context.Player.GetActions());
            UpdateActions();
        }

        private string actionsSearch;
        public string ActionsSearch
        {
            get => actionsSearch;
            set
            {
                SetProperty(ref actionsSearch, value);
                UpdateActions();
            }
        }
        private List<ActionInfo> actions;
        public ObservableRangeCollection<ActionInfo> Actions { get; set; } = new ObservableRangeCollection<ActionInfo>();

        public void UpdateActions() => Actions.ReplaceRange(from f in actions where actionsSearch == null || actionsSearch == ""
            || Culture.CompareInfo.IndexOf(f.ToString() ?? "", actionsSearch, CompareOptions.IgnoreCase) >= 0 orderby f.ToString() select f);

    }
}
