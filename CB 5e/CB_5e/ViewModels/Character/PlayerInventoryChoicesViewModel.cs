using CB_5e.Helpers;
using CB_5e.ViewModels.Character.Choices;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class PlayerInventoryChoicesViewModel : SubModel
    {
        public PlayerInventoryChoicesViewModel(PlayerModel parent) : base(parent, "Item Choices")
        {
            Image = ImageSource.FromResource("CB_5e.images.invoptions.png");
            UpdateInventoryChoices();
            parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            UpdateInventoryChoices();
        }

        public ObservableRangeCollection<ChoiceViewModel> InventoryChoices { get; set; } = new ObservableRangeCollection<ChoiceViewModel>();
        public void UpdateInventoryChoices()
        {
            List<ChoiceViewModel> choices = new List<ChoiceViewModel>();
            foreach (Feature f in Context.Player.GetPossessionFeatures())
            {
                ChoiceViewModel c = ChoiceViewModel<Feature>.GetChoice(this, f);
                if (c != null) choices.Add(c);
            }
            foreach (Feature f in Context.Player.GetBoons())
            {
                ChoiceViewModel c = ChoiceViewModel<Feature>.GetChoice(this, f);
                if (c != null) choices.Add(c);
            }
            InventoryChoices.ReplaceRange(choices);
        }
    }
}
