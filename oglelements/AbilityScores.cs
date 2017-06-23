using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace OGL
{
    public class AbilityScores
    {
        private static AbilityScores current;
        public List<int> PointBuyCost = new List<int>();
        public int PointBuyPoints { get; set; }
        public int PointBuyMinScore { get; set; }
        public int PointBuyMaxScore { get; set; }
        public int DefaultMax { get; set; }
        public List<String> Arrays = new List<String>();
        [XmlIgnore]
        private static XmlSerializer serializer = new XmlSerializer(typeof(AbilityScores));
        [XmlIgnore]
        private string filename;
        public static AbilityScores Load(String file)
        {
            using (TextReader reader = new StreamReader(file)) current = (AbilityScores)serializer.Deserialize(reader);
            current.filename = file;
            return current;
        }
        public void Save(String file)
        {
            using (TextWriter writer = new StreamWriter(file)) serializer.Serialize(writer, this);
        }
        public static int getMod(int score)
        {
            return (score - 10) >> 1;
        }
        public static int getPointBuyCost(int score) {
            if (score > current.PointBuyMaxScore) return -1;
            if (score < current.PointBuyMinScore) return -1;
            return current.PointBuyCost[score - current.PointBuyMinScore];
        }
        public static int getPointBuyPoints()
        {
            return current.PointBuyPoints;
        }
        public static List<AbilityScoreArray> GetArrays()
        {
            List<AbilityScoreArray> res = new List<AbilityScoreArray>();
            foreach (String s in current.Arrays) res.Add(new AbilityScoreArray(s));
            return res;
        }
        
        public static void Generate()
        {
            current = new AbilityScores();
            current.PointBuyCost = new List<int>() { 0, 1, 2, 3, 4, 5, 7, 9 };
            current.PointBuyPoints = 27;
            current.PointBuyMinScore = 8;
            current.PointBuyMaxScore = 15;
            foreach (AbilityScoreArray a in AbilityScoreArray.Generate()) current.Arrays.Add(a.ToString());
            current.Save(ConfigManager.loaded.AbilityScores);
        }
        public static int Max { get {
                if (current.DefaultMax == 0) current.DefaultMax = 20;
                return current.DefaultMax;
        } }

        public static void Save()
        {
            current.Save(current.filename);
        }
    }
}
