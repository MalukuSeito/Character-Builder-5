using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character_Builder_5
{
    public class SaveProficiencyFeature: Feature 
    {
        public Ability Ability { get; set; }
        public SaveProficiencyFeature()
            : base()
        {
            Ability = Ability.None;
        }
        public SaveProficiencyFeature(string name, string text, Ability ability, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Ability = ability;
        }
        public override string Displayname()
        {
            return "Save Proficiency Feature";
        }
    }
}
