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
using PCLStorage;
using OGL.Descriptions;
using OGL.Features;

namespace CB_5e.ViewModels
{
    public class BackgroundEditModel : EditModel<Background>
    {
        
        public BackgroundEditModel(Background cond, OGLContext context): base(cond, context)
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

        public ImageSource Image
        {
            get { if (Model != null && Model.ImageData != null) return ImageSource.FromStream(() => new MemoryStream(Model.ImageData)); return null; }
        }

        public Command ShowImage { get; private set; }
        public Command SaveImage { get; private set; }

        public override string GetPath(Background obj)
        {
            return PortablePath.Combine(obj.Source, Context.Config.Backgrounds_Directory);
        }

        public List<Description> Descriptions { get => Model.Descriptions; }
        public List<Feature> Features { get => Model.Features; }
        public List<TableEntry> Traits { get => Model.PersonalityTrait; }
        public List<TableEntry> Ideals { get => Model.Ideal; }
        public List<TableEntry> Bonds { get => Model.Bond; }
        public List<TableEntry> Flaws { get => Model.Flaw; }
    }
}
