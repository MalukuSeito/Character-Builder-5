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
using OGL.Base;

namespace CB_5e.ViewModels.Modify
{
    public class SubClassEditModel : EditModel<SubClass>
    {
        
        public SubClassEditModel(SubClass cls, OGLContext context): base(cls, context)
        {
            if (cls.ClassName == null) cls.ClassName = "";
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
            SaveCostumClass = new Command((par) =>
            {
                if (par is string s)
                {
                    Classes.Add(s);
                    ClassName = s;
                }
            });
            Classes.Add("");
            Classes.AddRange(context.ClassesSimple.Keys);
            if (!Classes.Contains(cls.ClassName)) Classes.Add(cls.ClassName);
            Classes.Add(CUSTOM);
        }

        private static string CUSTOM = ConfigManager.SourceSeperator + " Custom " + ConfigManager.SourceSeperator;
        public string Name { get => Model.Name; set { if (value == Name) return; MakeHistory("Name"); Model.Name = value; OnPropertyChanged("Name"); } }
        public string SheetName { get => Model.SheetName; set { if (value == SheetName) return; MakeHistory("SheetName"); Model.SheetName = value; OnPropertyChanged("SheetName"); } }
        public string Source { get => Model.Source; set { if (value == Source) return; MakeHistory("Source"); Model.Source = value; OnPropertyChanged("Source"); } }
        public string Description { get => Model.Description; set { if (value == Description) return; MakeHistory("Description"); Model.Description = value ; OnPropertyChanged("Description"); } }
        public string Flavour { get => Model.Flavour; set { if (value == Flavour) return; MakeHistory("Flavour"); Model.Flavour = value; OnPropertyChanged("Flavour"); } }
        public string ClassName { get => Model.ClassName; set { if (value == ClassName) return; MakeHistory("ClassName"); if (value == CUSTOM) Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new CustomTextEntryPage("Parent Class", SaveCostumClass))); else Model.ClassName = value; OnPropertyChanged("ClassName"); } }

        public ImageSource Image
        {
            get { if (Model != null && Model.ImageData != null) return ImageSource.FromStream(() => new MemoryStream(Model.ImageData)); return null; }
        }

        public Command ShowImage { get; private set; }
        public Command SaveImage { get; private set; }

        public override string GetPath(SubClass obj)
        {
            return Path.Combine(obj.Source, Context.Config.SubClasses_Directory);
        }
        public List<Description> Descriptions { get => Model.Descriptions; }
        public List<Feature> Features { get => Model.Features; }
        public List<Feature> MulticlassingFeatures { get => Model.MulticlassingFeatures; }
        public List<Feature> FirstClassFeatures { get => Model.FirstClassFeatures; }
        public List<int> MulticlassingSpellLevels { get => Model.MulticlassingSpellLevels; }
        public Command SaveCostumClass { get; private set; }
        public ObservableRangeCollection<string> Classes { get; set; } = new ObservableRangeCollection<string>();
    }
}

