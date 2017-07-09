using Character_Builder_Forms;
using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Items;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class BonusFeatureForm : Form, IEditor<BonusFeature> { 
        private BonusFeature bf;
        public BonusFeatureForm(BonusFeature f)
        {
            bf = f;

            if (f.DamageBonusModifier != Ability.None)
            {
                f.DamageBonus = ResourceFeatureForm.convert(f.DamageBonusModifier, f.DamageBonus);
                f.DamageBonusModifier = Ability.None;
            }

            InitializeComponent();
            foreach (Ability s in Enum.GetValues(typeof(Ability))) if (s != Ability.None) BaseAbility.Items.Add(s, f.BaseAbility.HasFlag(s));
            foreach (Ability s in Enum.GetValues(typeof(Ability))) if (s != Ability.None) SaveBonusAbility.Items.Add(s, f.SavingThrowAbility.HasFlag(s));
            Condition.DataBindings.Add("Text", f, "Condition", true, DataSourceUpdateMode.OnPropertyChanged);
            Attack.DataBindings.Add("Text", f, "AttackBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            SaveDC.DataBindings.Add("Text", f, "SaveDCBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            AC.DataBindings.Add("Text", f, "ACBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            Initiative.DataBindings.Add("Text", f, "InitiativeBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            Damage.DataBindings.Add("Text", f, "DamageBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            DamageText.DataBindings.Add("Text", f, "DamageBonusText", true, DataSourceUpdateMode.OnPropertyChanged);
            Skill.DataBindings.Add("Text", f, "SkillBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            Passive.DataBindings.Add("Checked", f, "SkillPassive", true, DataSourceUpdateMode.OnPropertyChanged);
            saveBonus.DataBindings.Add("Text", f, "SavingThrowBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            profBonus.DataBindings.Add("Text", f, "ProficiencyBonus", true, DataSourceUpdateMode.OnPropertyChanged);
            BonusSize.DataBindings.Add("Value", f, "SizeChange", true, DataSourceUpdateMode.OnPropertyChanged);
            SkillList.Items = f.Skills;
            proficiencyOptions.Items = f.ProficiencyOptions;
            ImportExtensions.ImportSkills();
            SkillList.Suggestions = OGL.Skill.simple.Keys;
            ImportExtensions.ImportItems();
            proficiencyOptions.Suggestions = from it in Item.simple.Values where it is Weapon select it.Name;
            basicFeature1.Feature = f;
            foreach (string s in OGL.Item.simple.Keys)
            {
                if (Item.simple[s] is Weapon)
                {
                    Spell.AutoCompleteCustomSource.Add(s);
                    Spell.Items.Add(s);
                }
            }
            Spell.DataBindings.Add("Text", f, "BaseItemChange");
        }

        public BonusFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }

        private void BaseAbility_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) bf.BaseAbility |= (Ability)BaseAbility.Items[e.Index];
            else if (e.NewValue == CheckState.Unchecked) bf.BaseAbility &= ~(Ability)BaseAbility.Items[e.Index];
        }

        private void SaveBonusAbility_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) bf.SavingThrowAbility |= (Ability)SaveBonusAbility.Items[e.Index];
            else if (e.NewValue == CheckState.Unchecked) bf.SavingThrowAbility &= ~(Ability)SaveBonusAbility.Items[e.Index];
        }
    }
}

