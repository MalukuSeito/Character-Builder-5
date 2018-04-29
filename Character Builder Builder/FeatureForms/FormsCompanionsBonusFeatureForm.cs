using OGL.Common;
using OGL.Features;
using Character_Builder_Forms;
using System.Windows.Forms;
using OGL;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class FormsCompanionsBonusFeatureForm : Form, IEditor<FormsCompanionsBonusFeature>
    {
        private FormsCompanionsBonusFeature bf;
        public FormsCompanionsBonusFeatureForm(FormsCompanionsBonusFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            foreach (string s in FormsCompanionsFeatureForm.MONSTER_FEATURES)
            {
                UniqueID.AutoCompleteCustomSource.Add(s);
                UniqueID.Items.Add(s);
            }
            Program.Context.ImportSpells();
            foreach (Spell s in Program.Context.SpellsSimple.Values)
            {
                if (s.FormsCompanionsFilter != null && s.FormsCompanionsFilter != "" && s.FormsCompanionsFilter != "false")
                {
                    UniqueID.AutoCompleteCustomSource.Add(s.Name.ToLowerInvariant());
                    UniqueID.Items.Add(s.Name.ToLowerInvariant());
                }
            }
            UniqueID.DataBindings.Add("Text", f, "UniqueID", true, DataSourceUpdateMode.OnPropertyChanged);
            HPBonus.DataBindings.Add("Text", f, "HPBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            ACBonus.DataBindings.Add("Text", f, "ACBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            SavesBonus.DataBindings.Add("Text", f, "SavesBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            SkillsBonus.DataBindings.Add("Text", f, "SkillsBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            StrengthBonus.DataBindings.Add("Text", f, "StrengthBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            DexterityBonus.DataBindings.Add("Text", f, "DexterityBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            ConstitutionBonus.DataBindings.Add("Text", f, "ConstitutionBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            IntelligenceBonus.DataBindings.Add("Text", f, "IntelligenceBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            WisdomBonus.DataBindings.Add("Text", f, "WisdomBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            CharismaBonus.DataBindings.Add("Text", f, "CharismaBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            TraitBonusName.DataBindings.Add("Text", f, "TraitBonusName", true, DataSourceUpdateMode.OnPropertyChanged);
            DamageBonus.DataBindings.Add("Text", f, "DamageBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            AttackBonus.DataBindings.Add("Text", f, "AttackBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            NewlineFormatter.Add(TraitBonusText.DataBindings, "Text", f, "TraitBonusText", true, DataSourceUpdateMode.OnPropertyChanged);
            Senses.Items = f.Senses;
            Speed.Items = f.Speed;
            Languages.Items = f.Languages;
        }

        public FormsCompanionsBonusFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
