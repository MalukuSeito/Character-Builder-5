using CB_5e.Views;
using OGL;
using OGL.Common;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Modify.Items
{
    public interface IItemEditModel: IEditModel
    {
        string Name { get; set; }
        string Source { get; set; }
        string Description { get; set; }
        ImageSource Image { get; }

        Command ShowImage { get; }
        Command SaveImage { get; }
        int StackSize { get; set; }
        double Weight { get; set; }
        string Unit { get; set; }
        string SingleUnit { get; set; }
        int GP { get; set; }
        int PP { get; set; }
        int SP { get; set; }
        int CP { get; set; }
        int EP { get; set; }
        List<Keyword> Keywords { get; }
    }
    public class ItemEditModel<T> : EditModel<T>, IItemEditModel where T : Item, IOGLElement<T>
    {
        public ItemEditModel(T obj, OGLContext context): base(obj, context)
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
        public override string GetPath(T obj)
        {
            return Path.Combine(obj.Source, obj.Category == null || obj.Category.Path == "" ? Context.Config.Items_Directory : obj.Category.Path);
        }

        public string Name { get => Model.Name; set { if (value == Name) return; MakeHistory("Name"); Model.Name = value; OnPropertyChanged("Name"); } }
        public string Source { get => Model.Source; set { if (value == Source) return; MakeHistory("Source"); Model.Source = value; OnPropertyChanged("Source"); } }
        public string Description { get => Model.Description; set { if (value == Description) return; MakeHistory("Description"); Model.Description = value; OnPropertyChanged("Description"); } }
        public ImageSource Image
        {
            get { if (Model != null && Model.ImageData != null) return ImageSource.FromStream(() => new MemoryStream(Model.ImageData)); return null; }
        }

        public Command ShowImage { get; private set; }
        public Command SaveImage { get; private set; }
        public int StackSize { get => Model.StackSize; set { if (value == StackSize) return; MakeHistory("StackSize"); Model.StackSize = value; OnPropertyChanged("StackSize"); } }
        public double Weight { get => Model.Weight; set { if (value == Weight) return; MakeHistory("Weight"); Model.Weight = value; OnPropertyChanged("Weight"); } }
        public string Unit { get => Model.Unit; set { if (value == Unit) return; MakeHistory("Unit"); Model.Unit = value; OnPropertyChanged("Unit"); } }
        public string SingleUnit { get => Model.SingleUnit; set { if (value == SingleUnit) return; MakeHistory("SingleUnit"); Model.SingleUnit = value; OnPropertyChanged("SingleUnit"); } }
        public int GP
        {
            get { return Model.Price.gp; }
            set
            {
                if (Model.Price.gp == value) return;
                MakeHistory("MoneyGP");
                Model.Price.gp = value;
                OnPropertyChanged("Money");
                OnPropertyChanged("GP");
            }
        }
        public int PP
        {
            get { return Model.Price.pp; }
            set
            {
                if (Model.Price.pp == value) return;
                MakeHistory("MoneyPP");
                Model.Price.pp = value;
                OnPropertyChanged("Money");
                OnPropertyChanged("PP");
            }
        }
        public int SP
        {
            get { return Model.Price.sp; }
            set
            {
                if (Model.Price.sp == value) return;
                MakeHistory("MoneySP");
                Model.Price.sp = value;
                OnPropertyChanged("Money");
                OnPropertyChanged("SP");
            }
        }
        public int CP
        {
            get { return Model.Price.cp; }
            set
            {
                if (Model.Price.cp == value) return;
                MakeHistory("MoneyCP");
                Model.Price.cp = value;
                OnPropertyChanged("Money");
                OnPropertyChanged("CP");
            }
        }
        public int EP
        {
            get { return Model.Price.ep; }
            set
            {
                if (Model.Price.ep == value) return;
                MakeHistory("MoneyEP");
                Model.Price.ep = value;
                OnPropertyChanged("Money");
                OnPropertyChanged("EP");
            }
        }
        public List<Keyword> Keywords { get => Model.Keywords; }
    }
}
