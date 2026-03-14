namespace OGL.Features
{
    public class VisionFeature: Feature
    {
        public int Range { get; set; }
        public bool Additive { get; set; }
        public VisionFeature()
            : base()
        {
            Action = Base.ActionType.ForceHidden;
            Range = 0;
            Additive = false;
        }
        public VisionFeature(string name, string text, int range, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            Range = range;
			Additive = false;
		}
        public override string Displayname()
        {
            return "Vision Feature";
        }
    }
    
}
