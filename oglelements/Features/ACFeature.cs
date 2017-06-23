namespace OGL.Features
{
    public class ACFeature : Feature
    {
        public string Expression { get; set; }
        public ACFeature()
            : base()
        {
            Expression = "0";
        }
        public ACFeature(string name, string text, string expression, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Expression = expression;
        }
        public override string Displayname()
        {
            return "AC Calculation Feature";
        }
    }
}
