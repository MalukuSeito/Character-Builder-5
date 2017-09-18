using OGL;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OGL.Common;
using CB_5e.ViewModels.Character.ChoiceOptions;

namespace CB_5e.ViewModels.Character.Choices
{

    public class ClassHPChoice : ChoiceViewModel<Feature>
    {

        public int Level { get; set; }
        public ClassHPChoice(PlayerModel model, int level, int max) : base(model, null, 1 , null , false)
        {
            Max = max;
            Level = level;
            Name = "Hitpoints rolled at level " + level;
            OnSelect = new Command((par) =>
            {
                if (par is ChoiceOption co)
                {
                    Model.MakeHistory();
                    if (co.Selected)
                    {
                        Model.Context.Player.SetHPRoll(Level, 0);
                    }
                    else if (co.Value is HP h)
                    {
                        Model.Context.Player.SetHPRoll(Level, h.Value);
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
            int hp = Model.Context.Player.GetHPRoll(Level);
            return hp == 0 ? new List<string>() : new List<string>()
            {
                hp.ToString()
            };
        }
        public override List<string> GetAllTakenChoices()
        {
            int hp = Model.Context.Player.GetHPRoll(Level);
            return hp == 0 ? new List<string>() : new List<string>()
            {
                hp.ToString()
            };
        }

        public override int Taken => Model.Context.Player.GetHPRoll(Level) == 0 ? 0 : 1;

        public int Max { get; private set; }

        public override void Refresh(Feature feature)
        {
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return (from l in Enumerable.Range(1, Max) select new HP() { Value = l }).ToList();
        }

        public override IXML GetValue(string nameWithSource)
        {
            if (int.TryParse(nameWithSource, out int v)) return new HP() { Value = v };
            return null;
        }
    }
}
