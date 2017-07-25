using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace OGL
{
    public class Level
    {
        public List<int> Experience = new List<int>();
        public List<int> Proficiency = new List<int>();
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Level));
        
        public void Sort() {
            Experience.Sort();
            Proficiency.Sort();
        }
        public int Get(int xp) {
            for (int level = 0; level < Experience.Count; level++) if (Experience[level] > xp) return level;
            return Experience.Count;
        }
        public int GetProficiency(int level)
        {
            if (level < 1) level = 1;
            if (level >= Proficiency.Count) level = Proficiency.Count;
            return Proficiency[level - 1];
        }
        public int GetXP(int level)
        {
            if (level < 1) level = 1;
            if (level >= Experience.Count) level = Experience.Count;
            return Experience[level - 1];
        }
        public int XpToLevelUp(int xp)
        {
            for (int level = 0; level < Experience.Count; level++) if (Experience[level] > xp) return Experience[level] - xp;
            return 0;
        }
        public int XpToLevelDown(int xp)
        {
            if (xp < Experience[1]) return 0;
            for (int level = 0; level < Experience.Count; level++) if (Experience[level] > xp) return xp - GetXP(level) + 1;
            return 1;
        }
    }
}
