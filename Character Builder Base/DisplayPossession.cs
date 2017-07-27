using OGL;
using OGL.Base;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Character_Builder
{
    public class DisplayPossession: IXML
    {
        [XmlIgnore]
        private static XmlSerializer Serializer = new XmlSerializer(typeof(DisplayPossession));
        public String Name { get; set; }
        public int Count { get; set; }
        public String Description { get; set; }
        public double Weight { get; set; }
        public AttackInfo Info;
        public Item Item;
        public List<MagicProperty> Magic;
        public DisplayPossession()
        {

        }
        public DisplayPossession(Possession p, Player player)
        {
            Count = p.Count;
            Description = p.Description;
            Name = p.FullName;
            Weight = p.Weight;
            Item = p.Item;
            Magic = p.Magic;
            Info = player.GetAttack(p);
        }
        public String ToXML()
        {
            using (StringWriter mem = new StringWriter())
            {
                Serializer.Serialize(mem, this);
                return mem.ToString();
            }
        }

        public MemoryStream ToXMLStream()
        {
            MemoryStream mem = new MemoryStream();
            Serializer.Serialize(mem, this);
            return mem;
        }

    }
}
