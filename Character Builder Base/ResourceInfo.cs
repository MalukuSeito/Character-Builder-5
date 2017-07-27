using OGL.Base;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder
{
    public class ResourceInfo : IComparable<ResourceInfo>
    {
        public string ResourceID;
        public string Name;
        public int Max;
        public int Used;
        public RechargeModifier Recharge;
        public bool DisplayUsed;
        public ResourceInfo(string id, string name, int used, int max, RechargeModifier recharge, bool displayUsed)
        {
            ResourceID = id;
            Name = name;
            Used = used;
            Max = max;
            Recharge = recharge;
            DisplayUsed = displayUsed;
        }
        public override string ToString()
        {
            if (Recharge >= RechargeModifier.AtWill) return Name;
            if (DisplayUsed) return Name + ": " + (Max - Used) + "/" + Max + " (" + ModifiedSpell.RechargeName(Recharge) + ")";
            return Name + ": " + Max + " (" + ModifiedSpell.RechargeName(Recharge) + ")";
        }


        public int CompareTo(ResourceInfo other)
        {
            if (Max == other.Max) return Recharge.CompareTo(other.Recharge);
            return Max.CompareTo(other.Max);
        }

        public string Text
        {
            get => Name;
        }
        public string Desc
        {
            get
            {
                if (Recharge >= RechargeModifier.Charges) return RechargeModifier.AtWill == Recharge ? "" : ModifiedSpell.RechargeName(Recharge);
                if (DisplayUsed) return (Max - Used) + "/" + Max + " (" + ModifiedSpell.RechargeName(Recharge) + ")";
                return  Max + " (" + ModifiedSpell.RechargeName(Recharge) + ")";
            }
        }
    }
}
