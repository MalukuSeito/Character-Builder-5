using OGL.Base;
using System;
using System.Collections.Generic;

namespace OGL.Features
{
    public class SkillProficiencyFeature: Feature
    {
        public List<String> Skills;
        public double ProficiencyMultiplier { get; set; }
        public ProficiencyBonus BonusType { get; set; } = ProficiencyBonus.AddOnlyIfNotProficient;
        public SkillProficiencyFeature()
            : base()
        {
            Skills = new List<string>();
            ProficiencyMultiplier = 1.0;
        }
        public SkillProficiencyFeature(string name, string text, Skill skill, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Skills = new List<string>();
            Skills.Add(skill.Name);
            ProficiencyMultiplier = 1.0;
        }
        public SkillProficiencyFeature(string name, string text, Skill skill, Skill skill2, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Skills = new List<string>();
            Skills.Add(skill.Name);
            Skills.Add(skill2.Name);
            ProficiencyMultiplier = 1.0;
        }
        public SkillProficiencyFeature(string name, string text, Skill skill, Skill skill2, Skill skill3, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Skills = new List<string>();
            Skills.Add(skill.Name);
            Skills.Add(skill2.Name);
            Skills.Add(skill3.Name);
            ProficiencyMultiplier = 1.0;
        }
        public SkillProficiencyFeature(string name, string text, Skill skill, Skill skill2, Skill skill3, Skill skill4, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Skills = new List<string>();
            Skills.Add(skill.Name);
            Skills.Add(skill2.Name);
            Skills.Add(skill3.Name);
            Skills.Add(skill4.Name);
            ProficiencyMultiplier = 1.0;
        }
        public override string Displayname()
        {
            return "Skill Proficiency Feature";
        }
    }
}
