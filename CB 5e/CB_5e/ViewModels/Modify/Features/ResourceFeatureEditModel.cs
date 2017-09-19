using OGL.Base;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{
    public class ResourceFeatureEditModel : FeatureEditModel<ResourceFeature>
    {
        public static HashSet<string> ResourceIDs = new HashSet<string>();
        public static HashSet<string> ExclusionIDs = new HashSet<string>();
        public static string convert(Ability a, string s = null)
        {
            StringBuilder sb = new StringBuilder();
            if (s != null && s.Trim() != "" && s.Trim() != "0") sb.Append(s);
            if (a.HasFlag(Ability.Strength)) sb.Append((sb.Length > 0 ? " + " : "") + "StrMod");
            if (a.HasFlag(Ability.Dexterity)) sb.Append((sb.Length > 0 ? " + " : "") + "DexMod");
            if (a.HasFlag(Ability.Constitution)) sb.Append((sb.Length > 0 ? " + " : "") + "ConMod");
            if (a.HasFlag(Ability.Intelligence)) sb.Append((sb.Length > 0 ? " + " : "") + "IntMod");
            if (a.HasFlag(Ability.Wisdom)) sb.Append((sb.Length > 0 ? " + " : "") + "WisMod");
            if (a.HasFlag(Ability.Charisma)) sb.Append((sb.Length > 0 ? " + " : "") + "ChaMod");
            return sb.ToString();
        }
        public ResourceFeatureEditModel(ResourceFeature f, IEditModel parent, FeatureViewModel fvm) : base(f, parent, fvm)
        {
            if (f.ValueBonus != Ability.None)
            {
                f.Value = convert(f.ValueBonus, f.Value);
                f.ValueBonus = Ability.None;
            }
        }
        public string ResourceID
        {
            get => Feature.ResourceID;
            set
            {
                if (value == null) return;
                if (value == ResourceID) return;
                MakeHistory("ResourceID");
                Feature.ResourceID = value;
                OnPropertyChanged("ResourceID");
            }
        }
        public string ExclusionID
        {
            get => Feature.ExclusionID;
            set
            {
                if (value == null) return;
                if (value == ExclusionID) return;
                MakeHistory("ExclusionID");
                Feature.ExclusionID = value;
                OnPropertyChanged("ExclusionID");
            }
        }
        public string Value
        {
            get => Feature.Value;
            set
            {
                if (value == Value) return;
                MakeHistory("Value");
                Feature.Value = value;
                OnPropertyChanged("Value");
            }
        }

        public String Recharge
        {
            get => Feature.Recharge.ToString();
            set
            {
                if (value == Recharge) return;
                if (Enum.TryParse(value, out RechargeModifier v))
                {    
                    MakeHistory("Recharge");
                    Feature.Recharge = v;
                    OnPropertyChanged("Recharge");
                }
            }
        }

        public List<String> RechargeValues { get => Enum.GetNames(typeof(RechargeModifier)).ToList(); }
        public List<String> ResourceIDValues { get => ResourceIDs.OrderBy(s => s).ToList(); }
        public List<String> ExclusionIDValues { get => ResourceIDs.OrderBy(s => s).ToList(); }
    }
}
