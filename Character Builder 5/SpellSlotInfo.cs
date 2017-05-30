using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class SpellSlotInfo : IComparable<SpellSlotInfo>
    {
        public string SpellcastingID { get; set; }
        public int Level { get; set; }
        public int Slots { get; set; }
        public int Used { get; set; }
        public SpellSlotInfo(string spellcastingid, int level, int slots, int used)
        {
            SpellcastingID = spellcastingid;
            Level = level;
            Slots = slots;
            Used = used;
        }
        public override string ToString()
        {
            return AddOrdinal(Level) + ": " + (Slots - Used) + "/" + Slots;
        }
        public int CompareTo(SpellSlotInfo other)
        {
            return Level.CompareTo(other.Level);
        }
        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }
            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }
    }
}
