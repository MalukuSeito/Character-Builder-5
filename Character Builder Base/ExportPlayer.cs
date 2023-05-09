using OGL;
using OGL.Base;
using OGL.Features;
using OGL.Items;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class ExportPlayer
    {
        public Player Player { get; set; }
        public ExportPlayer()
        {
            Player = new Player();  
        }
        public ExportPlayer(Player player)
        {
            Player = player;
        }
        public List<Possession> Possessions { get => Player.GetItemsAndPossessions(); }
        public double Weigth { get => Player.GetWeight(); }
        public List<Feature> Features { get => Player.GetFeatures(includeOnUseFeatures: true); }
        public IEnumerable<AbilityFeatChoice> AbilityFeatChoices { get => Player.GetAbilityFeatChoices(); }
        public Dictionary<string, int> ClassLevels { get => Player.GetClassLevels().ToDictionary(k => k.Key.Name, v => v.Value); }
        public List<string> SpellcastingIDs { get => new List<string>(from f in Player.GetFeatures() where f is SpellcastingFeature sf && sf.SpellcastingID != "MULTICLASS" select ((SpellcastingFeature)f).SpellcastingID); }
        public Dictionary<string, SpellcastingExport> Spellcasting { get => SpellcastingIDs.ToDictionary(s => s, s => new SpellcastingExport(Player.GetSpellcasting(s), Player)); }
        public Dictionary<string, int> SpellSaveDC { get => (from f in Player.GetFeatures() where f is SpellcastingFeature sf && sf.SpellcastingID != "MULTICLASS" select (SpellcastingFeature)f).ToDictionary(f => f.SpellcastingID, f => Player.GetSpellSaveDC(f.SpellcastingID, f.SpellcastingAbility)); }
        public Dictionary<string, int> SpellAttack { get => (from f in Player.GetFeatures() where f is SpellcastingFeature sf && sf.SpellcastingID != "MULTICLASS" select (SpellcastingFeature)f).ToDictionary(f => f.SpellcastingID, f => Player.GetSpellAttack(f.SpellcastingID, f.SpellcastingAbility)); }
        public Dictionary<string, int> SpellcastingLevel { get => SpellcastingIDs.ToDictionary(s => s, s => Player.GetClassLevel(s)); }
        public Dictionary<string, List<int>> SpellcastingSlots { get => SpellcastingIDs.ToDictionary(s => s, s => Player.GetSpellSlots(s)); }
        public Dictionary<string, List<int>> SpellcastingUsedSlots { get => SpellcastingIDs.ToDictionary(s => s, s => Player.GetUsedSpellSlots(s)); }
        public Dictionary<string, List<SpellSlotInfo>> SpellcastingSlotInfo { get => SpellcastingIDs.ToDictionary(s => s, s => Player.GetSpellSlotInfo(s)); }
        public List<int> HpRolls { get => Player.GetHProllsByLevel(); }
        public List<ClassInfoExport> ClassInfo { get => Player.GetClassInfos().Select(c => new ClassInfoExport(c)).ToList(); }
        public List<string> ClassNamesAndLevels { get => (from pc in Player.Classes select pc.ToString(Player.Context, Player.GetLevel())).ToList(); }
        public Price Money { get => Player.GetMoney(); }
        public Dictionary<string, ResourceInfoExport> Resources { get => Player.GetResourceInfo(false).ToDictionary(k => k.Key, v => new ResourceInfoExport(v.Value)); }
        public List<Skill> SkillProficiencies { get => Player.GetSkillProficiencies().ToList(); }
        public Ability SaveProficiencies { get => Player.GetSaveProficiencies(); }
        public List<Language> Languages { get => Player.GetLanguages().ToList(); }
        public List<Tool> ToolProficiencies { get => Player.GetToolProficiencies().ToList(); }
        public List<ModifiedSpell> BonusSpells { get => Player.GetBonusSpells().ToList(); }
        public List<string> ToolKWProficiencies { get => Player.GetToolKWProficiencies().ToList(); }
        public List<string> OtherProficiencies { get => Player.GetOtherProficiencies().ToList(); }
        public List<string> Immunities { get => Player.GetImmunities().ToList(); }
        public List<string> Vulnerabilities { get => Player.GetVulnerabilities().ToList(); }
        public List<string> Resistances { get => Player.GetResistances().ToList(); }
        public List<string> SavingThrowAdvantages { get => Player.GetSavingThrowAdvantages().ToList(); }
        public string RaceSubName { get => Player.GetRaceSubName(); }
        public double Speed { get => Player.GetSpeed(); }
        public int DarkVisionRange { get => Player.GetVisionRange(); }
        public AbilityScoreArray Abilities { get => Player.GetFinalAbilityScores(); }
        public AbilityScoreArray SavingThrowBoni { get => Player.GetSavingThrowsBoni(); }
        public Dictionary<Ability, int> Saves { get => Player.GetSaves(); }
        public int MaxHP { get => Player.GetHitpointMax(); }
        public List<HitDie> HitDice { get => Player.GetHitDie(); }
        public int StrengthSave { get => Player.GetSave(Ability.Strength); }
        public int ConstitutionSave { get => Player.GetSave(Ability.Constitution); }
        public int DexteritySave { get => Player.GetSave(Ability.Dexterity); }
        public int IntelligenceSave { get => Player.GetSave(Ability.Intelligence); }
        public int WisdomSave { get => Player.GetSave(Ability.Wisdom); }
        public int CharismaSave { get => Player.GetSave(Ability.Charisma); }
        public List<SkillInfo> Skills { get => Player.GetSkills(); }
        public int PassivePerception { get => Player.GetPassiveSkill(Player.Context.GetSkill("Perception", null)); }
        public int PassiveInvestigation { get => Player.GetPassiveSkill(Player.Context.GetSkill("Investigation", null)); }
        public int AC { get => Player.GetAC(); }
        public Item Armor { get => Player.GetArmor(); }
        public Item MainHand { get => Player.GetMainHand(); }
        public Item OffHand { get => Player.GetOffHand(); }
        public List<Feature> PossessionFeatures { get => Player.GetPossessionFeatures(); }
        public List<Feature> Boons { get => Player.GetBoons(); }
        public List<KeyValuePair<string, AttackInfo>> Attacks
        {
            get
            {
                int level = Player.GetLevel();
                List<Possession> equip = new List<Possession>();
                List<Possession> treasure = new List<Possession>();
                List<Feature> onUse = new List<Feature>();

                foreach (Possession pos in Player.GetItemsAndPossessions())
                {
                    if (pos.Count > 0)
                    {
                        if (pos.BaseItem != null && pos.BaseItem != "") equip.Add(pos);
                        else
                        {
                            treasure.Add(pos);
                        }
                        onUse.AddRange(pos.CollectOnUse(level, Player, Player.Context));
                    }
                }

                List<KeyValuePair<string, AttackInfo>> attackinfos = new List<KeyValuePair<string, AttackInfo>>();
                List<KeyValuePair<string, AttackInfo>> addattackinfos = new List<KeyValuePair<string, AttackInfo>>();
                List<SpellcastingFeature> spellcasts = new List<SpellcastingFeature>(from f in Player.GetFeatures() where f is SpellcastingFeature && ((SpellcastingFeature)f).SpellcastingID != "MULTICLASS" select (SpellcastingFeature)f);
                foreach (SpellcastingFeature scf in spellcasts)
                {
                    Spellcasting sc = Player.GetSpellcasting(scf.SpellcastingID);
                    foreach (Spell s in sc.GetLearned(Player, Player.Context))
                    {
                        if (sc.Highlight != null && sc.Highlight != "" && s.Name.ToLowerInvariant() == sc.Highlight.ToLowerInvariant())
                        {
                            AttackInfo ai = Player.GetAttack(s, scf.SpellcastingAbility);
                            if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                        }
                        else
                        {
                            AttackInfo ai = Player.GetAttack(s, scf.SpellcastingAbility);
                            if (ai != null && ai.Damage != null && ai.Damage != "") addattackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                        }
                    }
                }
                foreach (Possession pos in equip)
                {
                    IEnumerable<AttackInfo> ais = Player.GetAttack(pos);
                    if (ais != null) attackinfos.AddRange(ais.Select(ai => new KeyValuePair<string, AttackInfo>(pos.ToString() + (ai.AttackOptions.Count > 0 ? " (" + string.Join(", ", ai.AttackOptions) + " )" : ""), ai)));
                }
                foreach (ModifiedSpell s in Player.GetBonusSpells(false))
                {
                    if (Utils.Matches(Player.Context, s, "Attack or Save", null))
                    {
                        AttackInfo ai = Player.GetAttack(s, s.differentAbility);
                        if (ai != null && ai.Damage != null && ai.Damage != "") attackinfos.Add(new KeyValuePair<string, AttackInfo>(s.Name, ai));
                    }
                }
                attackinfos.AddRange(addattackinfos);
                return attackinfos;
            }
        }
        public int Initiative { get => Player.GetInitiative(); }
        public int Level { get => Player.GetLevel(); }
        public int ProficiencyBonus { get => Player.GetProficiency(); }
        public int XP { get => Player.GetXP(); }
        public double CarryCapacity { get => Player.GetCarryCapacity(); }
        public List<ActionInfo> Actions { get => Player.GetActions(); }
        public List<MonsterInfo> SummonsAndForms
        {
            get
            {
                Dictionary<Monster, MonsterInfo> monsters = new Dictionary<Monster, MonsterInfo>(new MonsterInfo());
                foreach (FormsCompanionInfo fc in Player.GetFormsCompanionChoices())
                {
                    foreach (Monster m in fc.AppliedChoices(Player.Context, Player.GetFinalAbilityScores()))
                    {
                        if (monsters.ContainsKey(m))
                        {
                            monsters[m].Sources.Add(fc.DisplayName);
                        }
                        else
                        {
                            monsters.Add(m, new MonsterInfo()
                            {
                                Monster = m,
                                Sources = new List<string>() { fc.DisplayName }
                            });
                        }
                    }
                }
                return monsters.Values.ToList();
            }
        }
    }

    public class SpellcastingExport
    {
        private Spellcasting spellcasting;
        private Player player;
        public SpellcastingExport(Spellcasting spellcasting, Player player)
        {
            this.spellcasting = spellcasting;
            this.player = player;
        }
        public string SpellcastingID { get => spellcasting.SpellcastingID; }
        public SpellcastingFeature SpellcastingFeature { get => player.GetFeatures().Find(f => f is SpellcastingFeature sf && sf.SpellcastingID == SpellcastingID) as SpellcastingFeature; }
        public List<string> Prepared { get => spellcasting.Prepared; }
        public List<SpellChoice> Spellchoices { get => spellcasting.Spellchoices; }
        public List<string> SpellbookAdditional { get => spellcasting.SpellbookAdditional; }
        public List<int> UsedSlots { get => spellcasting.UsedSlots; }
        public List<ModifiedSpell> PreparedSpells { get => spellcasting.GetPrepared(player, player.Context); }
        public List<ModifiedSpell> LearnedSpells { get => spellcasting.GetLearned(player, player.Context).ToList(); }
        public List<Spell> Spellbook { get => spellcasting.GetSpellbook(player, player.Context).ToList(); }
        public List<Spell> AdditonalClassSpells { get => spellcasting.GetAdditionalClassSpells(player, player.Context).ToList(); }
        public List<string> PreparedList { get => spellcasting.GetPreparedList(player, player.Context); }
        public List<string> AdditonalList { get => spellcasting.GetAdditionalList(player, player.Context); }
        public int SaveDC { get => player.GetSpellSaveDC(SpellcastingID, SpellcastingFeature?.SpellcastingAbility ?? Ability.None); }
        public int SpellAttack { get => player.GetSpellAttack(SpellcastingID, SpellcastingFeature?.SpellcastingAbility ?? Ability.None); }
        public List<int> Slots { get => player.GetSpellSlots(SpellcastingID); }
        public List<SpellSlotInfo> SlotInfo { get => player.GetSpellSlotInfo(SpellcastingID); }
    }

    public class ClassInfoExport
    {
        private ClassInfo classInfo;
        public ClassInfoExport(ClassInfo info) { classInfo = info; }
        public int Level { get => classInfo.Level; }
        public int Hp { get => classInfo.Hp; }
        public int ClassLevel { get => classInfo.ClassLevel; }
        public string Name { get => classInfo?.Class?.Name; }
    }
    public class ResourceInfoExport
    {
        private ResourceInfo resourceInfo;
        public ResourceInfoExport(ResourceInfo info) { resourceInfo = info; }
        public string Name { get => resourceInfo.Name; }
        public string ResourceID { get => resourceInfo.ResourceID; }
        public int Max { get => resourceInfo.Max; }
        public int Used { get => resourceInfo.Used; }
        public RechargeModifier Recharge { get => resourceInfo.Recharge; }

    }
}
