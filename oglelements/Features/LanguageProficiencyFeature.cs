using System;
using System.Collections.Generic;

namespace OGL.Features
{
    public class LanguageProficiencyFeature: Feature
    {
        public List<String> Languages { get; set; }
        public LanguageProficiencyFeature()
            : base()
        {
            Languages = new List<string>();
        }
        public LanguageProficiencyFeature(string name, string text, Language lang, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Languages = new List<string>();
            Languages.Add(lang.Name);
        }
        public LanguageProficiencyFeature(string name, string text, Language lang, Language lang2, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Languages = new List<string>();
            Languages.Add(lang.Name);
            Languages.Add(lang2.Name);
        }
        public LanguageProficiencyFeature(string name, string text, Language lang, Language lang2, Language lang3, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Languages = new List<string>();
            Languages.Add(lang.Name);
            Languages.Add(lang2.Name);
            Languages.Add(lang3.Name);
        }
        public LanguageProficiencyFeature(string name, string text, Language lang, Language lang2, Language lang3, Language lang4, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Languages = new List<string>();
            Languages.Add(lang.Name);
            Languages.Add(lang2.Name);
            Languages.Add(lang3.Name);
            Languages.Add(lang4.Name);
        }
        public override string Displayname()
        {
            return "Language Proficiency Feature";
        }
    }
}
