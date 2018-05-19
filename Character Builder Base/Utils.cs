using OGL;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Items;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Text;
using NCalc;

namespace Character_Builder
{
    public class Utils
    {
        private static EvaluateFunctionHandler MakeFunctionExtensions(BuilderContext context)
        {
            return (string name, FunctionArgs args) =>
            {
                if (name.Equals("ClassLevel", StringComparison.OrdinalIgnoreCase))
                {
                    object[] o = args.EvaluateParameters();
                    if (o.Length > 0)
                    {
                        object cls = o[0];
                        if (cls is string)
                        {
                            Dictionary<ClassDefinition, int> classes = context.Player.GetClassLevels();
                            foreach (ClassDefinition c in classes.Keys)
                            {
                                if (c.Name.Equals(cls as string, StringComparison.OrdinalIgnoreCase))
                                {
                                    args.Result = classes[c];
                                    return;
                                }
                            }
                        }
                        args.Result = 0;
                    }
                    else
                    {
                        args.Result = context.Player.GetLevel();
                    }
                }
                if (name.Equals("SubClass", StringComparison.OrdinalIgnoreCase))
                {
                    object[] o = args.EvaluateParameters();
                    if (o.Length > 0)
                    {
                        object cls = o[0];
                        if (cls is string)
                        {
                            SubClass s = context.Player.GetSubclass((string)cls);
                            if (s != null)
                            {
                                args.Result = s.Name.ToLowerInvariant();
                            }
                        }
                        args.Result = "";
                    }
                    else
                    {
                        args.Result = "";
                    }
                }
            };
        }


        

        private static string Convert(Ability a, string s = null)
        {
            StringBuilder sb = new StringBuilder();
            if (s != null && s.Trim() != "" && s.Trim() != "0") sb.Append(s);
            if (a.HasFlag(Ability.Strength)) sb.Append((sb.Length > 0 ? " + " : "") + "StrMod");
            if (a.HasFlag(Ability.Dexterity)) sb.Append((sb.Length > 0 ? " + " : "") + "DexMod");
            if (a.HasFlag(Ability.Constitution)) sb.Append((sb.Length > 0 ? " + " : "") + "ConMod");
            if (a.HasFlag(Ability.Intelligence)) sb.Append((sb.Length > 0 ? " + " : "") + "IntMod");
            if (a.HasFlag(Ability.Wisdom)) sb.Append((sb.Length > 0 ? " + " : "") + "WisMod");
            if (a.HasFlag(Ability.Charisma)) sb.Append((sb.Length > 0 ? " + " : "") + "ChaMod");
            return sb.ToString();
        }


        public static int Evaluate(BuilderContext context, ResourceFeature f, AbilityScoreArray asa, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            if (f.ValueBonus != Ability.None)
            {
                f.Value = Convert(f.ValueBonus, f.Value);
                f.ValueBonus = Ability.None;
            }
            return Evaluate(context, f.Value, asa, additionalKeywords, classlevel, level);
        }

        public static int Evaluate(BuilderContext context, BonusFeature f, AbilityScoreArray asa, List<string> additionalKeywords = null, int classlevel = 0, int level = 0, Item i = null)
        {
            if (f.DamageBonusModifier != Ability.None)
            {
                f.DamageBonus = Convert(f.DamageBonusModifier, f.DamageBonus);
                f.DamageBonusModifier = Ability.None;
            }
            return Evaluate(context, f.DamageBonus, asa, additionalKeywords, classlevel, level, i);
        }

        public static int Evaluate(BuilderContext context, Spell s, BonusFeature f, AbilityScoreArray asa, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            if (f.DamageBonusModifier != Ability.None)
            {
                f.DamageBonus = Convert(f.DamageBonusModifier, f.DamageBonus);
                f.DamageBonusModifier = Ability.None;
            }
            return Evaluate(context, s, f.DamageBonus, asa, additionalKeywords, classlevel, level);
        }

        public static int Evaluate(BuilderContext context, SpellcastingFeature f, AbilityScoreArray asa, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            if (f.PrepareCountAdditionalModifier != Ability.None)
            {
                f.PrepareCount = Convert(f.PrepareCountAdditionalModifier, f.PrepareCount);
                f.PrepareCountAdditionalModifier = Ability.None;
            }
            if (f.PrepareCountPerClassLevel != 0)
            {
                f.PrepareCount = (f.PrepareCount == null || f.PrepareCount.Trim() == "0" || f.PrepareCount.Trim() == "" ? "" : f.PrepareCount + " + ") + "ClassLevel * " + f.PrepareCountPerClassLevel;
                f.PrepareCountPerClassLevel = 0;
            }
            if (f.PrepareCountAdditional != 0)
            {
                f.PrepareCount = (f.PrepareCount == null || f.PrepareCount.Trim() == "0" || f.PrepareCount.Trim() == "" ? "" : f.PrepareCount + " + ") + f.PrepareCountAdditional;
                f.PrepareCountAdditional = 0;
            }
            if (f.PrepareCount == null) f.PrepareCount = "0";
            return Evaluate(context, f.PrepareCount, asa, additionalKeywords, classlevel, level, null, f.SpellcastingID);
        }

