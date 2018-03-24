using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OGL.Base
{
    public enum ActionType
    {
        [XmlEnum(Name = "Default")]
        DetectAction = 0,
        [XmlEnum(Name = "Action")]
        Action = 1,
        [XmlEnum(Name = "Bonus")]
        BonusAction = 2,
        [XmlEnum(Name = "Reaction")]
        Reaction = 4,
        [XmlEnum(Name = "Move")]
        MoveAction = 8,
        [XmlEnum(Name = "Other")]
        Other = 16,
        [XmlEnum(Name = "Hidden")]
        ForceHidden = 32
        
    }
}
