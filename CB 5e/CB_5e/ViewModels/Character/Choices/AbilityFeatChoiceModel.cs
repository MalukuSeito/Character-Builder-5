using System;
using System.Collections.Generic;
using OGL.Common;
using OGL.Features;
using Character_Builder;
using OGL.Base;
using System.Linq;
using Xamarin.Forms;
using OGL;
using CB_5e.ViewModels.Character.ChoiceOptions;

namespace CB_5e.ViewModels.Character.Choices
{

    public class AbilityFeatChoiceModel : ChoiceViewModel<AbilityScoreFeatFeature>
    {
        private static AbilityChoice str = new AbilityChoice(Ability.Strength);
        private static AbilityChoice dex = new AbilityChoice(Ability.Dexterity);
        private static AbilityChoice con = new AbilityChoice(Ability.Constitution);
        private static AbilityChoice intl = new AbilityChoice(Ability.Intelligence);
        private static AbilityChoice wis = new AbilityChoice(Ability.Wisdom);
        private static AbilityChoice cha = new AbilityChoice(Ability.Charisma);
        public AbilityFeatChoiceModel(PlayerModel model, IChoiceProvider provider, AbilityScoreFeatFeature asff): base(model, provider, asff.UniqueID, 2, asff, true)
        {
            Name = "Ability Score Improvement";
            OnSelect = new Command((par) =>
            {
                if (par is ChoiceOption co)
                {
                    AbilityFeatChoice afc = Model.Context.Player.GetAbilityFeatChoice(Feature as AbilityScoreFeatFeature);
                    Model.MakeHistory();
                    if (co.Value is AbilityChoice ac)
                    {
                        afc.Feat = null;
                        if (co.Selected)
                        {
                            if (afc.Ability2 == ac.Ability) afc.Ability2 = Ability.None;
                            else if (afc.Ability1 == ac.Ability) afc.Ability1 = Ability.None;
                        }
                        else
                        {
                            if (afc.Ability1 == Ability.None) afc.Ability1 = ac.Ability;
                            else afc.Ability2 = ac.Ability;
                        }
                    } else
                    {
                        afc.Ability1 = Ability.None;
                        afc.Ability2 = Ability.None;
                        if (co.Selected)
                        {
                            afc.Feat = null;
                        }
                        else
                        {
                            afc.Feat = co.NameWithSource;
                        }
                    }
                    Model.Save();
                    Model.FirePlayerChanged();
                }
            });
        }

        public override void Refresh(AbilityScoreFeatFeature feature)
        {
            Feature = feature;
        }

        public override List<string> GetMyTakenChoices()
        {
            List<String> res = new List<string>();
            AbilityFeatChoice afc = Model.Context.Player.GetAbilityFeatChoice(Feature as AbilityScoreFeatFeature);
            if (afc.Ability1 != Ability.None) res.Add("+1 " + afc.Ability1.ToString());
            if (afc.Ability2 != Ability.None) res.Add("+1 " + afc.Ability2.ToString());
            if (afc.Feat != null && afc.Feat != "")
            {
                Amount = 1;
                res.Add(afc.Feat);
            }
            else Amount = 2;
            return res;
        }
        public override List<string> GetAllTakenChoices()
        {
            List<String> res = new List<string>();
            AbilityFeatChoice afc = Model.Context.Player.GetAbilityFeatChoice(Feature as AbilityScoreFeatFeature);
            if (afc.Ability1 != Ability.None) res.Add("+1 " + afc.Ability1.ToString());
            if (afc.Ability2 != Ability.None) res.Add("+1 " + afc.Ability2.ToString());
            if (afc.Feat != null && afc.Feat != "") res.Add(afc.Feat);
            return res;
        }

        public override int Taken {
            get {
                AbilityFeatChoice afc = Model.Context.Player.GetAbilityFeatChoice(Feature as AbilityScoreFeatFeature);
                if (afc.Feat != null && afc.Feat != "") return 1;
                int c = 0;
                if (afc.Ability1 != Ability.None) c++;
                if (afc.Ability2 != Ability.None) c++;
                return c;
            }
        }

        protected override IEnumerable<IXML> GetOptions()
        {
            List<IXML> res = new List<IXML>
            {
                str,
                dex,
                con,
                intl,
                wis,
                cha
            };
            List<string> taken = new List<string>(Model.Context.Player.GetFeatNames());
            foreach (Feature f in Model.Context.Player.GetFeatures()) if (f is CollectionChoiceFeature && (((CollectionChoiceFeature)f).Collection == null || ((CollectionChoiceFeature)f).Collection == "")) taken.AddRange(((CollectionChoiceFeature)f).Choices(Model.Context.Player));
            string ff = Model.Context.Player.GetAbilityFeatChoice(Feature).Feat;
            taken.RemoveAll(s => ConfigManager.SourceInvariantComparer.Equals(s, ff));
            int level = Model.Context.Player.GetLevel();
            res.AddRange(Model.Context.GetFeatureCollection("").Where(f => !taken.Contains(f.Name) && f.Level <= level));
            return res;
        }

        public override IXML GetValue(string nameWithSource)
        {
            if (nameWithSource == str.Name) return str;
            if (nameWithSource == dex.Name) return dex;
            if (nameWithSource == con.Name) return con;
            if (nameWithSource == wis.Name) return wis;
            if (nameWithSource == cha.Name) return cha;
            if (nameWithSource == intl.Name) return intl;
            Feature res = Model.Context.GetFeatureCollection("").FirstOrDefault(f => StringComparer.OrdinalIgnoreCase.Equals(f.Name + " " + ConfigManager.SourceSeperator + " " + f.Source, nameWithSource));
            if (res != null) return res;
            return Model.Context.GetFeatureCollection("").FirstOrDefault(f => ConfigManager.SourceInvariantComparer.Equals(f, nameWithSource));
        }
    }
}