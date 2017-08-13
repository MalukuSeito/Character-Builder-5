using OGL;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OGL.Common;

namespace CB_5e.ViewModels
{
    public class ClassChoice: ChoiceViewModel<Feature>
    {

        public int Level { get; set; }
        public ClassChoice(PlayerModel model, int level) : base(model, null, 1, null, false)
        {
            Level = level;
            Name = "Class for level " + (level);
            OnSelect = new Command((par) =>
            {
                if (par is ChoiceOption co)
                {
                    Model.MakeHistory();
                    if (co.Selected)
                    {
                        Model.Context.Player.DeleteClass(Level);
                    }
                    else if (co.Value is ClassDefinition cd)
                    {
                        Model.Context.Player.AddClass(cd, Level);
                    }
                    Model.Save();
                    Model.FirePlayerChanged();
                }
            });
            UpdateOptions();
        }

        protected override void UpdateOptionsImmediately()
        {
        }

        public override List<string> GetMyTakenChoices()
        {
            string cd = Model.Context.Player.GetClassName(Level);
            return cd == null ? new List<string>() : new List<string>()
            {
                cd
            };
        }
        public override List<string> GetAllTakenChoices()
        {
            string cd = Model.Context.Player.GetClassName(Level);
            return cd == null ? new List<string>() : new List<string>()
            {
                cd
            };
        }

        public override int Taken => Model.Context.Player.GetClassName(Level) == null ? 0 : 1;

        public override void Refresh(Feature feature)
        {
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return Model.Context.GetClasses(Level, Model.Context.Player).OrderBy(r => r.Name).ToList();
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetClass(nameWithSource, null);
        }
    }
}
