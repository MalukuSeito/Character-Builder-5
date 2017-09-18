using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;
using Xamarin.Forms;
using OGL.Descriptions;
using CB_5e.ViewModels.Character.ChoiceOptions;

namespace CB_5e.ViewModels.Character.Choices
{
    public class FlawChoice : ChoiceViewModel<Feature>
    {
        public FlawChoice(SubModel model, List<TableEntry> choices) : base(model, null, 1, null, false, true)
        {
            Choices = choices;
            Navigation = model.Navigation;
            Name = "Flaw";
            OnSelect = new Command((par) =>
            {
                if (par is string s)
                {
                    Model.MakeHistory();
                    Model.Context.Player.Flaw = s;
                    Model.Save();
                    Model.FirePlayerChanged();
                }
                if (par is ChoiceOption co)
                {
                    Model.MakeHistory();
                    if (co.Selected)
                    {
                        Model.Context.Player.Flaw = null;
                    } 
                    else
                    {
                        Model.Context.Player.Flaw = co.NameWithSource;
                    }
                    Model.Save();
                    Model.FirePlayerChanged();
                } 
            });
            UpdateOptions();
        }

        public override List<string> GetMyTakenChoices()
        {
            return Model.Context.Player.Flaw == null || Model.Context.Player.Flaw == "" ? new List<string>() : new List<string>()
            {
                Model.Context.Player.Flaw
            };
        }
        public override List<string> GetAllTakenChoices()
        {
            return Model.Context.Player.Flaw == null || Model.Context.Player.Flaw == "" ? new List<string>() : new List<string>()
            {
                Model.Context.Player.Flaw
            };
        }

        public override int Taken => Model.Context.Player.Flaw == null || Model.Context.Player.Flaw == "" ? 0 : 1;

        public override void Refresh(Feature feature)
        {
        }

        public List<TableEntry> Choices { get; private set; }

        protected override IEnumerable<IXML> GetOptions()
        {
            return (from s in Choices select new TableValue { Entry = s }).ToList();
        }

        public override IXML GetValue(string nameWithSource)
        {
            TableEntry c = Choices.FirstOrDefault(s => s.ToString() == nameWithSource);
            if (c != null) return new TableValue { Entry = c };
            return null;
        }

        protected override void UpdateOptionsImmediately()
        {
        }
    }
}
