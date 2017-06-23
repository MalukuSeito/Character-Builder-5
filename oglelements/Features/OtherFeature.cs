namespace OGL.Features
{
    public class OtherProficiencyFeature: Feature
    {
        public OtherProficiencyFeature() : base() { }
        public OtherProficiencyFeature(string name, string text, int level = 1, bool hidden = false) : base(name, text, level, hidden) { }
        public override string Displayname()
        {
            return "Other Proficiency Feature";
        }
    }
}
