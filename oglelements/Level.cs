using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace OGL
{
    public class Level
    {
        public static Level Current { get; set; }
        public List<int> Experience = new List<int>();
        public List<int> Proficiency = new List<int>();
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Level));
        
        public void Sort() {
            Experience.Sort();
            Proficiency.Sort();
        }
        public static int Get(int xp) {
            for (int level = 0; level < Current.Experience.Count; level++) if (Current.Experience[level] > xp) return level;
            return Current.Experience.Count;
        }
        public static int GetProficiency(int level)
        {
            if (level < 1) level = 1;
            if (level >= Current.Proficiency.Count) level = Current.Proficiency.Count;
            return Current.Proficiency[level - 1];
        }
        public static int GetXP(int level)
        {
            if (level < 1) level = 1;
            if (level >= Current.Experience.Count) level = Current.Experience.Count;
            return Current.Experience[level - 1];
        }
        public static int XpToLevelUp(int xp)
        {
            for (int level = 0; level < Current.Experience.Count; level++) if (Current.Experience[level] > xp) return Current.Experience[level] - xp;
            return 0;
        }
        public static int XpToLevelDown(int xp)
        {
            if (xp < Current.Experience[1]) return 0;
            for (int level = 0; level < Current.Experience.Count; level++) if (Current.Experience[level] > xp) return xp - GetXP(level) + 1;
            return 1;
        }
        public static void Generate()
        {
            Current = new Level()
            {
                Experience = new List<int>() { 0, 300, 900, 2700, 6500, 14000, 23000, 34000, 48000, 64000, 85000, 100000, 120000, 140000, 165000, 195000, 225000, 265000, 305000, 355000 },
                Proficiency = new List<int>() { +2, +2, +2, +2, +3, +3, +3, +3, +4, +4, +4, +4, +5, +5, +5, +5, +6, +6, +6, +6 }
            };
            Current.Sort();
        }
    }
}
