using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class MultiFeature : Feature
    {
        [XmlArrayItem(Type = typeof(AbilityScoreFeature)),
        XmlArrayItem(Type = typeof(BonusSpellKeywordChoiceFeature)),
        XmlArrayItem(Type = typeof(ChoiceFeature)),
        XmlArrayItem(Type = typeof(CollectionChoiceFeature)),
        XmlArrayItem(Type = typeof(Feature)),
        XmlArrayItem(Type = typeof(FreeItemAndGoldFeature)),
        XmlArrayItem(Type = typeof(ItemChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(ItemChoiceFeature)),
        XmlArrayItem(Type = typeof(HitPointsFeature)),
        XmlArrayItem(Type = typeof(LanguageProficiencyFeature)),
        XmlArrayItem(Type = typeof(LanguageChoiceFeature)),
        XmlArrayItem(Type = typeof(MultiFeature)),
        XmlArrayItem(Type = typeof(OtherProficiencyFeature)),
        XmlArrayItem(Type = typeof(SaveProficiencyFeature)),
        XmlArrayItem(Type = typeof(SpeedFeature)),
        XmlArrayItem(Type = typeof(SkillProficiencyChoiceFeature)),
        XmlArrayItem(Type = typeof(SkillProficiencyFeature)),
        XmlArrayItem(Type = typeof(SubRaceFeature)),XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
         XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)),XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)),XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> Features;

        public String Condition { get; set; }

            public MultiFeature()
            : base()
        {
            Features = new List<Feature>();
        }
        public MultiFeature(string name, string text, Feature feature1, Feature feature2, int level = 1, bool hidden = false)
        : base(name, text, level, hidden)
        {
            Features = new List<Feature>();
            Features.Add(feature1);
            Features.Add(feature2);
        }
        public MultiFeature(string name, string text, Feature feature1, Feature feature2, Feature feature3, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Features = new List<Feature>();
            Features.Add(feature1);
            Features.Add(feature2);
            Features.Add(feature3);
        }
        public MultiFeature(string name, string text, Feature feature1, Feature feature2, Feature feature3, Feature feature4, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Features = new List<Feature>();
            Features.Add(feature1);
            Features.Add(feature2);
            Features.Add(feature3);
            Features.Add(feature4);
        }
        public override List<Feature> Collect(int level, IChoiceProvider choiceProvider)
        {
            if (Level > level) return new List<Feature>();
            List<Feature> res = new List<Feature>() {this};
            if (Condition != null && Condition.Length > 0 && !choiceProvider.matches(Condition, null, level)) return res;
            foreach (Feature f in Features)
            {
                res.AddRange(f.Collect(level, choiceProvider));
            }
            return res;
        }
        public override string Displayname()
        {
            return "Multi-Feature";
        }
        public override string Source
        {
            get
            {
                return base.Source;
            }

            set
            {
                foreach (Feature f in Features) f.Source = value;
                base.Source = value;
            }
        }
    }
}
