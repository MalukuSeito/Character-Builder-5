using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL.Common;
using OGL.Features;
using OGL;

namespace CB_5e.ViewModels
{
    public class SkillProficiencyChoice : ChoiceViewModel
    {
        public SkillProficiencyChoice(PlayerModel model, SkillProficiencyChoiceFeature feature) : base(model, feature.UniqueID, feature.Amount, feature, GetChoices(feature, model))
        {
        }

        public override void Refresh(Feature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = ((SkillProficiencyChoiceFeature)feature).Amount;
            Options = GetChoices((SkillProficiencyChoiceFeature)feature, Model);
            UpdateOptions();
        }
        private static List<Skill> GetChoices(SkillProficiencyChoiceFeature f, PlayerModel model)
        {
            List<Skill> shown;
            if (f.Skills.Count == 0) shown = (from s in model.Context.Skills.Values orderby s select s).ToList();
            else shown = new List<Skill>(from s in f.Skills select model.Context.GetSkill(s, f.Source));
            if (f.OnlyAlreadyKnownSkills)
            {
                IEnumerable<Skill> known = model.Context.Player.GetSkillProficiencies();
                shown.RemoveAll(e => !known.Any(s => s == e));
            }
            return shown;
        }
    }
}
