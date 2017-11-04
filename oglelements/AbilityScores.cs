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
        public int GetPointBuyCost(int score) {
            if (score > PointBuyMaxScore) return -1;
            if (score < PointBuyMinScore) return -1;
            return PointBuyCost[score - PointBuyMinScore];
        }
        public int GetPointBuyPoints()
        {
            return PointBuyPoints;
        }

        public AbilityScores Clone()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                Serializer.Serialize(mem, this);
                mem.Seek(0, SeekOrigin.Begin);
                AbilityScores r = (AbilityScores)Serializer.Deserialize(mem);
                return r;
            }
        }

        public List<AbilityScoreArray> GetArrays()
        {
            List<AbilityScoreArray> res = new List<AbilityScoreArray>();
            foreach (String s in Arrays) res.Add(new AbilityScoreArray(s));
            return res;
        }
        
        public int Max { get {
                if (DefaultMax == 0) DefaultMax = 20;
                return DefaultMax;
        } }

        
    }
}
