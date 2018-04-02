using System;

namespace OGL.Features
{
    public class SpeedFeature : Feature
    {
        public int BaseSpeed { get; set; }
        public String ExtraSpeed { get; set; }
        public bool IgnoreArmor { get; set; }
        public String Condition { get; set; }
        public SpeedFeature()
            : base()
        {
            Action = Base.ActionType.ForceHidden;
            Condition = "true";
            BaseSpeed = 0;
            ExtraSpeed = "0";
        }
        public SpeedFeature(string name, string text, int basespeed, int extraspeed = 0, bool ignoreArmor=false, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            BaseSpeed = basespeed;
            ExtraSpeed = extraspeed.ToString();
            IgnoreArmor = ignoreArmor;
        }
        public override string Displayname()
        {
            return "Speed Feature";
        }
    }
}
