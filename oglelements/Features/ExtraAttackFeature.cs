namespace OGL.Features
{
    public class ExtraAttackFeature: Feature
    {
        public int ExtraAttacks { get; set; }
        public ExtraAttackFeature():base()
        {
            ExtraAttacks = 1;
        }
        public ExtraAttackFeature(string name, string text, int extraAttacks, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            ExtraAttacks = extraAttacks;
        }

        public override string Displayname()
        {
            return "Extra Attack Feature";
        }
    }
}
