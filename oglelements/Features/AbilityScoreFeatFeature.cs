using System;

namespace OGL.Features
{
    public class AbilityScoreFeatFeature : Feature
    {
        public String UniqueID { get; set; }
        public AbilityScoreFeatFeature() : base() { }
        public AbilityScoreFeatFeature(string uniqueID, int level, String description = "You can increase one ability score of your choice by 2, or you can increase two ability scores of your choice by 1. As normal, you can’t increase an ability score above 20 using this feature.")
            : base("Ability Score Improvement", description, level, true)
        {
            UniqueID = uniqueID;
        }
        public override string Displayname()
        {
            return "Ability Score / Feat Selection Feature";
        }
    }
}
