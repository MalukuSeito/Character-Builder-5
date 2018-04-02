namespace OGL.Features
{
    public class ACFeature : Feature
    {
        public string Expression { get; set; }
        public ACFeature()
            : base()
        {
            Action = Base.ActionType.ForceHidden;
            Expression = "0";
        }
        public ACFeature(string name, string text, string expression, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            Expression = expression;
        }
        public override string Displayname()
        {
            return "AC Calculation Feature";
        }
    }
}
