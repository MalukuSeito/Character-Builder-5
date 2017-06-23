using System.Xml.Serialization;

namespace OGL.Base
{
    public enum RechargeModifier
    {
        [XmlEnum(Name = "Unmodified")]
        Unmodified = 0,
        [XmlEnum(Name = "NoRecharge")]
        NoRecharge = 1,
        [XmlEnum(Name = "Special")]
        Special = 2,
        [XmlEnum(Name = "Dawn")]
        Dawn = 3,
        [XmlEnum(Name = "Dusk")]
        Dusk = 4,
        [XmlEnum(Name = "LongRest")]
        LongRest = 5,
        [XmlEnum(Name = "ShortRest")]
        ShortRest = 6,
        [XmlEnum(Name = "Charges")]
        Charges = 7,
        [XmlEnum(Name = "AtWill")]
        AtWill = 8,
        [XmlEnum(Name = "Ritual")]
        Ritual = 9,
    }
}
