namespace OGL.Features
{
    public class ExtraAttackFeature: Feature
    {
        public int ExtraAttacks { get; set; }
        public ExtraAttackFeature():base()
        {
            Action = Base.ActionType.ForceHidden;
            ExtraAttacks = 1;
        }
        public ExtraAttackFeature(string name, string text, int extraAttacks, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            ExtraAttacks = extraAttacks;
        }

        public override string Displayname()
        {
            return "Extra Attack Feature";
        }
    }
}
