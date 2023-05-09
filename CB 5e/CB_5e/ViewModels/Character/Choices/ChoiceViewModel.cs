using CB_5e.Helpers;
using CB_5e.ViewModels.Character.ChoiceOptions;
using CB_5e.Views;
using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Character.Choices
{

    public interface ChoiceViewModel
    {
        string UniqueID { get; }
        Command OnCustom { get; }
        Command OnSelect { get; }
    }
    public abstract class ChoiceViewModel<T> : ObservableRangeCollection<ChoiceOption>, ChoiceViewModel where T: Feature 
    {
        public ChoiceViewModel(PlayerModel model, IChoiceProvider provider, String uniqueID, int amount, T feature, bool multiple = false, bool allowCustom = false)
        {
            Model = model;
            UniqueID = uniqueID;
            Amount = amount;
            Feature = feature;
            Multiple = multiple;
            Name = Feature?.Name;
            AllowCustom = allowCustom;
            ChoiceProvider = provider;
            if (Feature != null)
            {
                Offset = provider.GetChoiceOffset(Feature, UniqueID, Amount);
                Total = provider.GetChoiceTotal(UniqueID);
            }

            OnCustom = new Command(async () =>
            {
                await Navigation?.PushAsync(new CustomTextEntryPage(Name, OnSelect));
            });

            OnSelect = new Command((par) =>
            {
                if (AllowCustom && par is string s )
                {
                    Model.MakeHistory();
                    int offset = Offset;
                    for (int c = 0; c < Amount; c++)
                    {
                        String counter = "";

                        if (c + offset > 0) counter = "_" + (c + offset).ToString();
                        Choice cho = provider.GetChoice(UniqueID + counter);
                        if (cho == null)
                        {
                            provider.SetChoice(UniqueID + counter, s);
                            Model.Save();
                            Model.FirePlayerChanged();
                            break;
                        }

                    }
                }
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
                            Choice cho = provider.GetChoice(UniqueID + counter);
                            if (cho != null && ConfigManager.SourceInvariantComparer.Equals(cho.Value, val))
                            {
                                co.Selected = false;
                                ChoiceProvider.RemoveChoice(cho.UniqueID);
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
                            Choice cho = provider.GetChoice(UniqueID + counter);
                            if (cho == null)
                            {
                                co.Selected = true;
                                ChoiceProvider.SetChoice(UniqueID + counter, co.NameWithSource);
                                Model.Save();
                                Model.FirePlayerChanged();
                                break;
                            }

                        }
                    }
                }
            });
            UpdateOptionsImmediately();
        }
        IChoiceProvider ChoiceProvider { get; set; }
        public PlayerModel Model { get; set; }
        public string Name { get; set; }
        public virtual string DisplayName { get => Amount > 1 ? Name + " (" + Taken + "/" + Amount + ")" : Name; }
        public Command OnSelect { get; set; }
        public string UniqueID { get; protected set; }
        public int Amount { get; protected set; }
        public T Feature { get; protected set; }

        public abstract void Refresh(T feature);

        protected virtual void UpdateOptionsImmediately()
        {
            UpdateOptions();
        }

        protected abstract IEnumerable<IXML> GetOptions();


        public virtual List<string> GetAllTakenChoices()
        {
            List<string> taken = new List<string>();
            for (int c = 0; c < Total; c++)
            {
                String counter = "";
                if (c > 0) counter = "_" + c.ToString();
                Choice cho = ChoiceProvider.GetChoice(UniqueID + counter);
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
                Choice cho = ChoiceProvider.GetChoice(UniqueID + counter);
                if (cho != null && cho.Value != "") taken.Add(cho.Value);
            }
            return taken;
        }



        public virtual int Offset { get; set; }
        public virtual int Total { get; set; }
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
                    Choice cho = ChoiceProvider.GetChoice(UniqueID + counter);
                    if (cho != null && cho.Value != "") res++;
                }
                return res;
            }
        }

        public bool AllowCustom { get; private set; }

        public abstract IXML GetValue(string nameWithSource);

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
            if (!allMade)
            {
                foreach (IXML opt in from o in GetOptions() orderby o.Name, prio.Contains(o.Name + " " + ConfigManager.SourceSeperator + " " + o.Source) descending select o)
                {
                    if (opt == null) continue;
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
                    while (co.Selected && Multiple && (counter[id] > 0 || counter[id] == 0 && !allMade))
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
            }
            foreach (KeyValuePair<string, int> e in counter)
            {
                int c = e.Value;
                while (c > 0)
                {
                    IXML val = GetValue(e.Key);
                    if (val != null)
                    {
                        options.Add(new ChoiceOption()
                        {
                            Value = val,
                            Model = this,
                            Feature = Feature,
                            Selected = true
                        });
                    }
                    else if (AllowCustom) options.Add(new CustomChoiceOption(e.Key)
                    {
                        Model = this,
                        Feature = Feature,
                        Selected = true
                    });
                    else options.Add(new WrongChoiceOption(e.Key)
                    {
                        Model = this,
                        Feature = Feature,
                        Selected = true
                    });
                    c--;
                }
            }
            if (!allMade && AllowCustom)
            {
                options.Add(new ChoiceOption()
                {
                    Custom = true,
                    Model = this,
                    Feature = Feature,
                    Value = new Feature(" - Custom - ", " - Custom Text - ", 0, true),
                    Selected = false
                    
                });
            }
            ReplaceRange(options);
        }

        public INavigation Navigation { get; set; }
        public Command OnCustom { get; set; }
        public static ChoiceViewModel GetChoice(PlayerModel model, IChoiceProvider choiceProvider, Feature f)
        {
            if (f is BonusSpellKeywordChoiceFeature bskcf) return new BonusSpellKeywordChoice(model, choiceProvider, bskcf);
            if (f is ChoiceFeature cf) return new ChoiceFeatureChoice(model, choiceProvider, cf);
            if (f is ItemChoiceFeature icf) return new ItemChoice(model, choiceProvider, icf);
            if (f is CollectionChoiceFeature ccf) return new CollectionChoice(model, choiceProvider, ccf);
            if (f is ItemChoiceConditionFeature iccf) return new ItemConditionChoice(model, choiceProvider, iccf);
            if (f is LanguageChoiceFeature lcf) return new LanguageChoice(model, choiceProvider, lcf);
            if (f is SkillProficiencyChoiceFeature spcf) return new SkillProficiencyChoice(model, choiceProvider, spcf);
            if (f is ToolProficiencyChoiceConditionFeature tpccf) return new ToolProficiencyChoice(model, choiceProvider, tpccf);
            return null;
        }
    }
}
