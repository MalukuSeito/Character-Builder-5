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
    public class PersonalityTraitChoice : ChoiceViewModel<Feature>
    {
        public string Trait {
            get
            {
                if (Other) return Model.Context.Player.PersonalityTrait2;
                else return Model.Context.Player.PersonalityTrait;
            }
            set
            {
                if (Other) Model.Context.Player.PersonalityTrait2 = value;
                else Model.Context.Player.PersonalityTrait = value;
            }
        }
        private bool Other = false;
        public PersonalityTraitChoice(SubModel model,List<TableEntry> choices, bool other) : base(model, null, 1, null, false, true)
        {
            Other = other;
            Name = other ? "2nd Personality Trait" : "Personality Trait";
            Choices = choices;
            Navigation = model.Navigation;
            OnSelect = new Command((par) =>
            {
                if (par is string s)
                {
                    Model.MakeHistory();
                    Trait = s;
                    Model.Save();
                    Model.FirePlayerChanged();
                }
                if (par is ChoiceOption co)
                {
                    Model.MakeHistory();
                    if (co.Selected)
                    {
                        Trait = null;
                    } 
                    else
                    {
                        Trait = co.NameWithSource;
                    }
                    Model.Save();
                    Model.FirePlayerChanged();
                } 
            });
            UpdateOptions();
        }

        public override List<string> GetMyTakenChoices()
        {
            return (Trait == null || Trait == "") ? new List<string>() : new List<string>()
            {
                Trait
            };
        }
        public override List<string> GetAllTakenChoices()
        {
            return GetMyTakenChoices();
        }

        public override int Taken => (Trait == null || Trait == "") ? 0 : 1;

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
