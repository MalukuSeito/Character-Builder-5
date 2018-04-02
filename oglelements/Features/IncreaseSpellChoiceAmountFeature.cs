namespace OGL.Features
{
    public class IncreaseSpellChoiceAmountFeature : Feature
    {
        public string UniqueID { get; set; }
        public int Amount { get; set; }
        public IncreaseSpellChoiceAmountFeature()
            : base()
        {
            Action = Base.ActionType.ForceHidden;
            Amount = 1;
        }
        public IncreaseSpellChoiceAmountFeature(int level, string uniqueID, int amount = 1)
            : base("", "", level, true)
        {
            Action = Base.ActionType.ForceHidden;
            UniqueID = uniqueID;
            Amount = amount;
        }
        public override string Displayname()
        {
            return "Increase Spellchoices Feature";
        }
    }
}
