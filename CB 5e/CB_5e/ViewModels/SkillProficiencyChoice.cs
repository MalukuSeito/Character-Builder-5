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
    public class SkillProficiencyChoice : ChoiceViewModel<SkillProficiencyChoiceFeature>
    {
        public SkillProficiencyChoice(PlayerModel model, SkillProficiencyChoiceFeature feature) : base(model, feature.UniqueID, feature.Amount, feature)
        {
        }

        public override IXML GetValue(string nameWithSource)
        {
            return Model.Context.GetSkill(nameWithSource, Feature.Source);
        }

        public override void Refresh(SkillProficiencyChoiceFeature feature)
        {
            Feature = feature;
            Name = feature.Name;
            Amount = feature.Amount;
        }
        protected override IEnumerable<IXML> GetOptions()
        {
            List<IXML> shown;
            if (Feature.Skills.Count == 0) shown = (from s in Model.Context.Skills.Values orderby s select s).ToList<IXML>();
            else shown = new List<IXML>(from s in Feature.Skills select Model.Context.GetSkill(s, Feature.Source));
            if (Feature.OnlyAlreadyKnownSkills)
            {
                IEnumerable<Skill> known = Model.Context.Player.GetSkillProficiencies();
                shown.RemoveAll(e => !known.Any(s => s == e));
            }
            return shown;
        }
    }
}
