using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class Material: Keyword
    {
        public String Components { get; set; }
        public Material(): base("material")
        {
            Components = "";
        }
        public Material(String components)
            : base("material")
        {
            Components = components;
        }
        public override int CompareTo(Keyword other)
        {
            if (Name != other.Name) return Name.CompareTo(other.Name);
            if (other is Material)
            {
                Material o = (Material)other;
                return Components.CompareTo(o.Components);
            }
            return 1;

        }
        public override bool Equals(object other)
        {
            if (other is Material) return Name.Equals(((Material)other).Name) && Components == ((Material)other).Components;
            return false;
        }
        public override int GetHashCode()
        {
            return (Name + "_" + Components).GetHashCode();
        }
        public override string ToString()
        {
            return "Material (" + Components + ")";
        }
    }
}
