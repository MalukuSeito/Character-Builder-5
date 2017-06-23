using System;

namespace OGL.Descriptions
{
    public class TableEntry
    {
        public int MinRoll { get; set; }
        public int MaxRoll { get; set; }
        public String Title { get; set; }
        public String Text { get; set; }
        public TableEntry() { }
        public TableEntry(int min, String text, int max = 0)
        {
            if (max == 0) max = min;
            Text = text;
            Title = "";
            MinRoll = min;
            MaxRoll = max;
        }
        public TableEntry(int min, String title, String text, int max = 0)
        {
            if (max == 0) max = min;
            Text = text;
            Title = title;
            MinRoll = min;
            MaxRoll = max;
        }
        public override string ToString()
        {
            if (Title != null && Title != "") return Title + " " + Text;
            return Text;
        }

        public string ToFullString()
        {
            if (Title != null && Title != "") return roll() + Title + " " + Text;
            return roll() + Text;
        }
        private string roll()
        {
            if (MinRoll >= 0)
            {
                if (MaxRoll > MinRoll) return MinRoll + "-" + MaxRoll + ": ";
                return MinRoll + ": ";
            }
            return "";
        }
    }
}
