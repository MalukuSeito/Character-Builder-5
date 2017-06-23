using System;

namespace OGL.Keywords
{
    public class Versatile: Keyword
    {
        public String Damage { get; set; }
        public Versatile(): base("versatile")
        {
            Damage = "";
        }
        public Versatile(String damage): base("versatile")
        {
            Damage = damage;
        }
        public override int CompareTo(Keyword other)
        {
            if (Name != other.Name) return Name.CompareTo(other.Name);
            if (other is Versatile)
            {
                Versatile o = (Versatile)other;
                return Damage.CompareTo(o.Damage);
            }
            return 1;

        }
        public override bool Equals(object other)
        {
            if (other is Versatile) return Name.Equals(((Versatile)other).Name) && Damage == ((Versatile)other).Damage;
            return false;
        }
        public override int GetHashCode()
        {
            return (Name+"_"+Damage).GetHashCode();
        }
        public override string ToString()
        {
            return "Versatile (" + Damage + ")";
        }
    }
}
