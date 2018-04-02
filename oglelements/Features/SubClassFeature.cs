using System;

namespace OGL.Features
{
    public class SubClassFeature: Feature
    {
        public String ParentClass { get; set; }
        public SubClassFeature() : base() 
        {
            Action = Base.ActionType.ForceHidden;
            ParentClass = "";
        }
        public SubClassFeature(string name, string text, String Class, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            ParentClass = Class;
        }

        public override string Displayname()
        {
            return "Subclass Feature";
        }
    }
}
