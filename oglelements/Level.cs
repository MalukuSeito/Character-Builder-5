using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Character_Builder_5
{
    public class Level
    {
        private static Level current;
        public List<int> Experience = new List<int>();
        public List<int> Proficiency = new List<int>();
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(Level));
        public static Level Load(String file)
        {
            using (TextReader reader = new StreamReader(file)) current=(Level)serializer.Deserialize(reader);
            current.Sort();
            return current;
        }
        public static void Save(String file)
        {
            using (TextWriter writer = new StreamWriter(file)) serializer.Serialize(writer, current);
        }
        private void Sort() {
            Experience.Sort();
            Proficiency.Sort();
        }
        public static int Get(int xp) {
            for (int level = 0; level < current.Experience.Count; level++) if (current.Experience[level] > xp) return level;
            return current.Experience.Count;
        }
        public static int GetProficiency(int level)
        {
            if (level < 1) level = 1;
            if (level >= current.Proficiency.Count) level = current.Proficiency.Count;
            return current.Proficiency[level - 1];
        }
        public static int GetXP(int level)
        {
            if (level < 1) level = 1;
            if (level >= current.Experience.Count) level = current.Experience.Count;
            return current.Experience[level - 1];
        }
        public static int XpToLevelUp(int xp)
        {
            for (int level = 0; level < current.Experience.Count; level++) if (current.Experience[level] > xp) return current.Experience[level] - xp;
            return 0;
        }
        public static int XpToLevelDown(int xp)
        {
            if (xp < current.Experience[1]) return 0;
            for (int level = 0; level < current.Experience.Count; level++) if (current.Experience[level] > xp) return xp - GetXP(level) + 1;
            return 1;
        }
        public static void Generate()
        {
            current = new Level();
            current.Experience = new List<int>() { 0, 300, 900, 2700, 6500, 14000, 23000, 34000, 48000, 64000, 85000, 100000, 120000, 140000, 165000, 195000, 225000, 265000, 305000, 355000 };
            current.Proficiency = new List<int>() { +2, +2, +2, +2, +3, +3, +3, +3, +4, +4, +4, +4, +5, +5, +5, +5, +6, +6, +6, +6};
            current.Sort();
            Save(ConfigManager.loaded.Levels);
        }
    }
}
