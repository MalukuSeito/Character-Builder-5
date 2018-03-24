using OGL.Common;
using OGL.Features;
using OGL.Base;
using System.IO;

namespace CB_5e.ViewModels.Character.ChoiceOptions
{
    public class AbilityChoice: IXML
    {
        public AbilityChoice(Ability ability)
        {
            Ability = ability;
        }
        public Ability Ability { get; set; }

        public string Name => "+1 " + Ability.ToString();

        public string Source => "";

        public string ToXML()
        {
            return new Feature(Ability.ToString(), Name).ToXML();
        }

        public MemoryStream ToXMLStream()
        {
            return new Feature(Ability.ToString(), Name).ToXMLStream();
        }
        public override string ToString()
        {
            return Name;
        }

        public bool Matches(string text, bool nameOnly)
        {
            throw new System.NotImplementedException();
        }
    }
}