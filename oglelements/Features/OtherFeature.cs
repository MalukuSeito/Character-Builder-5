namespace OGL.Features
{
    public class OtherProficiencyFeature: Feature
    {
        public OtherProficiencyFeature() : base() {
            Action = Base.ActionType.ForceHidden;
        }
        public OtherProficiencyFeature(string name, string text, int level = 1, bool hidden = false) : base(name, text, level, hidden) {
            Action = Base.ActionType.ForceHidden;
        }
        public override string Displayname()
        {
            return "Other Proficiency Feature";
        }
    }
}
