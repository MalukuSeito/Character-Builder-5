using OGL.Base;
using System;

namespace OGL.Features
{
    public class ResourceFeature: Feature
    {
        public String Value { get; set; }
        public string ResourceID { get; set; }
        public RechargeModifier Recharge { get; set; }
        public string ExclusionID { get; set; } //Multiclassing: Channel Divinity
        public Ability ValueBonus = Ability.None;
        public ResourceFeature():base()
        {
            ResourceID = "";
            Value = "1";
            Recharge = RechargeModifier.LongRest;
            ExclusionID = "";
        }
        public ResourceFeature(string name, string text, string resourceID, string exclusionID, int value, RechargeModifier recharge, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Value = value.ToString();
            ResourceID = resourceID;
            ExclusionID = exclusionID;
            Recharge = recharge;
        }
        public override string Displayname()
        {
            return "Resource Feature";
        }
    }
}
