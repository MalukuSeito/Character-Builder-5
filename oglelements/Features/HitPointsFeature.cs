using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class HitPointsFeature: Feature

    {
        public String HitPoints { get; set; }
        public int HitPointsPerLevel { get; set; }
        public HitPointsFeature() : base(null, null, 1, true) 
        {
            HitPoints = "0";
            HitPointsPerLevel = 0;
        }
        public HitPointsFeature(string name, string text, int hp, int hplevel, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            HitPoints = hp.ToString();
            HitPointsPerLevel = hplevel;
        }
        public override string Displayname()
        {
            return "Hit Point Feature";
        }
    }
}
