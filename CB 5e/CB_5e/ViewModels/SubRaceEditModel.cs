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
using CB_5e.Services;

namespace CB_5e.ViewModels
{
    public class SubRaceEditModel : EditModel<SubRace>
    {
        private static string CUSTOM = ConfigManager.SourceSeperator + " Custom " + ConfigManager.SourceSeperator;
        public SubRaceEditModel(SubRace cond, OGLContext context): base(cond, context)
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
            SaveCostumRace = new Command((par) =>
            {
                if (par is string s)
                {
                    Races.Add(s);
                    ParentRace = s;
                }
            });
            Races.AddRange(context.RacesSimple.Keys);
            Races.Add(CUSTOM);
        }

        public string Name { get => Model.Name; set { if (value == Name) return; MakeHistory("Name"); Model.Name = value; OnPropertyChanged("Name"); } }
        public string Source { get => Model.Source; set { if (value == Source) return; MakeHistory("Source"); Model.Source = value; OnPropertyChanged("Source"); } }
        public string Description { get => Model.Description; set { if (value == Description) return; MakeHistory("Description"); Model.Description = value ; OnPropertyChanged("Description"); } }
        public string Flavour { get => Model.Flavour; set { if (value == Flavour) return; MakeHistory("Flavour"); Model.Flavour = value; OnPropertyChanged("Flavour"); } }
        public string ParentRace { get => Model.RaceName; set { if (value == ParentRace) return; MakeHistory("ParentRace"); if (value == CUSTOM) Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new CustomTextEntryPage("Parent Race", SaveCostumRace))); else Model.RaceName = value; OnPropertyChanged("ParentRace"); } }



        public ImageSource Image
        {
            get { if (Model != null && Model.ImageData != null) return ImageSource.FromStream(() => new MemoryStream(Model.ImageData)); return null; }
        }

        public Command ShowImage { get; private set; }
        public Command SaveImage { get; private set; }

        public override string GetPath(SubRace obj)
        {
            return PortablePath.Combine(obj.Source, Context.Config.SubRaces_Directory);
        }

        public ObservableRangeCollection<string> Races { get; set; } = new ObservableRangeCollection<string>();

        public List<Description> Descriptions { get => Model.Descriptions; }
        public List<Feature> Features { get => Model.Features; }
        public Command SaveCostumRace { get; private set; }
    }
}
