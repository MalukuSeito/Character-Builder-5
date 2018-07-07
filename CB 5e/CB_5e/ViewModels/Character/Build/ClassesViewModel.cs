using CB_5e.Helpers;
using CB_5e.ViewModels.Character.Choices;
using Character_Builder;
using OGL.Descriptions;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character.Build
{
    public class ClassesViewModel : SubModel
    {
        public ClassesViewModel(PlayerModel parent) : base(parent, "Class(es)")
        {
            UpdateClassChoices();
            Image = ImageSource.FromResource("CB_5e.images.class.png");
            Parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            UpdateClassChoices();
        }
        public ObservableRangeCollection<ChoiceViewModel> ClassChoices { get; set; } = new ObservableRangeCollection<ChoiceViewModel>();
        public void UpdateClassChoices()
        {
            int level = Context.Player.GetLevel();
            List<ChoiceViewModel> choices = new List<ChoiceViewModel>();
            int nextlevel = 1;
            foreach (ClassInfo ci in Context.Player.GetClassInfos(level))
            {
                if (ci == null || ci.Class == null || ci.Level > level) break;
                nextlevel = ci.Level + 1;
                choices.Add(new ClassChoice(this, ci.Level));
                choices.Add(new ClassHPChoice(this, ci.Level, ci.Class.HitDieCount * Math.Max(1, ci.Class.HitDie)));
            }
            if (nextlevel <= level) choices.Add(new ClassChoice(this, nextlevel));
            choices.AddRange(from Feature f in Context.Player.GetFeatures(level) where f is SubClassFeature select new SubClassChoice(this, (SubClassFeature)f));

            foreach (Feature f in Context.Player.GetClassFeatures(level))
            {
                ChoiceViewModel c = ChoiceViewModel<Feature>.GetChoice(this, f);
                if (c != null) choices.Add(c);
            }
            foreach (PlayerClass pc in Context.Player.Classes)
            {
                foreach (TableDescription d in pc.CollectTables(Context))
                {
                    if (d.Amount > 0) choices.Add(new DescriptionChoice(this, d, Navigation));
                }
            }
            ClassChoices.ReplaceRange(choices);
        }
    }
}
