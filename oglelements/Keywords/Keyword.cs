using OGL.Descriptions;
using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace OGL.Keywords
{
    [JsonDerivedType(typeof(Keyword), "Keyword")]
    [JsonDerivedType(typeof(Range), "Range")]
    [JsonDerivedType(typeof(Material), "Material")]
    [JsonDerivedType(typeof(Royalty), "Royalty")]
    [JsonDerivedType(typeof(Versatile), "Versatile")]
    [JsonDerivedType(typeof(Save), "Save")]
    [XmlInclude(typeof(Versatile)),
    XmlInclude(typeof(Range))]
    public class Keyword: IComparable<Keyword>
    {
        
        public String Name { get; set; }
        public Keyword() 
        {
            Name = "";
        }
        public Keyword(String keyword)
        {
            Name = keyword.ToLowerInvariant();
        }
        public virtual int CompareTo(Keyword other)
        {
            return Name.CompareTo(other.Name);
        }
        public override bool Equals(object other)
        {
            if (other is Keyword ) return Name == ((Keyword)other).Name;
            return false;
        }
        public override int GetHashCode()
        {
 	        return Name.GetHashCode();
        }
        public override string ToString()
        {
            if (Name.Length == 0)
            {
                return "ERROR";
            }
            return char.ToUpper(Name[0]) + Name.Substring(1);
        }
        public void check()
        {
            Name = Name.ToLowerInvariant();
        }
    }
}
