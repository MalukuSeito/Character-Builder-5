using OGL;
using OGL.Base;
using System;

namespace Character_Builder
{
    public class AbilityFeatChoice
    {
        public string UniqueID { get; set; } = "";
        public int Level { get; set; } = 1;
        public String Class { get; set; } = "";
        public Ability Ability1 { get; set; } = Ability.None;
        public Ability Ability2 { get; set; } = Ability.None;
        public String Feat { get; set; } = null;
        public override string ToString()
        {
            if (Ability1 == Ability2)
                if (Ability1 == Ability.None)
                    if (Feat == null || Feat == "") return "Unassigned Ability Score Improvement";
                    else return "Feat: " + (ConfigManager.AlwaysShowSource ? Feat : SourceInvariantComparer.NoSource(Feat));
                else return "+2 " + Enum.GetName(typeof(Ability), Ability1);
            else if (Ability2 == Ability.None) return "+1 to " + Enum.GetName(typeof(Ability), Ability1) + " and ...";
            else return "+1 to " + Enum.GetName(typeof(Ability), Ability1) + " and " + Enum.GetName(typeof(Ability), Ability2);
        }

        public bool Assigned { get=> (Ability1 != Ability.None && Ability2 != Ability.None) || !string.IsNullOrWhiteSpace(Feat); }
    }
}
