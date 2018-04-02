using OGL.Common;
using System;
using System.Collections.Generic;

namespace OGL.Features
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
            Action = Base.ActionType.ForceHidden;
            Collection = "";
            Amount = 1;
        }
        public CollectionChoiceFeature(string name, string text, string uniqueID, String collection, int amount = 1, int level = 1, bool hidden = false)
        : base(name, text, level, hidden)
        {
            Action = Base.ActionType.ForceHidden;
            Collection = collection;
            Amount = amount;
            UniqueID = uniqueID;
        }
        public override List<Feature> Collect(int level, IChoiceProvider choiceProvider, OGLContext context)
        {
            if (Level > level) return new List<Feature>();
            int offset = choiceProvider.GetChoiceOffset(this, UniqueID, Amount);
            List<Feature> res= new List<Feature>() { this };
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = choiceProvider.GetChoice(UniqueID + counter);
                if (cho != null && cho.Value != "")
                {
                    Feature feat = context.GetFeatureCollection(Collection, AllowSameChoice ? c : 0).Find(fe => fe.Name + " " + ConfigManager.SourceSeperator + " " + fe.Source == cho.Value);
                    if (feat == null) feat = context.GetFeatureCollection(Collection, AllowSameChoice ? c : 0).Find(fe => ConfigManager.SourceInvariantComparer.Equals(fe.Name + " " + ConfigManager.SourceSeperator + " " + fe.Source, cho.Value));
                    if (feat != null) res.AddRange(feat.Collect(level, choiceProvider, context));
                    else ConfigManager.LogError("Missing Feature: " + cho.Value + " in " + Collection);
                }
            }
            return res;
        }
        public List<string> Choices(IChoiceProvider choiceProvider)
        {
            int offset = choiceProvider.GetChoiceOffset(this, UniqueID, Amount);
            List<string> res = new List<string>();
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = choiceProvider.GetChoice(UniqueID + counter);
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
