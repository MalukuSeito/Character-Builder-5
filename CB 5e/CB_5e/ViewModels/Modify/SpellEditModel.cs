using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using CB_5e.Helpers;
using CB_5e.Views;
using OGL;
using PCLStorage;
using OGL.Base;
using OGL.Descriptions;
using OGL.Spells;
using OGL.Keywords;

namespace CB_5e.ViewModels.Modify
{
    public class SpellEditModel : EditModel<Spell>
    {
        
        public SpellEditModel(Spell obj, OGLContext context): base(obj, context)
        {
        }

        public string Name { get => Model.Name; set { if (value == Name) return; MakeHistory("Name"); Model.Name = value; OnPropertyChanged("Name"); } }
        public string Source { get => Model.Source; set { if (value == Source) return; MakeHistory("Source"); Model.Source = value; OnPropertyChanged("Source"); } }
        public string Description { get => Model.Description; set { if (value == Description) return; MakeHistory("Description"); Model.Description = value ; OnPropertyChanged("Description"); Description_TextChanged(); } }
        public string CastingTime { get => Model.CastingTime; set { if (value == CastingTime) return; MakeHistory("CastingTime"); Model.CastingTime = value; OnPropertyChanged("CastingTime"); } }
        public string Range { get => Model.Range; set { if (value == Range) return; MakeHistory("Range"); Model.Range = value; OnPropertyChanged("Range"); Range_TextChanged(); } }
        public string Duration { get => Model.Duration; set { if (value == Duration) return; MakeHistory("Duration"); Model.Duration = value; OnPropertyChanged("Duration"); Duration_TextChanged(); } }
        public int Level { get => Model.Level; set { if (value == Level) return; MakeHistory("Level"); Model.Level = value; OnPropertyChanged("Level"); Level_ValueChanged(); } }

        private void AddKeyword(Keyword k)
        {
            if (!Keywords.Contains(k))
            {
                Keywords.Add(k);
                OnPropertyChanged("Keywords");
            }
        }

        private void RemoveKeyword(Keyword k)
        {
            if (Keywords.Remove(k))
            {
                OnPropertyChanged("Keywords");
            }
        }

        private void Range_TextChanged()
        {
            if (!TrackChanges) return;
            if (Range != null && Range.ToLowerInvariant().Contains("self")) AddKeyword(new Keyword("Self"));
            else RemoveKeyword(new Keyword("Self"));
            if (Range != null && Range.ToLowerInvariant().Contains("touch")) AddKeyword(new Keyword("Touch"));
            else RemoveKeyword(new Keyword("Touch"));
            if (Range != null && (Range.ToLowerInvariant().Contains("touch") || Range.ToLowerInvariant().Contains("self"))) AddKeyword(new Keyword("Melee"));
            else RemoveKeyword(new Keyword("Melee"));
            if (Range != null && Range.ToLowerInvariant().Contains("feet") && !Range.ToLowerInvariant().Contains("self")) AddKeyword(new Keyword("Ranged"));
            else RemoveKeyword(new Keyword("Ranged"));
        }

        private void Duration_TextChanged()
        {
            if (!TrackChanges) return;
            if (Duration != null && Duration.ToLowerInvariant().Contains("instantaneous")) AddKeyword(new Keyword("Instantaneous"));
            else RemoveKeyword(new Keyword("Instantaneous"));
            if (Duration != null && Duration.ToLowerInvariant().Contains("concentration")) AddKeyword(new Keyword("Concentration"));
            else RemoveKeyword(new Keyword("Concentration"));
        }

