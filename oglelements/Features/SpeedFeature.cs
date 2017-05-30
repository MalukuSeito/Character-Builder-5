using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
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
            Condition = "true";
            BaseSpeed = 0;
            ExtraSpeed = "0";
        }
        public SpeedFeature(string name, string text, int basespeed, int extraspeed = 0, bool ignoreArmor=false, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
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
