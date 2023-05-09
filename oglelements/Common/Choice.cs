using System;

namespace OGL.Common
{
    public class Choice
    {
        public string UniqueID { get; set; }
        public string Value { get; set; }
        public Choice() {
        }
        public Choice(string ID, string value)
        {
            UniqueID = ID;
            Value = value;
        }
        public Choice(Choice choice)
        {
            UniqueID = choice.UniqueID;
            Value = choice.Value;
        }
    }
}
