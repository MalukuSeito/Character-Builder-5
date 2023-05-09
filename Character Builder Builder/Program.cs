using Character_Builder_Forms;
using OGL;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Character_Builder_Builder
{
    public static class Program
    {
        public static ErrorLog Errorlog = null;

        public static OGLContext Context = new OGLContext();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //String s = "Name = \"hallo'welt\" or name = \"Foobar\"";
            //String Name = "Hallo'Welt";
            //var e = new Expression(ConfigManager.fixQuotes(s));
            //e.EvaluateParameter += (name, args) => args.Result = (name.ToLowerInvariant() == "name") ? Name.ToLowerInvariant() : "";
            //Console.WriteLine(s + " = " + e.Evaluate());
            AppContext.SetSwitch("Switch.System.Xml.AllowDefaultResolver", true);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigManager.LogEvents += (sender, text, e) => Console.WriteLine((text != null ? text + ": " : "") + e?.StackTrace);
            ConfigManager.LicenseProvider = new LicenseProvider();
            Errorlog = new ErrorLog();
            ConfigManager.AlwaysShowSource = true;
            Context.LoadConfig(Application.StartupPath);
            if (SourceManager.Init(Program.Context, Application.StartupPath, true))
            {
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length > 1)
                {
                    Dictionary<string, OGL.Base.Ability> skills = new Dictionary<string, OGL.Base.Ability>();
                    Dictionary<string, OGL.Base.Ability> abilities = new Dictionary<string, OGL.Base.Ability>();
                    abilities.Add("str", OGL.Base.Ability.Strength);
                    abilities.Add("strength", OGL.Base.Ability.Strength);
                    abilities.Add("dex", OGL.Base.Ability.Dexterity);
                    abilities.Add("dexterity", OGL.Base.Ability.Dexterity);
                    abilities.Add("con", OGL.Base.Ability.Constitution);
                    abilities.Add("constitution", OGL.Base.Ability.Constitution);
                    abilities.Add("int", OGL.Base.Ability.Intelligence);
                    abilities.Add("intelligence", OGL.Base.Ability.Intelligence);
                    abilities.Add("wis", OGL.Base.Ability.Wisdom);
                    abilities.Add("wisdom", OGL.Base.Ability.Wisdom);
                    abilities.Add("cha", OGL.Base.Ability.Charisma);
                    abilities.Add("charisma", OGL.Base.Ability.Charisma);
                    Context.ImportSkills();
                    foreach (OGL.Skill s in Context.SkillsSimple.Values)
                    {
                        skills.Add(s.Name.ToLowerInvariant(), s.Base);
                    }
                    Dictionary<decimal, int> crs = new Dictionary<decimal, int>();
                    crs.Add(0, 10);
                    crs.Add(0.125m, 25);
                    crs.Add(0.25m, 50);
                    crs.Add(0.5m, 100);
                    crs.Add(1, 200);
                    crs.Add(2, 450);
                    crs.Add(3, 700);
                    crs.Add(4, 1100);
                    crs.Add(5, 1800);
                    crs.Add(6, 2300);
                    crs.Add(7, 2900);
                    crs.Add(8, 3900);
                    crs.Add(9, 5000);
                    crs.Add(10, 5900);
                    crs.Add(11, 7200);
                    crs.Add(12, 8400);
                    crs.Add(13, 10000);
                    crs.Add(14, 11500);
                    crs.Add(15, 13000);
                    crs.Add(16, 15000);
                    crs.Add(17, 18000);
                    crs.Add(18, 20000);
                    crs.Add(19, 22000);
                    crs.Add(20, 25000);
                    crs.Add(21, 33000);
                    crs.Add(22, 41000);
                    crs.Add(23, 50000);
                    crs.Add(24, 62000);
                    crs.Add(25, 75000);
                    crs.Add(26, 90000);
                    crs.Add(27, 105000);
                    crs.Add(28, 120000);
                    crs.Add(29, 135000);
                    crs.Add(30, 155000);
                    string file = args[1];
                    if (File.Exists(file) && ".json".Equals(Path.GetExtension(file), StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (StreamReader sr = new StreamReader(file))
                        {
                            JObject input = JObject.Load(new JsonTextReader(sr));
                            var monsters = input["monster"];
                            foreach (var monster in monsters)
                            {
                                Console.WriteLine(monster["name"].ToString());
                                Monster m = new Monster();
                                List<string> monsterSkills = new List<string>();
                                List<string> monsterSaves = new List<string>();
                                int passive = 10;
                                foreach (var e in monster)
                                {
                                    if (e is JProperty prop) {
                                        if (prop.Value.ToString() == "[]") continue;
                                        switch (prop.Name.ToLowerInvariant())
                                        {
                                            case "name":
                                                m.Name = prop.Value.ToString();
                                                break;
                                            case "size":
                                                switch (prop.Value.ToString().ToLowerInvariant())
                                                {
                                                    case "t":
                                                        m.Size = OGL.Base.Size.Tiny;
                                                        break;
                                                    case "m":
                                                        m.Size = OGL.Base.Size.Medium;
                                                        break;
                                                    case "s":
                                                        m.Size = OGL.Base.Size.Small;
                                                        break;
                                                    case "l":
                                                        m.Size = OGL.Base.Size.Large;
                                                        break;
                                                    case "g":
                                                        m.Size = OGL.Base.Size.Gargantuan;
                                                        break;
                                                    case "h":
                                                        m.Size = OGL.Base.Size.Huge;
                                                        break;
                                                    default:
                                                        throw new Exception("Unexpected " + prop + " in " + m);
                                                }
                                                break;
                                            case "type":
                                                string type = prop.Value.ToString();
                                                string[] types = prop.Value is JArray ? prop.Values().Select(val => val.ToString()).ToArray() : prop.Value.ToString().Split(',');
                                                if (types.Length == 1)
                                                {
                                                    types = new string[] { types[0], "Unknown Source" };
                                                }
                                                string[] firsttypes = types[0].Split('(');
                                                for (int i = 0; i < firsttypes.Length; i++) m.Keywords.Add(new OGL.Keywords.Keyword(i == firsttypes.Length - 1 ? firsttypes[i].Trim().TrimEnd(')') : firsttypes[i].Trim()));
                                                for (int i = 1; i < types.Length - 1; i++) m.Keywords.Add(new OGL.Keywords.Keyword(i == types.Length - 2 ? types[i].Trim().TrimEnd(')') : types[i].Trim()));
                                                string source = types.Last();
                                                switch (source.ToLowerInvariant().Trim()) {
                                                    case "monster manual":
                                                        m.Source = "Monster Manual";
                                                        break;
                                                    case "tome of beasts":
                                                        m.Source = "Tome of Beasts";
                                                        break;
                                                    case "volo's guide":
                                                        m.Source = "Volo's Guide to Monsters";
                                                        break;
                                                    case "elemental evil":
                                                        m.Source = "Elemental Evil";
                                                        break;
                                                    case "tyranny of dragons":
                                                        m.Source = "Tyranny of Dragons";
                                                        break;
                                                    case "storm kings thunder":
                                                        m.Source = "Storm King's Thunder";
                                                        break;
                                                    case "unknown source":
                                                        m.Source = Path.GetFileNameWithoutExtension(file);
                                                        break;
                                                    case "out of the abyss":
                                                        m.Source = "Out of the Abyss";
                                                        break;
                                                    case "curse of strahd":
                                                        m.Source = "Curse of Strahd";
                                                        break;
                                                    case "lost mine of phandelver":
                                                        m.Source = "Lost Mine of Phandelver";
                                                        break;
                                                    default:
                                                        m.Source = source;
                                                        break;
                                                }
                                                break;
                                            case "alignment":
                                                m.Alignment = prop.Value.ToString();
                                                break;
                                            case "ac":
                                                string[] ac = prop.Value.ToString().Split(" (".ToCharArray(), 2);
                                                if (TryParse(ac[0], out int acvalue)) m.AC = acvalue;
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                if (ac.Length > 1) m.ACText = ac[1].TrimStart('(').TrimEnd(')');
                                                break;
                                            case "hp":
                                                string[] hp = prop.Value.ToString().Split(" (".ToCharArray(), 2);
                                                if (TryParse(hp[0], out int hpvalue)) m.HP = hpvalue;
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                if (hp.Length > 1) m.HPRoll = hp[1].TrimStart('(').TrimEnd(')');
                                                break;
                                            case "speed":
                                                string[] speeds = prop.Value.ToString().Split(',');
                                                foreach (string speed in speeds) m.Speeds.Add(speed.Trim());
                                                break;
                                            case "str":
                                                if (TryParse(prop.Value.ToString(), out int str)) m.Strength = str;
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                break;
                                            case "dex":
                                                if (TryParse(prop.Value.ToString(), out int dex)) m.Dexterity = dex;
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                break;
                                            case "con":
                                                if (TryParse(prop.Value.ToString(), out int con)) m.Constitution = con;
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                break;
                                            case "int":
                                                if (TryParse(prop.Value.ToString(), out int intt)) m.Intelligence = intt;
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                break;
                                            case "wis":
                                                if (TryParse(prop.Value.ToString(), out int wis)) m.Wisdom = wis;
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                break;
                                            case "cha":
                                                if (TryParse(prop.Value.ToString(), out int cha)) m.Charisma = cha;
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                break;
                                            case "skill":
                                                string[] skillss = prop.Value.ToString().Split(',');
                                                foreach (string skill in skillss) monsterSkills.Add(skill.Trim());
                                                break;
                                            case "passive":
                                                if (TryParse(prop.Value.ToString().ToLowerInvariant().Replace("passive perception", "").Trim(), out passive));
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                break;
                                            case "languages":
                                                string[] languages = prop.Value.ToString().Split(',');
                                                foreach (string lang in languages) m.Languages.Add(lang.Trim());
                                                break;
                                            case "cr":
                                                string cr = prop.Value.ToString();
                                                if ("1/8".Equals(cr)) m.CR = 0.125m;
                                                else if ("1/4".Equals(cr)) m.CR = 0.25m;
                                                else if ("1/2".Equals(cr)) m.CR = 0.5m;
                                                else if ("I".Equals(cr)) m.CR = 1m;
                                                else if ("l".Equals(cr)) m.CR = 1m;
                                                else if (decimal.TryParse(prop.Value.ToString(), out decimal crvalue)) m.CR = crvalue;
                                                else throw new Exception("Unexpected " + prop + " in " + m);
                                                m.XP = crs[m.CR];
                                                break;
                                            case "trait":
                                                if (prop.Value is JArray traits)
                                                {
                                                    foreach (var trait in traits)
                                                    {
                                                        String text;
                                                        if (trait["text"] is JArray a) text = string.Join("\n", a.Select(s => s.ToString()));
                                                        else text = trait["text"]?.ToString();
                                                        m.Traits.Add(new OGL.Monsters.MonsterTrait(trait["name"]?.ToString(), text));
                                                    }
                                                }
                                                else
                                                {
                                                    String text;
                                                    if (prop.Value["text"] is JArray a) text = string.Join("\n", a.Select(s => s.ToString()));
                                                    else text = prop.Value["text"]?.ToString();
                                                    m.Traits.Add(new OGL.Monsters.MonsterTrait(prop.Value["name"]?.ToString(), text));
                                                }
                                                break;
                                            case "action":
                                                if (prop.Value is JArray actions)
                                                {
                                                    foreach (var trait in actions)
                                                    {
                                                        String text;
                                                        if (trait["text"] is JArray a) text = string.Join("\n", a.Select(s => s.ToString()));
                                                        else text = trait["text"]?.ToString();
                                                        if (trait["attack"] != null)
                                                        {
                                                            string[] attack = trait["attack"].ToString().Split('|');
                                                            int attackbonus = 0;
                                                            if (attack.Length > 1) TryParse(attack[1], out attackbonus);
                                                            m.Actions.Add(new OGL.Monsters.MonsterAction(trait["name"]?.ToString(), text, attackbonus, attack.Length > 2 ? attack[2] : null));
                                                        }
                                                        else m.Actions.Add(new OGL.Monsters.MonsterTrait(trait["name"]?.ToString(), text));
                                                    }
                                                } else
                                                {
                                                    String text;
                                                    if (prop.Value["text"] is JArray a) text = string.Join("\n", a.Select(s => s.ToString()));
                                                    else text = prop.Value["text"]?.ToString();
                                                    if (prop.Value["attack"] != null)
                                                    {
                                                        string[] attack = prop.Value["attack"].ToString().Split('|');
                                                        int attackbonus = 0;
                                                        if (attack.Length > 1) TryParse(attack[1], out attackbonus);
                                                        m.Actions.Add(new OGL.Monsters.MonsterAction(prop.Value["name"]?.ToString(), text, attackbonus, attack.Length > 2 ? attack[2] : null));
                                                    }
                                                    else m.Actions.Add(new OGL.Monsters.MonsterTrait(prop.Value["name"]?.ToString(), text));
                                                }
                                                break;
                                            case "save":
                                            case "saves":
                                                string[] saves = prop.Value is JArray ? prop.Values().Select(val=>val.ToString()).ToArray() : prop.Value.ToString().Split(',');
                                                foreach (string save in saves) monsterSaves.Add(save.Trim());
                                                break;
                                            case "spell":
                                            case "spells":
                                                string[] spells = prop.Value is JArray ? prop.Values().Select(val => val.ToString()).ToArray() : prop.Value.ToString().Split(',');
                                                foreach (string s in spells) m.Spells.Add(s.Trim());
                                                break;
                                            case "slots":
                                                string[] slots = prop.Value is JArray ? prop.Values().Select(val => val.ToString()).ToArray() : prop.Value.ToString().Split(',');
                                                foreach (string s in slots) if (s.Length > 0) m.Slots.Add(int.Parse(s.Trim()));
                                                break;
                                            case "senses":
                                                string[] senses = prop.Value is JArray ? prop.Values().Select(val => val.ToString()).ToArray() : prop.Value.ToString().Split(',');
                                                foreach (string s in senses) m.Senses.Add(s.Trim());
                                                break;
                                            case "legendary":
                                                if (prop.Value is JArray legendary)
                                                {
                                                    foreach (var trait in legendary)
                                                    {
                                                        String text;
                                                        if (trait["text"] is JArray a) text = string.Join("\n", a.Select(s => s.ToString()));
                                                        else text = trait["text"]?.ToString();
                                                        if (trait["attack"] != null)
                                                        {
                                                            string[] attack = trait["attack"].ToString().Split('|');
                                                            int attackbonus = 0;
                                                            if (attack.Length > 1) TryParse(attack[1], out attackbonus);
                                                            m.LegendaryActions.Add(new OGL.Monsters.MonsterAction(trait["name"]?.ToString(), text, attackbonus, attack.Length > 2 ? attack[2] : null));
                                                        }
                                                        else m.LegendaryActions.Add(new OGL.Monsters.MonsterTrait(trait["name"]?.ToString(), text));
                                                    }
                                                }
                                                else
                                                {
                                                    String text;
                                                    if (prop.Value["text"] is JArray a) text = string.Join("\n", a.Select(s => s.ToString()));
                                                    else text = prop.Value["text"]?.ToString();
                                                    if (prop.Value["attack"] != null)
                                                    {
                                                        string[] attack = prop.Value["attack"].ToString().Split('|');
                                                        int attackbonus = 0;
                                                        if (attack.Length > 1) TryParse(attack[1], out attackbonus);
                                                        m.LegendaryActions.Add(new OGL.Monsters.MonsterAction(prop.Value["name"]?.ToString(), text, attackbonus, attack.Length > 2 ? attack[2] : null));
                                                    }
                                                    else m.LegendaryActions.Add(new OGL.Monsters.MonsterTrait(prop.Value["name"]?.ToString(), text));
                                                }
                                                break;
                                            case "reaction":
                                                if (prop.Value is JArray reaction)
                                                {
                                                    foreach (var trait in reaction)
                                                    {
                                                        String text;
                                                        if (trait["text"] is JArray a) text = string.Join("\n", a.Select(s => s.ToString()));
                                                        else text = trait["text"]?.ToString();
                                                        if (trait["attack"] != null)
                                                        {
                                                            string[] attack = trait["attack"].ToString().Split('|');
                                                            int attackbonus = 0;
                                                            if (attack.Length > 1) TryParse(attack[1], out attackbonus);
                                                            m.Reactions.Add(new OGL.Monsters.MonsterAction(trait["name"]?.ToString(), text, attackbonus, attack.Length > 2 ? attack[2] : null));
                                                        }
                                                        else m.Reactions.Add(new OGL.Monsters.MonsterTrait(trait["name"]?.ToString(), text));
                                                    }
                                                }
                                                else
                                                {
                                                    String text;
                                                    if (prop.Value["text"] is JArray a) text = string.Join("\n", a.Select(s => s.ToString()));
                                                    else text = prop.Value["text"]?.ToString();
                                                    if (prop.Value["attack"] != null)
                                                    {
                                                        string[] attack = prop.Value["attack"].ToString().Split('|');
                                                        int attackbonus = 0;
                                                        if (attack.Length > 1) TryParse(attack[1], out attackbonus);
                                                        m.Reactions.Add(new OGL.Monsters.MonsterAction(prop.Value["name"]?.ToString(), text, attackbonus, attack.Length > 2 ? attack[2] : null));
                                                    }
                                                    else m.Reactions.Add(new OGL.Monsters.MonsterTrait(prop.Value["name"]?.ToString(), text));
                                                }
                                                break;
                                            case "immune":
                                                string[] immune = prop.Value.ToString().Split(',');
                                                foreach (string s in immune) m.Immunities.Add(s.Trim());
                                                break;
                                            case "description":
                                            case "text":
                                                m.Description = prop.Value.ToString();
                                                break;
                                            case "resist":
                                                string[] resist = prop.Value.ToString().Split(',');
                                                foreach (string s in resist) m.Resistances.Add(s.Trim());
                                                break;
                                            case "conditionimmune":
                                                string[] conditionImmune = prop.Value.ToString().Split(',');
                                                foreach (string s in conditionImmune) m.ConditionImmunities.Add(s.Trim());
                                                break;
                                            case "vulnerable":
                                                string[] vulnerable = prop.Value.ToString().Split(',');
                                                foreach (string s in vulnerable) m.Vulnerablities.Add(s.Trim());
                                                break;
                                            case "spellability":
                                                break;
                                            case "init":
                                                break;
                                            case "environment":
                                                break;
                                            default:
                                                throw new Exception("Unexpected property " + prop + " in " + m);
                                        }
                                    } else
                                    {
                                        throw new Exception("Expected property instead of " + e + " in " + m);
                                    }
                                }
                                m.PassivePerception = passive - (m.Wisdom / 2 - 5) - 10;
                                foreach (string skill in monsterSkills)
                                {
                                    foreach (Match match in Regex.Matches(skill, @"(\D+?)\s+\+?(\-?\s*\d+)(?:\s*\(([^\)]+)\))?", RegexOptions.IgnoreCase))
                                    {
                                        string name = match.Groups[1].Value.Trim();
                                        if (TryParse(match.Groups[2].Value, out int bonus))
                                        {
                                            if ("perception".Equals(name, StringComparison.OrdinalIgnoreCase))
                                            {
                                                m.PassivePerception -= bonus - (m.Wisdom / 2 - 5);
                                            }
                                            if (skills.ContainsKey(name.ToLowerInvariant())) m.SkillBonus.Add(new OGL.Monsters.MonsterSkillBonus()
                                            {
                                                Skill = name,
                                                Bonus = bonus - m.getAbility(skills[name.ToLowerInvariant()]) / 2 + 5,
                                                Text = match.Groups.Count > 2 ? match.Groups[3].Value : null
                                                
                                            });
                                            else m.SkillBonus.Add(new OGL.Monsters.MonsterSkillBonus()
                                            {
                                                Skill = name,
                                                Bonus = bonus,
                                                Text = match.Captures.Count > 2 ? match.Captures[3].Value : null
                                            });
                                        }
                                        else throw new Exception("Unexpected bonus " + bonus + " in " + skill);
                                    }
                                }
                                foreach (string save in monsterSaves)
                                {
                                    foreach (Match match in Regex.Matches(save, @"(\D+?)\s+\+?(\-?\s*\d+)(?:\s*\(([^\)]+)\))?", RegexOptions.IgnoreCase))
                                    {
                                        string name = match.Groups[1].Value.Trim();
                                        if (TryParse(match.Groups[2].Value, out int bonus))
                                        {
                                            if (abilities.ContainsKey(name.ToLowerInvariant())) m.SaveBonus.Add(new OGL.Monsters.MonsterSaveBonus()
                                            {
                                                Ability = abilities[name.ToLowerInvariant()],
                                                Bonus = bonus - m.getAbility(abilities[name.ToLowerInvariant()]) / 2 + 5,
                                                Text = match.Captures.Count > 2 ? match.Groups[3].Value : null

                                            });
                                            else throw new Exception("Unexpected ability in " + save);
                                        }
                                        else throw new Exception("Unexpected bonus " + bonus + " in " + save);
                                    }
                                }
                                m.Save(false);
                            }
                        }
                        return;
                    }
                }


                Application.Run(new MainTab());
            } else
            {
                Application.Exit();
            }
        }

        private static bool TryParse(string v, out int x)
        {
            return int.TryParse(v.Replace("I", "1").Replace("l", "1"), out x);
        }
    }
}
