using CB_5e.Helpers;
using Character_Builder;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character
{
    public class InventoryViewModel : ObservableObject
    {
        public Possession Item { get; set; }
        public Feature Boon { get; set; }
        public string Name { get => Item != null ? Item.FullName + (Item.Amount != null ? " (" + Item.Amount + ")" : "") : Boon.ToString(); }
        public string Description { get => Item != null ? Item.Description : Boon.Text; }
        public string Detail { get => Item != null ? Item.Stats : "Boon" + (Boon.Level > 1 ? ", Level " + Boon.Level: ""); }
        public Command ShowInfo { get; set; }
        public Command Edit { get; set; }
        public Command Delete { get; set; }
    }
}