        private void Level_ValueChanged()
        {
            if (!TrackChanges) return;
            if (Level == 0) AddKeyword(new Keyword("Cantrip"));
            else RemoveKeyword(new Keyword("Cantrip"));
        }
        private void Description_TextChanged()
        {
            if (!TrackChanges) return;
            string desc = Description;
            if (desc != null) desc = desc.ToLowerInvariant();
            // if (desc != null && desc.Contains("range"))
            // {
            //    AddKeyword(new Keyword("Ranged"));
            // } else RemoveKeyword(new Keyword("Ranged"));
            //if (desc != null && desc.Contains("make a melee spell attack"))
            //{
            //    AddKeyword(new Keyword("Melee"));
            //} else
            //{
            //    RemoveKeyword(new Keyword("Melee"));
            //}
            if (desc != null && (desc.Contains("make a melee spell attack") || desc.Contains("make a ranged spell attack")))
            {
                AddKeyword(new Keyword("Attack"));
            }
            else RemoveKeyword(new Keyword("Attack"));
            if (desc != null && (desc.Contains("your spell save dc")))
            {
                AddKeyword(new Save(Ability.None));
            }
            else RemoveKeyword(new Save(Ability.None));
            if (desc != null && (desc.Contains("must make a strength saving throw") || desc.Contains("must succeed on a strength saving throw") || desc.Contains("must make strength saving throws")))
            {
                AddKeyword(new Save(Ability.Strength));
            }
            else RemoveKeyword(new Save(Ability.Strength));
            if (desc != null && (desc.Contains("must make a dexterity saving throw") || desc.Contains("must succeed on a dexterity saving throw") || desc.Contains("must make dexterity saving throws")))
            {
                AddKeyword(new Save(Ability.Dexterity));
            }
            else RemoveKeyword(new Save(Ability.Dexterity));
            if (desc != null && (desc.Contains("must make a constitution saving throw") || desc.Contains("must succeed on a constitution saving throw") || desc.Contains("must make constitution saving throws")))
            {
                AddKeyword(new Save(Ability.Constitution));
            }
            else RemoveKeyword(new Save(Ability.Constitution));
            if (desc != null && (desc.Contains("must make an intelligence saving throw") || desc.Contains("must succeed on an intelligence saving throw") || desc.Contains("must make intelligence saving throws")))
            {
                AddKeyword(new Save(Ability.Intelligence));
            }
            else RemoveKeyword(new Save(Ability.Intelligence));
            if (desc != null && (desc.Contains("must make a wisdom saving throw") || desc.Contains("must succeed on a wisdom saving throw") || desc.Contains("must make wisdom saving throws")))
            {
                AddKeyword(new Save(Ability.Wisdom));
            }
            else RemoveKeyword(new Save(Ability.Wisdom));
            if (desc != null && (desc.Contains("must make a charisma saving throw") || desc.Contains("must succeed on a charisma saving throw") || desc.Contains("must make charisma saving throws")))
            {
                AddKeyword(new Save(Ability.Charisma));
            }
            else RemoveKeyword(new Save(Ability.Charisma));
            //Acid, Cold, Fire, Force, Lightning, Necrotic, Poison, Psychic, Radiant, Thunder
            if (desc != null && desc.Contains("acid damage")) AddKeyword(new Keyword("acid"));
            else RemoveKeyword(new Keyword("acid"));
            if (desc != null && desc.Contains("cold")) AddKeyword(new Keyword("cold"));
            else RemoveKeyword(new Keyword("cold damage"));
            if (desc != null && desc.Contains("fire damage")) AddKeyword(new Keyword("fire"));
            else RemoveKeyword(new Keyword("fire"));
            if (desc != null && desc.Contains("force damage")) AddKeyword(new Keyword("force"));
            else RemoveKeyword(new Keyword("force"));
            if (desc != null && desc.Contains("lightning damage")) AddKeyword(new Keyword("lightning"));
            else RemoveKeyword(new Keyword("lightning"));
            if (desc != null && desc.Contains("necrotic damage")) AddKeyword(new Keyword("necrotic"));
            else RemoveKeyword(new Keyword("necrotic"));
            if (desc != null && desc.Contains("poison damage")) AddKeyword(new Keyword("poison"));
            else RemoveKeyword(new Keyword("poison"));
            if (desc != null && desc.Contains("psychic damage")) AddKeyword(new Keyword("psychic"));
            else RemoveKeyword(new Keyword("psychic"));
            if (desc != null && desc.Contains("radiant damage")) AddKeyword(new Keyword("radiant"));
            else RemoveKeyword(new Keyword("radiant"));
            if (desc != null && desc.Contains("thunder damage")) AddKeyword(new Keyword("thunder"));
            else RemoveKeyword(new Keyword("thunder"));
            //Cone, Cube, Cylinder, Line, Sphere, Wall
            if (desc != null && desc.Contains("cone")) AddKeyword(new Keyword("cone"));
            else RemoveKeyword(new Keyword("cone"));
            if (desc != null && desc.Contains("cube")) AddKeyword(new Keyword("cube"));
            else RemoveKeyword(new Keyword("cube"));
            if (desc != null && desc.Contains("cylinder")) AddKeyword(new Keyword("cylinder"));
            else RemoveKeyword(new Keyword("cylinder"));
            if (desc != null && desc.Contains("line")) AddKeyword(new Keyword("line"));
            else RemoveKeyword(new Keyword("line"));
            if (desc != null && desc.Contains("sphere")) AddKeyword(new Keyword("sphere"));
            else RemoveKeyword(new Keyword("sphere"));
            if (desc != null && desc.Contains("wall")) AddKeyword(new Keyword("wall"));
            else RemoveKeyword(new Keyword("wall"));
            if (desc != null && desc.Contains("hit points")) AddKeyword(new Keyword("healing"));
            else RemoveKeyword(new Keyword("healing"));
        }


        public override string GetPath(Spell obj)
        {
            return PortablePath.Combine(obj.Source, Context.Config.Spells_Directory);
        }

        public List<Description> Descriptions { get => Model.Descriptions; }
        public List<CantripDamage> CantripDamage { get => Model.CantripDamage; }
        public List<Keyword> Keywords { get => Model.Keywords; }

        public List<string> CastingTimes
        {
            get => new List<string>
                {
                    "1 action",
                    "1 bonus action",
                    "1 minute",
                    "10 minutes",
                    "1 hour"
                };
        }
        public List<string> Ranges
        {
            get => new List<string>
                {
                    "Self",
                    "Touch",
                    "30 feet",
                    "60 feet",
                    "90 feet",
                    "120 feet"
                };
        }
        public List<string> Durations
        {
            get => new List<string>
                {
                    "Instantaneous",
                    "1 round",
                    "1 minute",
                    "10 minutes",
                    "1 hour",
                    "8 hours",
                    "24 hours",
                    "Concentration, up to 1 minute",
                    "Concentration, up to 10 minutes",
                    "Concentration, up to 1 hour",
                    "Concentration, up to 8 hours"
            };
        }
    }
}
