using CB_5e.Helpers;
using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public abstract class ChoiceViewModel: ObservableRangeCollection<ChoiceOption>
    {
        public ChoiceViewModel(PlayerModel model, String uniqueID, int amount, Feature feature, IEnumerable<IXML> options, bool multiple = false)
        {
            Model = model;
            UniqueID = uniqueID;
            Amount = amount;
            Feature = feature;
            Options = options;
            Multiple = multiple;
            Name = Feature?.Name;

            OnSelect = new Command((par) =>
            {
                if (par is ChoiceOption co)
                {
                    Model.MakeHistory();
                    int offset = Offset;
                    if (co.Selected)
                    {
                        bool NeedsUpdate = Taken == Amount;
                        string val = co.NameWithSource;
                        for (int c = 0; c < Amount; c++)
                        {
                            String counter = "";

                            if (c + offset > 0) counter = "_" + (c + offset).ToString();
                            Choice cho = Model.Context.Player.GetChoice(UniqueID + counter);
                            if (cho != null && ConfigManager.SourceInvariantComparer.Equals(cho.Value, val))
                            {
                                co.Selected = false;
                                Model.Context.Player.RemoveChoice(cho.UniqueID);
                                Model.Save();
                                Model.FirePlayerChanged();
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int c = 0; c < Amount; c++)
                        {
                            String counter = "";

                            if (c + offset > 0) counter = "_" + (c + offset).ToString();
                            Choice cho = Model.Context.Player.GetChoice(UniqueID + counter);
                            if (cho == null)
                            {
                                co.Selected = true;
                                Model.Context.Player.SetChoice(UniqueID + counter, co.NameWithSource);
                                Model.Save();
                                Model.FirePlayerChanged();
                                break;
                            }

                        }
                    }
                }
            });
            UpdateOptions();
        }
        public PlayerModel Model { get; set; }
        public string Name { get; set; }
        public virtual string DisplayName { get => Amount > 1 ? Name + " (" + Amount + "/" + Taken + ")" : Name; }
        public Command OnSelect { get; set; }
        public string UniqueID { get; protected set; }
        public int Amount { get; protected set; }
        public Feature Feature { get; protected set; }

        public abstract void Refresh(Feature feature);
        public virtual List<string> GetAllTakenChoices()
        {
            List<string> taken = new List<string>();
            for (int c = 0; c < Total; c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                Choice cho = Model.Context.Player.GetChoice(UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            return taken;
        }

        public virtual List<string> GetMyTakenChoices()
        {
            List<string> taken = new List<string>();
            int offset = Offset;
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";

                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = Model.Context.Player.GetChoice(UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            return taken;
        }



        public virtual int Offset { get => Model.Context.Player.GetChoiceOffset(Feature, UniqueID, Amount); }
        public virtual int Total { get => Model.Context.Player.GetChoiceTotal(UniqueID); }
        public IEnumerable<IXML> Options { get; protected set; }
        public bool Multiple { get; protected set; }
        public virtual int Taken
        {
            get
            {
                int res = 0;
                int offset = Offset;
                for (int c = 0; c < Amount; c++)
                {
                    String counter = "";

                    if (c + offset > 0) counter = "_" + (c + offset).ToString();
                    Choice cho = Model.Context.Player.GetChoice(UniqueID + counter);
                    if (cho != null && cho.Value != "") res++;
                }
                return res;
            }
        }

        public virtual void UpdateOptions()
        {
            HashSet<string> taken = new HashSet<string>(ConfigManager.SourceInvariantComparer);
            List<string> t = GetAllTakenChoices();
            List<string> m = GetMyTakenChoices();
            if (!Multiple) taken.UnionWith(t);
            HashSet<string> prio = new HashSet<string>(m);
            prio.UnionWith(t);
            Dictionary<string, int> counter = new Dictionary<string, int>(ConfigManager.SourceInvariantComparer);
            foreach (string s in m)
            {
                if (counter.ContainsKey(s)) counter[s] = counter[s] + 1;
                else counter.Add(s, 1);
            }
            List<ChoiceOption> options = new List<ChoiceOption>();
            bool allMade = Amount == m.Count;
            foreach (IXML opt in from o in Options orderby o.Name, prio.Contains(o.Name + " " + ConfigManager.SourceSeperator + " " + o.Source) descending select o)
            {
                ChoiceOption co = new ChoiceOption()
                {
                    Value = opt,
                    Model = this,
                    Feature = Feature
                };
                string id = co.NameWithSource;
                if (counter.ContainsKey(id))
                {
                    co.Selected = counter[id] > 0;
                    counter[id]--;
                }
                if ((!allMade && !taken.Contains(id)) || co.Selected) options.Add(co);
                while (co.Selected && Multiple && (counter[id] > 0 || counter[id]==0 && !allMade) )
                {
                    
                    ChoiceOption co2 = new ChoiceOption()
                    {
                        Value = opt,
                        Model = this,
                        Feature = Feature
                    };
                    co2.Selected = counter.ContainsKey(id) && counter[id] > 0;
                    counter[id]--;
                    options.Add(co2);
                }
            }
            foreach (KeyValuePair<string, int> e in counter)
            {
                int c = e.Value;
                while (c > 0)
                {
                    options.Add(new WrongChoiceOption(e.Key)
                    {
                        Model = this,
                        Feature = Feature
                    });
                    c--;
                }
            }
            ReplaceRange(options);
        }

        public static ChoiceViewModel GetChoice(PlayerModel model, Feature f)
        {
            if (f is BonusSpellKeywordChoiceFeature bskcf)  return new BonusSpellKeywordChoice(model, bskcf);
            if (f is ChoiceFeature cf) return new ChoiceFeatureChoice(model, cf);
            if (f is ItemChoiceFeature icf) return new ItemChoice(model, icf);
            if (f is CollectionChoiceFeature ccf) return new CollectionChoice(model, ccf, model.Context.Player.GetLevel());
            if (f is ItemChoiceConditionFeature iccf) return new ItemConditionChoice(model, iccf);
            if (f is LanguageChoiceFeature lcf) return new LanguageChoice(model, lcf);
            if (f is SkillProficiencyChoiceFeature spcf) return new SkillProficiencyChoice(model, spcf);
            if (f is ToolProficiencyChoiceConditionFeature tpccf) return new ToolProficiencyChoice(model, tpccf);
            return null;
        }
    }
}
