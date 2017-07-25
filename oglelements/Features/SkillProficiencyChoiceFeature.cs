using OGL.Base;
using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OGL.Features
{
    public class SkillProficiencyChoiceFeature: Feature
    {
        public int Amount { get; set; }
        public double ProficiencyMultiplier { get; set; }
        public ProficiencyBonus BonusType { get; set; } = ProficiencyBonus.AddOnlyIfNotProficient;
        public bool OnlyAlreadyKnownSkills { get; set; }
        public String UniqueID { get; set; }
        public List<string> Skills;
            public SkillProficiencyChoiceFeature()
            : base()
        {
            Skills = new List<string>();
            Amount = 1;
            ProficiencyMultiplier = 1.0;
        }
        public SkillProficiencyChoiceFeature(string name, string text, string uniqueID, int amount = 1, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Skills = new List<string>();
            Amount = amount;
            UniqueID = uniqueID;
            ProficiencyMultiplier = 1.0;
        }
        public SkillProficiencyChoiceFeature(string name, string text, string uniqueID, List<Skill> choices, double multiplier=1.0, int amount = 1, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Skills = new List<string>(from c in choices select c.Name);
            Amount = amount;
            UniqueID = uniqueID;
            ProficiencyMultiplier = multiplier;
        }
        public SkillProficiencyChoiceFeature(string name, string text, string uniqueID, Skill choice1, Skill choice2, int amount = 1, int level = 1, bool hidden = true)
        : base(name, text, level, hidden)
        {
            Skills = new List<string>();
            Amount = amount;
            UniqueID = uniqueID;
            Skills.Add(choice1.Name);
            Skills.Add(choice2.Name);
            ProficiencyMultiplier = 1.0;
        }
        public SkillProficiencyChoiceFeature(string name, string text, string uniqueID, Skill choice1, Skill choice2, Skill choice3, int amount = 1, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Skills = new List<string>();
            Amount = amount;
            UniqueID = uniqueID;
            Skills.Add(choice1.Name);
            Skills.Add(choice2.Name);
            Skills.Add(choice3.Name);
            ProficiencyMultiplier = 1.0;
        }
        public SkillProficiencyChoiceFeature(string name, string text, string uniqueID, Skill choice1, Skill choice2, Skill choice3, Skill choice4, int amount = 1, int level = 1, bool hidden = true)
            : base(name, text, level, hidden)
        {
            Skills = new List<string>();
            Amount = amount;
            UniqueID = uniqueID;
            Skills.Add(choice1.Name);
            Skills.Add(choice2.Name);
            Skills.Add(choice3.Name);
            Skills.Add(choice4.Name);
            ProficiencyMultiplier = 1.0;
        }
        
        public List<Skill> getSkills(IChoiceProvider provider, OGLContext context)
        {
            List<Skill> res = new List<Skill>();
            int offset = provider.GetChoiceOffset(this, UniqueID, Amount);
            for (int c = 0; c < Amount; c++)
            {
                String counter = "";
                if (c + offset > 0) counter = "_" + (c + offset).ToString();
                Choice cho = provider.GetChoice(UniqueID + counter);
                if (cho != null && cho.Value != "") res.Add(context.GetSkill(cho.Value, Source));
            }
            return res;
        }
        public override string Displayname()
        {
            return "Skill Choice Proficiency Feature";
        }
    }
}

