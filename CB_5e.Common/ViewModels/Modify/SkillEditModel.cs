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
    public class SkillEditModel : EditModel<Skill>
    {
        
        public SkillEditModel(Skill obj, OGLContext context): base(obj, context)
        {
            
        }

        public string Name { get => Model.Name; set { if (value == Name) return; MakeHistory("Name"); Model.Name = value; OnPropertyChanged("Name"); } }
        public string Source { get => Model.Source; set { if (value == Source) return; MakeHistory("Source"); Model.Source = value; OnPropertyChanged("Source"); } }
        public string Description { get => Model.Description; set { if (value == Description) return; MakeHistory("Description"); Model.Description = value ; OnPropertyChanged("Description"); } }
        public string Base { get => Model.Base != Ability.None ? Model.Base.ToString() : null; set { if (value == Base) return; MakeHistory("Base"); if (Enum.TryParse(value, out Ability a)) Model.Base = a; else Model.Base = Ability.None ; OnPropertyChanged("Base"); } }


        public override string GetPath(Skill obj)
        {
            return Path.Combine(obj.Source, Context.Config.Skills_Directory);
        }

        public List<string> Abilities { get; set; } = Enum.GetNames(typeof(Ability)).ToList();
    }
}
