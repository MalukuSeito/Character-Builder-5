using Character_Builder_Plugin;
using NCalc;
using OGL;
using OGL.Common;
using OGL.Features;
using OGL.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class ToolChoice: IPlugin
    {
        public int ExecutionOrdering { get => 0; }
        public string Name => "Customizing Your Origin - Choose Armor/Weapon/Tool Proficiency";

        private Dictionary<ToolProficiencyFeature, List<ToolProficiencyChoiceConditionFeature>> cache = new Dictionary<ToolProficiencyFeature, List<ToolProficiencyChoiceConditionFeature>>();
        private Dictionary<ToolProficiencyChoiceConditionFeature, ToolProficiencyChoiceConditionFeature> cache2 = new Dictionary<ToolProficiencyChoiceConditionFeature, ToolProficiencyChoiceConditionFeature>();
        private Dictionary<ToolKWProficiencyFeature, Feature> cache3 = new Dictionary<ToolKWProficiencyFeature, Feature>();

        public List<Feature> FilterBackgroundFeatures(Background background, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterBoons(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterClassFeatures(ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterCommonFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterFeats(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterPossessionFeatures(List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterRaceFeatures(Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            List<Feature> result = new List<Feature>();
            int counter = 0;
            int counter2 = 0;
            foreach (Feature f in features)
            {
                if (f is ToolProficiencyFeature tf)
                {
                    counter++;
                    if (!cache.ContainsKey(tf)) cache.Add(tf, MakeFeatures(race.Name, tf.Tools, counter, Context, race.Source));
                    result.AddRange(cache[tf]);
                }
                else if (f is ToolProficiencyChoiceConditionFeature tcf)
                {
                    if (!cache2.ContainsKey(tcf)) cache2.Add(tcf, Convert(tcf, Context));
                    result.Add(cache2[tcf]);
                }
                else if (f is ToolKWProficiencyFeature tkf)
                {
                    counter2++;
                    if (!cache3.ContainsKey(tkf)) cache3.Add(tkf, Convert(tkf, Context, race.Name, counter2));
                    result.AddRange(cache3[tkf].Collect(level, provider, Context));
                }
                else result.Add(f);
                
            }
            return result;
        }

        private List<ToolProficiencyChoiceConditionFeature> MakeFeatures(string name, List<string> tools, int counter, OGLContext context, string sourcehint)
        {
            List<ToolProficiencyChoiceConditionFeature> result = new List<ToolProficiencyChoiceConditionFeature>();
            foreach (string tool in tools)
            {
                Item i = context.GetItem(tool, sourcehint);
                if (i is Armor || i is Weapon w && w.Keywords.Exists(k => Utils.MatchesKW(k.Name, "martial"))) {
                    result.Add(MakeFeature(name, "(Weapon and (Martial or Simple)) or (Tool and not Armor and not Weapon) or Name = '" + tool + "'", tool, counter));
                }
                else if (i is Weapon ww && ww.Keywords.Exists(k => Utils.MatchesKW(k.Name, "simple")))
                {
                    result.Add(MakeFeature(name, "(Weapon and Simple) or (Tool and not Armor and not Weapon) or Name = '" + tool + "'", tool, counter));
                }
                else
                {
                    result.Add(MakeFeature(name, "(Tool and not Armor and not Weapon) or Name = '" + tool + "'", tool, counter));
                }

            }
            return result;
        }

        private ToolProficiencyChoiceConditionFeature Convert(ToolProficiencyChoiceConditionFeature tcf, OGLContext context)
        {
            List<Item> items = Filter(context.Items.Values, tcf.Condition);
            if (items.Count == 0) return tcf;
            ToolProficiencyChoiceConditionFeature res = new ToolProficiencyChoiceConditionFeature()
            {
                Action = tcf.Action,
                Amount = tcf.Amount,
                NoDisplay = tcf.NoDisplay,
                Category = tcf.Category,
                Hidden = tcf.Hidden,
                Level = tcf.Level,
                Keywords = new List<OGL.Keywords.Keyword>(tcf.Keywords),
                KWChanged = tcf.KWChanged,
                Name = tcf.Name,
                Prerequisite = tcf.Prerequisite,
                ShowSource = tcf.ShowSource,
                Source = tcf.Source,
                Text = tcf.Text,
                UniqueID = tcf.UniqueID,
                Condition = "(Tool and not Armor and not Weapon) or (" + tcf.Condition + ")"
            };
            if (Filter(items, "Armor or (Weapon and Martial)").Count > 0) res.Condition = "(Weapon and (Martial or Simple)) or (Tool and not Armor and not Weapon) or (" + tcf.Condition + ")";
            else if (Filter(items, "Weapon and Simple").Count > 0) res.Condition = "(Weapon and Simple) or (Tool and not Armor and not Weapon) or (" + tcf.Condition + ")";
            return res;
        }

        private Feature Convert(ToolKWProficiencyFeature tkf, OGLContext context, string name, int counter2)
        {
            List<Item> items = Filter(context.Items.Values, tkf.Condition);
            if (items.Count == 0) return tkf;
            ToolProficiencyChoiceConditionFeature res = new ToolProficiencyChoiceConditionFeature()
            {
                Action = OGL.Base.ActionType.ForceHidden,
                Amount = 1,
                NoDisplay = true,
                Hidden = true,
                Level = 0,
                Name = "Replacement Tool Proficency",
                ShowSource = false,
                Source = "Tool Proficiency Choice " + name,
                Text = "Choose a replacement tool (or weapon if you qualify)",
                UniqueID = "PLUGIN_TOOL_CHOICE_" + name + "_" + counter2,
                Condition = "(Tool and not Armor and not Weapon) or (" + tkf.Condition + ")"
            };
            if (Filter(items, "Armor or (Weapon and Martial)").Count > 0)
            {
                res.Condition = "(Weapon and (Martial or Simple)) or (Tool and not Armor and not Weapon) or (" + tkf.Condition + ")";
                res.Name = "Replacement Weapon or Tool Proficiency";
            }
            else if (Filter(items, "Weapon and Simple").Count > 0)
            {
                res.Condition = "(Weapon and Simple) or (Tool and not Armor and not Weapon) or (" + tkf.Condition + ")";
                res.Name = "Replacement Simple Weapon or Tool Proficiency";
            }
            return new ChoiceFeature() {
                Action = OGL.Base.ActionType.ForceHidden,
                AllowSameChoice = false,
                Amount = 1,
                Hidden = true,
                NoDisplay = true,
                Level = 0,
                Name = "Tool Proficiency Replacement " + name,
                ShowSource = false,
                Source = "Tool Proficiency Choice " + name,
                Text = "Keep your tool proficiency or replace it with a different one",
                UniqueID = "PLUGIN_TOOL_REPLACEMENT_" + name + "_" + counter2,
                Choices = new List<Feature>() { tkf, res }
            };
        }

        private ToolProficiencyChoiceConditionFeature MakeFeature(string name, string condition, String tool, int counter)
        {
            return new ToolProficiencyChoiceConditionFeature()
            {
                Action = OGL.Base.ActionType.ForceHidden,
                Amount = 1,
                Hidden = true,
                NoDisplay = true,
                Level = 0,
                UniqueID = "PLUGIN_TOOL_CHOICE_" + name.ToUpperInvariant() + "_" + tool.ToUpperInvariant() + "_" + counter,
                Name = "Choose your tool proficiency - " + name + " (" + tool + ")",
                Source = "Tool Proficiency Choice " + name,
                Text = "Tool proficienc(y/ies) to replace " + tool,
                Condition = condition,
                ShowSource = false
            };
        }

        public List<Feature> FilterSubClassFeatures(SubClass subcls, ClassDefinition cls, int classlevel, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            return features;
        }

        public List<Feature> FilterSubRaceFeatures(SubRace subrace, Race race, List<Feature> features, int level, IChoiceProvider provider, OGLContext Context)
        {
            List<Feature> result = new List<Feature>();
            int counter = 0;
            int counter2 = 0;
            foreach (Feature f in features)
            {
                if (f is ToolProficiencyFeature tf)
                {
                    counter++;
                    if (!cache.ContainsKey(tf)) cache.Add(tf, MakeFeatures(subrace.Name, tf.Tools, counter, Context, race.Source));
                    result.AddRange(cache[tf]);
                }
                else if (f is ToolProficiencyChoiceConditionFeature tcf)
                {
                    if (!cache2.ContainsKey(tcf)) cache2.Add(tcf, Convert(tcf, Context));
                    result.Add(cache2[tcf]);
                }
                else if (f is ToolKWProficiencyFeature tkf)
                {
                    counter2++;
                    if (!cache3.ContainsKey(tkf)) cache3.Add(tkf, Convert(tkf, Context, subrace.Name, counter2));
                    result.AddRange(cache3[tkf].Collect(level, provider, Context));
                }
                else result.Add(f);

            }
            return result;
        }

        public static List<Item> Filter(IEnumerable<Item> items, string expression)
        {
            if (expression == null || expression == "") expression = "true";
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
                    else if (name == "source") args.Result = current.Source.ToLowerInvariant();
                    else if (current.Keywords.Count > 0 && current.Keywords.Exists(k => Utils.MatchesKW(k.Name, name))) args.Result = true;
                    else args.Result = false;
                };
                List<Item> res = new List<Item>();
                foreach (Item f in items)
                {
                    current = f;
                    object o = ex.Evaluate();
                    if (o is Boolean && (Boolean)o) res.Add(current);

                }
                return res;
            }
            catch (Exception e)
            {
                ConfigManager.LogError("Error while evaluating " + expression, e);
            }
            return new List<Item>();
        }
    }
}
