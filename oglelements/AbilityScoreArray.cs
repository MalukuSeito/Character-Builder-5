using OGL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OGL
{
    public class AbilityScoreArray
    {
        [XmlAttribute]
        public int Strength { get; set; }
        [XmlAttribute]
        public int Dexterity { get; set; }
        [XmlAttribute]
        public int Constitution { get; set; }
        [XmlAttribute]
        public int Intelligence { get; set; }
        [XmlAttribute]
        public int Wisdom { get; set; }
        [XmlAttribute]
        public int Charisma { get; set; }
        [XmlIgnore]
        public int StrMod
        {
            get
            {
                return AbilityScores.GetMod(Strength);
            }
        }
        [XmlIgnore]
        public int DexMod
        {
            get
            {
                return AbilityScores.GetMod(Dexterity);
            }
        }
        [XmlIgnore]
        public int ConMod
        {
            get
            {
                return AbilityScores.GetMod(Constitution);
            }
        }
        [XmlIgnore]
        public int IntMod
        {
            get
            {
                return AbilityScores.GetMod(Intelligence);
            }
        }
        [XmlIgnore]
        public int WisMod
        {
            get
            {
                return AbilityScores.GetMod(Wisdom);
            }
        }
        [XmlIgnore]
        public int ChaMod
        {
            get
            {
                return AbilityScores.GetMod(Charisma);
            }
        }
        public AbilityScoreArray(int str, int dex, int con, int intel, int wis, int cha)
        {
            Strength = str;
            Dexterity = dex;
            Constitution = con;
            Intelligence = intel;
            Wisdom = wis;
            Charisma = cha;
        }
        public AbilityScoreArray(String scores)
        {
            String[] score = scores.Split(',');
            Strength = int.Parse(score[0].Trim());
            Dexterity = int.Parse(score[1].Trim()); ;
            Constitution = int.Parse(score[2].Trim()); ;
            Intelligence = int.Parse(score[3].Trim()); ;
            Wisdom = int.Parse(score[4].Trim()); ;
            Charisma = int.Parse(score[5].Trim()); ;
        }
        public static List<AbilityScoreArray> Generate()
        {
            return new List<AbilityScoreArray>() {
                new AbilityScoreArray("15,14,13,12,10,8"),
                new AbilityScoreArray("15,15,15,8,8,8"),
                new AbilityScoreArray("13,13,13,12,12,12")
            };
        }
        public List<int> Get()
        {
            return new List<int>() { Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma };
        }
        public override string ToString()
        {
            return String.Join(", ", Get());
        }
        public void Max(AbilityScoreArray other)
        {
            Strength = Math.Max(Strength, other.Strength);
            Dexterity = Math.Max(Dexterity, other.Dexterity);
            Constitution = Math.Max(Constitution, other.Constitution);
            Intelligence = Math.Max(Intelligence, other.Intelligence);
            Wisdom = Math.Max(Wisdom, other.Wisdom);
            Charisma = Math.Max(Charisma, other.Charisma);
        }
        public void Min(AbilityScoreArray other)
        {
            Strength = Math.Min(Strength, other.Strength);
            Dexterity = Math.Min(Dexterity, other.Dexterity);
            Constitution = Math.Min(Constitution, other.Constitution);
            Intelligence = Math.Min(Intelligence, other.Intelligence);
            Wisdom = Math.Min(Wisdom, other.Wisdom);
            Charisma = Math.Min(Charisma, other.Charisma);
        }
        public void Add(AbilityScoreArray other)
        {
            Strength += other.Strength;
            Dexterity += other.Dexterity;
            Constitution += other.Constitution;
            Intelligence += other.Intelligence;
            Wisdom += other.Wisdom;
            Charisma += other.Charisma;
        }
        public int Apply(Ability ab)
        {
            int value = 0;
            if (ab.HasFlag(Ability.Charisma)) value = Math.Max(value, Charisma);
            if (ab.HasFlag(Ability.Constitution)) value = Math.Max(value, Constitution);
            if (ab.HasFlag(Ability.Dexterity)) value = Math.Max(value, Dexterity);
            if (ab.HasFlag(Ability.Intelligence)) value = Math.Max(value, Intelligence);
            if (ab.HasFlag(Ability.Strength)) value = Math.Max(value, Strength);
            if (ab.HasFlag(Ability.Wisdom)) value = Math.Max(value, Wisdom);
            return value;
        }
        public int ApplyMod(Ability ab)
        {
            int value = -5;
            if (ab == Ability.None) return 0;
            if (ab.HasFlag(Ability.Charisma)) value = Math.Max(value, ChaMod);
            if (ab.HasFlag(Ability.Constitution)) value = Math.Max(value, ConMod);
            if (ab.HasFlag(Ability.Dexterity)) value = Math.Max(value, DexMod);
            if (ab.HasFlag(Ability.Intelligence)) value = Math.Max(value, IntMod);
            if (ab.HasFlag(Ability.Strength)) value = Math.Max(value, StrMod);
            if (ab.HasFlag(Ability.Wisdom)) value = Math.Max(value, WisMod);
            return value;
        }
        public Ability Highest(Ability ab)
        {
            int value = 0;
            Ability high = Ability.None;
            if (ab.HasFlag(Ability.Charisma) && Charisma > value)
            {
                value = Charisma;
                high = Ability.Charisma;
            }
            if (ab.HasFlag(Ability.Constitution) && Constitution > value)
            {
                value = Constitution;
                high = Ability.Constitution;
            }
            if (ab.HasFlag(Ability.Dexterity) && Dexterity > value)
            {
                value = Dexterity;
                high = Ability.Dexterity;
            }
            if (ab.HasFlag(Ability.Intelligence) && Intelligence > value)
            {
                value = Intelligence;
                high = Ability.Intelligence;
            }
            if (ab.HasFlag(Ability.Strength) && Strength > value)
            {
                high = Ability.Strength;
                value = Strength;
            }
            if (ab.HasFlag(Ability.Wisdom) && Wisdom > value)
            {
                high = Ability.Wisdom;
                value = Wisdom;
            }
            return high;
        }

        public void AddBonus(int bonus, Ability ab)
        {
            if (ab.HasFlag(Ability.Charisma)) Charisma += bonus;
            if (ab.HasFlag(Ability.Constitution)) Constitution += bonus;
            if (ab.HasFlag(Ability.Dexterity)) Dexterity += bonus;
            if (ab.HasFlag(Ability.Intelligence)) Intelligence += bonus;
            if (ab.HasFlag(Ability.Strength)) Strength += bonus;
            if (ab.HasFlag(Ability.Wisdom)) Wisdom += bonus;
        }
    }
}
