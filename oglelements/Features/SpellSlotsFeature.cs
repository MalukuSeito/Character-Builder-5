using System;
using System.Collections.Generic;

namespace OGL.Features
{
    public class SpellSlotsFeature : Feature
    {
        public List<int> Slots { get; set; }
        public string SpellcastingID { get; set; }

        public bool Add { get; set; } = false;
        public SpellSlotsFeature()
            : base()
        {
            Action = Base.ActionType.ForceHidden;
            SpellcastingID = "";
            Slots = new List<int>();
        }
        //"Spell slots", SpellSlotsFeature.tostring(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth), level, hidden)
        public SpellSlotsFeature(string name, string text, string spellcastingID, List<int> slots, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            SpellcastingID = spellcastingID;
            Slots = slots;
        }
        public SpellSlotsFeature(string spellcastingID, int level, int first = 0, int second = 0, int third = 0, int fourth = 0, int fifth = 0, int sixth = 0, int seventh = 0, int eighth = 0, int ninth = 0)
            : base("Spell slots", SpellSlotsFeature.tostring(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth), level, true)
        {
            Action = Base.ActionType.ForceHidden;
            SpellcastingID = spellcastingID;
            Slots = new List<int>() { first, second, third, fourth, fifth, sixth, seventh, eighth, ninth };
        }
        public static string tostring(int first, int second, int third, int fourth, int fifth, int sixth, int seventh, int eighth, int ninth)
        {
            List<string> slots = new List<string>();
            if (first > 0) slots.Add("1st: " + first);
            if (second > 0) slots.Add("2nd: " + second);
            if (third > 0) slots.Add("3rd: " + third);
            if (fourth > 0) slots.Add("4th: " + fourth);
            if (fifth > 0) slots.Add("5th: " + fifth);
            if (sixth > 0) slots.Add("6th: " + sixth);
            if (seventh > 0) slots.Add("7th: " + seventh);
            if (eighth > 0) slots.Add("8th: " + eighth);
            if (ninth > 0) slots.Add("9th: " + ninth);
            return String.Join(", ", slots);
        }

        public string tostring()
        {
            List<string> slots = new List<string>();
            if (Slots.Count > 0 && Slots[0] > 0) slots.Add("1st: " + Slots[0]);
            if (Slots.Count > 1 && Slots[1] > 0) slots.Add("2nd: " + Slots[1]);
            if (Slots.Count > 2 && Slots[2] > 0) slots.Add("3rd: " + Slots[2]);
            if (Slots.Count > 3 && Slots[3] > 0) slots.Add("4th: " + Slots[3]);
            if (Slots.Count > 4 && Slots[4] > 0) slots.Add("5th: " + Slots[4]);
            if (Slots.Count > 5 && Slots[5] > 0) slots.Add("6th: " + Slots[5]);
            if (Slots.Count > 6 && Slots[6] > 0) slots.Add("7th: " + Slots[6]);
            if (Slots.Count > 7 && Slots[7] > 0) slots.Add("8th: " + Slots[7]);
            if (Slots.Count > 8 && Slots[8] > 0) slots.Add("9th: " + Slots[8]);
            return String.Join(", ", slots);
        }
        public override string Displayname()
        {
            return "Spellslot Feature";
        }
    }
}
