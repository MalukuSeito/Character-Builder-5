using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Character_Builder_5
{
    [Flags]
    public enum BackgroundOption
    {
        [XmlEnum(Name = "None")]
        None = 0,
        [XmlEnum(Name = "Bond")]
        Bond = 1,
        [XmlEnum(Name = "Ideal")]
        Ideal = 2,
        [XmlEnum(Name = "Flaw")]
        Flaw = 4,
        [XmlEnum(Name = "Trait")]
        Trait = 8
    }
}
