using System;

namespace Character_Builder
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