        public static int Evaluate(BuilderContext context, HitPointsFeature f, AbilityScoreArray asa, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            if (f.HitPointsPerLevel != 0)
            {
                if (classlevel != 0) f.HitPoints = (f.HitPoints == null || f.HitPoints.Trim() == "0" || f.HitPoints.Trim() == "" ? "": f.HitPoints + " + ") + "ClassLevel * " + f.HitPointsPerLevel;
                else f.HitPoints = (f.HitPoints == null || f.HitPoints.Trim() == "0" || f.HitPoints.Trim() == "" ? "" : f.HitPoints + " + ") + "PlayerLevel * " + f.HitPointsPerLevel;
                f.HitPointsPerLevel = 0;
            }
            return Evaluate(context, f.HitPoints, asa, additionalKeywords, classlevel, level);
        }

        public static int Evaluate(BuilderContext context, String expression, AbilityScoreArray asa, List<string> additionalKeywords = null, int classlevel = 0, int level = 0, Item i = null, String SpellcastingID = null)
        {
            if (level == 0) level = context.Player.GetLevel();
            if (classlevel == 0) classlevel = level;
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression), EvaluateOptions.MatchStringsOrdinal | EvaluateOptions.MatchStringsWithIgnoreCase);
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "str" || name == "strength") args.Result = asa.Strength;
                    else if (name == "dex" || name == "dexterity") args.Result = asa.Dexterity;
                    else if (name == "con" || name == "constitution") args.Result = asa.Constitution;
                    else if (name == "int" || name == "intelligence") args.Result = asa.Intelligence;
                    else if (name == "wis" || name == "wisdom") args.Result = asa.Wisdom;
                    else if (name == "cha" || name == "charisma") args.Result = asa.Charisma;
                    else if (name == "strmod" || name == "strengthmodifier") args.Result = asa.StrMod;
                    else if (name == "dexmod" || name == "dexteritymodifier") args.Result = asa.DexMod;
                    else if (name == "conmod" || name == "constitutionmodifier") args.Result = asa.ConMod;
                    else if (name == "intmod" || name == "intelligencemodifier") args.Result = asa.IntMod;
                    else if (name == "wismod" || name == "wisdommodifier") args.Result = asa.WisMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(level);
                    else if (name == "playerlevel") args.Result = level;
                    else if (name == "race") args.Result = context.Player.RaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.RaceName.ToLowerInvariant());
                    else if (name == "subrace") args.Result = context.Player.SubRaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.SubRaceName.ToLowerInvariant());
                    else if (name == "classlevel") args.Result = classlevel;
                    else if (name == "weapon") args.Result = (i is Weapon);
                    else if (name == "armor") args.Result = (i is Armor);
                    else if (name == "shield") args.Result = (i is Shield);
                    else if (name == "baseac" && i is Armor) args.Result = ((Armor)i).BaseAC;
                    else if (name == "damageroll" && i is Weapon) args.Result = ((Weapon)i).Damage;
                    else if (name == "damagetype" && i is Weapon) args.Result = ((Weapon)i).DamageType;
                    else if (name == "tool") args.Result = i is Tool;
                    else if (name == "maxspellslot" && SpellcastingID != null) args.Result = context.Player.GetSpellSlotsMax(SpellcastingID);
                    else if (name == "source") args.Result = i?.Source?.ToLowerInvariant();
                    else if (additionalKeywords.Exists(k => MatchesKW(k, name))) args.Result = true;
                    else if (i != null && i.Keywords.Count > 0 && i.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                object o = ex.Evaluate();
                if (o is Int32) return (int)o;
                if (o is UInt32) return (int)(UInt32)o;
                if (o is UInt16) return (int)(UInt16)o;
                if (o is Int16) return (int)(Int16)o;
                if (o is UInt64) return (int)(UInt64)o;
                if (o is Int64) return (int)(Int64)o;
                if (o is Single) return (int)(Single)o;
                if (o is Double) return (int)(Double)o;
                if (o is Decimal) return (int)(Decimal)o;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + expression, e);
            }
            return 0;
        }

        

        public static bool Matches(BuilderContext context,string expression, AbilityScoreArray asa, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            if (level == 0) level = context.Player.GetLevel();
            if (classlevel == 0) classlevel = level;
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "str" || name == "strength") args.Result = asa.Strength;
                    else if (name == "dex" || name == "dexterity") args.Result = asa.Dexterity;
                    else if (name == "con" || name == "constitution") args.Result = asa.Constitution;
                    else if (name == "int" || name == "intelligence") args.Result = asa.Intelligence;
                    else if (name == "wis" || name == "wisdom") args.Result = asa.Wisdom;
                    else if (name == "cha" || name == "charisma") args.Result = asa.Charisma;
                    else if (name == "strmod" || name == "strengthmodifier") args.Result = asa.StrMod;
                    else if (name == "dexmod" || name == "dexteritymodifier") args.Result = asa.DexMod;
                    else if (name == "conmod" || name == "constitutionmodifier") args.Result = asa.ConMod;
                    else if (name == "intmod" || name == "intelligencemodifier") args.Result = asa.IntMod;
                    else if (name == "wismod" || name == "wisdommodifier") args.Result = asa.WisMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(level);
                    else if (name == "playerlevel") args.Result = level;
                    else if (name == "race") args.Result = context.Player.RaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.RaceName.ToLowerInvariant());
                    else if (name == "subrace") args.Result = context.Player.SubRaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.SubRaceName.ToLowerInvariant());
                    else if (name == "classlevel") args.Result = classlevel;
                    else if (additionalKeywords.Exists(k => MatchesKW(k, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                object o = ex.Evaluate();
                if (o is Boolean && (Boolean)o)
                    return true;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + expression, e);
            }
            return false;
        }

        public static int Evaluate(BuilderContext context, Spell s, String expression, AbilityScoreArray asa, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            if (level == 0) level = context.Player.GetLevel();
            if (classlevel == 0) classlevel = level;
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "str" || name == "strength") args.Result = asa.Strength;
                    else if (name == "dex" || name == "dexterity") args.Result = asa.Dexterity;
                    else if (name == "con" || name == "constitution") args.Result = asa.Constitution;
                    else if (name == "int" || name == "intelligence") args.Result = asa.Intelligence;
                    else if (name == "wis" || name == "wisdom") args.Result = asa.Wisdom;
                    else if (name == "cha" || name == "charisma") args.Result = asa.Charisma;
                    else if (name == "strmod" || name == "strengthmodifier") args.Result = asa.StrMod;
                    else if (name == "dexmod" || name == "dexteritymodifier") args.Result = asa.DexMod;
                    else if (name == "conmod" || name == "constitutionmodifier") args.Result = asa.ConMod;
                    else if (name == "intmod" || name == "intelligencemodifier") args.Result = asa.IntMod;
                    else if (name == "wismod" || name == "wisdommodifier") args.Result = asa.WisMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(level);
                    else if (name == "playerlevel") args.Result = level;
                    else if (name == "race") args.Result = context.Player.RaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.RaceName.ToLowerInvariant());
                    else if (name == "subrace") args.Result = context.Player.SubRaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.SubRaceName.ToLowerInvariant());
                    else if (name == "classlevel") args.Result = classlevel;
                    else if (name == "name") args.Result = s.Name.ToLowerInvariant();
                    else if (name == "spell") args.Result = true;
                    else if (name == "castingtime") args.Result = s.CastingTime;
                    else if (name == "duration") args.Result = s.Duration;
                    else if (name == "range") args.Result = s.Range;
                    else if (name == "namelower") args.Result = s.Name.ToLowerInvariant();
                    else if (name == "level") args.Result = s.Level;
                    else if (name == "classlevel") args.Result = classlevel;
                    else if (name == "classspelllevel") args.Result = (classlevel + 1) / 2;
                    else if (name == "source") args.Result = s.Source.ToLowerInvariant();
                    else if (s.Keywords.Count > 0 && s.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else if (s is ModifiedSpell && ((ModifiedSpell)s).AdditionalKeywords.Count > 0 && ((ModifiedSpell)s).AdditionalKeywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else if (additionalKeywords.Exists(k => MatchesKW(k, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                object o = ex.Evaluate();
                if (o is Int32) return (int)o;
                if (o is UInt32) return (int)(UInt32)o;
                if (o is UInt16) return (int)(UInt16)o;
                if (o is Int16) return (int)(Int16)o;
                if (o is UInt64) return (int)(UInt64)o;
                if (o is Int64) return (int)(Int64)o;
                if (o is Single) return (int)(Single)o;
                if (o is Double) return (int)(Double)o;
                if (o is Decimal) return (int)(Decimal)o;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + expression, e);
            }
            return 0;
        }

        public static bool CanMulticlass(BuilderContext context, ClassDefinition cls, AbilityScoreArray asa, List<string> additionalKeywords = null)
        {
            if (cls.MulticlassingCondition == null || cls.MulticlassingCondition.Length == 0)
            {
                List<string> cond = new List<string>();
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Strength)) cond.Add("Strength >= " + context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Dexterity)) cond.Add("Dexterity >= " + context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Constitution)) cond.Add("Constitution >= " + context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Intelligence)) cond.Add("Intelligence >= " + context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Wisdom)) cond.Add("Wisdom >= " + context.Config.MultiClassTarget);
                if (cls.MulticlassingAbilityScores.HasFlag(Ability.Charisma)) cond.Add("Charisma >= " + context.Config.MultiClassTarget);
                if (cond.Count > 0) cls.MulticlassingCondition = String.Join(" and ", cond);
                else cls.MulticlassingCondition = "true";
            }
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(cls.MulticlassingCondition), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "str" || name == "strength") args.Result = asa.Strength;
                    else if (name == "dex" || name == "dexterity") args.Result = asa.Dexterity;
                    else if (name == "con" || name == "constitution") args.Result = asa.Constitution;
                    else if (name == "int" || name == "intelligence") args.Result = asa.Intelligence;
                    else if (name == "wis" || name == "wisdom") args.Result = asa.Wisdom;
                    else if (name == "cha" || name == "charisma") args.Result = asa.Charisma;
                    else if (name == "strmod" || name == "strengthmodifier") args.Result = asa.StrMod;
                    else if (name == "dexmod" || name == "dexteritymodifier") args.Result = asa.DexMod;
                    else if (name == "conmod" || name == "constitutionmodifier") args.Result = asa.ConMod;
                    else if (name == "intmod" || name == "intelligencemodifier") args.Result = asa.IntMod;
                    else if (name == "wismod" || name == "wisdommodifier") args.Result = asa.WisMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(context.Player.GetLevel());
                    else if (name == "playerlevel") args.Result = context.Player.GetLevel();
                    else if (name == "race") args.Result = context.Player.RaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.RaceName.ToLowerInvariant());
                    else if (name == "subrace") args.Result = context.Player.SubRaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.SubRaceName.ToLowerInvariant());
                    else if (name == "classlevel") args.Result = context.Player.GetLevel();
                    else if (additionalKeywords.Exists(k => MatchesKW(k, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                object o = ex.Evaluate();
                if (o is Boolean && (Boolean)o)
                    return true;
                return false;

            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + cls?.MulticlassingCondition, e);
            }
            return false;
        }

        public static int CalcAC(BuilderContext context, ACFeature acf, Item armor, int shieldbonus, List<string> additionalKeywords, AbilityScoreArray asa, int otherbonus, int classlevel, bool ignoreItemClass = false)
        {
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(acf.Expression), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "category") args.Result = armor.Category.Path;
                    else if (name == "weapon") args.Result = (!ignoreItemClass && armor is Weapon); //stupid!
                    else if (name == "armor") args.Result = (!ignoreItemClass && armor is Armor);
                    else if (name == "shield") args.Result = (!ignoreItemClass && armor is Shield); //equally stupidendus
                    else if (name == "baseac" && armor is Armor) args.Result = ((Armor)armor).BaseAC;
                    else if (name == "acbonus") args.Result = otherbonus;
                    else if (name == "shieldbonus") args.Result = shieldbonus;
                    else if (name == "damageroll" && armor is Weapon) args.Result = ((Weapon)armor).Damage; //I don't even
                    else if (name == "damagetype" && armor is Weapon) args.Result = ((Weapon)armor).DamageType; //THIS WILL NEVER HAPPEN
                    else if (name == "tool") args.Result = (!ignoreItemClass && armor is Tool);
                    else if (name == "name") args.Result = acf.Name.ToLowerInvariant();
                    else if (name == "str" || name == "strength") args.Result = asa.Strength;
                    else if (name == "dex" || name == "dexterity") args.Result = asa.Dexterity;
                    else if (name == "con" || name == "constitution") args.Result = asa.Constitution;
                    else if (name == "int" || name == "intelligence") args.Result = asa.Intelligence;
                    else if (name == "wis" || name == "wisdom") args.Result = asa.Wisdom;
                    else if (name == "cha" || name == "charisma") args.Result = asa.Charisma;
                    else if (name == "strmod" || name == "strengthmodifier") args.Result = asa.StrMod;
                    else if (name == "dexmod" || name == "dexteritymodifier") args.Result = asa.DexMod;
                    else if (name == "conmod" || name == "constitutionmodifier") args.Result = asa.ConMod;
                    else if (name == "intmod" || name == "intelligencemodifier") args.Result = asa.IntMod;
                    else if (name == "wismod" || name == "wisdommodifier") args.Result = asa.WisMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(context.Player.GetLevel());
                    else if (name == "playerlevel") args.Result = context.Player.GetLevel();
                    else if (name == "race") args.Result = context.Player.RaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.RaceName.ToLowerInvariant());
                    else if (name == "subrace") args.Result = context.Player.SubRaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.SubRaceName.ToLowerInvariant());
                    else if (name == "classlevel") args.Result = context.Player.GetLevel();
                    else if (additionalKeywords.Exists(k => MatchesKW(k, name))) args.Result = true;
                    else if (armor.Keywords.Count > 0 && armor.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                object o = ex.Evaluate();
                if (o is int) return (int)o;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + acf?.Expression, e);
            }
            return -1;
        }
        public static bool Matches(BuilderContext context,BonusFeature bf, Item w, int classlevel, List<string> additionalKeywords = null, AbilityScoreArray asa = null, bool ignoreItemClass = false)
        {
            if (bf == null) return false;
            return Matches(context, w, bf.Condition, classlevel, additionalKeywords, asa, ignoreItemClass);
            /*            List<Keyword> kws = new List<Keyword>(Keywords);
                        kws.RemoveAll(k=>k.Name=="weapon"); //If it doesn't contain the weapon or the spell kw, it will match both spell attacks and weapon attacks
                        foreach (Keyword kw in kws) if (!w.Keywords.Contains(kw)) return false;
                        return true;*/

        }
        public static bool Matches(BuilderContext context,BonusFeature bf, Spell s, int classlevel, List<string> additionalKeywords = null)
        {
            if (bf == null) return false;
            return Matches(context, s, bf.Condition, null, additionalKeywords, classlevel);
            /*           List<Keyword> kws = new List<Keyword>(Keywords);
                       kws.RemoveAll(k => k.Name == "spell");
                       foreach (Keyword kw in kws) if (!s.Keywords.Contains(kw)) return false;
                       return true;*/
        }

        public static bool Matches(BuilderContext context, BonusFeature bf, Ability baseAbility, string SpellcastingID, string kw, int classlevel, List<string> additionalKeywords = null)
        {

            if (additionalKeywords == null) additionalKeywords = new List<string>();
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(bf.Condition), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "name") args.Result = "";
                    else if (name == "category") args.Result = "";
                    else if (name == "level") args.Result = context.Player.GetLevel();
                    else if (name == "playerlevel") args.Result = context.Player.GetLevel();
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(context.Player.GetLevel());
                    else if (name == "classlevel") args.Result = classlevel;
                    else if (name == "classspelllevel") args.Result = (classlevel + 1) / 2;
                    else if (name == "race") args.Result = context.Player.RaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.RaceName.ToLowerInvariant());
                    else if (name == "subrace") args.Result = context.Player.SubRaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.SubRaceName.ToLowerInvariant());
                    else if (name == "str" || name == "strength") args.Result = baseAbility.HasFlag(Ability.Strength);
                    else if (name == "dex" || name == "dexterity") args.Result = baseAbility.HasFlag(Ability.Dexterity);
                    else if (name == "con" || name == "constitution") args.Result = baseAbility.HasFlag(Ability.Constitution);
                    else if (name == "int" || name == "intelligence") args.Result = baseAbility.HasFlag(Ability.Intelligence);
                    else if (name == "wis" || name == "wisdom") args.Result = baseAbility.HasFlag(Ability.Wisdom);
                    else if (name == "cha" || name == "charisma") args.Result = baseAbility.HasFlag(Ability.Charisma);
                    else if (name == SpellcastingID.ToLowerInvariant()) args.Result = true;
                    else if (name == "maxspellslot" && SpellcastingID != null) args.Result = context.Player.GetSpellSlotsMax(SpellcastingID);
                    else if (name == kw.ToLowerInvariant()) args.Result = true;
                    else if (additionalKeywords.Exists(s => MatchesKW(name, s))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                object o = ex.Evaluate();
                if (o is Boolean && (Boolean)o) return true;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + bf.Condition, e);
            }
            return false;
        }

        public static List<ModifiedSpell> GetSpells(BuilderContext context, BonusSpellKeywordChoiceFeature f)
        {
            List<ModifiedSpell> res = new List<ModifiedSpell>();
            int offset = context.Player.GetChoiceOffset(f, f.UniqueID, f.Amount);
            for (int c = 0; c < f.Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = context.Player.GetChoice(f.UniqueID + counter);
                if (cho != null && cho.Value != "") res.Add(new ModifiedSpell(context.GetSpell(cho.Value, f.Source), f.KeywordsToAdd, f.SpellCastingAbility, f.SpellCastModifier));
            }
            return res;
        }

        public static int AvailableToPrepare(BuilderContext context, SpellcastingFeature f, int classlevel)
        {
            //return Math.Max(1, context.Player.getFinalAbilityScores().ApplyMod(f.PrepareCountAdditionalModifier) + f.PrepareCountAdditional + (int)Math.Floor(f.PrepareCountPerClassLevel * classlevel));
            return Evaluate(context, f, context.Player.GetFinalAbilityScores(), null, classlevel);
        }

        public static bool MatchesKW(string kw, string kw2)
        {
            return kw.Replace('-', '_').Equals(kw2.Replace('-', '_'), StringComparison.OrdinalIgnoreCase);
        }

        public static bool Matches(BuilderContext context,Item i, string expression, int classlevel, List<string> additionalKeywords = null, AbilityScoreArray asa = null, bool ignoreItemClass = false)
        {
            if (asa == null) asa = new AbilityScoreArray(10, 10, 10, 10, 10, 10);
            if (additionalKeywords == null) additionalKeywords = new List<string>();
            if (i != null && i.CachedMatches == null) i.CachedMatches = new Dictionary<string, bool>();
            if (i != null && i.CachedMatches.ContainsKey(expression + additionalKeywords.ToString())) return i.CachedMatches[expression + additionalKeywords.ToString()];
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "category") args.Result = i != null ? i.Category.Path : "";
                    else if (name == "weapon") args.Result = (!ignoreItemClass && i is Weapon);
                    else if (name == "armor") args.Result = (!ignoreItemClass && i is Armor);
                    else if (name == "shield") args.Result = (!ignoreItemClass && i is Shield);
                    else if (name == "tool") args.Result = (!ignoreItemClass && i is Tool);
                    else if (name == "baseac" && i is Armor) args.Result = ((Armor)i).BaseAC;
                    else if (name == "acbonus" && i is Shield) args.Result = ((Shield)i).ACBonus;
                    else if (name == "damageroll" && i is Weapon) args.Result = ((Weapon)i).Damage;
                    else if (name == "damagetype" && i is Weapon) args.Result = ((Weapon)i).DamageType;
                    else if (name == "str" || name == "strength") args.Result = asa.Strength;
                    else if (name == "dex" || name == "dexterity") args.Result = asa.Dexterity;
                    else if (name == "con" || name == "constitution") args.Result = asa.Constitution;
                    else if (name == "int" || name == "intelligence") args.Result = asa.Intelligence;
                    else if (name == "wis" || name == "wisdom") args.Result = asa.Wisdom;
                    else if (name == "cha" || name == "charisma") args.Result = asa.Charisma;
                    else if (name == "strmod" || name == "strengthmodifier") args.Result = asa.StrMod;
                    else if (name == "dexmod" || name == "dexteritymodifier") args.Result = asa.DexMod;
                    else if (name == "conmod" || name == "constitutionmodifier") args.Result = asa.ConMod;
                    else if (name == "intmod" || name == "intelligencemodifier") args.Result = asa.IntMod;
                    else if (name == "wismod" || name == "wisdommodifier") args.Result = asa.WisMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "autogenerated" && i is Item) args.Result = i.autogenerated;
                    else if (name == "name") args.Result = i != null ? i.Name.ToLowerInvariant() : "";
                    else if (name == "playerlevel") args.Result = context.Player.GetLevel();
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(context.Player.GetLevel());
                    else if (name == "race") args.Result = context.Player.RaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.RaceName.ToLowerInvariant());
                    else if (name == "subrace") args.Result = context.Player.SubRaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.SubRaceName.ToLowerInvariant());
                    else if (name == "classlevel") args.Result = classlevel;
                    else if (name == "source") args.Result = i?.Source?.ToLowerInvariant();
                    else if (additionalKeywords.Exists(s => MatchesKW(name, s))) args.Result = true;
                    else if (i != null && i.Keywords != null && i.Keywords.Count > 0 && i.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                object o = ex.Evaluate();
                if (o is Boolean && (Boolean)o)
                {
                    if (i != null) i.CachedMatches[expression + additionalKeywords.ToString()] = true;
                    return true;
                }
                else if (i != null) i.CachedMatches[expression + additionalKeywords.ToString()] = false;
                return false;
                 
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + expression, e);
            }
            return false;
        }
        public static List<Item> Filter(BuilderContext context, string expression)
        {
            if (expression == null || expression == "") expression = "true";
            if (context.ItemLists.ContainsKey(expression)) return new List<Item>(context.ItemLists[expression]);
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                Item current = null;
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "category") args.Result = current.Category.Path;
                    else if (name == "weapon") args.Result = (current is Weapon);
                    else if (name == "armor") args.Result = (current is Armor);
                    else if (name == "shield") args.Result = (current is Shield);
                    else if (name == "tool") args.Result = (current is Tool);
                    else if (name == "name") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "source") args.Result = current.Source.ToLowerInvariant();
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                List<Item> res = new List<Item>();
                foreach (Item f in context.Items.Values)
                {
                    current = f;
                    object o = ex.Evaluate();
                    if (o is Boolean && (Boolean)o) res.Add(current);

                }
                res.Sort();
                context.ItemLists[expression] = res;
                return res;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + expression, e);
            }
            return new List<Item>();
        }
        public static bool Fits(BuilderContext context, MagicProperty mp, Item item)
        {
            return Matches(context, item, mp.Base, 0);
        }
        public static bool Matches(BuilderContext context,ToolKWProficiencyFeature f, Item tool, int classlevel)
        {
            return Matches(context, tool, f.Condition, classlevel);
        }
        public static List<Spell> FilterSpell(BuilderContext context, string expression, String SpellcastingID, int classlevel = 0)
        {
            if (expression == null || expression == "") expression = "true";
            //if (Spell.SpellLists.ContainsKey(expression + "." + classlevel)) return new List<Spell>(Spell.SpellLists[expression + "." + classlevel]);
            try
            {
                bool caching = true;
                Expression ex = new Expression(ConfigManager.FixQuotes(expression), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                Spell current = null;
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "classlevel") args.Result = classlevel;
                    else if (name == "classspelllevel")
                        args.Result = (classlevel + 1) / 2;
                    else if (name == "maxspellslot" && SpellcastingID != null) args.Result = context.Player.GetSpellSlotsMax(SpellcastingID);
                    else if (name == "name") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "namelower") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "level") args.Result = current.Level;
                    else if (name == "source") args.Result = current.Source.ToLowerInvariant();
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                List<Spell> res = new List<Spell>();
                foreach (Spell f in context.Spells.Values)
                {
                    current = f;
                    object o = ex.Evaluate();
                    if (o is Boolean && (Boolean)o) res.Add(current);

                }
                res.Sort();
                if (caching) context.SpellLists[expression + "." + classlevel] = res;
                return res;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + expression, e);
            }
            return new List<Spell>();
        }
        public static bool Matches(BuilderContext context,Spell s, string expression, string SpellcastingID, List<string> additionalKeywords = null, int classlevel = 0)
        {
            if (expression == null || expression == "") expression = "true";
            if (additionalKeywords == null) additionalKeywords = new List<string>();
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "name") args.Result = s.Name.ToLowerInvariant();
                    else if (name == "spell") args.Result = true;
                    else if (name == "castingtime") args.Result = s.CastingTime;
                    else if (name == "duration") args.Result = s.Duration;
                    else if (name == "range") args.Result = s.Range;
                    else if (name == "namelower") args.Result = s.Name.ToLowerInvariant();
                    else if (name == "level") args.Result = s.Level;
                    else if (name == "classlevel") args.Result = classlevel;
                    else if (name == "classspelllevel") args.Result = (classlevel + 1) / 2;
                    else if (name == "maxspellslot" && SpellcastingID != null) args.Result = context.Player.GetSpellSlotsMax(SpellcastingID);
                    else if (name == "playerlevel") args.Result = context.Player.GetLevel();
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(context.Player.GetLevel());
                    else if (name == "race") args.Result = context.Player.RaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.RaceName.ToLowerInvariant());
                    else if (name == "subrace") args.Result = context.Player.SubRaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.SubRaceName.ToLowerInvariant());
                    else if (s.Keywords.Count > 0 && s.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else if (name == "source") args.Result = s.Source.ToLowerInvariant();
                    else if (s is ModifiedSpell && ((ModifiedSpell)s).AdditionalKeywords.Count > 0 && ((ModifiedSpell)s).AdditionalKeywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else if (additionalKeywords.Exists(k => MatchesKW(k, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                object o = ex.Evaluate();
                if (o is Boolean && (Boolean)o) return true;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + expression, e);
            }
            return false;
        }
        public static String GetPointsRemaining(Player p, OGLContext context)
        {
            int have = context.Scores.GetPointBuyPoints();
            foreach (int score in new List<int>() { p.BaseStrength, p.BaseDexterity, p.BaseConstitution, p.BaseIntelligence, p.BaseWisdom, p.BaseCharisma })
            {
                int points = context.Scores.GetPointBuyCost(score);
                if (points < 0) return "--";
                have -= points;
            }
            return have.ToString();
        }

        public static List<Monster> FilterMonsters(string expression, BuilderContext context, AbilityScoreArray asa, int classlevel)
        {
            if (expression == null || expression == "") expression = "true";
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                Monster current = null;
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "level" || name == "playerlevel") args.Result = context.Player.GetLevel();
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(context.Player.GetLevel());
                    else if (name == "classlevel") args.Result = classlevel;
                    else if (name == "classspelllevel") args.Result = (classlevel + 1) / 2;
                    else if (name == "maxspellslot") args.Result = context.Player.GetSpellSlotsMax();
                    else if (name == "str" || name == "strength") args.Result = asa.Strength;
                    else if (name == "dex" || name == "dexterity") args.Result = asa.Dexterity;
                    else if (name == "con" || name == "constitution") args.Result = asa.Constitution;
                    else if (name == "int" || name == "intelligence") args.Result = asa.Intelligence;
                    else if (name == "wis" || name == "wisdom") args.Result = asa.Wisdom;
                    else if (name == "cha" || name == "charisma") args.Result = asa.Charisma;
                    else if (name == "strmod" || name == "strengthmodifier") args.Result = asa.StrMod;
                    else if (name == "dexmod" || name == "dexteritymodifier") args.Result = asa.DexMod;
                    else if (name == "conmod" || name == "constitutionmodifier") args.Result = asa.ConMod;
                    else if (name == "intmod" || name == "intelligencemodifier") args.Result = asa.IntMod;
                    else if (name == "wismod" || name == "wisdommodifier") args.Result = asa.WisMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "name") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "source") args.Result = current.Source.ToLowerInvariant();
                    else if (name == "namelower") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "cr") args.Result = current.CR;
                    else if (name == "monsterstrength" || name == "monsterstr") args.Result = current.Strength;
                    else if (name == "monsterdexterity" || name == "monsterdex") args.Result = current.Dexterity;
                    else if (name == "monsterconstitution" || name == "monstercon") args.Result = current.Constitution;
                    else if (name == "monsterintelligence" || name == "monsterint") args.Result = current.Intelligence;
                    else if (name == "monsterwisdom" || name == "monsterwis") args.Result = current.Wisdom;
                    else if (name == "monstercharisma" || name == "monstercha") args.Result = current.Charisma;
                    else if (name == "passiveperception") args.Result = current.PassivePerception;
                    else if (name == "xp") args.Result = current.XP;
                    else if (name == "spells") args.Result = current.Spells?.Count ?? 0;
                    else if (name == "slots") args.Result = current.Slots?.Count ?? 0;
                    else if (name == "ac") args.Result = current.AC;
                    else if (name == "actext") args.Result = current.ACText;
                    else if (name == "hp") args.Result = current.HP;
                    else if (name == "hproll") args.Result = current.HPRoll;
                    else if (name == "alignment") args.Result = current.Alignment?.ToLowerInvariant() ?? "";
                    else if (name == "flying") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("fly", StringComparison.OrdinalIgnoreCase));
                    else if (name == "swimming") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("swim", StringComparison.OrdinalIgnoreCase));
                    else if (name == "climbing") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("climb", StringComparison.OrdinalIgnoreCase));
                    else if (name == "burrowing") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("burrow", StringComparison.OrdinalIgnoreCase));
                    else if (name == "darkvision") args.Result = current.Senses.Exists(s => s.TrimStart().StartsWith("darkvision", StringComparison.OrdinalIgnoreCase));
                    else if (name == "blindsight") args.Result = current.Senses.Exists(s => s.TrimStart().StartsWith("blindsight", StringComparison.OrdinalIgnoreCase));
                    else if (name == "tremorsense") args.Result = current.Senses.Exists(s => s.TrimStart().StartsWith("tremorsense", StringComparison.OrdinalIgnoreCase));
                    else if (name == "truesight") args.Result = current.Senses.Exists(s => s.TrimStart().StartsWith("truesight", StringComparison.OrdinalIgnoreCase));
                    else if (name == "hover") args.Result = current.Speeds.Exists(s => s.ToLowerInvariant().Contains("hover"));
                    else if (name == "teleporting") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("teleport", StringComparison.OrdinalIgnoreCase));
                    else if (name == "size") args.Result = current.Size.ToString() ?? "";
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                List<Monster> res = new List<Monster>();
                foreach (Monster f in context.Monsters.Values)
                {
                    current = f;
                    object o = ex.Evaluate();
                    if (o is Boolean && (Boolean)o) res.Add(current);

                }
                res.Sort();
                return res;
            }
            catch (Exception e)
            {
                throw new Exception("Error while evaluating expression " + expression, e);
            }
        }

        public static int EvaluateMonster(BuilderContext context, Monster current, String expression, AbilityScoreArray asa, int value, string valname, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            if (level == 0) level = context.Player.GetLevel();
            if (classlevel == 0) classlevel = level;
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression), EvaluateOptions.MatchStringsWithIgnoreCase | EvaluateOptions.MatchStringsOrdinal);
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "value") args.Result = value;
                    else if (name == "valuename" || name == "skill" || name == "Save") args.Result = valname;
                    else if (name == "str" || name == "strength") args.Result = asa.Strength;
                    else if (name == "dex" || name == "dexterity") args.Result = asa.Dexterity;
                    else if (name == "con" || name == "constitution") args.Result = asa.Constitution;
                    else if (name == "int" || name == "intelligence") args.Result = asa.Intelligence;
                    else if (name == "wis" || name == "wisdom") args.Result = asa.Wisdom;
                    else if (name == "cha" || name == "charisma") args.Result = asa.Charisma;
                    else if (name == "strmod" || name == "strengthmodifier") args.Result = asa.StrMod;
                    else if (name == "dexmod" || name == "dexteritymodifier") args.Result = asa.DexMod;
                    else if (name == "conmod" || name == "constitutionmodifier") args.Result = asa.ConMod;
                    else if (name == "intmod" || name == "intelligencemodifier") args.Result = asa.IntMod;
                    else if (name == "wismod" || name == "wisdommodifier") args.Result = asa.WisMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "chamod" || name == "charismamodifier") args.Result = asa.ChaMod;
                    else if (name == "proficiencybonus") args.Result = context.Player.GetProficiency(level);
                    else if (name == "level" || name == "playerlevel") args.Result = level;
                    else if (name == "maxspellslot") args.Result = context.Player.GetSpellSlotsMax();
                    else if (name == "race") args.Result = context.Player.RaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.RaceName.ToLowerInvariant());
                    else if (name == "subrace") args.Result = context.Player.SubRaceName == null ? "" : SourceInvariantComparer.NoSource(context.Player.SubRaceName.ToLowerInvariant());
                    else if (name == "classlevel") args.Result = classlevel;
                    else if (name == "classspelllevel") args.Result = (classlevel + 1) / 2;
                    else if (name == "name") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "source") args.Result = current.Source.ToLowerInvariant();
                    else if (name == "namelower") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "cr") args.Result = current.CR;
                    else if (name == "monsterstrength" || name == "str") args.Result = current.Strength;
                    else if (name == "monsterdexterity" || name == "dex") args.Result = current.Dexterity;
                    else if (name == "monsterconstitution" || name == "con") args.Result = current.Constitution;
                    else if (name == "monsterintelligence" || name == "int") args.Result = current.Intelligence;
                    else if (name == "monsterwisdom" || name == "wis") args.Result = current.Wisdom;
                    else if (name == "monstercharisma" || name == "cha") args.Result = current.Charisma;
                    else if (name == "passiveperception") args.Result = current.PassivePerception;
                    else if (name == "xp") args.Result = current.XP;
                    else if (name == "spells") args.Result = current.Spells?.Count ?? 0;
                    else if (name == "slots") args.Result = current.Slots?.Count ?? 0;
                    else if (name == "ac") args.Result = current.AC;
                    else if (name == "actext") args.Result = current.ACText;
                    else if (name == "hp") args.Result = current.HP;
                    else if (name == "hproll") args.Result = current.HPRoll;
                    else if (name == "alignment") args.Result = current.Alignment?.ToLowerInvariant() ?? "";
                    else if (name == "flying") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("fly", StringComparison.OrdinalIgnoreCase));
                    else if (name == "swimming") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("swim", StringComparison.OrdinalIgnoreCase));
                    else if (name == "climbing") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("climb", StringComparison.OrdinalIgnoreCase));
                    else if (name == "burrowing") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("burrow", StringComparison.OrdinalIgnoreCase));
                    else if (name == "darkvision") args.Result = current.Senses.Exists(s => s.TrimStart().StartsWith("darkvision", StringComparison.OrdinalIgnoreCase));
                    else if (name == "blindsight") args.Result = current.Senses.Exists(s => s.TrimStart().StartsWith("blindsight", StringComparison.OrdinalIgnoreCase));
                    else if (name == "tremorsense") args.Result = current.Senses.Exists(s => s.TrimStart().StartsWith("tremorsense", StringComparison.OrdinalIgnoreCase));
                    else if (name == "truesight") args.Result = current.Senses.Exists(s => s.TrimStart().StartsWith("truesight", StringComparison.OrdinalIgnoreCase));
                    else if (name == "hover") args.Result = current.Speeds.Exists(s => s.ToLowerInvariant().Contains("hover"));
                    else if (name == "teleporting") args.Result = current.Speeds.Exists(s => s.TrimStart().StartsWith("teleport", StringComparison.OrdinalIgnoreCase));
                    else if (name == "size") args.Result = current.Size.ToString() ?? "";
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else if (additionalKeywords.Exists(k => MatchesKW(k, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += MakeFunctionExtensions(context);
                object o = ex.Evaluate();
                if (o is Int32) return (int)o;
                if (o is UInt32) return (int)(UInt32)o;
                if (o is UInt16) return (int)(UInt16)o;
                if (o is Int16) return (int)(Int16)o;
                if (o is UInt64) return (int)(UInt64)o;
                if (o is Int64) return (int)(Int64)o;
                if (o is Single) return (int)(Single)o;
                if (o is Double) return (int)(Double)o;
                if (o is Decimal) return (int)(Decimal)o;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + expression, e);
            }
            return 0;
        }
    }
}
