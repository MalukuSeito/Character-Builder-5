using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class HitDie : IComparable<HitDie>
    {
        public int Dice { get; set; }
        public int Count { get; set; }
        public int Used { get; set; }
        public HitDie(int dice, int count, int used)
        {
            Dice = dice;
            Count = count;
            Used = used;
        }
        public override string ToString()
        {
            return (Count - Used) + "d" + Dice;
        }
        public int CompareTo(HitDie other)
        {
            return Dice.CompareTo(other.Dice);
        }
        public string Total()
        {
            return Count + "d" + Dice;
        }
    }
}
