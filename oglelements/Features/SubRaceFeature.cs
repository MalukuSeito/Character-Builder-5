using System;
using System.Collections.Generic;

namespace OGL.Features
{
    public class SubRaceFeature: Feature
    {
        public List<String> Races;
        public SubRaceFeature() : base() 
        { 
            Races=new List<string>();
        }
        public SubRaceFeature(string name, string text, String Race, int level = 1, bool hidden = false)
            : base(name, text, level, hidden)
        {
            Races = new List<string>();
            Races.Add(Race);
        }
        public override string Displayname()
        {
            return "Subrace Feature";
        }
    }
}
