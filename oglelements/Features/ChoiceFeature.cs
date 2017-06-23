using OGL.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OGL.Features
{
    public class ChoiceFeature: Feature
    {
        public int Amount { get; set; }
        public String UniqueID { get; set; }
        public bool AllowSameChoice { get; set; } = false;
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
        XmlArrayItem(Type = typeof(SubRaceFeature)), XmlArrayItem(Type = typeof(SubClassFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolKWProficiencyFeature)),
        XmlArrayItem(Type = typeof(ToolProficiencyChoiceConditionFeature)),
        XmlArrayItem(Type = typeof(BonusFeature)),
        XmlArrayItem(Type = typeof(SpellcastingFeature)),
        XmlArrayItem(Type = typeof(IncreaseSpellChoiceAmountFeature)), XmlArrayItem(Type = typeof(ModifySpellChoiceFeature)),
        XmlArrayItem(Type = typeof(SpellChoiceFeature)), XmlArrayItem(Type = typeof(SpellSlotsFeature)),
        XmlArrayItem(Type = typeof(BonusSpellPrepareFeature)),
        XmlArrayItem(Type = typeof(BonusSpellFeature)),
        XmlArrayItem(Type = typeof(ACFeature)),
        XmlArrayItem(Type = typeof(AbilityScoreFeatFeature)),
        XmlArrayItem(Type = typeof(ExtraAttackFeature)),
        XmlArrayItem(Type = typeof(ResourceFeature)),
        XmlArrayItem(Type = typeof(SpellModifyFeature)),
        XmlArrayItem(Type = typeof(VisionFeature))]
        public List<Feature> Choices;
        [XmlIgnore]
        private List<List<Feature>> copies = new List<List<Feature>>();
        public ChoiceFeature()
            : base()
        {
            Choices = new List<Feature>();
            Amount = 1;
        }
        public ChoiceFeature(string name, string text, string uniqueID, List<Feature> choices, int amount = 1, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Choices = new List<Feature>(choices);
            Amount = amount;
            UniqueID = uniqueID;
        }
        public ChoiceFeature(string name, string text, string uniqueID, Feature choice1, Feature choice2, int amount = 1, int level = 1, bool hidden = false)
        : base(name, text, level, hidden)
        {
            Choices = new List<Feature>();
            Amount = amount;
            UniqueID = uniqueID;
            Choices.Add(choice1);
            Choices.Add(choice2);
        }
        public ChoiceFeature(string name, string text, string uniqueID, Feature choice1, Feature choice2, Feature choice3, int amount = 1, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Choices = new List<Feature>();
            Amount = amount;
            UniqueID = uniqueID;
            Choices.Add(choice1);
            Choices.Add(choice2);
            Choices.Add(choice3);
        }
        public ChoiceFeature(string name, string text, string uniqueID, Feature choice1, Feature choice2, Feature choice3, Feature choice4, int amount = 1, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Choices = new List<Feature>();
            Amount = amount;
            UniqueID = uniqueID;
            Choices.Add(choice1);
            Choices.Add(choice2);
            Choices.Add(choice3);
            Choices.Add(choice4);
        }
        public ChoiceFeature(string name, string text, string uniqueID, Feature choice1, Feature choice2, Feature choice3, Feature choice4, Feature choice5, int amount = 1, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Choices = new List<Feature>();
            Amount = amount;
            UniqueID = uniqueID;
            Choices.Add(choice1);
            Choices.Add(choice2);
            Choices.Add(choice3);
            Choices.Add(choice4);
            Choices.Add(choice5);
        }
        public ChoiceFeature(string name, string text, string uniqueID, Feature choice1, Feature choice2, Feature choice3, Feature choice4, Feature choice5, Feature choice6, int amount = 1, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Choices = new List<Feature>();
            Amount = amount;
            UniqueID = uniqueID;
            Choices.Add(choice1);
            Choices.Add(choice2);
            Choices.Add(choice3);
            Choices.Add(choice4);
            Choices.Add(choice5);
            Choices.Add(choice6);
        }
        
        public override List<Feature> Collect(int level, IChoiceProvider choiceProvider)
        {
            if (Level > level) return new List<Feature>();
            List<Feature> res= new List<Feature>() { this };
            int offset = choiceProvider.getChoiceOffset(this, UniqueID, Amount);
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";
                if (c + offset> 0) counter = "_" + (c + offset).ToString();
                Choice cho = choiceProvider.getChoice(UniqueID + counter);
                List<Feature> choices = Choices;
                if (AllowSameChoice) choices = getCopy(c);
                if (cho != null && cho.Value != "")
                {
                    Feature feat = choices.Find(fe => fe.Name + " " + ConfigManager.SourceSeperator + " " + fe.Source == cho.Value);
                    if (feat == null) feat = choices.Find(fe => ConfigManager.SourceInvariantComparer.Equals(fe.Name + " " + ConfigManager.SourceSeperator + " " + fe.Source, cho.Value));
                    if (feat != null) res.AddRange(feat.Collect(level, choiceProvider));
                }
            }
            return res;
        }

        private List<Feature> getCopy(int c)
        {
            if (c == 0) return Choices;
            c--;
            while (c >= copies.Count)
            {
                copies.Add(FeatureCollection.MakeCopy(Choices));
            }
            return copies[c];
        }

        public override string Source
        {
            get
            {
                return base.Source;
            }

            set
            {
                foreach (Feature f in Choices) f.Source = value;
                base.Source = value;
            }
        }
        public override string Displayname()
        {
            return "Feature Choice Feature";
        }
    }
}
