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
        public static AbilityScores Current { get; set; }
        public static XmlSerializer Serializer = new XmlSerializer(typeof(AbilityScores));
        public List<int> PointBuyCost = new List<int>();
        public int PointBuyPoints { get; set; }
        public int PointBuyMinScore { get; set; }
        public int PointBuyMaxScore { get; set; }
        public int DefaultMax { get; set; }
        public List<String> Arrays = new List<String>();
        [XmlIgnore]
        public string Filename { get; set; }
        
        public static int GetMod(int score)
        {
            return (score - 10) >> 1;
        }
        public static int getPointBuyCost(int score) {
            if (score > Current.PointBuyMaxScore) return -1;
            if (score < Current.PointBuyMinScore) return -1;
            return Current.PointBuyCost[score - Current.PointBuyMinScore];
        }
        public static int getPointBuyPoints()
        {
            return Current.PointBuyPoints;
        }
        public static List<AbilityScoreArray> GetArrays()
        {
            List<AbilityScoreArray> res = new List<AbilityScoreArray>();
            foreach (String s in Current.Arrays) res.Add(new AbilityScoreArray(s));
            return res;
        }
        
        public static void Generate()
        {
            Current = new AbilityScores()
            {
                PointBuyCost = new List<int>() { 0, 1, 2, 3, 4, 5, 7, 9 },
                PointBuyPoints = 27,
                PointBuyMinScore = 8,
                PointBuyMaxScore = 15
            };
            foreach (AbilityScoreArray a in AbilityScoreArray.Generate()) Current.Arrays.Add(a.ToString());
        }
        public static int Max { get {
                if (Current.DefaultMax == 0) Current.DefaultMax = 20;
                return Current.DefaultMax;
        } }

        
    }
}
