using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class CollectionChoiceFeature: Feature
    {
        public int Amount { get; set; }
        public String UniqueID { get; set; }
        public String Collection {get; set;}
        public bool AllowSameChoice { get; set; } = false;
        public CollectionChoiceFeature()
            : base()
        {
            Collection = "";
            Amount = 1;
        }
        public CollectionChoiceFeature(string name, string text, string uniqueID, String collection, int amount = 1, int level = 1, bool hidden = false)
        : base(name, text, level, hidden)
        {
            Collection = collection;
            Amount = amount;
            UniqueID = uniqueID;
        }
        public override List<Feature> Collect(int level, IChoiceProvider choiceProvider)
        {
            if (Level > level) return new List<Feature>();
            int offset = choiceProvider.getChoiceOffset(this, UniqueID, Amount);
            List<Feature> res= new List<Feature>() { this };
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = choiceProvider.getChoice(UniqueID + counter);
                if (cho != null && cho.Value != "")
                {
                    Feature feat = FeatureCollection.Get(Collection, AllowSameChoice ? c : 0).Find(fe => fe.Name + " " + ConfigManager.SourceSeperator + " " + fe.Source == cho.Value);
                    if (feat == null) feat = FeatureCollection.Get(Collection, AllowSameChoice ? c : 0).Find(fe => ConfigManager.SourceInvariantComparer.Equals(fe.Name + " " + ConfigManager.SourceSeperator + " " + fe.Source, cho.Value));
                    if (feat != null) res.AddRange(feat.Collect(level, choiceProvider));
                }
            }
            return res;
        }
        public List<string> Choices(IChoiceProvider choiceProvider)
        {
            int offset = choiceProvider.getChoiceOffset(this, UniqueID, Amount);
            List<string> res = new List<string>();
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = choiceProvider.getChoice(UniqueID + counter);
                if (cho != null && cho.Value != "") res.Add(cho.Value);
            }
            return res;
        }

        public override string Displayname()
        {
            return "Feature Collection Choice Feature";
        }
    }
}
