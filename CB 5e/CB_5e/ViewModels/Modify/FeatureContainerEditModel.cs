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
using OGL.Base;
using OGL.Features;

namespace CB_5e.ViewModels.Modify
{
    public class FeatureContainerEditModel : EditModel<FeatureContainer>
    {
        
        public FeatureContainerEditModel(FeatureContainer obj, OGLContext context): base(obj, context)
        {
            
        }

        public string Name { get => Model.Name; set { if (value == Name) return; MakeHistory("Name"); Model.Name = value; OnPropertyChanged("Name"); } }
        public string Source { get => Model.Source; set { if (value == Source) return; MakeHistory("Source"); Model.Source = value; OnPropertyChanged("Source"); } }


        public override string GetPath(FeatureContainer obj)
        {
            return PortablePath.Combine(obj.Source, obj.category == null || obj.category == "" ? Context.Config.Features_Directory : obj.category);
        }

        public List<Feature> Features { get => Model.Features; }
    }
}
