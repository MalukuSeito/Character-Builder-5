using OGL;
using OGL.Common;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Character_Builder
{
    public class JournalBoon: IChoiceProvider
    {
        [XmlIgnore]
        public Dictionary<Feature, int> ChoiceCounter = new Dictionary<Feature, int>(new ObjectIdentityEqualityComparer());
        public bool ShouldSerializeChoiceCounter() => false;
        [XmlIgnore]
        public Dictionary<string, int> ChoiceTotal = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public bool ShouldSerializeChoiceTotal() => false;

        public Guid Guid { get; set; }
        public bool Deleted { get; set; }
        public bool Banked { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DisplayText { get; set; }
        public bool New { get; set; }
        public List<Choice> Choices = new();
        public JournalBoon() { }
        public JournalBoon(string boon)
        {
            Name = boon;
            New = true;
            Guid = Guid.NewGuid();
        }

        public JournalBoon(JournalBoon b)
        {
            Name = b.Name;
            Banked = b.Banked;
            New = false;
            Guid = b.Guid;
            Choices = b.Choices.Select(x => new Choice(x)).ToList();
        }

        public string InfoName()
        {
            return (Deleted ? "-" : "") + (New ? "+" : "") + ToString();
        }
        public override string ToString()
        {
            return DisplayName?? SourceInvariantComparer.NoSource(Name);
        }

        public Choice GetChoice(string ID)
        {
            return (from c in Choices where c.UniqueID == ID select c).FirstOrDefault();
        }
        public void SetChoice(String ID, String Value)
        {
            Choice c = GetChoice(ID);
            if (c != null) c.Value = Value;
            else Choices.Add(new Choice(ID, Value));
        }
        public void RemoveChoice(String ID)
        {
            Choices.RemoveAll(c => c.UniqueID == ID);
        }
        public int GetChoiceOffset(Feature f, string uniqueID, int amount)
        {
            if (!ChoiceCounter.ContainsKey(f))
            {
                ChoiceCounter.Add(f, GetChoiceTotal(uniqueID));
                ChoiceTotal[uniqueID] += amount;
            }
            return ChoiceCounter[f];
        }
        public void ResetChoices()
        {

        }
        public int GetChoiceTotal(string uniqueID)
        {
            if (!ChoiceTotal.ContainsKey(uniqueID)) ChoiceTotal.Add(uniqueID, 0);
            return ChoiceTotal[uniqueID];
        }
        public bool Matches(string expression, List<string> additionalKeywords = null, int classlevel = 0, int level = 0)
        {
            return true;
        }
    }
}
