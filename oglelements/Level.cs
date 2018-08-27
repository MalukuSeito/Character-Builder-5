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
        public List<int> Advancement = new List<int>();
        public List<int> Tier = new List<int>();
        [XmlIgnore]
        public static XmlSerializer Serializer = new XmlSerializer(typeof(Level));
        
        public void Sort() {
            Experience.Sort();
            Proficiency.Sort();
        }
        public int Get(int xp, bool advancement) {
            if (advancement)
            {
                GenerateAP();
                for (int level = 0; level < Advancement.Count; level++) if (Advancement[level] > xp) return level;
                return Advancement.Count;
            }
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

        public int GetAP(int level)
        {
            GenerateAP();
            if (level < 1) level = 1;
            if (level >= Advancement.Count) level = Advancement.Count;
            return Advancement[level - 1];
        }

        private void GenerateAP()
        {
            if (Advancement.Count == 0)
            {
                int adv = 0;
                for (int i = 0; i < 4; i++)
                {
                    Advancement.Add(adv);
                    adv += 4;
                }
                for (int i = 4; i < 20; i++)
                {
                    Advancement.Add(adv);
                    adv += 8;
                }
            }
            if (Tier.Count == 0)
            {
                Tier.AddRange(new int[] { 1, 1, 1, 1 });
                Tier.AddRange(new int[] { 2, 2, 2, 2, 2, 2 });
                Tier.AddRange(new int[] { 3, 3, 3, 3, 3, 3 });
                Tier.AddRange(new int[] { 4, 4, 4, 4 });
            }
        }

        public Level Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                Level r = (Level)Serializer.Deserialize(mem);
                return r;
            }
        }

        public int XpToLevelUp(int xp, bool advancement)
        {
            if (advancement)
            {
                GenerateAP();
                for (int level = 0; level < Advancement.Count; level++) if (Advancement[level] > xp) return Advancement[level] - xp;
                return 0;
            }
            for (int level = 0; level < Experience.Count; level++) if (Experience[level] > xp) return Experience[level] - xp;
            return 0;
        }
        public int XpToLevelDown(int xp, bool advancement)
        {
            if (advancement)
            {
                GenerateAP();
                if (xp < Advancement[1]) return 0;
                for (int level = 0; level < Advancement.Count; level++) if (Advancement[level] > xp) return xp - GetAP(level) + 1;
                return 1;
            }
            if (xp < Experience[1]) return 0;
            for (int level = 0; level < Experience.Count; level++) if (Experience[level] > xp) return xp - GetXP(level) + 1;
            return 1;
        }
        public int ToAP(int xp)
        {
            GenerateAP();
            int c = Math.Min(Experience.Count, Advancement.Count);
            for (int level = 0; level < c; level++)
            {
                if (Experience[level] > xp)
                {
                    int up = Experience[level] - xp;
                    int down = xp - GetXP(level);
                    int ap = Advancement[level] - GetAP(level);
                    if (up + down > 0)
                    {
                        return GetAP(level) + (int)Math.Ceiling((decimal)down / (up + down) * ap);
                    } else
                    {
                        return Advancement[level];
                    }
                }
            }
            return Advancement[c - 1];
        }
        public int ToXP(int ap)
        {
            GenerateAP();
            int c = Math.Min(Experience.Count, Advancement.Count);
            for (int level = 0; level < c; level++)
            {
                if (Advancement[level] > ap)
                {
                    int up = Advancement[level] - ap;
                    int down = ap - GetAP(level);
                    int xp = Experience[level] - GetXP(level);
                    if (up + down > 0)
                    {
                        return GetXP(level) + (int)Math.Ceiling((decimal)down / (up + down) * xp);
                    }
                    else
                    {
                        return Experience[level];
                    }
                }
            }
            return Experience[c - 1];
        }
    }
}
 