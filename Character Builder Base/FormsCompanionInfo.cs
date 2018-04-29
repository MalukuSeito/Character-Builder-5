using OGL;
using OGL.Base;
using OGL.Features;
using OGL.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class FormsCompanionInfo
    {
        public String DisplayName { get; set; }
        public String ID { get; set; }
        public String SourceHint { get; set; }
        public List<String> Options { get; set; } = new List<string>();
        public List<Monster> Choices { get; set; } = new List<Monster>();
        public List<Feature> Modifiers { get; set; } = new List<Feature>();
        public OGL.Common.IXML Source { get; set; }
        public int Count { get; set; } = 0;
        public int ClassLevel { get; set; }

        public List<Monster> AvailableOptions(BuilderContext context, AbilityScoreArray asa)
        {
            return Options.SelectMany(s => Utils.FilterMonsters(s, context, asa, ClassLevel)).OrderBy(m => m.Name).ThenBy(m => m.Source).Distinct().ToList();
        }
        public override string ToString()
        {
            return DisplayName + ": " + Choices.Count + (Count < 0 ? "" : " / " + Count ) + (Choices.Count > 0 ? " (" + String.Join(", ", Choices) + ")" : "" );
        }
        public List<Monster> AppliedChoices(BuilderContext context, AbilityScoreArray asa, int level = 0)
        {
            if (Modifiers.Exists(f=>f is FormsCompanionsBonusFeature))
            {
                List<Monster> res = Choices.Select(m => m.Clone()).ToList();
                foreach (Feature f in Modifiers)
                {
                    if (f is FormsCompanionsBonusFeature bf)
                    {
                        foreach (Monster m in res) {
                            if (bf.HPBonus != null && bf.HPBonus != "" && bf.HPBonus != "value")
                                m.HP = Utils.EvaluateMonster(context, m, bf.HPBonus, asa, m.HP, "HP", null, ClassLevel, level);
                            if (bf.ACBonus != null && bf.ACBonus != "" && bf.HPBonus != "value")
                                m.AC = Utils.EvaluateMonster(context, m, bf.ACBonus, asa, m.AC, "AC", null, ClassLevel, level);
                            if (bf.SavesBonus != null && bf.SavesBonus != "" && bf.SavesBonus != "value")
                            {
                                List<MonsterSaveBonus> result = new List<MonsterSaveBonus>();
                                foreach (Ability a in Enum.GetValues(typeof(Ability)))
                                {
                                    List<String> add = new List<string>();
                                    MonsterSaveBonus msb = new MonsterSaveBonus()
                                    {
                                        Ability = a,
                                        Bonus = 0
                                    };
                                    foreach (MonsterSaveBonus ms in m.SaveBonus) if (ms.Ability == a)
                                        {
                                            if (ms.Bonus > 0) add.Add("proficient");
                                            msb = ms;
                                        }
                                    msb.Bonus = Utils.EvaluateMonster(context, m, bf.SavesBonus, asa, msb.Bonus, a.ToString(), add, ClassLevel, level);
                                    if (msb.Bonus != 0) result.Add(msb);
                                }
                                m.SaveBonus = result;
                            }
                            if (bf.SkillsBonus != null && bf.SkillsBonus != "" && bf.SkillsBonus != "value")
                            {
                                List<MonsterSkillBonus> result = new List<MonsterSkillBonus>();
                                foreach (Skill a in context.SkillsSimple.Values)
                                {
                                    List<String> add = new List<string>();
                                    MonsterSkillBonus msb = new MonsterSkillBonus()
                                    {
                                        Skill = a.Name,
                                        Bonus = 0
                                    };
                                    foreach (MonsterSkillBonus ms in m.SkillBonus) if (ConfigManager.SourceInvariantComparer.Equals(ms.Skill, a.Name))
                                        {
                                            if (ms.Bonus > 0) add.Add("proficient");
                                            msb = ms;
                                        }
                                    msb.Bonus = Utils.EvaluateMonster(context, m, bf.SkillsBonus, asa, msb.Bonus, a.ToString(), add, ClassLevel, level);
                                    if (msb.Bonus != 0) result.Add(msb);
                                }
                                m.SkillBonus = result;
                            }
                            if (bf.StrengthBonus != null && bf.StrengthBonus != "" && bf.StrengthBonus != "value")
                                m.Strength = Utils.EvaluateMonster(context, m, bf.StrengthBonus, asa, m.Strength, "Strength", null, ClassLevel, level);
                            if (bf.DexterityBonus != null && bf.DexterityBonus != "" && bf.DexterityBonus != "value")
                                m.Dexterity = Utils.EvaluateMonster(context, m, bf.DexterityBonus, asa, m.Dexterity, "Dexterity", null, ClassLevel, level);
                            if (bf.ConstitutionBonus != null && bf.ConstitutionBonus != "" && bf.ConstitutionBonus != "value")
                                m.Constitution = Utils.EvaluateMonster(context, m, bf.ConstitutionBonus, asa, m.Constitution, "Constitution", null, ClassLevel, level);
                            if (bf.IntelligenceBonus != null && bf.IntelligenceBonus != "" && bf.IntelligenceBonus != "value")
                                m.Intelligence = Utils.EvaluateMonster(context, m, bf.IntelligenceBonus, asa, m.Intelligence, "Intelligence", null, ClassLevel, level);
                            if (bf.WisdomBonus != null && bf.WisdomBonus != "" && bf.WisdomBonus != "value")
                                m.Wisdom = Utils.EvaluateMonster(context, m, bf.WisdomBonus, asa, m.Wisdom, "Wisdom", null, ClassLevel, level);
                            if (bf.CharismaBonus != null && bf.CharismaBonus != "" && bf.CharismaBonus != "value")
                                m.Charisma = Utils.EvaluateMonster(context, m, bf.CharismaBonus, asa, m.Charisma, "Charisma", null, ClassLevel, level);
                            if (bf.AttackBonus != null && bf.AttackBonus != "" && bf.AttackBonus != "value")
                            {
                                foreach (MonsterTrait mt in m.Actions) if (mt is MonsterAction ma) ma.AttackBonus = Utils.EvaluateMonster(context, m, bf.AttackBonus, asa, ma.AttackBonus, "Attack", null, ClassLevel, level);
                                foreach (MonsterTrait mt in m.Reactions) if (mt is MonsterAction ma) ma.AttackBonus = Utils.EvaluateMonster(context, m, bf.AttackBonus, asa, ma.AttackBonus, "Attack", null, ClassLevel, level);
                                foreach (MonsterTrait mt in m.LegendaryActions) if (mt is MonsterAction ma) ma.AttackBonus = Utils.EvaluateMonster(context, m, bf.AttackBonus, asa, ma.AttackBonus, "Attack", null, ClassLevel, level);
                            }
                            if (bf.DamageBonus != null && bf.DamageBonus != "" && bf.DamageBonus != "value")
                            {
                                foreach (MonsterTrait mt in m.Actions) if (mt is MonsterAction ma)
                                    {
                                        int bonus = Utils.EvaluateMonster(context, m, bf.DamageBonus, asa, 0, "Damage", null, ClassLevel, level);
                                        if (bonus > 0) ma.Damage = ma.Damage + "+" + bonus.ToString();
                                        else if (bonus  < 0) ma.Damage = ma.Damage + bonus.ToString();
                                    }
                                foreach (MonsterTrait mt in m.Reactions) if (mt is MonsterAction ma)
                                    {
                                        int bonus = Utils.EvaluateMonster(context, m, bf.DamageBonus, asa, 0, "Damage", null, ClassLevel, level);
                                        if (bonus > 0) ma.Damage = ma.Damage + "+" + bonus.ToString();
                                        else if (bonus < 0) ma.Damage = ma.Damage + bonus.ToString();
                                    }

                                foreach (MonsterTrait mt in m.LegendaryActions) if (mt is MonsterAction ma)
                                    {
                                        int bonus = Utils.EvaluateMonster(context, m, bf.DamageBonus, asa, 0, "Damage", null, ClassLevel, level);
                                        if (bonus > 0) ma.Damage = ma.Damage + "+" + bonus.ToString();
                                        else if (bonus < 0) ma.Damage = ma.Damage + bonus.ToString();
                                    }
                            }
                            if (bf.TraitBonusName != null && bf.TraitBonusName != "")
                            {
                                m.Traits.Add(new MonsterTrait(bf.TraitBonusName, bf.TraitBonusText));
                            }
                            m.Senses.AddRange(bf.Senses);
                            m.Speeds.AddRange(bf.Speed);
                            m.Languages.AddRange(bf.Languages);
                        }
                    }
                }
                return res;
            }
            return Choices;
        }
    }
}
