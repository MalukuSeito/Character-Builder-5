namespace OGL.Features
{
    public class VisionFeature: Feature
    {
        public int Range { get; set; }
        public VisionFeature()
            : base()
        {
            Range = 0;
        }
        public VisionFeature(string name, string text, int range, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Range = range;
        }
        public override string Displayname()
        {
            return "Vision Feature";
        }
    }
    
}
