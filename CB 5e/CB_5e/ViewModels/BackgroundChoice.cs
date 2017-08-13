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
    public class BackgroundChoice : ChoiceViewModel<Feature>
    {
        public BackgroundChoice(PlayerModel model) : base(model, null, 1, null, false)
        {
            Name = "Background";
            OnSelect = new Command((par) =>
            {
                if (par is ChoiceOption co)
                {
                    Model.MakeHistory();
                    if (co.Selected)
                    {
                        Model.Context.Player.BackgroundName = null;
                    } 
                    else
                    {
                        Model.Context.Player.BackgroundName = co.NameWithSource;
                    }
                    Model.Save();
                    Model.FirePlayerChanged();
                } 
            });
        }

        public override List<string> GetMyTakenChoices()
        {
            return Model.Context.Player.BackgroundName == null || Model.Context.Player.BackgroundName == "" ? new List<string>() : new List<string>()
            {
                Model.Context.Player.BackgroundName
            };
        }
        public override List<string> GetAllTakenChoices()
        {
            return Model.Context.Player.BackgroundName == null || Model.Context.Player.BackgroundName == "" ? new List<string>() : new List<string>()
            {
                Model.Context.Player.BackgroundName
            };
        }

        public override int Taken => Model.Context.Player.BackgroundName == null || Model.Context.Player.BackgroundName == "" ? 0 : 1;

        public override void Refresh(Feature feature)
        {
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            return Model.Context.Backgrounds.Values.OrderBy(r => r.Name).ToList();
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetBackground(nameWithSource, null);
        }
    }
}
