namespace OGL.Features
{
    public class SpellModifyFeature: Feature
    {
        public string Spells { get; set; }
        public SpellModifyFeature():base()
        {
            Spells = "";
        }
        public SpellModifyFeature(string name, string text, string spells, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Spells = spells;
        }
        public override string Displayname()
        {
            return "Spell Modify Feature";
        }
    }
}
