using System;

namespace OGL.Keywords
{
    public class Royalty : Keyword
    {
        public string Price { get; set; }
        public Royalty() : base("royalty")
        {
            Price = "";
        }
        public Royalty(string price)
            : base("royalty")
        {
            Price = price;
        }
        public override int CompareTo(Keyword other)
        {
            if (Name != other.Name) return Name.CompareTo(other.Name);
            if (other is Royalty)
            {
                Royalty o = (Royalty)other;
                return Price.CompareTo(o.Price);
            }
            return 1;

        }
        public override bool Equals(object other)
        {
            if (other is Royalty) return Name.Equals(((Royalty)other).Name) && Price == ((Royalty)other).Price;
            return false;
        }
        public override int GetHashCode()
        {
            return (Name + "_" + Price).GetHashCode();
        }
        public override string ToString()
        {
            return "Royalty (" + Price + ")";
        }
    }
}
