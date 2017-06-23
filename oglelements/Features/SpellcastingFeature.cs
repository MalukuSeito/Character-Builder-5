using OGL.Base;
using System;

namespace OGL.Features
{
    public class SpellcastingFeature: Feature
    {
        //public int MinSpellLevel = 1;
        //public int MaxSpellLevel = 1;
        public PreparationMode Preparation { get; set; }
        public string SpellcastingID { get; set; }
        public String PrepareableSpells { get; set; }
        public bool OverwrittenByMulticlassing { get; set; }
        public bool IgnoreMulticlassing { get { return !OverwrittenByMulticlassing; } set { OverwrittenByMulticlassing = !value; } }
        public String DisplayName { get; set; }
        public Ability PrepareCountAdditionalModifier { get; set; }
        public Ability SpellcastingAbility { get; set; }
        public double PrepareCountPerClassLevel { get; set; }
        public int PrepareCountAdditional { get; set; }
        public String PrepareCount { get; set; }
        public RechargeModifier PreparationChange { get; set; }
        public SpellcastingFeature() : base() { 
            OverwrittenByMulticlassing=true;
            SpellcastingID = "";
            DisplayName = "Spellcasting";
            Preparation = PreparationMode.LearnSpells;
            PreparationChange = RechargeModifier.LongRest;
            SpellcastingAbility=Ability.None;
        }
        public SpellcastingFeature(string name, string text, string spellcastingID, Ability spellcastingAbility, string displayName, PreparationMode preparation, string prepareableSpells, Ability prepareCountAdditionalModifier = Ability.None, int prepareCountPerClassLevel = 1, bool overwrittenByMulticlassing = true, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Preparation = preparation;
            SpellcastingID = spellcastingID;
            DisplayName = displayName;
            PrepareableSpells = prepareableSpells;
            OverwrittenByMulticlassing = overwrittenByMulticlassing;
            PrepareCountAdditionalModifier = prepareCountAdditionalModifier;
            PrepareCountPerClassLevel = prepareCountPerClassLevel;
            PreparationChange = RechargeModifier.LongRest;
            SpellcastingAbility=spellcastingAbility;
            PrepareCountAdditional = 0;

        }
        public SpellcastingFeature(string name, string text, string spellcastingID, Ability spellcastingAbility, string displayName, bool overwrittenByMulticlassing = true, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Preparation = PreparationMode.LearnSpells;
            SpellcastingID = spellcastingID;
            DisplayName = displayName;
            PrepareableSpells = "false";
            OverwrittenByMulticlassing = overwrittenByMulticlassing;
            PrepareCountAdditionalModifier = Ability.None;
            PrepareCountPerClassLevel = 0;
            PreparationChange = RechargeModifier.LongRest;
            SpellcastingAbility=spellcastingAbility;
            PrepareCountAdditional = 0;
        }
        public override string Displayname()
        {
            return "Spellcasting Feature";
        }
    }
}
