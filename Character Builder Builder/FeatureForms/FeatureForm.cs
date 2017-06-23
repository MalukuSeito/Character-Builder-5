using OGL.Common;
using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class FeatureForm : Form, IEditor<Feature>
    {
        private Feature bf;
        public FeatureForm(Feature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            Text = f.Displayname();
        }
        public static Feature dispatch(Feature f, IHistoryManager manager)
        {
            if (f is AbilityScoreFeatFeature) return new AbilityScoreFeatFeatureForm(f as AbilityScoreFeatFeature).edit(manager);
            else if (f is AbilityScoreFeature) return new AbilityScoreFeatureForm(f as AbilityScoreFeature).edit(manager);
            else if (f is ACFeature) return new ACFeatureForm(f as ACFeature).edit(manager);
            else if (f is BonusFeature) return new BonusFeatureForm(f as BonusFeature).edit(manager);
            else if (f is BonusSpellFeature) return new BonusSpellFeatureForm(f as BonusSpellFeature).edit(manager);
            else if (f is BonusSpellKeywordChoiceFeature) return new BonusSpellKeywordChoiceFeatureForm(f as BonusSpellKeywordChoiceFeature).edit(manager);
            else if (f is BonusSpellPrepareFeature) return new BonusSpellPrepareFeatureForm(f as BonusSpellPrepareFeature).edit(manager);
            else if (f is ChoiceFeature) return new ChoiceFeatureForm(f as ChoiceFeature).edit(manager);
            else if (f is CollectionChoiceFeature) return new CollectionChoiceFeatureForm(f as CollectionChoiceFeature).edit(manager);
            else if (f is ExtraAttackFeature) return new ExtraAttackFeatureForm(f as ExtraAttackFeature).edit(manager);
            else if (f is ItemChoiceFeature) return new ItemChoiceFeatureForm(f as ItemChoiceFeature).edit(manager);
            else if (f is FreeItemAndGoldFeature) return new FreeItemAndGoldFeatureForm(f as FreeItemAndGoldFeature).edit(manager);
            else if (f is HitPointsFeature) return new HitPointsFeatureForm(f as HitPointsFeature).edit(manager);
            else if (f is IncreaseSpellChoiceAmountFeature) return new IncreaseSpellChoiceAmountFeatureForm(f as IncreaseSpellChoiceAmountFeature).edit(manager);
            else if (f is ItemChoiceConditionFeature) return new ItemChoiceConditionFeatureForm(f as ItemChoiceConditionFeature).edit(manager);
            else if (f is LanguageChoiceFeature) return new LanguageChoiceFeatureForm(f as LanguageChoiceFeature).edit(manager);
            else if (f is LanguageProficiencyFeature) return new LanguageProficiencyFeatureForm(f as LanguageProficiencyFeature).edit(manager);
            else if (f is ResourceFeature) return new ResourceFeatureForm(f as ResourceFeature).edit(manager);
            else if (f is SaveProficiencyFeature) return new SaveProficiencyFeatureForm(f as SaveProficiencyFeature).edit(manager);
            else if (f is SkillProficiencyFeature) return new SkillProficiencyFeatureForm(f as SkillProficiencyFeature).edit(manager);
            else if (f is SkillProficiencyChoiceFeature) return new SkillProficiencyChoiceFeatureForm(f as SkillProficiencyChoiceFeature).edit(manager);
            else if (f is SpeedFeature) return new SpeedFeatureForm(f as SpeedFeature).edit(manager);
            else if (f is ModifySpellChoiceFeature) return new ModifySpellChoiceFeatureForm(f as ModifySpellChoiceFeature).edit(manager);
            else if (f is MultiFeature) return new MultiFeatureForm(f as MultiFeature).edit(manager);
            else if (f is SpellcastingFeature) return new SpellcastingFeatureForm(f as SpellcastingFeature).edit(manager);
            else if (f is SpellChoiceFeature) return new SpellChoiceFeatureForm(f as SpellChoiceFeature).edit(manager);
            else if (f is SpellModifyFeature) return new SpellModifyFeatureForm(f as SpellModifyFeature).edit(manager);
            else if (f is SpellSlotsFeature) return new SpellSlotsFeatureForm(f as SpellSlotsFeature).edit(manager);
            else if (f is SubClassFeature) return new SubClassFeatureForm(f as SubClassFeature).edit(manager);
            else if (f is SubRaceFeature) return new SubRaceFeatureForm(f as SubRaceFeature).edit(manager);
            else if (f is ToolKWProficiencyFeature) return new ToolKWProficiencyFeatureForm(f as ToolKWProficiencyFeature).edit(manager);
            else if (f is ToolProficiencyChoiceConditionFeature) return new ToolProficiencyChoiceConditionFeatureForm(f as ToolProficiencyChoiceConditionFeature).edit(manager);
            else if (f is ToolProficiencyFeature) return new ToolProficiencyFeatureForm(f as ToolProficiencyFeature).edit(manager);
            else if (f is VisionFeature) return new VisionFeatureForm(f as VisionFeature).edit(manager);
            else return new FeatureForm(f).edit(manager);
        }

        public Feature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
