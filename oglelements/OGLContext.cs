using NCalc;
using OGL.Base;
using OGL.Common;
using OGL.Features;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OGL
{
    public class OGLContext
    {

        private static Regex quotes = new Regex("([\\\"])(?:\\\\\\1|.)*?\\1");
        private static string FixQuotes(string exp)
        {
            StringBuilder res = new StringBuilder();
            int last = 0;
            for (Match m = quotes.Match(exp); m.Success; m = m.NextMatch())
            {
                res.Append(exp.Substring(last, m.Index - last));
                last = m.Index + m.Length;
                res.Append("'");
                res.Append(m.Value.Substring(1, m.Length - 2).Replace("\\\"", "\"").Replace("'", "\\'"));
                res.Append("'");
            }
            if (last == 0) return exp;
            if (last < exp.Length) res.Append(exp.Substring(last));
            return res.ToString();
        }

        private static bool MatchesKW(string kw, string kw2)
        {
            return kw.Replace('-', '_').Equals(kw2.Replace('-', '_'), StringComparison.OrdinalIgnoreCase);
        }

        //Search Filter
        public String Search = "";

        //Ability Scores
        public AbilityScores Scores = null;
        public void GenerateAbilityScores()
        {
            Scores = new AbilityScores()
            {
                PointBuyCost = new List<int>() { 0, 1, 2, 3, 4, 5, 7, 9 },
                PointBuyPoints = 27,
                PointBuyMinScore = 8,
                PointBuyMaxScore = 15
            };
            foreach (AbilityScoreArray a in AbilityScoreArray.Generate()) Scores.Arrays.Add(a.ToString());
        }

        //Background
        public Dictionary<String, Background> Backgrounds = new Dictionary<string, Background>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, Background> BackgroundsSimple = new Dictionary<string, Background>(StringComparer.OrdinalIgnoreCase);
        public Background GetBackground(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Backgrounds.ContainsKey(name)) return Backgrounds[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Backgrounds.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Backgrounds[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (BackgroundsSimple.ContainsKey(name)) return BackgroundsSimple[name];
            ConfigManager.LogError("Unknown Background: " + name);
            return new Background(this, name, "Missing Entry");
        }

        //Classes
        public Dictionary<String, ClassDefinition> Classes = new Dictionary<string, ClassDefinition>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, ClassDefinition> ClassesSimple = new Dictionary<string, ClassDefinition>(StringComparer.OrdinalIgnoreCase);

        public ClassDefinition GetClass(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Classes.ContainsKey(name)) return Classes[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Classes.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Classes[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (ClassesSimple.ContainsKey(name)) return ClassesSimple[name];
            ConfigManager.LogError("Unknown Class: " + name);
            return new ClassDefinition(this, name, "Missing Entry", 4);
        }

        public IEnumerable<ClassDefinition> GetClasses(int level, IChoiceProvider provider)
        {
            if (level > 1)
            {
                return from c in Classes.Values where provider.CanMulticlass(c, level) select c;
            }
            return from c in Classes.Values where c.AvailableAtFirstLevel select c;
        }

        //Conditions
        public Dictionary<String, Condition> Conditions = new Dictionary<string, Condition>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, Condition> ConditionsSimple = new Dictionary<string, Condition>(StringComparer.OrdinalIgnoreCase);
        public Condition GetCondition(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Conditions.ContainsKey(name)) return Conditions[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Conditions.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Conditions[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (ConditionsSimple.ContainsKey(name)) return ConditionsSimple[name];
            return new Condition(this, name, "");
        }

        //ConfigManager
        public ConfigManager Config;
        public HashSet<string> ExcludedSources { get; private set; } = new HashSet<string>();

        //FeatureCollections
        public List<Dictionary<string, List<Feature>>> FeatureCollections = new List<Dictionary<string, List<Feature>>>();
        public Dictionary<string, List<FeatureContainer>> FeatureContainers = new Dictionary<string, List<FeatureContainer>>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, List<Feature>> FeatureCategories = new Dictionary<string, List<Feature>>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Feature> Boons = new Dictionary<string, Feature>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Feature> BoonsSimple = new Dictionary<string, Feature>(StringComparer.OrdinalIgnoreCase);
        public List<Feature> Features = new List<Feature>();
        private List<List<Feature>> copies = new List<List<Feature>>();
        public List<Feature> GetFeatureCollection (string expression, int copy = 0)
        {
            while (FeatureCollections.Count <= copy) FeatureCollections.Add(new Dictionary<string, List<Feature>>(StringComparer.OrdinalIgnoreCase));
            int c = copy - 1;
            while (copies.Count <= c) copies.Add(FeatureContainer.MakeCopy(Features));
            List<Feature> features = Features;
            if (c >= 0) features = copies[c];
            if (expression == null || expression == "") expression = "Category = 'Feats'";
            if (expression == "Boons") expression = "Category = 'Boons'";
            if (FeatureCollections[copy].ContainsKey(expression)) return new List<Feature>(FeatureCollections[copy][expression]);
            try
            {
                Expression ex = new Expression(FixQuotes(expression));
                Feature current = null;
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "category") args.Result = current.Category;
                    else if (name == "level") args.Result = current.Level;
                    else if (name == "name") args.Result = current.Name.ToLowerInvariant();
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k => k.Name == name)) args.Result = true;
                    else args.Result = false;
                };
                List<Feature> res = new List<Feature>();
                foreach (Feature f in features)
                {
                    current = f;
                    object o = ex.Evaluate();
                    if (o is Boolean && (Boolean)o) res.Add(current);

                }
                res.Sort();
                FeatureCollections[copy][expression] = res;
                return res;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating expression " + expression, e);
                return new List<Feature>();
            }
        }
        public Feature GetBoon(string name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Boons.ContainsKey(name)) return Boons[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Boons.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Boons[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (BoonsSimple.ContainsKey(name)) return BoonsSimple[name];
            ConfigManager.LogError("Unknown Boon: " + name);
            Feature b = new Feature(name, "Missing Boon", 0, false);
            BoonsSimple[name] = b;
            return b;
        }
        public IEnumerable<string> FeatureSection()
        {
            if (Search == null) return from s in FeatureCategories.Keys where s.EndsWith("/Boons") orderby s select s;
            return from s in FeatureCategories where s.Key.EndsWith("/Boons") && s.Value.Exists(f => f.Test(this)) orderby s.Key select s.Key;
        }
        public IEnumerable<Feature> FeatureSubsection(string s)
        {
            if (Search == null) return from f in FeatureCategories[s] orderby f select f;
            else return from f in FeatureCategories[s] where f.Test(this) orderby f select f;
        }

        public void Add(Feature f)
        {
            Features.Add(f);
        }
        public void AddRange(List<Feature> f)
        {
            Features.AddRange(f);
        }

        //Items
        public Dictionary<String, Item> Items = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, Item> ItemsSimple = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, List<Item>> ItemLists = new Dictionary<string, List<Item>>(StringComparer.OrdinalIgnoreCase);
        public Item GetItem(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Items.ContainsKey(name)) return Items[name];
                if (Spells.ContainsKey(name)) return new Scroll(GetSpell(name, sourcehint));
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Items.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Items[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (sourcehint != null && Spells.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return new Scroll(GetSpell(name + " " + ConfigManager.SourceSeperator + " " + sourcehint, sourcehint));
            if (ItemsSimple.ContainsKey(name)) return ItemsSimple[name];
            if (SpellsSimple.ContainsKey(name)) return new Scroll(GetSpell(name, sourcehint));
            return new Item(Category.Make(this), name);
        }
        public IOrderedEnumerable<Item> Subsection(Category section)
        {
            if (Search == "")
            {
                if (section == null) return (from i in Items.Values orderby i select i);
                else return (from i in Items.Values where i.Category == section orderby i select i);
            }
            else
            {
                Search = Search.ToLowerInvariant();
                if (section == null) return (from i in Items.Values where i.Test(this) orderby i select i);
                else return (from i in Items.Values where i.Category == section && i.Test(this) orderby i select i);
            }
        }
        public IOrderedEnumerable<Category> Section()
        {
            if (Search == "")
            {
                return Category.Section();
            }
            else
            {
                Search = Search.ToLowerInvariant();
                return (from i in Items.Values where i.Test(this) select i.Category).Distinct().Where(i => i.ToString() != "Items").OrderBy(i => i);
            }
        }
        public List<Item> FilterPreview(string expression)
        {
            if (expression == null || expression == "") expression = "true";
            if (ItemLists.ContainsKey(expression)) return new List<Item>(ItemLists[expression]);
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression));
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
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                List<Item> res = new List<Item>();
                foreach (Item f in Items.Values)
                {
                    current = f;
                    object o = ex.Evaluate();
                    if (o is Boolean && (Boolean)o) res.Add(current);

                }
                res.Sort();
                ItemLists[expression] = res;
                return res;
            }
            catch (Exception e)
            {
                throw new Exception("Error while evaluating expression " + expression, e);
            }
        }

        //Languages
        public Dictionary<String, Language> Languages = new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, Language> LanguagesSimple = new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase);
        public Language GetLanguage(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Languages.ContainsKey(name)) return Languages[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Languages.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Languages[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (LanguagesSimple.ContainsKey(name)) return LanguagesSimple[name];
            ConfigManager.LogError("Unknown Language: " + name);
            return new Language(this, name, "Missing Entry", "", "");
        }

        //Level
        public Level Levels;
        public void GenerateLevels()
        {
            Levels = new Level()
            {
                Experience = new List<int>() { 0, 300, 900, 2700, 6500, 14000, 23000, 34000, 48000, 64000, 85000, 100000, 120000, 140000, 165000, 195000, 225000, 265000, 305000, 355000 },
                Proficiency = new List<int>() { +2, +2, +2, +2, +3, +3, +3, +3, +4, +4, +4, +4, +5, +5, +5, +5, +6, +6, +6, +6 }
            };
            Levels.Sort();
        }

        //Magic
        public Dictionary<string, MagicCategory> MagicCategories = new Dictionary<string, MagicCategory>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, MagicProperty> Magic = new Dictionary<string, MagicProperty>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, MagicProperty> MagicSimple = new Dictionary<string, MagicProperty>(StringComparer.OrdinalIgnoreCase);

        public MagicProperty GetMagic(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Magic.ContainsKey(name)) return Magic[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Magic.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Magic[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (MagicSimple.ContainsKey(name)) return MagicSimple[name];
            ConfigManager.LogError("Unknown property: " + name);
            return new MagicProperty(this, name, "Missing Entry");
        }
        public IEnumerable<MagicCategory> MagicSection()
        {
            if (Search == null) return from mc in MagicCategories.Values orderby mc select mc;
            List<MagicCategory> res = new List<MagicCategory>();
            foreach (MagicCategory mc in MagicCategories.Values)
            {
                if (mc.Contents.Exists(mp => mp.Test(this))) res.Add(mc);
            }
            return res;
        }

        //Races
        public Dictionary<String, Race> Races = new Dictionary<string, Race>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, Race> RacesSimple = new Dictionary<string, Race>(StringComparer.OrdinalIgnoreCase);
        public Race GetRace(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Races.ContainsKey(name)) return Races[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Races.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Races[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (RacesSimple.ContainsKey(name)) return RacesSimple[name];
            ConfigManager.LogError("Unknown Race: " + name);
            Race r = new Race(this, name)
            {
                Description = "Missing Entry"
            };
            return r;
        }

        //Skills
        public Dictionary<String, Skill> Skills = new Dictionary<string, Skill>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, Skill> SkillsSimple = new Dictionary<string, Skill>(StringComparer.OrdinalIgnoreCase);
        public Skill GetSkill(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Skills.ContainsKey(name)) return Skills[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Skills.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Skills[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (SkillsSimple.ContainsKey(name)) return SkillsSimple[name];
            ConfigManager.LogError("Unknown Skill: " + name);
            return new Skill(this, name, "Missing Entry", Ability.None);
        }

        //Spells
        public Dictionary<String, Spell> Spells = new Dictionary<string, Spell>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, List<Spell>> SpellLists = new Dictionary<string, List<Spell>>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, Spell> SpellsSimple = new Dictionary<string, Spell>(StringComparer.OrdinalIgnoreCase);

        public Spell GetSpell(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (Spells.ContainsKey(name)) return Spells[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && Spells.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return Spells[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (SpellsSimple.ContainsKey(name)) return SpellsSimple[name];
            ConfigManager.LogError("Spell not found: " + name);
            Spell missing = new Spell()
            {
                Name = name,
                Source = "Autogenerated Entry",
                ShowSource = true
            };
            missing.Register(this, null);
            return missing;
        }
        public IOrderedEnumerable<Spell> SpellSubsection()
        {
            if (Search == "") return from s in Spells.Values orderby s select s;
            else return from s in Spells.Values where s.Test(this) orderby s select s;
        }
        public List<Spell> FilterSpells(string expression)
        {
            if (expression == null || expression == "") expression = "true";
            try
            {
                Expression ex = new Expression(ConfigManager.FixQuotes(expression));
                Spell current = null;
                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                {
                    name = name.ToLowerInvariant();
                    if (name == "classlevel") args.Result = int.MaxValue;
                    else if (name == "classspelllevel") args.Result = int.MaxValue;
                    else if (name == "maxspellslot") args.Result = int.MaxValue;
                    else if (name == "name") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "namelower") args.Result = current.Name.ToLowerInvariant();
                    else if (name == "level") args.Result = current.Level;
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k => MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                ex.EvaluateFunction += FunctionExtensions;
                List<Spell> res = new List<Spell>();
                foreach (Spell f in Spells.Values)
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
        private static void FunctionExtensions(string name, FunctionArgs args)
        {
            if (name.Equals("ClassLevel", StringComparison.OrdinalIgnoreCase))
            {
                args.Result = int.MaxValue;
            }
            if (name.Equals("SubClass", StringComparison.OrdinalIgnoreCase))
            {
                args.Result = "";
            }
        }

        //SubClasses
        public Dictionary<String, SubClass> SubClasses = new Dictionary<string, SubClass>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, SubClass> SubClassesSimple = new Dictionary<string, SubClass>(StringComparer.OrdinalIgnoreCase);
        public SubClass GetSubClass(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (SubClasses.ContainsKey(name)) return SubClasses[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && SubClasses.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return SubClasses[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (SubClassesSimple.ContainsKey(name)) return SubClassesSimple[name];
            ConfigManager.LogError("Unknown subclass: " + name);
            SubClass sc = new SubClass(this, name, null)
            {
                Description = "Missing Entry"
            };
            return sc;
        }
        public IEnumerable<SubClass> SubClassFor(string classDefinition)
        {
            return (from s in SubClasses.Values where ConfigManager.SourceInvariantComparer.Equals(s.ClassName, classDefinition) || s.ClassName == "*" select s);
        }

        //SubRaces
        public Dictionary<String, SubRace> SubRaces = new Dictionary<string, SubRace>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<String, SubRace> SubRacesSimple = new Dictionary<string, SubRace>(StringComparer.OrdinalIgnoreCase);

        public SubRace GetSubRace(String name, string sourcehint)
        {
            if (name.Contains(ConfigManager.SourceSeperatorString))
            {
                if (SubRaces.ContainsKey(name)) return SubRaces[name];
                name = SourceInvariantComparer.NoSource(name);
            }
            if (sourcehint != null && SubRaces.ContainsKey(name + " " + ConfigManager.SourceSeperator + " " + sourcehint)) return SubRaces[name + " " + ConfigManager.SourceSeperator + " " + sourcehint];
            if (SubRacesSimple.ContainsKey(name)) return SubRacesSimple[name];
            ConfigManager.LogError("Unknown Subrace: " + name);
            SubRace sr = new SubRace(this, name, null)
            {
                Description = "Missing Entry"
            };
            return sr;
        }
        public IEnumerable<SubRace> SubRaceFor(List<String> races)
        {
            return (from s in SubRaces.Values where races.Contains(s.RaceName) || s.RaceName == "*" select s);
        }
    }
}
