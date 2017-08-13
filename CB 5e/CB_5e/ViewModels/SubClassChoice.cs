using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class SubClassChoice : ChoiceViewModel<SubClassFeature>
    {
        public SubClassChoice(PlayerModel model, SubClassFeature feature) : base(model, feature.ParentClass, 1, feature, false)
        {
            Name = "Subclass for " + UniqueID;
            OnSelect = new Command((par) =>
            {
                if (par is ChoiceOption co)
                {
                    Model.MakeHistory();
                    if (co.Selected)
                    {
                        Model.Context.Player.RemoveSubclass(UniqueID);
                    }
                    else
                    {
                        Model.Context.Player.AddSubclass(UniqueID, co.NameWithSource);
                    }
                    Model.Save();
                    Model.FirePlayerChanged();
                }
            });
        }

        public override List<string> GetMyTakenChoices()
        {
            string s = Model.Context.Player.GetSubclassName(UniqueID);
            return s == null || s == "" ? new List<string>() : new List<string>()
            {
                s
            };
        }
        public override List<string> GetAllTakenChoices()
        {
            string s = Model.Context.Player.GetSubclassName(UniqueID);
            return s == null || s == "" ? new List<string>() : new List<string>()
            {
                s
            };
        }

        public override int Taken {
            get {
                string s = Model.Context.Player.GetSubclassName(UniqueID);
                return s == null || s == "" ? 0 : 1;
            }
        }

        public override void Refresh(SubClassFeature feature)
        {
            Feature = feature;
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return Model.Context.SubClassFor(Feature.ParentClass).OrderBy(r => r.Name).ToList();
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetSubClass(nameWithSource, Feature.Source);
        }
    }
}
