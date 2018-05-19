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
using OGL.Base;

namespace CB_5e.ViewModels.Modify
{
    public class LanguageEditModel : EditModel<Language>
    {
        
        public LanguageEditModel(Language obj, OGLContext context): base(obj, context)
        {
            
        }

        public string Name { get => Model.Name; set { if (value == Name) return; MakeHistory("Name"); Model.Name = value; OnPropertyChanged("Name"); } }
        public string Source { get => Model.Source; set { if (value == Source) return; MakeHistory("Source"); Model.Source = value; OnPropertyChanged("Source"); } }
        public string Description { get => Model.Description; set { if (value == Description) return; MakeHistory("Description"); Model.Description = value ; OnPropertyChanged("Description"); } }
        public string Speakers { get => Model.TypicalSpeakers; set { if (value == Speakers) return; MakeHistory("Speakers"); Model.TypicalSpeakers = value; OnPropertyChanged("Speakers"); } }
        public string Script { get => Model.Skript; set { if (value == Script) return; MakeHistory("Script"); Model.Skript = value; OnPropertyChanged("Script"); } }


        public override string GetPath(Language obj)
        {
            return Path.Combine(obj.Source, Context.Config.Languages_Directory);
        }

        public List<string> Abilities { get; set; } = Enum.GetNames(typeof(Ability)).ToList();
    }
}
