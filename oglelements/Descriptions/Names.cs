using System;
using System.Collections.Generic;

namespace OGL.Descriptions
{
    public class Names
    {
        public String Title { get; set; }
        public List<string> ListOfNames;
        public Names()
        {
            ListOfNames = new List<string>();
        }
        public Names(string title, List<string> names)
        {
            Title = title;
            ListOfNames = names;
        }
        public override string ToString()
        {
            return Title;
        }
    }
}
