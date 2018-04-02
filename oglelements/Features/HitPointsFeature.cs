using System;

namespace OGL.Features
{
    public class HitPointsFeature: Feature

    {
        public String HitPoints { get; set; }
        public int HitPointsPerLevel { get; set; }
        public HitPointsFeature() : base(null, null, 1, true) 
        {
            Action = Base.ActionType.ForceHidden;
            HitPoints = "0";
            HitPointsPerLevel = 0;
        }
        public HitPointsFeature(string name, string text, int hp, int hplevel, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            HitPoints = hp.ToString();
            HitPointsPerLevel = hplevel;
        }
        public override string Displayname()
        {
            return "Hit Point Feature";
        }
    }
}
