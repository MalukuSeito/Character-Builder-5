using System.Collections.Generic;

namespace OGL.Features
{
    public class ResistanceFeature: Feature
    {
        public List<string> Resistances { get; set; } = new List<string>();
        public List<string> Vulnerabilities { get; set; } = new List<string>();
        public List<string> Immunities { get; set; } = new List<string>();
        public List<string> SavingThrowAdvantages { get; set; } = new List<string>();

        public ResistanceFeature() : base() {
            Action = Base.ActionType.ForceHidden;
        }
        public ResistanceFeature(string name, string text, int level = 1, bool hidden = false) : base(name, text, level, hidden) {
            Action = Base.ActionType.ForceHidden;
        }
        public override string Displayname()
        {
            return "Resistance Feature";
        }
    }
}
