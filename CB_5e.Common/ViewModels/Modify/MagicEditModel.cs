using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CB_5e.Helpers;
using CB_5e.Views;
using OGL;
using OGL.Descriptions;
using OGL.Features;
using OGL.Items;

namespace CB_5e.ViewModels.Modify
{
    public class MagicEditModel : EditModel<MagicProperty>
    {
        
        public MagicEditModel(MagicProperty cond, OGLContext context): base(cond, context)
        {
            
            ShowImage = new Command(async () =>
            {
                await Navigation.PushAsync(new ImageEditor(Image, Model.ImageData, SaveImage, "Image"));
            });
            SaveImage = new Command(async (par) =>
            {
                MakeHistory();
                Model.ImageData = par as byte[];
                OnPropertyChanged("Image");
                await Navigation.PopAsync();
            });
        }

        public string Name { get => Model.Name; set { if (value == Name) return; MakeHistory("Name"); Model.Name = value; OnPropertyChanged("Name"); } }
        public string Source { get => Model.Source; set { if (value == Source) return; MakeHistory("Source"); Model.Source = value; OnPropertyChanged("Source"); } }
        public string Description { get => Model.Description; set { if (value == Description) return; MakeHistory("Description"); Model.Description = value ; OnPropertyChanged("Description"); } }
        public string PrependName { get => Model.PrependName; set { if (value == PrependName) return; MakeHistory("PrependName"); Model.PrependName = value; OnPropertyChanged("PrependName"); } }
        public string PostName { get => Model.PostName; set { if (value == PostName) return; MakeHistory("PostName"); Model.PostName = value; OnPropertyChanged("PostName"); } }
        public string Base { get => Model.Base; set { if (value == Base) return; MakeHistory("Base"); Model.Base = value; OnPropertyChanged("Base"); } }
        public string Requirement { get => Model.Requirement; set { if (value == Requirement) return; MakeHistory("Requirement"); Model.Requirement = value; OnPropertyChanged("Requirement"); } }
        public string Slot { get => Model.Slot.ToString(); set { if (value == Slot) return; MakeHistory("Slot"); if (Enum.TryParse(value, out Slot a)) Model.Slot = a; else Model.Slot = OGL.Items.Slot.None; OnPropertyChanged("Slot"); } }
        public string Rarity { get => Model.Rarity.ToString(); set { if (value == Rarity) return; MakeHistory("Rarity"); if (Enum.TryParse(value, out OGL.Base.Rarity a)) Model.Rarity = a; else Model.Rarity = OGL.Base.Rarity.Common; OnPropertyChanged("Rarity"); } }
        

        public ImageSource Image
        {
            get { if (Model != null && Model.ImageData != null) return ImageSource.FromStream(() => new MemoryStream(Model.ImageData)); return null; }
        }

        public Command ShowImage { get; private set; }
        public Command SaveImage { get; private set; }

        public override string GetPath(MagicProperty obj)
        {
            return Path.Combine(obj.Source, obj.Category == null || obj.Category == "" ? Context.Config.Magic_Directory : obj.Category);
        }

        public List<string> Slots { get; set; } = Enum.GetNames(typeof(OGL.Items.Slot)).ToList();
        public List<string> Rarities { get; set; } = Enum.GetNames(typeof(OGL.Base.Rarity)).ToList();

        public List<Feature> AttunementFeatures { get => Model.AttunementFeatures; }
        public List<Feature> CarryFeatures { get => Model.CarryFeatures; }
        public List<Feature> EquipFeatures { get => Model.EquipFeatures; }
        public List<Feature> AttunedEquipFeatures { get => Model.AttunedEquipFeatures; }
        public List<Feature> OnUseFeatures { get => Model.OnUseFeatures; }
        public List<Feature> AttunedOnUseFeatures { get => Model.AttunedOnUseFeatures; }
        public Command Matches { get; set; }
    }
}
