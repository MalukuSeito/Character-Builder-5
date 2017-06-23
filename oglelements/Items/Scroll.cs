using OGL.Base;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace OGL.Items
{
    public class Scroll: Item
    {
        [XmlIgnore]
        private static XslCompiledTransform transform = new XslCompiledTransform();
        public Spell Spell;
        public Scroll()
        {

        }
        public Scroll(Spell spell) : base()
        {
            Spell = spell;
            Name = spell.Name;
            Price = new Price();
            Weight = 0;
            
        }
        public override string ToString()
        {
            return "Scroll of " + Name;
        }
    }
}
