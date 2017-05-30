using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class DisplayPossession
    {
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
        public DisplayPossession(Possession p)
        {
            Count = p.Count;
            Description = p.Description;
            Name = p.Name;
            Weight = p.Weight;
            Item = p.Item;
            Magic = p.Magic;
            Info = Player.current.getAttack(p);
        }
    }
}
