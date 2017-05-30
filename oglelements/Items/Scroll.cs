using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Character_Builder_5
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
